using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class HUDManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI InfoObject, redPotion, yellowPotion, bluePotion, yinyangPotion;
    public int redPotionAmount, yellowPotionAmount, bluePotionAmount, yinyangPotionAmount;
    [SerializeField] private Image[] HotkeyImages;
    private Color highlightColor = new Color(0.8f, 0.8f, 0.8f, 1f);
    private Color normalColor = new Color(0.272f, 0.272f, 0.272f, 1f);
    private int currentMission = 0;
    // INFOPANEL 
    [SerializeField] private Image infoImage;
    public static HUDManager Instance { get; private set; }
    [SerializeField] private Sprite[] InfoImagePool;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private Texture2D[] cursorImages;
    private int tooltipProgress = 0;
    [SerializeField] private GameObject tooltipObject;
    [SerializeField] private TextMeshProUGUI tooltipText;
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
        yellowPotionAmount = 0;
        yinyangPotionAmount = 0;
    }
    // UPDATE HUD 
    #region Ressource
    public void AddPotionRed()
    {
        redPotionAmount++;
        redPotion.text = redPotionAmount.ToString();
        if (redPotionAmount == 10)
        {
            currentMission = 1;
        }
        UpdateMission();
    }
    public void AddPotionBlue()
    {
        bluePotionAmount++;
        bluePotion.text = bluePotionAmount.ToString();
        if (bluePotionAmount == 10)
        {
            currentMission = 2;
        }
        UpdateMission();
    }
    public void AddPotionYellow()
    {
        yellowPotionAmount++;
        yellowPotion.text = yellowPotionAmount.ToString();
        if (yellowPotionAmount == 10)
        {
            currentMission = 3;
        }
        UpdateMission();
    }
    public void AddPotionYinYang()
    {
        redPotionAmount++;
        yinyangPotion.text = yinyangPotionAmount.ToString();
        if (yinyangPotionAmount == 10)
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
                infoText.text = "Blue Potion " + bluePotionAmount + " / 10";
                break;
            case 2:
                infoImage.sprite = InfoImagePool[currentMission];
                infoText.text = "Yellow Potion " + yellowPotionAmount + " / 10";
                break;
            case 3:
                infoImage.sprite = InfoImagePool[currentMission];
                infoText.text = "YinYang " + yinyangPotionAmount + " / 10";
                break;
            case 4:
                infoImage.sprite = InfoImagePool[currentMission];
                infoText.text = "You Win! :)";
                break;

            default:
                infoImage.sprite = InfoImagePool[currentMission];
                infoText.text = "Red Potions:  " + redPotionAmount + " / 10";
                break;
        }
    }
    #endregion MissionField
    #region Hotkeys
    public void UsePathtool(int value)
    {
        DisableAllPathtools();
        HotkeyImages[value].color = highlightColor;
        // Cursor.SetCursor(cursorImages[value], Vector2.zero, CursorMode.Auto);
    }
    public void DisableAllPathtools()
    {
        // Cursor.SetCursor(cursorImages[0], Vector2.zero, CursorMode.Auto);
        foreach (Image x in HotkeyImages)
        {
            x.color = normalColor;
        }
    }
    #endregion Hotkeys
    public void ChangetooltipText(int index)
    {
        if (tooltipProgress == 5 && index == 6)
        {
            tooltipObject.SetActive(false);
        }
        if (index - tooltipProgress == 1)
        {
            tooltipProgress++;
            switch (tooltipProgress)
            {
                case 1:
                    tooltipText.text = "Use the Resource Tool(3) to build a mushroomfarm.";
                    break;
                case 2:
                    tooltipText.text = "Use the Cauldron Tool(4) to build a crafting cauldron.";
                    break;
                case 3:
                    tooltipText.text = "Select the cauldron and change the recepie to red potion.";
                    break;
                case 4:
                    tooltipText.text = "Use the Railway Tool(2) to connect all buldings. Then select a resource miner and route the output to the cauldron.";
                    break;
                case 5:
                    tooltipText.text = "Deliver 1 red potion to the witchhouse.";
                    break;
                default:
                    tooltipText.text = "Use the Resource Tool(3) to build a waterpump.";
                    break;
            }
        }
    }
}
