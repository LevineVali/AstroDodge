using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using System.Reflection;

public class UIManager : MonoBehaviour
{
    public GameManager GM;

    public SkinManager SM;

    /// <summary>
    /// Data amount text
    /// </summary>
    public Text DataAmount;

    /// <summary>
    /// Satellite amount text
    /// </summary>
    public Text SatelliteAmount;

    [Header("Mainwindows")]
    #region Mainwindows

    /// <summary>
    /// Window of Options
    /// </summary>
    public GameObject OptionsWindow;

    /// <summary>
    /// Button to abort the flight
    /// </summary>
    public GameObject AbortFlightButton;

    /// <summary>
    /// Window of Shop
    /// </summary>
    public GameObject ShopWindow;

    /// <summary>
    /// Offlinebonuswindow
    /// </summary>
    public GameObject OfflineBonusPanel;

    /// <summary>
    /// All buttons which opens windows
    /// </summary>
    public Button[] WindowButtons;

    [Header("Colorpicker")]
    /// <summary>
    /// Image of ColorPalette
    /// </summary>
    public Image ColorPaletteImage;

    /// <summary>
    /// ColorCursor
    /// </summary>
    public RectTransform ColorCursor;

    /// <summary>
    /// Image of ColorCursor
    /// </summary>
    public Image ColorCursorImage;

    [Space(15)]
    /// <summary>
    /// Image of GrayPalette
    /// </summary>
    public Image GrayPaletteImage;

    /// <summary>
    /// ColorCursor
    /// </summary>
    public RectTransform GrayCursor;

    /// <summary>
    /// Image of ColorCursor
    /// </summary>
    public Image GrayCursorImage;

    [Header("Prestige")]
    /// <summary>
    /// 
    /// </summary>
    public Text PrestigeHeightText;

    /// <summary>
    /// Window for prestigeinformations
    /// </summary>
    public GameObject PrestigeInformationWindow;

    public Color GrayColor;

    /// <summary>
    /// PrestigeButton
    /// </summary>
    public Button PrestigeButton;

    /// <summary>
    /// Button for selekting skin
    /// </summary>
    public Button SkinButton;

    /// <summary>
    /// Skinpreview of Rocketskin
    /// </summary>
    public Image PrestigeRocketSkin;

    /// <summary>
    /// Skinpreview of the back of the Rocketskin
    /// </summary>
    public Image PrestigeRocketSkinBack;

    /// <summary>
    /// Image of the PrestigeButton
    /// </summary>
    public Image PrestigeButtonImage;

    /// <summary>
    /// List of all PrestigeButtonImages
    /// </summary>
    public Sprite[] PrestigeButtonImages;

    /// <summary>
    /// List of all PrestibeEmblem
    /// </summary>
    public Image[] PrestigeEmbleme;


    /// <summary>
    /// Speed of the Animation
    /// </summary>
    public float FrameCounter;

    /// <summary>
    /// index of which picture is atm active
    /// </summary>
    private int frameCount = 0;

    /// <summary>
    ///time need to past to set next image
    /// </summary>
    private float frameTimer = 0;

    /// <summary>
    /// Helper for Animation
    /// 0 = Default
    /// 1 = First Click
    /// 2 = Second Click
    /// </summary>
    private int imageStage = 0;

    /// <summary>
    /// stored index
    /// </summary>
    private int emblemeIndex;

    private int skinID;

    public void OpenPrestigeInformationWindow(bool _sound)
    {
        PrestigeInformationWindow.SetActive(true);

        if (_sound)
            buttonsound.Source.Play();
    }

    public void ClosePrestigeInformationWindow(bool _sound)
    {
        PrestigeInformationWindow.SetActive(false);

        if (_sound)
            buttonsound.Source.Play();
    }

    public void ChoosePrestigeRocket(int _id)
    {
        emblemeIndex = _id;

        if (_id < playerController.Stats.prestige)
        {
            SkinButton.interactable = true;
        }
        else
        {
            SkinButton.interactable = false;
        }

        skinID = _id;
    }

