using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class MenuManager : MonoBehaviour
{
    [SerializeField] public GameObject MenuUI, LogoUI, ControlUI, StartUI, ResumeUI, HUD_UI;
    [SerializeField] private Button ResumeButton, StartButton, SettingButton, CreditsButton, QuitButton;
    [SerializeField] private GameObject[] ShiftStart, ShiftSettings, ShiftCredits;
    public static MenuManager Instance { get; private set; }
    private bool gameisRunning;
    private float setting_Camera_value = 4f, setting_SFX_Volume = 50f, setting_Music_Volume = 50f;
    private float setting_Camera_valueMax = 10f, setting_SFX_VolumeMax = 100f, setting_Music_VolumeMax = 100f;
    [SerializeField] TextMeshProUGUI setting_Camera_Text, setting_SFX_Text, setting_Music_Text;
    private PlayerSettings _playerSettings => SettingsManager.PlayerSettings;
    private bool _hasGameStarted = false;
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
        // DontDestroyOnLoad(gameObject);
        SubscribeToActions();
        // if webGL then disable quit button
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WebGLPlayer)
        {
            QuitButton.interactable = false;
        }
        OnTriggerOpenMenu();
    }
    #region ButtonVoids
    public void OnButtonStartGame()
    {

        ControlUI.SetActive(false);
        HideMenus();
        DisableButtons();
        for (int i = 0; i < ShiftStart.Length; i++)
        {
            StartCoroutine(ActivateOverSecond(ShiftStart[i], i));
            // START COROUTINE Wait 1 Sec 
        }
        StartCoroutine(StartGame()); //IMPLEMENT!!
        _hasGameStarted = true;
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
        setting_Camera_Text.text = "Camera Sensitivity\n" + setting_Camera_value + "%";
        setting_Camera_Text.text = "SFX\n" + setting_SFX_Volume + "%";
        setting_Music_Text.text = "MUSIC\n" + setting_Music_Volume + "%";
    }
    public void OnButtonResumeGame()
    {
        HideMenus();
        MenuUI.SetActive(false);
        // HUD ON
        gameisRunning = true;
        // TIMESCALE = 1  ? 
        activeMenu = Menu.Default;
        HudReferences.Instance.gameObject.SetActive(true);
    }
    public void OnButtonSettings()
    {
        if (activeMenu != Menu.Setting)
        {
            ControlUI.SetActive(false);
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
            ControlUI.SetActive(false);
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
        yield return new WaitForSeconds(0.25f * time);
        obj.SetActive(true);
    }
    IEnumerator ActivateButtons()
    {
        yield return new WaitForSeconds(1.5f);
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
        if (add)
        {
            if (setting_Camera_value + 1 <= setting_Camera_valueMax)
                setting_Camera_value += 1;
            // TEXT UPDATE VALUE
        }
        else
        {
            if (setting_Camera_value - 1 >= 0)
                setting_Camera_value -= 1;
        }
        setting_Camera_Text.text = "Camera Sensitivity\n" + setting_Camera_value + "%";
        _playerSettings.DragSpeed = setting_Camera_value;
        _playerSettings.MouseMoveSpeed = setting_Camera_value;
    }
    public void OnChangeSoundVolume(bool add)
    {
        if (add)
        {
            if (setting_SFX_Volume + 10 <= setting_SFX_VolumeMax)
                setting_SFX_Volume += 10;
            // TEXT UPDATE VALUE
        }
        else
        {
            if (setting_SFX_Volume - 10 >= 0)
                setting_SFX_Volume -= 10;
        }
        setting_SFX_Text.text = "SFX\n" + setting_SFX_Volume + "%";
        SoundManager.Instance.OnChangeSFXVolume(setting_SFX_Volume);
    }
    public void OnChangeMusicVolume(bool add)
    {
        if (add)
        {
            if (setting_Music_Volume + 10 <= setting_Music_VolumeMax)
                setting_Music_Volume += 10;
            // TEXT UPDATE VALUE
        }
        else
        {
            if (setting_Music_Volume - 10 >= 0)
                setting_Music_Volume -= 10;
        }
        setting_Music_Text.text = "MUSIC\n" + setting_Music_Volume + "%";
        SoundManager.Instance.OnChangeMusicVolume(setting_Music_Volume);
    }
    #endregion OnChangeSettings


    private void OnMenuInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {

            if (HudReferences.Instance.CurrentBuilding != null)
            {
                HudReferences.Instance.BuildingPanel.SetActive(false);
                HudReferences.Instance.RecepiePanel.SetActive(false);
                HudReferences.Instance.CurrentBuilding = null;
            }
            else if (PlacementController.Instance.ActiveTool == PlacementController.Instance.PathTool)
            {
                PlacementController.Instance.SwitchActiveTool(PlacementController.Instance.DefaultTool);
            }
            else if (!MenuUI.activeSelf)
            {
                HudReferences.Instance.gameObject.SetActive(false);
                OnTriggerOpenMenu();
            }
            else if (_hasGameStarted)
            {
                HudReferences.Instance.gameObject.SetActive(true);
                MenuUI.SetActive(false);
            }
        }
    }
    private void SubscribeToActions()
    {
        InputManager.OnMenu += OnMenuInput;
    }
    private void UnsubscribeToActions()
    {
        InputManager.OnMenu -= OnMenuInput;
    }
    private void OnDestroy()
    {
        UnsubscribeToActions();
    }
    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(1);
        StartUI.SetActive(false);
        ResumeUI.SetActive(true);
        MenuUI.SetActive(false);
        EnableButtons();
        gameisRunning = true;
        activeMenu = Menu.Default;
        HexGrid.Instance.InitializeGrid();
        HudReferences.Instance.gameObject.SetActive(true);
        HideMenus();
    }
}
