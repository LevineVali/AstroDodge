using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.Purchasing;

public class SkinManager : MonoBehaviour
{
    /// <summary>
    /// Window for Skinshop
    /// </summary>
    public GameObject SkinWindow;

    /// <summary>
    /// Image of the activeSkin
    /// </summary>
    public SpriteRenderer PlayerActiveSkin;

    /// <summary>
    /// Tint of SkinButton when active
    /// </summary>
    public Color SkinActiveTint;

    /// <summary>
    /// Tint of SkinButton when deactive
    /// </summary>
    public Color SkinDeactiveTint;

    /// <summary>
    /// Tint of tracked-icon
    /// </summary>
    public Color TrackSelectedTint;

    /// <summary>
    /// Skins
    /// </summary>
    public RocketSkin[] Skins;

    /// <summary>
    /// List of all main ui elements for each skin
    /// </summary>
    public GameObject[] Skin_UI_Object;

    /// <summary>
    /// Buttons for buying/selecting
    /// </summary>
    public Button[] Buttons;

    /// <summary>
    /// array of all SkinPreviews
    /// </summary>
    public Image[] SkinPreviews;

    /// <summary>
    /// array of all Backs of the skins Previews
    /// </summary>
    public Image[] SkinBackPreviews;

    /// <summary>
    /// List of all currencyicons in shopwindow
    /// </summary>
    public Image[] CurrencyIcons_Shop;

    /// <summary>
    /// List of all Texts to show cost or amount for collectables
    /// </summary>
    public Text[] AmountTexts;

    /// <summary>
    /// List of all Images
    /// </summary>
    public Image[] AmountImages;

    /// <summary>
    /// List of all different icons
    /// </summary>
    public Sprite[] CurrencyIcons;

    /// <summary>
    /// list of all tracked icons
    /// </summary>
    public GameObject[] Tracked_Icons;

    /// <summary>
    /// list of all tracked button
    /// </summary>
    public Button[] Tracked_Buttons;

    [Header("Prestige")]
    /// <summary>
    /// Prestigeskins
    /// </summary>
    public RocketSkin[] PrestigeSkins;

    #region ColorPicker
    /// <summary>
    /// Colorpicker-Window
    /// </summary>
    [Header("ColorPicker")]
    public GameObject ColorPicker;

    /// <summary>
    /// Rect of Background
    /// </summary>
    public RectTransform Background;

    /// <summary>
    /// Rect of Colorpalette
    /// </summary>
    public RectTransform ColorPalette;

    /// <summary>
    /// RawImage of Colopalette
    /// </summary>
    public Image ColorPaletteImage;

    /// <summary>
    /// Rect of Colorcursor
    /// </summary>
    public RectTransform ColorCursor;

    /// <summary>
    /// Image of ColorCursor
    /// </summary>
    public Image ColorCursorImage;

    /// <summary>
    /// Button to open ColorPickerWindow
    /// </summary>
    public GameObject ColorPickerPanel;

    [Space(15)]
    /// <summary>
    /// Image of ColorCursor
    /// </summary>
    public Image GrayCursorImage;

    /// <summary>
    /// Rect of Colorcursor
    /// </summary>
    public RectTransform GrayCursor;

    /// <summary>
    /// RawImage of Colopalette
    /// </summary>
    public Image GrayPaletteImage;

    /// <summary>
    /// Rect of Colorpalette
    /// </summary>
    public RectTransform GrayPalette;

    private GraphicRaycaster raycaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;
    #endregion

    /// <summary>
    /// Stats of the player
    /// </summary>
    private PlayerController pc;

    /// <summary>
    /// sound of button
    /// </summary>
    private Sound buttonsound;

    /// <summary>
    /// is the game new started?
    /// </summary>
    private bool isStarted = false;

    /// <summary>
    /// index for grayPalette
    /// </summary>
    [HideInInspector]
    public int grayIndex = 0;

    // tracking stuff
    private List<CollactableTrack> tracks = new List<CollactableTrack>();

    // animation stuff
    private float timer_front;
    private float timer_back;

    private int count_front;
    private int count_back;

    private float count_front_hard_value;
    private float count_back_hard_value;
    private int count_front_hard;
    private int count_back_hard;

    /// <summary>
    /// Index for skinIDs in playerstats
    /// </summary>
    [HideInInspector]
    public int skinid;
    // Start is called before the first frame update

    /// <summary>
    /// UIManager
    /// </summary>
    private UIManager uiManager;

    /// <summary>
    /// SpawnManager
    /// </summary>
    private SpawnManager spawnManager;

    /// <summary>
    /// is game restart?
    /// </summary>
    public bool Reset;

    #region TimeSpecific Skins

    public bool PresentRocket_IsAvailable;
    public bool FireworkRocket_IsAvailable;

    #endregion