    public void SelectSkin()
    {
        int index = (skinID * -1) - 1;
        SM.SelectPrestigeSkin(index);
    }

    public void Prestige()
    {
        if (imageStage == 0)
            imageStage++;
        else if (imageStage == 1)
        {
            if (frameCount != 5)
            {
                imageStage++;
            }
        }
    }

    /// <summary>
    /// Close ShopWindow
    /// </summary>
    public void CloseShop(bool _sound)
    {
        ShopWindow.SetActive(false);
        //skinManager.DeselectTrack();

        if (_sound)
            buttonsound.Source.Play();
    }

    /// <summary>
    /// Close OptionsWindow
    /// </summary>
    public void CloseOptions(bool _sound)
    {
        ClosePrestigeInformationWindow(false);
        OptionsWindow.SetActive(false);

        imageStage = 0;

        if (_sound)
            buttonsound.Source.Play();
    }

    /// <summary>
    /// Open ShopWindow
    /// </summary>
    public void OpenShop(bool _sound)
    {
        CloseOptions(false);
        CloseCurrencyChangeWindow(false);
        CloseDataShop(false);

        // open ShopWindow
        ShopWindow.SetActive(true);

        if (_sound)
            buttonsound.Source.Play();
    }

    /// <summary>
    /// OpenOptionsWindow
    /// </summary>
    public void OpenOptions(bool _sound)
    {
        int score = (int)(20000f * (1f + GM.PC.Stats.prestige / 20f));
        PrestigeHeightText.text = "Prestige: Score > " + score;

        ClosePrestigeInformationWindow(false);
        CloseShop(false);
        CloseCurrencyChangeWindow(false);
        CloseDataShop(false);

        // open OptionsWindow
        OptionsWindow.SetActive(true);

        imageStage = 0;

        ChoosePrestigeRocket(emblemeIndex);

        if (playerController.Run)
        {
            AbortFlightButton.SetActive(true);
        }
        else
        {
            AbortFlightButton.SetActive(false);
        }

        if (_sound)
            buttonsound.Source.Play();
    }

    public void OpenColorpicker(bool _sound)
    {
        CloseShop(false);

        int x = ColorPaletteImage.sprite.texture.width;
        int y = ColorPaletteImage.sprite.texture.height;
        int grayx = GrayPaletteImage.sprite.texture.width;

        bool found = false;
        float h = 500f / 28f;

        SkinColor sc = new SkinColor();

        for (int i = 0; i < playerController.Stats.skinIDs.Count; i++)
        {
            if (playerController.Stats.skinIDs[i].id == playerController.Stats.activeSkin)
            {
                sc = playerController.Stats.skinIDs[i].color;
                break;
            }
        }

        Color c = new Color();

        for (int i = 0; i < y; i++)
        {
            for (int j = 0; j < x; j++)
            {
                // get selected color in the colorpicker image
                c = ColorPaletteImage.sprite.texture.GetPixel(j, i);

                if (CheckTrueColor(c, sc))
                {
                    Vector2 cursorPos = new Vector2()
                    {
                        x = (j - 14) * h - h / 2,
                        y = (i - 14) * h - h / 2
                    };

                    // set ColorCursor position
                    ColorCursor.localPosition = cursorPos;

                    // set color of ColorPickerCursor
                    float rgb = 1f - c.grayscale;
                    Color negativeColor = new Color(rgb, rgb, rgb, c.a);

                    ColorCursorImage.color = negativeColor;
                    GrayCursorImage.color = negativeColor;

                    found = true;

                    break;
                }
            }
            if (found) break;
        }

        // if we found the right true color
        if (found)
        {
            Color cGray = new Color()
            {
                r = c.grayscale,
                g = c.grayscale,
                b = c.grayscale,
                a = 1
            };

            for (int i = 0; i < grayx; i++)
            {
                Color color = Color.LerpUnclamped(cGray, c, i / 28f);
                GrayPaletteImage.sprite.texture.SetPixel(i, 0, color);
            }
            GrayPaletteImage.sprite.texture.Apply();

            Vector2 cursorPos = new Vector2()
            {
                x = (sc.grayIndex - 14) * h - h / 2,
                y = 0
            };
            // set ColorCursor position
            GrayCursor.localPosition = cursorPos;

            SM.grayIndex = sc.grayIndex;
        }

        // if we didnt found the right color
        else if (!found)
        {
            ColorCursor.localPosition = new Vector2(-13 * h - h / 2, 14 * h - h / 2);
            ColorCursorImage.color = Color.black;

            GrayCursor.localPosition = new Vector2(14 * h - h / 2, 0);
            GrayCursorImage.color = Color.black;

            for (int i = 0; i < GrayPaletteImage.sprite.texture.width; i++)
            {
                GrayPaletteImage.sprite.texture.SetPixel(i, 0, Color.white);
            }
            GrayPaletteImage.sprite.texture.Apply();

            SM.grayIndex = 27;
        }

        ColorPicker.SetActive(true);

        if (_sound)
            buttonsound.Source.Play();
    }

