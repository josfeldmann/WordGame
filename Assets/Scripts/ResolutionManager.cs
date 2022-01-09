using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResolutionManager : MonoBehaviour
{
    [HideInInspector] public Resolution[] resolutions;
    public TMP_Dropdown resolutionDropDown;
    public Toggle fullScreenToggle;

    public const string resolutionIndexKey = "resolution";
    public const string fullBoolKey = "fullscreen";

    bool shouldEditResolution = false;

    private void Awake() {

        if (Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.WebGLPlayer) {
            shouldEditResolution = true;
            resolutions = Screen.resolutions;
            resolutionDropDown.ClearOptions();
            List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
            foreach (Resolution r in resolutions) {
                options.Add(new TMP_Dropdown.OptionData(r.ToString()));
            }
            resolutionDropDown.AddOptions(options);

            if (PlayerPrefs.HasKey(fullBoolKey)) {
                if (PlayerPrefs.GetInt(fullBoolKey) == 1) {
                    fullScreenToggle.SetIsOnWithoutNotify(true);
                } else {
                    fullScreenToggle.SetIsOnWithoutNotify(false);
                }
            } else {
                PlayerPrefs.SetInt(fullBoolKey, 1);
                fullScreenToggle.SetIsOnWithoutNotify(true);
            }

            if (PlayerPrefs.HasKey(resolutionIndexKey)) {

                if (PlayerPrefs.GetInt(resolutionIndexKey) < resolutions.Length) {
                    resolutionDropDown.SetValueWithoutNotify(PlayerPrefs.GetInt(resolutionIndexKey));
                } else {
                    PlayerPrefs.SetInt(resolutionIndexKey, resolutions.Length - 1);
                    resolutionDropDown.SetValueWithoutNotify(PlayerPrefs.GetInt(resolutionIndexKey));
                }

            } else {
                PlayerPrefs.SetInt(resolutionIndexKey, resolutions.Length - 1);
                resolutionDropDown.SetValueWithoutNotify(PlayerPrefs.GetInt(resolutionIndexKey));
            }


            Apply();

        } else {
            shouldEditResolution = false;
            gameObject.SetActive(false);
        }
    }


    public void Apply() {
        if (!shouldEditResolution) return;
        if (fullScreenToggle.isOn) {
            Screen.SetResolution(resolutions[resolutionDropDown.value].width, resolutions[resolutionDropDown.value].height, FullScreenMode.ExclusiveFullScreen);
        } else {
            Screen.SetResolution(resolutions[resolutionDropDown.value].width, resolutions[resolutionDropDown.value].height, FullScreenMode.Windowed);
        }
        


        PlayerPrefs.SetInt(resolutionIndexKey, resolutionDropDown.value);
        if (fullScreenToggle.isOn)
        PlayerPrefs.SetInt(fullBoolKey, 1);
        else
        PlayerPrefs.SetInt(fullBoolKey, 0);
    }


}