    void Start()
    {
        // sort Skins-List
        SortSkinList();

        // get uimanager
        uiManager = FindObjectOfType<UIManager>();

        // get spawnmanager
        spawnManager = FindObjectOfType<SpawnManager>();

        // get playerstats
        pc = FindObjectOfType<PlayerController>();

        #region TimeSpecific Skins

        PresentRocket_IsAvailable = false;
        FireworkRocket_IsAvailable = false;
        int presentRocket_ID = 0;
        int fireworkRocket_ID = 0;

        for (int i = 0; i < Skins.Length; i++)
        {
            if (Skins[i].currencyType == CurrencyType.PRESENT)
            {
                presentRocket_ID = i;
            }
            if (Skins[i].currencyType == CurrencyType.FIREWORK)
            {
                fireworkRocket_ID = i;
            }
        }

        if (presentRocket_ID > 0)
        {
            for (int i = 0; i < pc.Stats.skinIDs.Count; i++)
            {
                if (pc.Stats.skinIDs[i].id == Skins[presentRocket_ID].ID)
                    PresentRocket_IsAvailable = true;

            }
            if (!PresentRocket_IsAvailable)
            {
                if (pc.Stats.tracks.Contains(Track.PRESENT))
                    pc.Stats.tracks.Remove(Track.PRESENT);
            }
        }
        if (fireworkRocket_ID > 0)
        {
            for (int i = 0; i < pc.Stats.skinIDs.Count; i++)
            {
                if (pc.Stats.skinIDs[i].id == Skins[fireworkRocket_ID].ID)
                    FireworkRocket_IsAvailable = true;

            }
            if (!FireworkRocket_IsAvailable)
            {
                if (pc.Stats.tracks.Contains(Track.FIREWORK))
                    pc.Stats.tracks.Remove(Track.FIREWORK);
            }
        }

        if (!PresentRocket_IsAvailable)
        {
            if (DateTime.Now.Month == 12 && DateTime.Now.Day > 16)
            {
                PresentRocket_IsAvailable = true;
            }
            else
            {
                PresentRocket_IsAvailable = false;
            }
        }
        if (!FireworkRocket_IsAvailable)
        {
            if (DateTime.Now.Month == 1 && DateTime.Now.Day < 15)
            {
                FireworkRocket_IsAvailable = true;
            }
            else
            {
                FireworkRocket_IsAvailable = false;
            }
        }

        #endregion

        // add buttonevent with right parameters
        for (int i = 0; i < Skins.Length; i++)
        {
            int i2 = i;
            Buttons[i2].onClick.AddListener(() => SelectSkin(Skins[i2].ID));
            Buttons[i2].enabled = true;
            Tracked_Buttons[i2].enabled = true;

            if (Skins[i2].collect)
            {
                if (CheckCurrencyType(Skins[i2].currencyType))
                {
                    Tracked_Buttons[i2].enabled = true;
                    Tracked_Buttons[i2].onClick.AddListener(() => TrackCollectable(Skins[i2].currencyType, Skins[i2].ID));

                    switch (Skins[i2].currencyType)
                    {
                        case CurrencyType.UFO:
                            if (pc.Stats.ufoCount < Skins[i2].cost)
                            {
                                if (pc.Stats.tracks.Contains(Track.UFO))
                                    SetTrackCollectable(CurrencyType.UFO, Skins[i2].ID);
                            }
                            else
                            {
                                Tracked_Buttons[i2].enabled = false;
                                Tracked_Icons[i2].SetActive(false);
                            }
                            break;
                        case CurrencyType.DUCK:
                            if (pc.Stats.duckCount < Skins[i2].cost)
                            {
                                if (pc.Stats.tracks.Contains(Track.DUCK))
                                    SetTrackCollectable(CurrencyType.DUCK, Skins[i2].ID);
                            }
                            else
                            {
                                Tracked_Buttons[i2].enabled = false;
                                Tracked_Icons[i2].SetActive(false);
                            }
                            break;
                        case CurrencyType.BURGER:
                            if (pc.Stats.burgerCount < Skins[i2].cost)
                            {
                                if (pc.Stats.tracks.Contains(Track.BURGER))
                                    SetTrackCollectable(CurrencyType.BURGER, Skins[i2].ID);
                            }
                            else
                            {
                                Tracked_Buttons[i2].enabled = false;
                                Tracked_Icons[i2].SetActive(false);
                            }
                            break;
                        case CurrencyType.SANDWICH:
                            if (pc.Stats.sandwichCount < Skins[i2].cost)
                            {
                                if (pc.Stats.tracks.Contains(Track.SANDWICH))
                                    SetTrackCollectable(CurrencyType.SANDWICH, Skins[i2].ID);
                            }
                            else
                            {
                                Tracked_Buttons[i2].enabled = false;
                                Tracked_Icons[i2].SetActive(false);
                            }
                            break;
                        case CurrencyType.PRESENT:
                            if (pc.Stats.presentCount < Skins[i2].cost)
                            {
                                if (pc.Stats.tracks.Contains(Track.PRESENT) && PresentRocket_IsAvailable)
                                    SetTrackCollectable(CurrencyType.PRESENT, Skins[i2].ID);
                            }
                            else
                            {
                                Tracked_Buttons[i2].enabled = false;
                                Tracked_Icons[i2].SetActive(false);
                            }
                            break;
                        case CurrencyType.TEDDY:
                            if (pc.Stats.teddyCount < Skins[i2].cost)
                            {
                                if (pc.Stats.tracks.Contains(Track.TEDDY))
                                    SetTrackCollectable(CurrencyType.TEDDY, Skins[i2].ID);
                            }
                            else
                            {
                                Tracked_Buttons[i2].enabled = false;
                                Tracked_Icons[i2].SetActive(false);
                            }
                            break;
                        case CurrencyType.FIREWORK:
                            if (pc.Stats.fireworksCount < Skins[i2].cost)
                            {
                                if (pc.Stats.tracks.Contains(Track.FIREWORK))
                                    SetTrackCollectable(CurrencyType.FIREWORK, Skins[i2].ID);
                            }
                            else
                            {
                                Tracked_Buttons[i2].enabled = false;
                                Tracked_Icons[i2].SetActive(false);
                            }
                            break;
                        case CurrencyType.WATER:
                            if (pc.Stats.waterCount < Skins[i2].cost)
                            {
                                if (pc.Stats.tracks.Contains(Track.WATER))
                                    SetTrackCollectable(CurrencyType.WATER, Skins[i2].ID);
                            }
                            else
                            {
                                Tracked_Buttons[i2].enabled = false;
                                Tracked_Icons[i2].SetActive(false);
                            }
                            break;
                        case CurrencyType.SODA:
                            if (pc.Stats.sodaCount < Skins[i2].cost)
                            {
                                if (pc.Stats.tracks.Contains(Track.SODA))
                                    SetTrackCollectable(CurrencyType.SODA, Skins[i2].ID);
                            }
                            else
                            {
                                Tracked_Buttons[i2].enabled = false;
                                Tracked_Icons[i2].SetActive(false);
                            }
                            break;
                    }
                }
                else
                {
                    Tracked_Buttons[i2].enabled = false;
                    Tracked_Icons[i2].SetActive(false);
                }
            }
            else
            {
                Tracked_Buttons[i2].enabled = false;
                Tracked_Icons[i2].SetActive(false);
            }
        }

        // get graphicraycaster-component
        raycaster = GetComponent<GraphicRaycaster>();

        // get eventsystem
        eventSystem = GetComponent<EventSystem>();

        for (int i = 0; i < SkinPreviews.Length; i++)
        {
            // set Skinpreview
            SkinPreviews[i].sprite = Skins[i].skin;

            // if current skin has a back
            if (Skins[i].back)
            {
                // set Skinback preview
                SkinBackPreviews[i].sprite = Skins[i].skin_back;
            }
            // otherwise hide skinback preview
            else
            {
                SkinBackPreviews[i].color = new Color(0, 0, 0, 0);
            }

            if (Skins[i].currencyType != CurrencyType.UNLOCKABLE)
            {
                CurrencyIcons_Shop[i].sprite = CurrencyIcons[(int)(Skins[i].currencyType) - 1];
            }
            else
            {
                CurrencyIcons_Shop[i].gameObject.SetActive(false);
            }

            for (int j = 0; j < pc.Stats.skinIDs.Count; j++)
            {
                if (pc.Stats.skinIDs[j].id == i)
                {
                    Skins[i].bought = true;
                }
            }
        }

        SelectSkin(pc.Stats.activeSkin);
        isStarted = true;

        AudioManager AM = FindObjectOfType<AudioManager>();

        buttonsound = Array.Find(AM.Sound, sound => sound.RealName == "Button");
    }