    public void CloseColorpicker(bool _sound)
    {
        ColorPicker.SetActive(false);

        // if currentskin has colorable thrust
        if (SM.Skins[playerController.Stats.skinIDs[SM.skinid].id].colorableThrust)
        {
            // modifire particlesystems
            for (int i = 0; i < playerController.FireParticles.Length; i++)
            {
                if (SM.Skins[playerController.Stats.skinIDs[SM.skinid].id].timeBased)
                {
                    ParticleSystem ps = playerController.FireParticles[i].GetComponentInChildren<ParticleSystem>();
                    ps.Stop();
                    var psmain = ps.main;

                    ParticleSystem.ColorOverLifetimeModule pcol = ps.colorOverLifetime;
                    pcol.color = SM.Skins[playerController.Stats.skinIDs[SM.skinid].id].particleColor;

                    if (SM.Skins[playerController.Stats.skinIDs[SM.skinid].id].colorableThrust)
                    {
                        Gradient gradient = new Gradient();
                        GradientColorKey[] colorkeys = new GradientColorKey[pcol.color.gradient.colorKeys.Length];
                        for (int j = 0; j < pcol.color.gradient.colorKeys.Length; j++)
                        {
                            Color c = new Color()
                            {
                                r = playerController.Stats.skinIDs[SM.skinid].color.r,
                                g = playerController.Stats.skinIDs[SM.skinid].color.g,
                                b = playerController.Stats.skinIDs[SM.skinid].color.b,
                                a = playerController.Stats.skinIDs[SM.skinid].color.a
                            };

                            GradientColorKey key = new GradientColorKey()
                            {
                                color = Color.Lerp(pcol.color.gradient.colorKeys[j].color, c, SM.Skins[playerController.Stats.skinIDs[SM.skinid].id].colorableThrustStrength),
                                time = pcol.color.gradient.colorKeys[j].time
                            };

                            colorkeys[j] = key;
                        }
                        gradient.SetKeys(colorkeys, pcol.color.gradient.alphaKeys);
                        pcol.color = gradient;
                    }

                    pcol.enabled = true;

                    ParticleSystem.MinMaxGradient g = ps.main.startColor;
                    g.gradient = new Gradient();

                    psmain.startColor = g;
                    ps.Play();
                }
                else
                {
                    ParticleSystem ps = playerController.FireParticles[i].GetComponentInChildren<ParticleSystem>();
                    ps.Stop();
                    var psmain = ps.main;

                    ParticleSystem.ColorOverLifetimeModule pcol = ps.colorOverLifetime;
                    pcol.color = SM.Skins[playerController.Stats.skinIDs[SM.skinid].id].particleColor;
                    pcol.enabled = false;

                    ParticleSystem.MinMaxGradient g = ps.main.startColor;
                    g.gradient = SM.Skins[playerController.Stats.skinIDs[SM.skinid].id].particleColor;

                    psmain.startColor = g;
                    ps.Play();
                }
            }
        }

        if (_sound)
            buttonsound.Source.Play();
    }

