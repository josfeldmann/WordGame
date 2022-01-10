using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameMasterManager : MonoBehaviour
{
    public StateMachine<GameMasterManager> controller;
    public GameObject mainMenu, gameMenu, optionsMenu;
    public WordGridManager gridManger;
    public Toggle hardModeToggle, dictionaryCheckToggle;
    public CanvasScaler scaler;
    public TextMeshProUGUI dailytext, dailyTimerText;
    public DateTime currentTime;
    public bool DailyDisabled = false;
    public Button dailyButton;
    public GameObject Tutorial;


    private void Start() {
       // PlayerPrefs.DeleteAll();

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
            scaler.matchWidthOrHeight = 0;
        } else if (Application.platform != RuntimePlatform.WindowsEditor && Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.LinuxEditor) {
            scaler.matchWidthOrHeight = 1;
        }

        if (!gridManger.HasInt(WordGridManager.SELECTEDLENGTH)) gridManger.SetInt(WordGridManager.SELECTEDLENGTH, 1);
        dropDown.SetValueWithoutNotify(gridManger.GetInt(WordGridManager.SELECTEDLENGTH));
        SetLength(gridManger.GetInt(WordGridManager.SELECTEDLENGTH));

        InitializeStringVars(new List<string>() { WordGridManager.CURRENTSTREAK, WordGridManager.LARGESTSTREAK, WordGridManager.NUMBEROFPUZZLESPLAYED, WordGridManager.NUMBERWINS, WordGridManager.WIN1, WordGridManager.WIN2, WordGridManager.WIN3, WordGridManager.WIN4, WordGridManager.WIN5, WordGridManager.WIN6 });
        if (!gridManger.HasBool(WordGridManager.DICTIONARYCHECK)) {
            gridManger.SetBool(WordGridManager.DICTIONARYCHECK, true);
            print("!!!");
        }
        if (!gridManger.HasBool(WordGridManager.HARDMODE)) {
            print("???");
            gridManger.SetBool(WordGridManager.HARDMODE, false);
        }

        bool hardMode = gridManger.GetBool(WordGridManager.HARDMODE);
        bool dict = gridManger.GetBool(WordGridManager.DICTIONARYCHECK);

        hardModeToggle.SetIsOnWithoutNotify(hardMode);
        dictionaryCheckToggle.SetIsOnWithoutNotify(dict);
        if (!gridManger.HasInt(WordGridManager.DAY)) gridManger.SetInt(WordGridManager.DAY, 0);
        if (!gridManger.HasInt(WordGridManager.MONTH)) gridManger.SetInt(WordGridManager.MONTH, 0);
        if (!gridManger.HasInt(WordGridManager.YEAR)) gridManger.SetInt(WordGridManager.YEAR, 0);

       

        controller = new StateMachine<GameMasterManager>(new MainMenuState(), this);
        GoToMainMenu();
        GetWord();
    }

    int dailySeed = 0;
    public void DailyCheck() {
        Vector3Int v = new Vector3Int();
        currentTime = DateTime.Now;
        v.x = currentTime.Month;
        v.y = currentTime.Day;
        v.z = currentTime.Year;
        dateVector = v;

        int day = gridManger.GetInt(WordGridManager.DAY);
        int month = gridManger.GetInt(WordGridManager.MONTH);
        int year = gridManger.GetInt(WordGridManager.YEAR);
        print(day);
        print(month);
        print(year);

        if (v.x == month && v.y == day && v.z == year) {
            dailytext.color = WordColors.instance.GREY;
            dailyButton.interactable = false;
            DailyDisabled = true;
        } else {
            dailytext.color = WordColors.instance.WHITE;
            DailyDisabled = false;
            dailyButton.interactable = true;
            dailySeed = v.GetHashCode();
        }
        dailyTimerText.gameObject.SetActive(DailyDisabled);


    }


    public void OpenTutorial() {
        Tutorial.SetActive(true);
    }

    public void CloseTutorial() {
        Tutorial.SetActive(false);
    }

    public static Vector3Int dateVector;
    public static int selectedLength = 5;
    public static Dictionary<int, List<string>> commonWords = new Dictionary<int, List<string>>();
    public static Dictionary<int, List<string>> answerWords = new Dictionary<int, List<string>>();
    public static bool initYet = false;


    public TMP_Dropdown dropDown;

    public List<WordBank> wordbanks = new List<WordBank>();

    public string GetWord() {

        if (!initYet) {
            InitializeWordBanks();
        }

        return commonWords[selectedLength].PickRandom();
    }


    public void SetLength(int i) {
        if (i == 0) {
            selectedLength = 4;
        } else if (i == 1) {
            selectedLength = 5;
        } else if (i == 2) {
            selectedLength = 6;
        }

        gridManger.SetInt(WordGridManager.SELECTEDLENGTH, i);

    }


    public void InitializeWordBanks() {
        commonWords = new Dictionary<int, List<string>>();
        answerWords = new Dictionary<int, List<string>>();
        foreach (WordBank b in wordbanks) {

            int len = b.wordLength;
            if (!commonWords.ContainsKey(len)) {
                commonWords.Add(len, new List<string>());
            }
            if (!answerWords.ContainsKey(len)) {
                answerWords.Add(len, new List<string>());
            }

            string contents = b.commonWordText.text;
            string[] w = contents.Split("\n");
            foreach (string s in w) {
                //print(s);
                string sss = s.ToUpper();
                sss = sss.Substring(0, b.wordLength);
                commonWords[len].Add(sss);
                answerWords[len].Add(sss);
            }

            if (b.answerWordText != null) {
                contents = b.answerWordText.text;
                w = contents.Split("\n");
                foreach (string s in w) {
                    // print(s);
                    string sss = s.ToUpper();
                    sss = sss.Substring(0, b.wordLength);
                    answerWords[len].Add(sss);
                }
            }
        }

        initYet = true;
    }



    public static string jsonSaveFileName = "LASTPUZZLE.json";


    public string GetLastPuzzleSavePath() {
        return Application.persistentDataPath +"/"+ jsonSaveFileName;
    }

    public LastPuzzle GetDefaultLastPuzzle() {


        LastPuzzle last = new LastPuzzle();
        last.grid = new WORDBUTTONSTATE[5, 6];
        return last;

    }

    public GridLayoutGroup gridgroup;
    public RectTransform gridTransform;
    public Image menuDisplayPrefab;
    public List<Image> spawnedImages = new List<Image>();

    public void GetLastPuzzleForDisplay() {

        LastPuzzle puzzle = null;

        if (File.Exists(GetLastPuzzleSavePath())) {
            puzzle = JsonTool.StringToObject<LastPuzzle>(File.ReadAllText(GetLastPuzzleSavePath()));
        } else {
            puzzle = GetDefaultLastPuzzle();
            File.WriteAllText(GetLastPuzzleSavePath(), JsonTool.ObjectToString(puzzle));
        }

        int width = 0;
        int height = 0;

        width = puzzle.grid.GetLength(0);
        height = puzzle.grid.GetLength(1);
        gridTransform.sizeDelta = new Vector2(width * (gridgroup.cellSize.x + gridgroup.spacing.x), gridTransform.rect.height);
        print("Width: " + width.ToString());
        print("Height: " + height.ToString());


        int toAdd = (width * height) -  spawnedImages.Count;
        print("To Add" + toAdd.ToString());
        for (int i = 0; i < toAdd; i++) {
            Image im = Instantiate(menuDisplayPrefab, gridTransform);
            spawnedImages.Add(im);
        }

        foreach (Image i in spawnedImages) {
            i.gameObject.SetActive(false);
        }
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                int index = (x) + (y * width);
                Image i = spawnedImages[index];
                i.gameObject.SetActive(true);
                switch (puzzle.grid[x, y]) {
                    case WORDBUTTONSTATE.GREEN:
                        i.color = WordColors.instance.GREEN;
                        break;
                    case WORDBUTTONSTATE.YELLOW:
                        i.color = WordColors.instance.YELLOW;
                        break;
                    case WORDBUTTONSTATE.EMPTY:
                        i.color = WordColors.instance.GREY;
                        break;
                }
            }
        }


    }



    public void SetHardMode(bool b) {
        gridManger.SetBool(WordGridManager.HARDMODE, b);
    }

    public void SetDict(bool b) {
        gridManger.SetBool(WordGridManager.DICTIONARYCHECK, b);
    }

    public void InitializeStringVars(List<string> s) {
        foreach (string ss in s) {
            if (!gridManger.HasInt(ss)) {
                gridManger.SetInt(ss, 0);
            }
        }
    }

    public void SetStringVars(List<string> s, int val) {
        foreach (string ss in s) {
                gridManger.SetInt(ss, val);
        }
    }


    private void Update() {
        controller.Update();

        



    }

    public void HideAll() {
        mainMenu.gameObject.SetActive(false);
        gameMenu.gameObject.SetActive(false);
        optionsMenu.SetActive(false);

    }

    public void GoToMainMenu() {
        HideAll();
        controller.ChangeState(new MainMenuState());
        mainMenu.SetActive(true);
        DailyCheck();
        GetLastPuzzleForDisplay();
    }

    public void GoToOptionsMenu() {
        HideAll();
        controller.ChangeState(new OptionsState());
        optionsMenu.SetActive(true);
    }

    public void GoToGameplayMenu(bool isDaily) {
        HideAll();
        controller.ChangeState(new GameState());
        gameMenu.SetActive(true);
        if (isDaily) {
            UnityEngine.Random.InitState(dailySeed);
            gridManger.SetInt(WordGridManager.DAY, dateVector.y);
            gridManger.SetInt(WordGridManager.MONTH, dateVector.x);
            gridManger.SetInt(WordGridManager.YEAR, dateVector.z);
            gridManger.Setup(GetWord(), true);
        } else {
            UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
            gridManger.Setup(GetWord(), false);
        }
    }

    public void DeleteProgress() {

        SetStringVars(new List<string>() { WordGridManager.CURRENTSTREAK, WordGridManager.LARGESTSTREAK, WordGridManager.NUMBEROFPUZZLESPLAYED, WordGridManager.NUMBERWINS, WordGridManager.WIN1, WordGridManager.WIN2, WordGridManager.WIN3, WordGridManager.WIN4, WordGridManager.WIN5, WordGridManager.WIN6, WordGridManager.DAY, WordGridManager.MONTH, WordGridManager.YEAR }, 0);



    }

    public void SaveLastPuzzle(LastPuzzle l) {
        File.WriteAllText(GetLastPuzzleSavePath(), JsonTool.ObjectToString(l));
    }
}

public class MainMenuState : State<GameMasterManager> {
    public override void Update(StateMachine<GameMasterManager> obj) {
        if (obj.target.DailyDisabled) {

            DateTime current = DateTime.Now; // current time
            DateTime tomorrow = current.AddDays(1).Date; // this is the "next" midnight

            int secondsUntilMidnight = (int)(tomorrow - current).TotalSeconds;

            int hours = (int)(secondsUntilMidnight / 3600);
            int minutes = (int)((secondsUntilMidnight / 60) % 60);
            int seconds = secondsUntilMidnight % 60;

            obj.target.dailyTimerText.text = "Time until next daily: " + hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");

            if (hours == 0 && minutes == 0 && seconds == 0) {
                obj.target.DailyCheck();
            }
        }
    }
}



public class GameState : State<GameMasterManager> {

}


public class OptionsState : State<GameMasterManager> {

}



[System.Serializable]
public class LastPuzzle {


    public WORDBUTTONSTATE[,] grid;


}