    // Update is called once per frame
    void Update()
    {
        if (Reset)
        {
            timer_front = 0;
            timer_back = 0;
            count_front = 0;
            count_back = 0;
            count_front_hard_value = 0;
            count_back_hard_value = 0;
            count_front_hard = 0;
            count_back_hard = 0;

            Reset = false;
        }

        if (!pc.Run)
        {
            if (SkinWindow.activeSelf)
            {
                // go throught all Skins
                for (int i = 0; i < AmountTexts.Length; i++)
                {
                    bool check = false;

                    // if player has the current skin
                    for (int j = 0; j < pc.Stats.skinIDs.Count; j++)
                    {
                        if (pc.Stats.skinIDs[j].id == i)
                        {
                            if (i == pc.Stats.activeSkin)
                                AmountTexts[i].text = "Selected";
                            else
                                AmountTexts[i].text = "Available";

                            // set bought bool
                            Skins[i].bought = true;
                            // hide currencyicon
                            CurrencyIcons_Shop[i].color = new Color(1, 1, 1, 0);
                            // set SkinPreviewcolor
                            Color c = new Color(pc.Stats.skinIDs[j].color.r, pc.Stats.skinIDs[j].color.g, pc.Stats.skinIDs[j].color.b, pc.Stats.skinIDs[j].color.a);
                            SkinPreviews[i].color = c;

                            check = true;
                        }
                    }
                    // otherwise
                    if (!check)
                    {
                        string amount = "non";

                        switch (Skins[i].currencyType)
                        {
                            case CurrencyType.COMET:
                                amount = pc.Stats.cometCount.ToString();
                                break;
                            case CurrencyType.ASTEROID:
                                amount = pc.Stats.asteroidCount.ToString();
                                break;
                            case CurrencyType.BIRD:
                                amount = pc.Stats.birdCount.ToString();
                                break;
                            case CurrencyType.COOKED_BIRD:
                                amount = pc.Stats.cookedbirdCount.ToString();
                                break;
                            case CurrencyType.CLOUD:
                                amount = pc.Stats.cloudCount.ToString();
                                break;
                            case CurrencyType.DUCK:
                                amount = pc.Stats.duckCount.ToString();
                                break;
                            case CurrencyType.FIREWORK:
                                if (FireworkRocket_IsAvailable)
                                    amount = pc.Stats.fireworksCount.ToString();
                                else
                                {
                                    AmountTexts[i].text = "01.01-14.01   ";
                                    Buttons[i].enabled = false;
                                    Tracked_Buttons[i].enabled = false;
                                    continue;
                                }
                                break;
                            case CurrencyType.UFO:
                                amount = pc.Stats.ufoCount.ToString();
                                break;
                            case CurrencyType.BURGER:
                                amount = pc.Stats.burgerCount.ToString();
                                break;
                            case CurrencyType.SANDWICH:
                                amount = pc.Stats.sandwichCount.ToString();
                                break;
                            case CurrencyType.PRESENT:
                                if (PresentRocket_IsAvailable)
                                    amount = pc.Stats.presentCount.ToString();
                                else
                                {
                                    AmountTexts[i].text = "17.12-31.12   ";
                                    Buttons[i].enabled = false;
                                    Tracked_Buttons[i].enabled = false;
                                    continue;
                                }
                                break;
                            case CurrencyType.TEDDY:
                                amount = pc.Stats.teddyCount.ToString();
                                break;
                            case CurrencyType.ELECTRIC:
                                amount = pc.Stats.electricCount.ToString();
                                break;
                            case CurrencyType.WATER:
                                amount = pc.Stats.waterCount.ToString();
                                break;
                            case CurrencyType.SODA:
                                amount = pc.Stats.sodaCount.ToString();
                                break;
                            default:
                                break;
                        }

                        if (Skins[i].currencyType == CurrencyType.UNLOCKABLE)
                        {
                            CurrencyIcons_Shop[i].color = new Color(1, 1, 1, 1);
                            AmountTexts[i].text = "Secret";
                        }
                        else if (Skins[i].collect)
                        {
                            CurrencyIcons_Shop[i].color = new Color(1, 1, 1, 1);
                            AmountTexts[i].text = amount + "/" + Skins[i].cost.ToString();
                        }
                        else
                        {
                            CurrencyIcons_Shop[i].color = new Color(1, 1, 1, 1);
                            AmountTexts[i].text = Skins[i].cost.ToString();
                        }
                    }
                }

                // get color
                Color currentColor = PlayerActiveSkin.color;

                SkinColor currentSkinColor = new SkinColor();

                for (int i = 0; i < pc.Stats.skinIDs.Count; i++)
                {
                    if (pc.Stats.skinIDs[i].id == pc.Stats.activeSkin)
                    {
                        currentSkinColor = pc.Stats.skinIDs[i].color;
                        break;
                    }
                    currentSkinColor = new SkinColor()
                    {
                        r = 1f,
                        g = 1f,
                        b = 1f,
                        a = 1f,
                        true_r = 1f,
                        true_g = 1f,
                        true_b = 1f,
                        true_a = 1f
                    };
                }

                // if PlayerActiveSkin-Color has changed
                if (currentColor.r != currentSkinColor.r ||
                    currentColor.g != currentSkinColor.g ||
                    currentColor.b != currentSkinColor.b ||
                    currentColor.a != currentSkinColor.a)
                {
                    PlayerActiveSkin.color = new Color(currentSkinColor.r, currentSkinColor.g, currentSkinColor.b, currentSkinColor.a);
                }

                // select Color from Colorpicker
                if (ColorPicker.activeSelf)
                {
                    if (Input.GetMouseButton(0))
                    {
                        // set up the new pointer event
                        pointerEventData = new PointerEventData(eventSystem);

                        // set pointer event position
                        pointerEventData.position = Input.mousePosition;

                        // create a list of raycast results
                        List<RaycastResult> results = new List<RaycastResult>();

                        // raycast using graphics raycaster and mouse click position
                        raycaster.Raycast(pointerEventData, results);

                        foreach (RaycastResult result in results)
                        {
                            if (result.gameObject.name == "Colorpalette")
                            {
                                // calculate imageposition
                                Vector2 pos = new Vector2()
                                {
                                    x = result.screenPosition.x - (1080 / 2) - Background.localPosition.x - ColorPalette.transform.localPosition.x,
                                    y = result.screenPosition.y - (1920 / 2) - Background.localPosition.y - ColorPalette.transform.localPosition.y
                                };

                                float h = 500f / 28f;

                                // get index for colorpicker_image
                                int pixelX = (int)(pos.x / h + .5f);
                                int pixelY = (int)(pos.y / h + .5f);

                                Vector2 cursorPos = new Vector2()
                                {
                                    x = pixelX * h - h / 2,
                                    y = pixelY * h - h / 2
                                };

                                // set ColorCursor position
                                ColorCursor.localPosition = cursorPos;

                                // get selected color in the colorpicker image
                                Color c = ColorPaletteImage.sprite.texture.GetPixel(pixelX + 14, pixelY + 14);
                                Color cGray = new Color(c.grayscale, c.grayscale, c.grayscale, c.a);

                                SkinColor sc = new SkinColor()
                                {
                                    grayIndex = grayIndex,

                                    r = c.r,
                                    g = c.g,
                                    b = c.b,
                                    a = c.a,

                                    true_r = c.r,
                                    true_g = c.g,
                                    true_b = c.b,
                                    true_a = c.a,
                                };

                                for (int i = 0; i < GrayPaletteImage.sprite.texture.width; i++)
                                {
                                    Color color = Color.LerpUnclamped(cGray, c, i / 28f);
                                    GrayPaletteImage.sprite.texture.SetPixel(i, 0, color);

                                    if (i == grayIndex)
                                    {
                                        sc.r = color.r;
                                        sc.g = color.g;
                                        sc.b = color.b;
                                        sc.a = color.a;
                                    }
                                }
                                GrayPaletteImage.sprite.texture.Apply();

                                int index = 0;

                                for (int i = 0; i < pc.Stats.skinIDs.Count; i++)
                                {
                                    if (pc.Stats.skinIDs[i].id == pc.Stats.activeSkin)
                                    {
                                        index = i;
                                        break;
                                    }
                                }

                                // create new skin with old id and new color
                                Skin skin = new Skin()
                                {
                                    id = pc.Stats.skinIDs[index].id,
                                    color = sc
                                };

                                // set new skin
                                pc.Stats.skinIDs[index] = skin;

                                // set color of ColorPickerCursor
                                float rgb = 1f - c.grayscale;
                                Color negativeColor = new Color(rgb, rgb, rgb, c.a);

                                ColorCursorImage.color = negativeColor;
                                GrayCursorImage.color = negativeColor;
                            }



                            else if (result.gameObject.name == "GrayScale")
                            {
                                // calculate imageposition
                                Vector2 pos = new Vector2()
                                {
                                    x = result.screenPosition.x - (1080 / 2) - Background.localPosition.x,
                                    y = result.screenPosition.y - (1920 / 2) - Background.localPosition.y
                                };

                                float h = 500f / 28f;

                                // get index for colorpicker_image
                                int pixelX = (int)(pos.x / h + .5f);

                                Vector2 cursorPos = new Vector2()
                                {
                                    x = pixelX * h - h / 2,
                                    y = 0
                                };

                                // set ColorCursor position
                                GrayCursor.localPosition = cursorPos;

                                // get selected color in the colorpicker image
                                Color c = GrayPaletteImage.sprite.texture.GetPixel(pixelX + 14, 0);

                                // set new grayIndex
                                grayIndex = pixelX + 14;

                                int index = 0;

                                for (int i = 0; i < pc.Stats.skinIDs.Count; i++)
                                {
                                    if (pc.Stats.skinIDs[i].id == pc.Stats.activeSkin)
                                    {
                                        index = i;
                                        break;
                                    }
                                }

                                SkinColor sc = new SkinColor()
                                {
                                    grayIndex = grayIndex,

                                    r = c.r,
                                    g = c.g,
                                    b = c.b,
                                    a = c.a,

                                    true_r = pc.Stats.skinIDs[index].color.true_r,
                                    true_g = pc.Stats.skinIDs[index].color.true_g,
                                    true_b = pc.Stats.skinIDs[index].color.true_b,
                                    true_a = pc.Stats.skinIDs[index].color.true_a,
                                };

                                // create new skin with old id and new color
                                Skin skin = new Skin()
                                {
                                    id = pc.Stats.skinIDs[index].id,
                                    color = sc
                                };

                                // set new skin
                                pc.Stats.skinIDs[index] = skin;
                            }
                        }
                    }
                }
            }
        }

        if (pc.Stats.activeSkin >= 0)
        {
            // if the current active skin has an animation for the backimages
            if (Skins[pc.Stats.activeSkin].backAnimatedFluid)
            {
                // if the timer is greater then 0
                if (Skins[pc.Stats.activeSkin].backAnimationTime > 0)
                {
                    // increase back-timer
                    timer_back += Time.deltaTime;

                    // if enought time passed, set next frame, substract the animationtime and increase counter
                    if (timer_back >= Skins[pc.Stats.activeSkin].frontAnimationTime)
                    {
                        timer_back -= Skins[pc.Stats.activeSkin].frontAnimationTime;

                        pc.Back.sprite = Skins[pc.Stats.activeSkin].skins_back[count_back];

                        count_back++;

                        // if count is too hight
                        if (count_back >= Skins[pc.Stats.activeSkin].skins_back.Length)
                        {
                            // reset count
                            count_back = 0;
                        }
                    }
                }
            }
            else if (Skins[pc.Stats.activeSkin].backAnimatedHard)
            {
                float animationBackwardStage = spawnManager.Cloudblanket2 + 500f;

                if (pc.Stats.currentSpeed > 0 && pc.Stats.currentScore < animationBackwardStage)
                {
                    float value = pc.Stats.currentSpeed / Skins[pc.Stats.activeSkin].lastFrameSpeed;
                    if (value >= count_back_hard_value * (count_back_hard + 1) && value <= 1f)
                    {
                        if (count_back_hard < 0)
                        {
                            count_back_hard++;
                            pc.Back.sprite = Skins[pc.Stats.activeSkin].skins_back[count_back_hard];
                        }
                    }
                    else if (value > 1f)
                    {
                        if (count_back_hard < Skins[pc.Stats.activeSkin].skins.Length - 1)
                        {
                            count_back_hard++;
                            pc.Back.sprite = Skins[pc.Stats.activeSkin].skins[count_back_hard];
                        }
                    }
                }
                else if (pc.Stats.currentScore >= animationBackwardStage)
                {
                    if (count_back_hard > 1)
                    {
                        // increase timer
                        timer_back += Time.deltaTime;

                        if (timer_back >= 1f)
                        {
                            timer_back -= 1f;
                            count_back_hard--;
                            pc.Back.sprite = Skins[pc.Stats.activeSkin].skins[count_back_hard];
                        }
                    }
                }
                else
                {
                    count_back_hard = 0;
                    pc.Back.sprite = Skins[pc.Stats.activeSkin].skins_back[count_back_hard];
                }
            }
            if (Skins[pc.Stats.activeSkin].frontAnimatedFluid)
            {
                // if the timer is greater then 0
                if (Skins[pc.Stats.activeSkin].frontAnimationTime > 0)
                {
                    // increase back-timer
                    timer_front += Time.deltaTime;

                    // if enought time passed, set next frame, substract the animationtime and increase counter
                    if (timer_front >= Skins[pc.Stats.activeSkin].frontAnimationTime)
                    {
                        timer_front -= Skins[pc.Stats.activeSkin].frontAnimationTime;

                        pc.Front.sprite = Skins[pc.Stats.activeSkin].skins[count_front];

                        count_front++;

                        // if count is too hight
                        if (count_front >= Skins[pc.Stats.activeSkin].skins.Length)
                        {
                            // reset count
                            count_front = 0;
                        }
                    }
                }
            }
            // if the skin has a hard animation
            else if (Skins[pc.Stats.activeSkin].frontAnimatedHard)
            {
                float animationBackwardStage = spawnManager.Cloudblanket2 + 500f;

                if (pc.Stats.currentSpeed > 0 && pc.Stats.currentScore < animationBackwardStage)
                {
                    float value = pc.Stats.currentSpeed / Skins[pc.Stats.activeSkin].lastFrameSpeed;
                    if (value >= count_front_hard_value * (count_front_hard + 1) && value <= 1f)
                    {
                        if (count_front_hard < 0)
                        {
                            count_front_hard++;
                            pc.Front.sprite = Skins[pc.Stats.activeSkin].skins[count_front_hard];
                        }
                    }
                    else if (value > 1f)
                    {
                        if (count_front_hard < Skins[pc.Stats.activeSkin].skins.Length - 1)
                        {
                            count_front_hard++;
                            pc.Front.sprite = Skins[pc.Stats.activeSkin].skins[count_front_hard];
                        }
                    }
                }
                else if (pc.Stats.currentScore >= animationBackwardStage)
                {
                    if (count_front_hard > 1)
                    {
                        // increase timer
                        timer_front += Time.deltaTime;

                        if (timer_front >= 1f)
                        {
                            timer_front -= 1f;
                            count_front_hard--;
                            pc.Front.sprite = Skins[pc.Stats.activeSkin].skins[count_front_hard];
                        }
                    }
                }
                else
                {
                    count_front_hard = 0;
                    pc.Front.sprite = Skins[pc.Stats.activeSkin].skins[count_front_hard];
                }
            }
        }
    }

