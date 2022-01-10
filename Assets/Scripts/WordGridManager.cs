using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WordGridManager : MonoBehaviour {

    public const string NUMBEROFPUZZLESPLAYED = "NUMBERPUZZLES", NUMBERWINS = "NUMBERWIN", WIN1 = "WIN1", WIN2 = "WIN2", WIN3 = "WIN3", WIN4 = "WIN4", WIN5 = "WIN5", WIN6 = "WIN6", CURRENTSTREAK = "CURRENTSTREAK", LARGESTSTREAK = "LARGESTSTREAK", HARDMODE = "HARDMODE", DICTIONARYCHECK = "DICTIONARYCHECK", MONTH = "MONTH", DAY = "DAY", YEAR = "YEAR", SELECTEDLENGTH = "SELECTEDLENGTH";



    public GameMasterManager manager;
    public int numberofguesses = 5;
    public string currentWord = "";
    public Transform wordRowGroupingTransform;
    public int currentRow;
    public CanvasKeyboard.CanvasKeyboard keyboard;
    public StateMachine<WordGridManager> controller;

    [Header("Word Rows")]
    public VerticalLayoutGroup vertGroup;
    public WordRow wordRowPrefab;
    public List<WordRow> rows = new List<WordRow>();

    [Header("WinScreen")]
    public List<GuessDistributionBar> bars = new List<GuessDistributionBar>();
    public GameObject winScreen;
    public GameObject guessDistribution;
    public TextMeshProUGUI NumTotal, winPercentText, currentStreakText, largestStreakText, winTitle, lossText;
    public float winLingerTime = 1f;
    public GameObject BackToWinScreenButton;

    [Header("Word Text")]
    public GameObject errorWindow;
    public TextMeshProUGUI errorText;
    public float errorLingerTime = 0.5f;
    
    float errorTimer = 0;
    bool showingError = false;

    public HashSet<char> necessaryCharacter = new HashSet<char>();

    private void Awake() {
        greenEmoji = char.ConvertFromUtf32(0x1F7E9);
        yellowEmoji = char.ConvertFromUtf32(0x1F7E8);
        emptyEmoji = char.ConvertFromUtf32(0x2B1B);
    }


    public void Update() {
        if (showingError) {
            if (errorTimer < Time.time) {
                showingError = false;
                errorWindow.SetActive(false);
            }
        }
    }

    bool isDaily;


    public void Setup(string s, bool isDaily) {
        // PlayerPrefs.DeleteAll();
        currentWord = s;
        winScreen.SetActive(false);
        BackToWinScreenButton.SetActive(false);
        vertGroup.enabled = true;
        
        print(currentWord);
        errorWindow.SetActive(false);
        int numberPlayed = GetInt(NUMBEROFPUZZLESPLAYED) + 1;
        SetInt(NUMBEROFPUZZLESPLAYED, numberPlayed);
        keyboard.gameObject.SetActive(true);
        currentWord = currentWord.ExceptChars(new List<char> { ' ', '\n', '\t' });
        currentWord = currentWord.Substring(0, currentWord.Length);
        CreateWordRows();
        currentRow = 0;
        keyboard.maxBuildStringSize = currentWord.Length;
        keyboard.minBuildString = currentWord.Length;
        keyboard.SetBuildString("");
        controller = new StateMachine<WordGridManager>(new EnterWordState(), this);
        keyboard.SetAllKeysNormal();
        necessaryCharacter = new HashSet<char>();
    }

    public void ShowError(string s) {
        errorWindow.SetActive(true);
        errorText.SetText(s);
        errorTimer = Time.time + errorLingerTime;
        showingError = true;
    }

    public void SetInputString(string s) {
        if (canEnterWords) {
            rows[currentRow].SetTypeText(s);
        }
    }


    public IEnumerator LayoutWorkAround() {
        yield return null;
        for (int i = 0; i < numberofguesses; i++) {
            rows[i].gameObject.SetActive(true);
        }
        yield return null;
        vertGroup.enabled = false;
    }

    public void CreateWordRows() {

        int toadd = numberofguesses - rows.Count;
        for (int i = 0; i < toadd; i++) {
            WordRow row = Instantiate(wordRowPrefab, wordRowGroupingTransform);
            rows.Add(row);
        }

        foreach (WordRow r in rows) {
            r.gameObject.SetActive(false);
        }

        for (int i = 0; i < numberofguesses; i++) {
            rows[i].SetSize(currentWord.Length);
        }
        StartCoroutine(LayoutWorkAround());

    }

    


    public void PlayerPressEnter() {

        if (canEnterWords) {
            print("here");
            bool ddddd = true;
            string s = keyboard.buildString.ToUpper();
            if (GetBool(DICTIONARYCHECK) && !GameMasterManager.answerWords[currentWord.Length].Contains(s) && !GameMasterManager.commonWords[currentWord.Length].Contains(s)) {
                rows[currentRow].Shake();
                ShowError("Not in word list");
                ddddd = false;
            } 
            
            if (ddddd && GetBool(HARDMODE)) {

                foreach (char c in necessaryCharacter) {
                    if (!s.Contains(c)) {
                        ddddd = false;
                    }
                }
                if (!ddddd) {
                    rows[currentRow].Shake();
                    ShowError("Must contain all previously revealed characters");
                }

            }

            
            if (ddddd) {
                controller.ChangeState(new WordAnimationState());
            }
        }


    }

    public bool canEnterWords = false;

    public float flipSpeed = 2f;

    public IEnumerator WordAnimationFlip() {


        yield return null;
        string inputString = keyboard.buildString.ToUpper();

        for (int i = 0; i < currentWord.Length; i++) {

            WordGridButton currentButton = rows[currentRow].wordgridButtons[i];

            while (currentButton.transform.localScale.y != 0) {
                currentButton.transform.localScale = new Vector3(1, Mathf.MoveTowards(currentButton.transform.localScale.y, 0, flipSpeed * Time.deltaTime), 1);
                yield return null;
            }

            print(inputString[i].ToString());
            print(currentWord[i].ToString());

            if (inputString[i] == currentWord[i]) {
                currentButton.SetCorrect(inputString[i]);
                keyboard.GreenChar(inputString[i]);
                necessaryCharacter.Add(inputString[i]);
            } else if (currentWord.Contains(inputString[i])) {
                currentButton.SetSemiCorrect(inputString[i]);
                keyboard.YellowChar(inputString[i]);
                necessaryCharacter.Add(inputString[i]);
            } else {
                currentButton.SetMissing(inputString[i]);
                keyboard.EmptyChar(inputString[i]);
            }

            while (currentButton.transform.localScale.y != 1) {
                currentButton.transform.localScale = new Vector3(1, Mathf.MoveTowards(currentButton.transform.localScale.y, 1, flipSpeed * Time.deltaTime), 1);
                yield return null;
            }

        }

        currentRow++;
        keyboard.SetBuildString("");

        print(inputString + " : " + currentWord);
        

        if (inputString == currentWord) {
           
            controller.ChangeState(new WinState(true));
        } else if (currentRow < numberofguesses) {
            controller.ChangeState(new EnterWordState());
        } else {
            controller.ChangeState(new WinState(false));
        }
        
    }

    string emptyEmoji = "\u1567", greenEmoji = "\u1563", yellowEmoji = "\u1562";


    public void EmojiCopy () {
        string result = "";
        for (int i = 0; i < currentRow; i++) {
            WordRow row = rows[i];
            for (int j = 0; j < row.currentSize; j++) {
                WordGridButton button = row.wordgridButtons[j];
                switch (button.state) {
                    case WORDBUTTONSTATE.GREEN:
                        result += greenEmoji;
                        break;
                    case WORDBUTTONSTATE.YELLOW:
                        result += yellowEmoji;
                        break;
                    case WORDBUTTONSTATE.EMPTY:
                        result += emptyEmoji;
                        break;
                }
            }
            result += '\n';
        }
        result.CopyToClipboard();
        ClipboardExtension.CopyToClipboard(result);
    }

    public void SetInt(string s, int val) {
        PlayerPrefs.SetInt(s, val);
    }

    public bool HasInt(string s) {
        return PlayerPrefs.HasKey(s);
    }

    public int GetInt(string s) {
        return PlayerPrefs.GetInt(s);
    }


    public void SetBool(string s, bool b) {
        if (b)
        PlayerPrefs.SetInt(s, 1);
        else
        PlayerPrefs.SetInt(s, 0);
    }

    public bool HasBool(string s) {
        return PlayerPrefs.HasKey(s);
    }

    public bool GetBool(string s) {
        if (PlayerPrefs.GetInt(s) > 0) {
            return true;
        } else {
            return false;
        }
    }


    public void HideWinScreen() {
        BackToWinScreenButton.SetActive(true);
        winScreen.gameObject.SetActive(false);
    }

    public void ReShowWinScreen() {
        BackToWinScreenButton.SetActive(false);
        winScreen.gameObject.SetActive(true);
    }


    public void PlayAgain() {
        Setup(manager.GetWord(), false); ;
    }

}


