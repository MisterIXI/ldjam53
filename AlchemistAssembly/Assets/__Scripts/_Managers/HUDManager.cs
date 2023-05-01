using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class HUDManager : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI InfoObject, redPotion, yellowPotion,bluePotion, yinyangPotion;
    private int redPotionAmount, yellowPotionAmount, bluePotionAmount, yinyangPotionAmount;
    [SerializeField]private Image[] HotkeyImages;
    private Color highlightColor =  new Color(0,0.990566f,0.02175521f,0.5254902f);
    private Color normalColor = new Color(0,0,0,0.5254902f);
    private int currentMission = 0;
    // INFOPANEL 
    [SerializeField] private Image infoImage;
    [SerializeField]private Sprite[] InfoImagePool;
    [SerializeField]private TextMeshProUGUI infoText;
    [SerializeField]private Texture2D[] cursorImages;
    public static HUDManager Instance{get; private set;}
     private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        redPotionAmount = 0;
        bluePotionAmount = 0;
        yellowPotionAmount= 0;
        yinyangPotionAmount  =0;
    }
    // UPDATE HUD 
    #region Ressource
    public void AddPotionRed()
    {
        redPotionAmount ++; 
        redPotion.text = redPotionAmount.ToString();
        if(redPotionAmount == 10)
        {
            currentMission = 1;
        }
        UpdateMission();
    }
    public void AddPotionBlue()
    {
        bluePotionAmount ++; 
        bluePotion.text = bluePotionAmount.ToString();
        if(bluePotionAmount == 10)
        {
            currentMission = 2;
        }
        UpdateMission();
    }
    public void AddPotionYellow()
    {
        yellowPotionAmount ++; 
        yellowPotion.text = yellowPotionAmount.ToString();
        if(yellowPotionAmount == 10)
        {
            currentMission = 3;
        }
        UpdateMission();
    }
    public void AddPotionYinYang()
    {
        redPotionAmount ++; 
        yinyangPotion.text = yinyangPotionAmount.ToString();
        if(yinyangPotionAmount == 10)
        {
            currentMission = 4; // WIN GAME ? 
        }
        UpdateMission();
    }
    #endregion Ressource
    #region MissionField
    private void UpdateMission()
    {
        switch (currentMission)
        {
            case 1:
                infoImage.sprite = InfoImagePool[currentMission];
                infoText.text = "Blue Potion "+ bluePotionAmount + " / 10";
                break;
            case 2:
                infoImage.sprite = InfoImagePool[currentMission];
                infoText.text = "Yellow Potion "+ yellowPotionAmount + " / 10";
                break;
            case 3:
                infoImage.sprite = InfoImagePool[currentMission];
                infoText.text = "YinYang "+ yinyangPotionAmount + " / 10";
                break;
            
            default:
                infoImage.sprite = InfoImagePool[currentMission];
                infoText.text = "Red Potions:  "+ bluePotionAmount + " / 10";
                break;
        }
    }
    #endregion MissionField
#region Hotkeys
    public void UsePathtool(int value)
    {
        DisableAllPathtools();
        HotkeyImages[value].color = highlightColor;
        Cursor.SetCursor (cursorImages[value], Vector2.zero, CursorMode.Auto);
    }
    public void DisableAllPathtools()
    {
        Cursor.SetCursor (cursorImages[0], Vector2.zero, CursorMode.Auto);
        foreach (Image x in HotkeyImages)
        {
            x.color = normalColor;
        }
    }
#endregion Hotkeys
}