    public void SelectPrestigeSkin(int _id)
    {
        pc.isLightning = false;

        timer_front = 0;
        timer_back = 0;
        count_front = 0;
        count_back = 0;
        count_front_hard_value = 0;
        count_back_hard_value = 0;
        count_front_hard = 0;
        count_back_hard = 0;

        int id = (_id + 1) * -1;

        if (isStarted)
        {
            if (buttonsound != null)
                buttonsound.Source.Play();
        }

        pc.GetCloudParticleDetailIndex(PrestigeSkins[id].Name);

        // activate right collider
        pc.Col.points = PrestigeSkins[id].Collider.points;

        // switch skin
        pc.Front.sprite = PrestigeSkins[id].skin;

        // save id of used skin
        pc.Stats.activeSkin = _id;

        // set right particletype
        pc.Stats.activeParticle = PrestigeSkins[id].particleType;
        pc.DestructionID = (int)PrestigeSkins[id].DestructionType;

        // modifire particlesystems
        for (int i = 0; i < pc.FireParticles.Length; i++)
        {
            if (PrestigeSkins[id].timeBased)
            {
                ParticleSystem ps = pc.FireParticles[i].GetComponentInChildren<ParticleSystem>();
                ps.Stop();
                var psmain = ps.main;

                ParticleSystem.ColorOverLifetimeModule pcol = ps.colorOverLifetime;
                pcol.color = PrestigeSkins[id].particleColor;
                pcol.enabled = true;

                ParticleSystem.MinMaxGradient g = ps.main.startColor;
                g.gradient = new Gradient();

                psmain.startColor = g;
                ps.Play();
            }
            else
            {
                ParticleSystem ps = pc.FireParticles[i].GetComponentInChildren<ParticleSystem>();
                ps.Stop();
                var psmain = ps.main;

                ParticleSystem.ColorOverLifetimeModule pcol = ps.colorOverLifetime;
                pcol.color = PrestigeSkins[id].particleColor;
                pcol.enabled = false;

                ParticleSystem.MinMaxGradient g = ps.main.startColor;
                g.gradient = PrestigeSkins[id].particleColor;

                psmain.startColor = g;
                ps.Play();
            }
        }

        // if this skin has a back
        if (PrestigeSkins[id].back)
        {
            if (PrestigeSkins[id].back_distance == 0)
            {
                // set the layer of the back
                pc.Back.sortingOrder = 8;
            }
            else
            {
                // set the layer of the back
                pc.Back.sortingOrder = 10;
            }

            // set skin for back
            pc.Back.sprite = PrestigeSkins[id].skin_back;
        }
        else
        {
            pc.Back.sprite = null;
        }

        for (int i = 0; i < pc.FireParticles.Length; i++)
        {
            // disable all firepartciles
            pc.FireParticles[i].SetActive(false);
        }

        pc.FuelDmg = PrestigeSkins[id].fuelDMG;

        ColorPickerPanel.SetActive(false);

        // save playerstats
        SaveSystem.SavePlayerStats(pc.Stats);

        // switch buttoncolors
        for (int i = 0; i < Buttons.Length; i++)
        {
            AmountImages[i].color = SkinDeactiveTint;
        }
    }