public class EnterWordState : State<WordGridManager> {
    public override void Enter(StateMachine<WordGridManager> obj) {
        obj.target.keyboard.SetBuildString("");
        obj.target.canEnterWords = true;
        obj.target.keyboard.ReEnable();
        obj.target.keyboard.doneButton.gameObject.SetActive(true);
    }

    public override void Exit(StateMachine<WordGridManager> obj) {
        obj.target.canEnterWords = false;

        obj.target.keyboard.doneButton.gameObject.SetActive(false);
    }
}




public class WordAnimationState : State<WordGridManager> {

    

        public override void Enter(StateMachine<WordGridManager> obj) {
            obj.target.canEnterWords = false;
            obj.target.StartCoroutine(obj.target.WordAnimationFlip());
        }
    }

public class WinState : State<WordGridManager> {


    public bool win = true;

    public WinState(bool b) {
        win = b;
    }

    public IEnumerator ShowScreen(StateMachine<WordGridManager> obj) {

        yield return new WaitForSeconds(obj.target.winLingerTime);

        obj.target.errorWindow.SetActive(false);
        if (win) {
            obj.target.winTitle.SetText("You Win!");
            int toAdd = 0;
            if (obj.target.currentRow == 1) {
                toAdd = obj.target.GetInt(WordGridManager.WIN1) + 1;
                obj.target.SetInt(WordGridManager.WIN1, toAdd );
            } else if (obj.target.currentRow == 2) {
                toAdd = obj.target.GetInt(WordGridManager.WIN2) + 1;
                obj.target.SetInt(WordGridManager.WIN2, toAdd);
            } else if (obj.target.currentRow == 3) {
                toAdd = obj.target.GetInt(WordGridManager.WIN3) + 1;
                obj.target.SetInt(WordGridManager.WIN3, toAdd);
            } else if (obj.target.currentRow == 4) {
                toAdd = obj.target.GetInt(WordGridManager.WIN4) + 1;
                obj.target.SetInt(WordGridManager.WIN4, toAdd);
            } else if (obj.target.currentRow == 5) {
                toAdd = obj.target.GetInt(WordGridManager.WIN5) + 1;
                obj.target.SetInt(WordGridManager.WIN5, toAdd);
            } else if (obj.target.currentRow == 6) {
                toAdd = obj.target.GetInt(WordGridManager.WIN6) + 1;
                obj.target.SetInt(WordGridManager.WIN6,  toAdd);
            }
        } else {
            obj.target.winTitle.SetText("You Lose!");
        }
        yield return null;
        obj.target.guessDistribution.SetActive(win);
        int currentStreak = 0;

        if (win) {
            currentStreak = obj.target.GetInt(WordGridManager.CURRENTSTREAK) + 1;
        }

        obj.target.lossText.SetText("Word was: " + obj.target.currentWord);
        obj.target.lossText.gameObject.SetActive(!win);

        obj.target.SetInt(WordGridManager.CURRENTSTREAK, currentStreak);
        if (currentStreak > obj.target.GetInt(WordGridManager.LARGESTSTREAK)) {
            obj.target.SetInt(WordGridManager.LARGESTSTREAK, currentStreak);
        }

        float numberOfWins = obj.target.GetInt(WordGridManager.NUMBERWINS);
        if (win) numberOfWins++;
        obj.target.SetInt(WordGridManager.NUMBERWINS, (int)numberOfWins);
        float winPercent = (numberOfWins) / ((float)obj.target.GetInt(WordGridManager.NUMBEROFPUZZLESPLAYED));

        obj.target.currentStreakText.SetText(obj.target.GetInt(WordGridManager.CURRENTSTREAK).ToString());
        obj.target.largestStreakText.SetText(obj.target.GetInt(WordGridManager.LARGESTSTREAK).ToString());
        obj.target.NumTotal.SetText(obj.target.GetInt(WordGridManager.NUMBEROFPUZZLESPLAYED).ToString());
        obj.target.winPercentText.SetText((winPercent * 100).ToString("0.0") + "%");


        for (int i = 0; i < 6; i++) {
            float numberWinsGuess = obj.target.GetInt("WIN" + (i + 1).ToString());
            float percent = (numberWinsGuess) / numberOfWins;
            obj.target.bars[i].Set(numberWinsGuess, percent);
        }
        obj.target.keyboard.gameObject.SetActive(false);

        obj.target.winScreen.SetActive(true);

        obj.target.BackToWinScreenButton.SetActive(false);
    }

