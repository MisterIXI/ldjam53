using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject MenuUI, LogoUI, ControlUI, StartUI, ResumeUI;
    [SerializeField] private TextMeshProUGUI ResumeButton, StartButton, SettingButton, CreditsButton;
    [SerializeField] private GameObject[] ShiftStart, ShiftSettings, ShiftCredits;
    public MenuManager Instance { get; private set; }
    private bool gameisRunning;
    private float setting_Camera_value, setting_SFX_Volume, setting_Music_Volume;
    private float setting_Camera_valueMax, setting_SFX_VolumeMax, setting_Music_VolumeMax;
    private enum Menu
    {
        Default,
        Start,
        Setting,
        Credit
    }
    private Menu activeMenu = Menu.Default;
    // BUTTON VOIDS
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #region ButtonVoids
    public void OnButtonStartGame()
    {
        HideMenus();
        DisableButtons();
        for (int i = 0; i < ShiftStart.Length; i++)
        {
            StartCoroutine(ActivateOverSecond(ShiftStart[i], i));
            // START COROUTINE Wait 1 Sec 
        }
        StartGame(); //IMPLEMENT!!
        StartUI.SetActive(false);
        ResumeUI.SetActive(true);
        MenuUI.SetActive(false);
        EnableButtons();
        gameisRunning = true;
        activeMenu = Menu.Default;
    }
    ////////////////////// OPEN MENU //////////////////////
    public void OnTriggerOpenMenu()
    {
        HideMenus();
        MenuUI.SetActive(true);
        ControlUI.SetActive(true);
        // HUD OFF
        gameisRunning = false;
        // TIMESCALE = 0 ? 
        activeMenu = Menu.Default;
    }
    public void OnButtonResumeGame()
    {
        HideMenus();
        MenuUI.SetActive(false);
        // HUD ON
        gameisRunning = true;
        // TIMESCALE = 1  ? 
        activeMenu = Menu.Default;
    }
    public void OnButtonSettings()
    {
        if (activeMenu != Menu.Setting)
        {
            HideMenus();
            DisableButtons();
            activeMenu = Menu.Setting;
            //ButtonHighlight ?
            for (int i = 0; i < ShiftSettings.Length; i++)
            {
                StartCoroutine(ActivateOverSecond(ShiftSettings[i], i));
            }
            StartCoroutine(ActivateButtons());
        }

    }
    public void OnButtonCredits()
    {
        if (activeMenu != Menu.Credit)
        {
            HideMenus();
            DisableButtons();
            activeMenu = Menu.Credit;
            //ButtonHighlight ?
            for (int i = 0; i < ShiftCredits.Length; i++)
            {
                StartCoroutine(ActivateOverSecond(ShiftCredits[i], i));
                StartCoroutine(ActivateButtons());
            }
            StartCoroutine(ActivateButtons());
        }
    }
    private void HideMenus()
    {
        for (int i = 0; i < ShiftCredits.Length; i++)
        {
            ShiftCredits[i].SetActive(false);
        }
        for (int i = 0; i < ShiftSettings.Length; i++)
        {
            ShiftSettings[i].SetActive(false);
        }
        for (int i = 0; i < ShiftStart.Length; i++)
        {
            ShiftStart[i].SetActive(false);
        }
    }
    private void DisableButtons()
    {
        StartButton.enabled = false;
        SettingButton.enabled = false;
        CreditsButton.enabled = false;
        ResumeButton.enabled = false;
    }
    private void EnableButtons()
    {
        StartButton.enabled = true;
        SettingButton.enabled = true;
        CreditsButton.enabled = true;
        ResumeButton.enabled = true;
    }
    IEnumerator ActivateOverSecond(GameObject obj, int time)
    {
        yield return new WaitForSeconds(time);
        obj.SetActive(true);
    }
    IEnumerator ActivateButtons()
    {
        yield return new WaitForSeconds(3);
        EnableButtons();
    }
    public void OnButtonQuitGame()
    {
        Application.Quit();
    }
    #endregion ButtonVoids
    // SETTING CHANGE
    #region OnChangeSettings
    public void OnChangeCameraSensitivity(bool add)
    {
        if(add)
        {   if(setting_Camera_value + 10 <= setting_Camera_valueMax)
                setting_Camera_value +=10;
            // TEXT UPDATE VALUE
        }else{
            if(setting_Camera_value - 10 >=0)
                setting_Camera_value -=10;
        }
    }
    public void OnChangeSoundVolume(bool add)
    {
        if(add)
        {   if(setting_SFX_Volume + 10 <= setting_SFX_VolumeMax)
                setting_SFX_Volume +=10;
            // TEXT UPDATE VALUE
        }else{
            if(setting_SFX_Volume - 10 >=0)
                setting_SFX_Volume -=10;
        }
    }public void OnChangeMusicVolume(bool add)
    {
        if(add)
        {   if(setting_Music_Volume + 10 <= setting_Music_VolumeMax)
                setting_Music_Volume +=10;
            // TEXT UPDATE VALUE
        }else{
            if(setting_Music_Volume - 10 >=0)
                setting_Music_Volume -=10;
        }
    }
    #endregion OnChangeSettings



    private void StartGame()
    {
        //IMPLEMENT!!
    }
}