    public void SelectSkin(int _id)
    {
        timer_front = 0;
        timer_back = 0;
        count_front = 0;
        count_back = 0;
        count_front_hard_value = 0;
        count_back_hard_value = 0;
        count_front_hard = 0;
        count_back_hard = 0;

        skinid = 0;

        if (_id < 0)
        {
            SelectPrestigeSkin(_id);
            return;
        }

        if (!Skins[_id].bought)
        {
            switch (Skins[_id].currencyType)
            {
                case CurrencyType.DATA:
                    if (pc.Stats.Currency.data >= Skins[_id].cost)
                    {
                        pc.Stats.Currency.data -= Skins[_id].cost;
                        uiManager.DataAmount.text = pc.Stats.Currency.data.ToString();
                    }
                    else
                        return;
                    break;
                case CurrencyType.SATELLITE:
                    if (pc.Stats.Currency.satellite >= Skins[_id].cost)
                    {
                        pc.Stats.Currency.satellite -= Skins[_id].cost;
                        uiManager.SatelliteAmount.text = pc.Stats.Currency.satellite.ToString();
                    }
                    else
                        return;
                    break;
                case CurrencyType.UNLOCKABLE:
                    return;
                default:
                    if (CheckCurrencyType(Skins[_id].currencyType))
                    {
                        TrackCollectable(Skins[_id].currencyType, Skins[_id].ID);
                        return;
                    }
                    return;
            }

            SkinColor sc = new SkinColor
            {
                r = Color.white.r,
                g = Color.white.g,
                b = Color.white.b,
                a = Color.white.a,

                true_r = Color.white.r,
                true_g = Color.white.g,
                true_b = Color.white.b,
                true_a = Color.white.a
            };


            Skin skin = new Skin()
            {
                id = _id,
                color = sc
            };
            pc.Stats.skinIDs.Add(skin);
            skinid = pc.Stats.skinIDs.Count - 1;
        }
        else
        {
            for (int i = 0; i < pc.Stats.skinIDs.Count; i++)
            {
                if (pc.Stats.skinIDs[i].id == _id)
                {
                    Color c = new Color()
                    {
                        r = pc.Stats.skinIDs[i].color.r,
                        g = pc.Stats.skinIDs[i].color.g,
                        b = pc.Stats.skinIDs[i].color.b,
                        a = 1
                    };
                    pc.Front.color = c;
                    skinid = i;
                    break;
                }
            }
        }

        pc.GetCloudParticleDetailIndex(Skins[_id].Name);

        if (Skins[_id].backAnimatedHard)
        {
            count_back_hard_value = 1f / (Skins[_id].skins_back.Length - 1);
        }
        if (Skins[_id].frontAnimatedHard)
        {
            count_front_hard_value = 1f / (Skins[_id].skins.Length - 1);
        }

        if (isStarted)
        {
            if (buttonsound != null)
                buttonsound.Source.Play();
        }

        if (Skins[_id].Name == "Lightning")
        {
            pc.isLightning = true;
        }
        else
        {
            pc.isLightning = false;
        }

        // activate right collider
        pc.Col.points = Skins[_id].Collider.points;

        // switch skin
        pc.Front.sprite = Skins[_id].skin;

        // save id of used skin
        pc.Stats.activeSkin = _id;

        // set right particletype
        pc.Stats.activeParticle = Skins[_id].particleType;
        pc.DestructionID = (int)Skins[_id].DestructionType;

        // modifire particlesystems
        for (int i = 0; i < pc.FireParticles.Length; i++)
        {
            if (Skins[_id].timeBased)
            {
                ParticleSystem ps = pc.FireParticles[i].GetComponentInChildren<ParticleSystem>();
                ps.Stop();
                var psmain = ps.main;

                ParticleSystem.ColorOverLifetimeModule pcol = ps.colorOverLifetime;
                pcol.color = Skins[_id].particleColor;

                if (Skins[_id].colorableThrust)
                {
                    Gradient gradient = new Gradient();
                    GradientColorKey[] colorkeys = new GradientColorKey[pcol.color.gradient.colorKeys.Length];
                    for (int j = 0; j < pcol.color.gradient.colorKeys.Length; j++)
                    {
                        Color c = new Color()
                        {
                            r = pc.Stats.skinIDs[skinid].color.r,
                            g = pc.Stats.skinIDs[skinid].color.g,
                            b = pc.Stats.skinIDs[skinid].color.b,
                            a = pc.Stats.skinIDs[skinid].color.a
                        };

                        GradientColorKey key = new GradientColorKey()
                        {
                            color = Color.Lerp(pcol.color.gradient.colorKeys[j].color, c, Skins[_id].colorableThrustStrength),
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
                ParticleSystem ps = pc.FireParticles[i].GetComponentInChildren<ParticleSystem>();
                ps.Stop();
                var psmain = ps.main;

                ParticleSystem.ColorOverLifetimeModule pcol = ps.colorOverLifetime;
                pcol.color = Skins[_id].particleColor;

                pcol.enabled = false;

                ParticleSystem.MinMaxGradient g = ps.main.startColor;
                g.gradient = Skins[_id].particleColor;

                psmain.startColor = g;
                ps.Play();
            }
        }

        // if this skin has a back
        if (Skins[_id].back)
        {
            if (Skins[_id].back_distance == 0)
            {
                // set the layer of the back
                pc.Back.sortingOrder = 8;
            }
            else
            {
                // set the layer of the back
                pc.Back.sortingOrder = 10;
            }

            // set skin for back
            pc.Back.sprite = Skins[_id].skin_back;
        }
        else
        {
            pc.Back.sprite = null;
        }

        for (int i = 0; i < pc.FireParticles.Length; i++)
        {
            // disable all firepartciles
            pc.FireParticles[i].SetActive(false);
        }

        // if current skin is colorable, visible ColorPickerButton
        if (Skins[_id].colorableRocket)
        {
            ColorPickerPanel.SetActive(true);
        }
        else
        {
            ColorPickerPanel.SetActive(false);
        }

        pc.FuelDmg = Skins[_id].fuelDMG;

        // save playerstats
        SaveSystem.SavePlayerStats(pc.Stats);

        // switch buttoncolors
        for (int i = 0; i < Buttons.Length; i++)
        {
            int index = 0;

            for (int j = 0; j < pc.Stats.skinIDs.Count; j++)
            {
                if (pc.Stats.skinIDs[j].id == pc.Stats.activeSkin)
                {
                    index = j;
                    break;
                }
            }

            if (Skins[i].ID == pc.Stats.skinIDs[index].id) { AmountImages[i].color = SkinActiveTint; }
            else { AmountImages[i].color = SkinDeactiveTint; }
        }
    }

    public void ResetColor()
    {
        SkinColor sc = new SkinColor();
        sc.r = Color.white.r;
        sc.g = Color.white.g;
        sc.b = Color.white.b;
        sc.a = Color.white.a;
        sc.true_r = Color.white.r;
        sc.true_g = Color.white.g;
        sc.true_b = Color.white.b;
        sc.true_a = Color.white.a;

        int index = 0;

        for (int i = 0; i < pc.Stats.skinIDs.Count; i++)
        {
            if (pc.Stats.skinIDs[i].id == pc.Stats.activeSkin)
            {
                index = i;
                break;
            }
        }

        Skin skin = new Skin()
        {
            id = pc.Stats.skinIDs[index].id,
            color = sc
        };

        pc.Stats.skinIDs[index] = skin;
        PlayerActiveSkin.color = Color.white;

        float h = 500f / 28f;
        ColorCursor.localPosition = new Vector2(-13 * h - h / 2, 14 * h - h / 2);
        ColorCursorImage.color = Color.black;
        GrayCursor.localPosition = new Vector2(14 * h - h / 2, 0);
        GrayCursorImage.color = Color.black;

        for (int i = 0; i < GrayPaletteImage.sprite.texture.width; i++)
        {
            GrayPaletteImage.sprite.texture.SetPixel(i, 0, Color.white);
        }
        GrayPaletteImage.sprite.texture.Apply();
    }

    public RocketSkin FindSkin(string _name)
    {
        for (int i = 0; i < Skins.Length; i++)
        {
            string searchedName = _name.ToLower();
            string skinName = Skins[i].Name.ToLower();

            if (searchedName == skinName) return Skins[i];
        }

        return new RocketSkin()
        {
            Name = "",
        };
    }

    public void SetTrackCollectable(CurrencyType _typ, int _id)
    {
        // create new track with right informations
        CollactableTrack collect = new CollactableTrack
        {
            Tack_Icon = Tracked_Icons[_id],
            Type = _typ,
            ID = _id,
            Selected = false
        };

        // add to tracks-list
        tracks.Add(collect);

        // show tracked-icon
        collect.Tack_Icon.SetActive(true);

        Track track = GetTrack(_typ, "001");

        if (!pc.Stats.tracks.Contains(track))
        {
            pc.Stats.tracks.Insert(0, track);
        }

        // remove last object
        while (pc.Stats.tracks.Count > 2)
            pc.Stats.tracks.RemoveAt(pc.Stats.tracks.Count - 1);
    }

    public void TrackCollectable(CurrencyType _typ, int _id)
    {
        if (tracks.Count < 2)
        {
            SetTrackCollectable(_typ, _id);
            return;
        }

        // if player click on tracked skin
        if (tracks[0].ID == _id)
        {
            // if this one is selected
            if (tracks[0].Selected)
            {
                // deselect
                tracks[0].Selected = false;

                // reset color
                tracks[0].Tack_Icon.GetComponent<Image>().color = Color.white;

                return;
            }
            // if the otherone is selected
            else if (tracks[1].Selected)
            {
                // do nothing
                return;
            }
            // otherwise
            else
            {
                // select that one
                tracks[0].Selected = true;

                // set tintcolor
                tracks[0].Tack_Icon.GetComponent<Image>().color = TrackSelectedTint;
            }
        }
        else if (tracks[1].ID == _id)
        {
            // if this one is selected
            if (tracks[1].Selected)
            {
                // deselect
                tracks[1].Selected = false;

                // reset color
                tracks[1].Tack_Icon.GetComponent<Image>().color = Color.white;

                return;
            }
            // if the otherone is selected
            else if (tracks[0].Selected)
            {
                // do nothing
                return;
            }
            // otherwise
            else
            {
                // select that one
                tracks[1].Selected = true;

                // set tintcolor
                tracks[1].Tack_Icon.GetComponent<Image>().color = TrackSelectedTint;
            }
        }
        // otherwise
        else
        {
            if (tracks[0].Selected)
            {
                // deactivate current track-icon
                tracks[0].Tack_Icon.SetActive(false);

                // reset color
                tracks[0].Tack_Icon.GetComponent<Image>().color = Color.white;

                // set new values for this track
                tracks[0].Selected = false;
                tracks[0].Tack_Icon = Tracked_Icons[_id];
                tracks[0].ID = _id;
                tracks[0].Type = _typ;

                // activate new track-icon
                tracks[0].Tack_Icon.SetActive(true);

                // add track to stats
                pc.Stats.tracks.Insert(0, GetTrack(_typ, "002"));
            }
            else if (tracks[1].Selected)
            {
                // deactivate current track-icon
                tracks[1].Tack_Icon.SetActive(false);

                // reset color
                tracks[1].Tack_Icon.GetComponent<Image>().color = Color.white;

                // set new values for this track
                tracks[1].Selected = false;
                tracks[1].Tack_Icon = Tracked_Icons[_id];
                tracks[1].ID = _id;
                tracks[1].Type = _typ;

                // activate new track-icon
                tracks[1].Tack_Icon.SetActive(true);

                // add track to stats
                pc.Stats.tracks.Insert(0, GetTrack(_typ, "003"));
            }
            // if nothing is selected
            else
            {
                return;
            }
        }

        // remove last object
        while (pc.Stats.tracks.Count > 2)
            pc.Stats.tracks.RemoveAt(pc.Stats.tracks.Count - 1);

        // save changes
        SaveSystem.SavePlayerStats(pc.Stats);
    }

    public void DeselectTrack()
    {
        tracks[0].Selected = false;
        tracks[1].Selected = false;

        // reset color
        tracks[0].Tack_Icon.GetComponent<Image>().color = Color.white;
        tracks[1].Tack_Icon.GetComponent<Image>().color = Color.white;
    }

    private void SortSkinList()
    {
        List<RocketSkin> skins = new List<RocketSkin>();

        for (int i = 0; i < Skins.Length; i++)
        {
            skins.Add(Skins[i]);
        }

        skins.Sort(SortByID);

        for (int i = 0; i < Skins.Length; i++)
        {
            Skins[i] = skins[i];
        }
    }

    private int SortByID(RocketSkin _rs1, RocketSkin _rs2)
    {
        return _rs1.ID.CompareTo(_rs2.ID);
    }

    /// <summary>
    /// return true, if currencytype is a spawnable collactable
    /// </summary>
    /// <param name="_type">CurrencyType to check</param>
    /// <returns></returns>
    private bool CheckCurrencyType(CurrencyType _type)
    {
        switch (_type)
        {
            case CurrencyType.DUCK:
                return true;
            case CurrencyType.UFO:
                return true;
            case CurrencyType.BURGER:
                return true;
            case CurrencyType.SANDWICH:
                return true;
            case CurrencyType.PRESENT:
                return true;
            case CurrencyType.FIREWORK:
                return true;
            case CurrencyType.TEDDY:
                return true;
            case CurrencyType.WATER:
                return true;
            case CurrencyType.SODA:
                return true;
            default:
                return false;
        }
    }

    private Track GetTrack(CurrencyType _typ, string _error)
    {
        switch (_typ)
        {
            case CurrencyType.UFO:
                return Track.UFO;

            case CurrencyType.DUCK:
                return Track.DUCK;

            case CurrencyType.BURGER:
                return Track.BURGER;

            case CurrencyType.SANDWICH:
                return Track.SANDWICH;

            case CurrencyType.PRESENT:
                return Track.PRESENT;

            case CurrencyType.TEDDY:
                return Track.TEDDY;

            case CurrencyType.FIREWORK:
                return Track.FIREWORK;

            case CurrencyType.WATER:
                return Track.WATER;

            case CurrencyType.SODA:
                return Track.SODA;
            default:
                Debug.LogWarning("ErrorCode: " + _error + ": Impossible to get Track, be sure to use the right Currency.");
                return Track.DUCK;
        }
    }

    /// <summary>
    /// remove completed track
    /// </summary>
    public void RemoveTrack(int _id)
    {
        // remove collected track
        for (int i = 0; i < tracks.Count; i++)
        {
            if (tracks[i].ID == _id)
                tracks.RemoveAt(i);
        }
    }

    [System.Serializable]
    public struct RocketSkin
    {
        public string Name;
        public int ID;
        public PolygonCollider2D Collider;
        [Space(10)]
        public Sprite skin;
        public Sprite skin_back;
        [Range(0, 1)]
        public int back_distance;
        public bool back;

        [Space(10)]
        public bool colorableRocket;
        public bool colorableThrust;
        [Range(0, 1f)]
        public float colorableThrustStrength;
        public bool colorableExplosion;
        [Range(0, 1f)]
        public float colorableExplosionStrength;

        [Space(10)]
        [Header("RocketAnimation")]
        public Sprite[] skins;
        public bool frontAnimatedFluid;
        public float frontAnimationTime;
        public Sprite[] skins_back;
        public bool backAnimatedFluid;
        public float backAnimationTime;
        [Space(10)]
        public bool frontAnimatedHard;
        public bool backAnimatedHard;
        public float lastFrameSpeed;
        [Space(20)]
        public int cost;
        [HideInInspector]
        public bool bought;
        public bool collect;
        public CurrencyType currencyType;

        [Header("Particle")]
        [Range(0, 4)]
        public int particleType;
        public Gradient particleColor;
        public bool timeBased;
        [Space(10)]
        public DestructionParticleType DestructionType;
        public bool fuelDMG;
    }

    public enum CurrencyType
    {
        UNLOCKABLE,
        DATA,
        SATELLITE,
        COMET,
        ASTEROID,
        BIRD,
        COOKED_BIRD,
        CLOUD,
        DUCK,
        FIREWORK,
        UFO,
        BURGER,
        SANDWICH,
        PRESENT,
        TEDDY,
        ELECTRIC,
        WATER,
        SODA
    }

    public enum DestructionParticleType
    {
        ROCKET,
        BARREL,
        ASTEROID,
        COMET,
        BONE,
        COOKED_BIRD,
        CLOUD,
        PRESENT,
        GLASS,
        LIGHTNING,
        BURGER,
        SANDWICH
    }

    private class CollactableTrack
    {
        public GameObject Tack_Icon;
        public CurrencyType Type;
        public bool Selected;
        public int ID;
    }
}