    public void Pause(bool _sound)
    {
        if (Time.timeScale == 0)
        {
            PauseImage.sprite = PauseSprite;
            Time.timeScale = 1;

            playerController.rocketFlight.Source.Play();
        }
        else
        {
            PauseImage.sprite = PlaySprite;
            Time.timeScale = 0;

            playerController.rocketFlight.Source.Pause();
        }

        if (_sound)
            buttonsound.Source.Play();

        CloseOptions(false);
    }

    #endregion

    [Header("ShopTabs")]
    #region ShopTabs

    /// <summary>
    /// List of all TabButtons
    /// </summary>
    public Image[] TabButtons;

    /// <summary>
    /// Tint of TabButton when active
    /// </summary>
    public Color TabActiveTint;

    /// <summary>
    /// Tint of TabButton when deactive
    /// </summary>
    public Color TabDeactiveTint;

    /// <summary>
    /// Window for buying Data
    /// </summary>
    public GameObject DataShopWindow;

    /// <summary>
    /// Window for changeing Currency
    /// </summary>
    public GameObject CurrencyChangeWindow;

    /// <summary>
    /// Window for ColorPicker
    /// </summary>
    public GameObject ColorPicker;

    /// <summary>
    /// Tab for Upgrades
    /// </summary>
    public GameObject Upgrade;

    /// <summary>
    /// Tab for Skins
    /// </summary>
    public GameObject Skin;

    /// <summary>
    /// Tab for Researches
    /// </summary>
    public GameObject Research;

    /// <summary>
    /// Playercontroller
    /// </summary>
    public PlayerController playerController;

    /// <summary>
    /// All Buttons for Upgrades
    /// </summary>
    public Button[] UpgradeButtons;

    /// <summary>
    /// All texts for Upgrades
    /// </summary>
    public Text[] UpgradeTexts;

    /// <summary>
    /// All texts for amount of Upgrades
    /// </summary>
    public Text[] UpgradeAmountTexts;

    /// <summary>
    /// All Buttons for Research
    /// </summary>
    public Button[] ResearchButtons;

    /// <summary>
    /// All texts for Research
    /// </summary>
    public Text[] ResearchTexts;

    /// <summary>
    /// All texts for amount of Research
    /// </summary>
    public Text[] ResearchAmountTexts;

    [HideInInspector]
    /// <summary>
    /// sound for button
    /// </summary>
    public Sound buttonsound;

    /// <summary>
    /// Close UpgradesTab
    /// </summary>
    private void CloseUpgrade(bool _sound)
    {
        Upgrade.SetActive(false);

        // set tint
        TabButtons[0].color = TabDeactiveTint;

        if (_sound)
            buttonsound.Source.Play();
    }

    /// <summary>
    /// Close SkinsTab
    /// </summary>
    private void CloseSkin(bool _sound)
    {
        Skin.SetActive(false);

        // set tint
        TabButtons[1].color = TabDeactiveTint;

        if (_sound)
            buttonsound.Source.Play();
    }

    /// <summary>
    /// Close ResearchTab
    /// </summary>
    private void CloseResearch(bool _sound)
    {
        Research.SetActive(false);

        // set tint
        TabButtons[2].color = TabDeactiveTint;

        if (_sound)
            buttonsound.Source.Play();
    }

    /// <summary>
    /// Open UpgradesTab
    /// </summary>
    public void OpenUpgrade(bool _sound)
    {
        // open UpgradesTab
        Upgrade.SetActive(true);

        // set tint
        TabButtons[0].color = TabActiveTint;

        // close all other Tabs
        CloseResearch(false);
        CloseSkin(false);

        if (_sound)
            buttonsound.Source.Play();
    }