    public override void Enter(StateMachine<WordGridManager> obj) {


        obj.target.StartCoroutine(ShowScreen(obj));
        LastPuzzle l = new LastPuzzle();
        l.grid = new WORDBUTTONSTATE[obj.target.currentWord.Length, obj.target.currentRow];
        
        for (int y = 0; y < obj.target.currentRow; y++) {
            for (int x = 0; x < obj.target.currentWord.Length;x++) {
            
                switch (obj.target.rows[y].wordgridButtons[x].state) {
                    case WORDBUTTONSTATE.GREEN:
                        l.grid[x, y] = WORDBUTTONSTATE.GREEN;
                        break;
                    case WORDBUTTONSTATE.YELLOW:
                        l.grid[x, y] = WORDBUTTONSTATE.YELLOW;
                        break;
                    case WORDBUTTONSTATE.EMPTY:
                        l.grid[x, y] = WORDBUTTONSTATE.EMPTY;
                        break;
                }
            }
        }
        obj.target.manager.SaveLastPuzzle(l);


    }
}


public class LoseState : State<WordGridManager> {

}


public static class ClipboardExtension {
    /// <summary>
    /// Puts the string into the Clipboard.
    /// </summary>
    public static void CopyToClipboard(this string str) {
        GUIUtility.systemCopyBuffer = str;
    }
}