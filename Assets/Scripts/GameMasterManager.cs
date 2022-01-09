using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMasterManager : MonoBehaviour
{
    public StateMachine<GameMasterManager> controller;
    public GameObject mainMenu, gameMenu, optionsMenu;
    public WordGridManager gridManger;
    public Toggle hardModeToggle, dictionaryCheckToggle;
    public CanvasScaler scaler;

    private void Awake() {
       // PlayerPrefs.DeleteAll();

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
            scaler.matchWidthOrHeight = 0;
        } else if (Application.platform != RuntimePlatform.WindowsEditor && Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.LinuxEditor) {
            scaler.matchWidthOrHeight = 1;
        }


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

        controller = new StateMachine<GameMasterManager>(new MainMenuState(), this);
        GoToMainMenu();
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
    }

    public void GoToOptionsMenu() {
        HideAll();
        controller.ChangeState(new OptionsState());
        optionsMenu.SetActive(true);
    }

    public void GoToGameplayMenu() {
        HideAll();
        controller.ChangeState(new GameState());
        gameMenu.SetActive(true);
        gridManger.Setup();
    }

    public void DeleteProgress() {

        SetStringVars(new List<string>() { WordGridManager.CURRENTSTREAK, WordGridManager.LARGESTSTREAK, WordGridManager.NUMBEROFPUZZLESPLAYED, WordGridManager.NUMBERWINS, WordGridManager.WIN1, WordGridManager.WIN2, WordGridManager.WIN3, WordGridManager.WIN4, WordGridManager.WIN5, WordGridManager.WIN6 }, 0);



    }

}

public class MainMenuState : State<GameMasterManager> {

}



public class GameState : State<GameMasterManager> {

}


public class OptionsState : State<GameMasterManager> {

}