    /// <summary>
    /// Open SkinsTab
    /// </summary>
    public void OpenSkin(bool _sound)
    {
        // open SkinsTab
        Skin.SetActive(true);

        // set tint
        TabButtons[1].color = TabActiveTint;

        // close all other tabs
        CloseResearch(false);
        CloseUpgrade(false);

        if (_sound)
            buttonsound.Source.Play();
    }

    /// <summary>
    /// Open ResearchTab
    /// </summary>
    public void OpenResearch(bool _sound)
    {
        // open ResearchTab
        Research.SetActive(true);

        // set tint
        TabButtons[2].color = TabActiveTint;

        // close all other tabs
        CloseSkin(false);
        CloseUpgrade(false);

        if (_sound)
            buttonsound.Source.Play();
    }

    /// <summary>
    /// open Currency change window
    /// </summary>
    public void OpenCurrencyChangeWindow(bool _sound)
    {
        CurrencyChangeWindow.SetActive(true);
        CloseDataShop(false);
        CloseOptions(false);
        CloseShop(false);

        if (_sound)
            buttonsound.Source.Play();
    }

    /// <summary>
    /// close currency change window
    /// </summary>
    public void CloseCurrencyChangeWindow(bool _sound)
    {
        CurrencyChangeWindow.SetActive(false);

        if (_sound)
            buttonsound.Source.Play();
    }

    /// <summary>
    /// Open Datashop Window
    /// </summary>
    public void OpenDataShop(bool _sound)
    {
        DataShopWindow.SetActive(true);
        CloseOptions(false);
        CloseShop(false);
        CloseCurrencyChangeWindow(false);

        if (_sound)
            buttonsound.Source.Play();
    }

    /// <summary>
    ///  Close Datashop window
    /// </summary>
    public void CloseDataShop(bool _sound)
    {
        DataShopWindow.SetActive(false);

        if (_sound)
            buttonsound.Source.Play();
    }

    #endregion

    [Header("GameOver")]
    /// <summary>
    /// Gameover Window
    /// </summary>
    public Transform GameOverScreen;

    /// <summary>
    /// Gameover Window Fade Speed
    /// </summary>
    public float GOSSpeed;

    [Header("CurrencyBar")]
    /// <summary>
    /// Topbar (Currencybar)
    /// </summary>
    public Transform TopBar;

    /// <summary>
    /// Topbar speed
    /// </summary>
    public float TBSpeed;

    [Header("Pause Button")]
    /// <summary>
    /// Pause-Button
    /// </summary>
    public GameObject PauseButton;

    public Image PauseImage;

    public Sprite PauseSprite;
    public Sprite PlaySprite;

    private UpgradeSystem upgradeSystem;

    private SkinManager skinManager;

    private void Start()
    {
        skinID = 0;

        GM = FindObjectOfType<GameManager>();

        SM = FindObjectOfType<SkinManager>();

        AudioManager AM = FindObjectOfType<AudioManager>();

        buttonsound = Array.Find(AM.Sound, sound => sound.Name == "Button");

        upgradeSystem = FindObjectOfType<UpgradeSystem>();

        emblemeIndex = 0;

        SatelliteAmount.text = playerController.Stats.Currency.satellite.ToString();
        DataAmount.text = playerController.Stats.Currency.data.ToString();

        skinManager = FindObjectOfType<SkinManager>();
    }

    private void Update()
    {
        // if offlinebonuswindow is open
        if (OfflineBonusPanel.activeSelf)
        {
            // disable all windowbuttons
            for (int i = 0; i < WindowButtons.Length; i++)
            {
                WindowButtons[i].interactable = false;
            }
        } // if the offlinebonuswindow is closed and all wintowbuttons arent interactable
        else if (WindowButtons[0].interactable == false)
        {
            // activate all windowbuttons
            for (int i = 0; i < WindowButtons.Length; i++)
            {
                WindowButtons[i].interactable = true;
            }
        }


        if (!playerController.Run)
        {
            if (GameOverScreen.position.y < 960)
            {
                if (GameOverScreen.position.y + GOSSpeed * Time.deltaTime > 960)
                    GameOverScreen.position = new Vector3(540, 960, 0);
                else
                    GameOverScreen.position = new Vector3(540, GameOverScreen.position.y + GOSSpeed * Time.deltaTime, 0);
            }
            if (TopBar.position.y > 1860)
            {
                PauseButton.SetActive(false);

                if (TopBar.position.y - TBSpeed * Time.deltaTime < 1860)
                    TopBar.position = new Vector3(540, 1860, 0);
                else
                    TopBar.position = new Vector3(540, TopBar.position.y - TBSpeed * Time.deltaTime, 0);
            }

            if (ShopWindow.activeSelf)
            {
                // go through all upgrades
                for (int i = 0; i < playerController.Stats.Upgrades.Length; i++)
                {
                    if (upgradeSystem.Limits[i].Amount > 0)
                    {
                        if (playerController.Stats.Upgrades[i].amount >= upgradeSystem.Limits[i].Amount)
                        {
                            UpgradeTexts[i].text = "Max Reached";

                            UpgradeAmountTexts[i].text = playerController.Stats.Upgrades[i].amount.ToString();

                            // otherwise false
                            UpgradeButtons[i].interactable = false;

                            continue;
                        }
                    }

                    UpgradeAmountTexts[i].text = playerController.Stats.Upgrades[i].amount.ToString();

                    // if max upgrades are reached
                    if (playerController.Stats.Upgrades[i].amount >= playerController.Stats.Upgrades[i].maxAmount)
                    {
                        UpgradeTexts[i].text = "Research needed";

                        // otherwise false
                        UpgradeButtons[i].interactable = false;
                    }
                    else
                    {
                        // get cost of current upgrade
                        int cost = playerController.Stats.Upgrades[i].cost;

                        // update the UI text for it
                        UpgradeTexts[i].text = cost.ToString();

                        // if player can buy it
                        if (playerController.Stats.Currency.data >= cost)
                        {

                            // set it true
                            UpgradeButtons[i].interactable = true;
                        }
                        else
                        {
                            // otherwise false
                            UpgradeButtons[i].interactable = false;
                        }
                    }
                }

                // go through all researches
                for (int i = 0; i < playerController.Stats.Researches.Length; i++)
                {
                    if (upgradeSystem.Limits[i].Amount > 0)
                    {
                        int limit = (int)Math.Ceiling(upgradeSystem.Limits[i].Amount / 10f);
                        if (playerController.Stats.Researches[i].amount >= limit)
                        {
                            ResearchTexts[i].text = "Max Reached";

                            ResearchAmountTexts[i].text = playerController.Stats.Researches[i].amount.ToString();

                            // otherwise false
                            ResearchButtons[i].interactable = false;

                            continue;
                        }
                    }

                    // get cost of current research
                    int cost = playerController.Stats.Researches[i].cost;

                    // update the UI text for it
                    ResearchTexts[i].text = cost.ToString();

                    ResearchAmountTexts[i].text = playerController.Stats.Researches[i].amount.ToString();

                    // if player can buy it
                    if (playerController.Stats.Currency.satellite >= cost)
                    {
                        // set it true
                        ResearchButtons[i].interactable = true;
                    }
                    else
                    {
                        // otherwise false
                        ResearchButtons[i].interactable = false;
                    }
                }
            }
        }
        else
        {
            GameOverScreen.position = new Vector3(540, -740, 0);

            if (TopBar.position.y < 2060)
            {
                if (TopBar.position.y - TBSpeed * Time.deltaTime > 2060)
                {
                    TopBar.position = new Vector3(540, 2060, 0);
                }
                else
                    TopBar.position = new Vector3(540, TopBar.position.y + TBSpeed * Time.deltaTime, 0);
            }
            else
            {
                PauseButton.SetActive(true);
            }
        }

        if (OptionsWindow.activeSelf)
        {
            if (playerController.Stats.isPrestigeable && !playerController.Run)
            {
                PrestigeButton.interactable = true;
            }
            else
            {
                PrestigeButton.interactable = false;
            }

            for (int i = 0; i < PrestigeEmbleme.Length; i++)
            {
                if (i < playerController.Stats.prestige)
                {
                    PrestigeEmbleme[i].color = Color.white;
                }
                else
                {
                    PrestigeEmbleme[i].color = GrayColor;
                }
            }

            if (imageStage == 0)
            {
                frameCount = 0;
                PrestigeButtonImage.sprite = PrestigeButtonImages[frameCount];
            }
            else if (imageStage == 1)
            {

                if (FrameCounter > 0)
                {
                    if (frameCount < 6)
                    {
                        // play animation
                        if (frameTimer >= FrameCounter)
                        {
                            // reduce timer
                            frameTimer -= FrameCounter;

                            // set new sprite
                            PrestigeButtonImage.sprite = PrestigeButtonImages[frameCount];

                            // increase counter
                            frameCount++;
                        }
                        else
                        {
                            frameTimer += Time.deltaTime;
                        }
                    }
                }
            }
            else
            {
                if (FrameCounter > 0)
                {
                    if (frameCount < PrestigeButtonImages.Length)
                    {
                        // play animation
                        if (frameTimer >= FrameCounter)
                        {
                            // reduce timer
                            frameTimer -= FrameCounter;

                            // set new sprite
                            PrestigeButtonImage.sprite = PrestigeButtonImages[frameCount];

                            // increase counter
                            frameCount++;
                        }
                        else
                        {
                            frameTimer += Time.deltaTime;
                        }
                    }
                }
            }

            if (skinID < playerController.Stats.prestige)
            {
                PrestigeRocketSkin.color = Color.white;
                PrestigeRocketSkinBack.color = Color.white;
                if (!playerController.Run)
                {
                    SkinButton.interactable = true;
                }
                else
                {
                    SkinButton.interactable = false;
                }
            }
            else
            {
                PrestigeRocketSkin.color = GrayColor;
                PrestigeRocketSkinBack.color = GrayColor;
                SkinButton.interactable = false;
            }
        }

        PrestigeRocketSkin.sprite = SM.PrestigeSkins[emblemeIndex].skin;
        PrestigeRocketSkinBack.sprite = SM.PrestigeSkins[emblemeIndex].skin_back;

        if (imageStage == 2)
        {
            if (frameCount == PrestigeButtonImages.Length - 1)
            {
                GM.PrestigePlayer();
                imageStage++;
            }
        }

    }

    /// <summary>
    /// return true, when color is equal with true SkinColor
    /// </summary>
    /// <param name="_c">PixelColor of ColorPicker</param>
    /// <param name="_sc">SkinColor of activeSkin</param>
    /// <returns></returns>
    private bool CheckTrueColor(Color _c, SkinColor _sc)
    {
        if (_c.r != _sc.true_r) return false;
        if (_c.g != _sc.true_g) return false;
        if (_c.b != _sc.true_b) return false;
        if (_c.a != _sc.true_a) return false;

        return true;
    }

    /// <summary>
    /// return true, when color is equal with used SkinColor
    /// </summary>
    /// <param name="_c">PixelColor of GrayScale</param>
    /// <param name="_sc">SkinColor of activeSkin</param>
    /// <returns></returns>
    private bool CheckColor(Color _c, SkinColor _sc)
    {
        if (_c.r != _sc.r) return false;
        if (_c.g != _sc.g) return false;
        if (_c.b != _sc.b) return false;
        if (_c.a != _sc.a) return false;

        return true;
    }
}
