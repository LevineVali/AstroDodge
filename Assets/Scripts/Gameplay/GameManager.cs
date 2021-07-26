using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Advertisements;
using System;
using GoogleMobileAds.Api;
using Firebase;
using Firebase.Analytics;

public class GameManager : MonoBehaviour
{
    public string PatreonURL;

    #region Advertisements
    private string advertismentID = "ca-app-pub-3524516393538705/1853555499";
    private string testAdvertismentID = "ca-app-pub-3940256099942544/5224354917";

    private RewardedAd rewardedAd;

    /// <summary>
    /// Testmode
    /// </summary>
    public bool TestMode = false;

    /// <summary>
    /// All skins aviable?
    /// </summary>
    public bool AllSkins = false;

    /// <summary>
    /// DeveloperMode
    /// </summary>
    public bool DeveloperMode = false;

    /// <summary>
    /// TesplayerMode
    /// </summary>
    public bool TestPlayerMode = false;

    /// <summary>
    /// Bonus Earning for faster process
    /// </summary>
    public float EarnMultiplier = 1.0f;

    #endregion

    public SkinManager SkinMan;

    public AudioManager AM;

    public int FrameRate;

    /// <summary>
    /// PlayerController
    /// </summary>
    public PlayerController PC;

    /// <summary>
    /// Button for open CashShop
    /// </summary>
    public Button CashShopButton;

    /// <summary>
    /// Button for open CurrencyChanger
    /// </summary>
    public Button CurrencyChangerButton;

    [Header("Backgrounds")]
    /// <summary>
    /// Main Camera
    /// </summary>
    public Camera MainCamera;

    /// <summary>
    /// Stars in the Background
    /// </summary>
    public MeshRenderer StarsBackground;

    /// <summary>
    /// Space in the Background
    /// </summary>
    public MeshRenderer SpaceBackground;

    /// <summary>
    /// GalaxySpace in the Background
    /// </summary>
    public MeshRenderer GalaxySpaceBackground;

    /// <summary>
    /// Galaxy in the Background
    /// </summary>
    public GameObject GalaxyBackground;

    /// <summary>
    /// Ground
    /// </summary>
    public GameObject Ground;

    [Space(10)]
    /// <summary>
    /// Speedmultiplier for Starsbackground
    /// </summary>
    public float StarsSpeed;

    /// <summary>
    /// Speedmultiplier for Spacebackground
    /// </summary>
    public float SpaceSpeed;

    /// <summary>
    /// Speedmultiplier for Galaxybackgroundspeed
    /// </summary>
    public float GalaxySpeed;

    /// <summary>
    /// Backgroundcolor for the Camera
    /// </summary>
    public Gradient CameraBackgroundColor;

    /// <summary>
    /// Backgroundcolor for the Camera
    /// </summary>
    public Gradient CameraBackgroundColorCloud;

    /// <summary>
    /// strength of lerping original background color with lerpingcolor for lightningstage
    /// </summary>
    [Range(0, 1f)]
    public float CBC_LerpStrength;

    /// <summary>
    /// Scorelimit for the player to start fade of starsbackground
    /// </summary>
    public float StartStarVisibleHeight;

    /// <summary>
    /// Scorelimit for the player to reach the darkest background
    /// </summary>
    public float DarkestHigh;

    [Header("UI Elements")]
    /// <summary>
    /// Scorecolor
    /// </summary>
    public Gradient ScoreColor;

    /// <summary>
    /// Height of the player in UI
    /// </summary>
    public Text Score;

    /// <summary>
    /// Button to Start the game
    /// </summary>
    public Button StartButton;

    /// <summary>
    /// GameObject of StartButton
    /// </summary>
    public GameObject StartButtonGO;

    /// <summary>
    /// GaomeObject of UpgradeButton
    /// </summary>
    public GameObject ShopButton;

    /// <summary>
    /// Toolbar for Shop and Options
    /// </summary>
    public GameObject BottomBar;

    /// <summary>
    /// Plays Add to increase the amount of Data (GamevOver)
    /// </summary>
    public Button AddButtonGameOver;

    /// <summary>
    /// Plays Add to increase the amount of Data (OfflineBonus)
    /// </summary>
    public Button AddButtonOfflineBonus;

    /// <summary>
    /// Optionwindow
    /// </summary>
    public GameObject OptionsMenu;

    [Header("Player")]
    /// <summary>
    /// Resetposition of player
    /// </summary>
    public Vector3 ResetPosition;

    /// <summary>
    /// Resetrotation of player
    /// </summary>
    public Quaternion ResetRotation;

    /// <summary>
    /// Height the player must reach to enable Prestige
    /// </summary>
    public int PrestigeHeight;

    /// <summary>
    /// all soundeffects will be fade to this value while flying into vacuum
    /// </summary>
    [Range(0, 1f)]
    public float SoundMinValue;

    [Header("Upgrade")]
    /// <summary>
    /// base cost of an upgrade
    /// </summary>
    public int BaseUpgradeCost;

    /// <summary>
    /// base cost of a research
    /// </summary>
    public int BaseResearchCost;

    [Space(10)]
    /// <summary>
    /// dmg multiplyer for overspeed
    /// </summary>
    public float SpeedDMG;

    /// <summary>
    /// Speed of Ground, Player fade
    /// </summary>
    public float FadeSpeed;

    /// <summary>
    /// Tab to start text
    /// </summary>
    public Text tts;

    /// <summary>
    /// Tab to start TextColor
    /// </summary>
    public Color ttsColor;

    [Space(10)]
    /// <summary>
    /// Window of offlinebonus
    /// </summary>
    public GameObject OfflineBonusWindow;

    /// <summary>
    /// Minimal time past to get offlinereward
    /// </summary>
    public float MinOfflineTime;

    /// <summary>
    /// Text for showing how much bonus the player get for being offline
    /// </summary>
    public Text OfflineBonusAmount;

    /// <summary>
    /// Bonus for being offline per minute
    /// </summary>
    public float OfflineBonusPerMinute;

    /// <summary>
    /// Offset of the Stars
    /// </summary>
    private Vector2 starsOffset = new Vector2();

    /// <summary>
    /// Offset of the space
    /// </summary>
    private Vector2 spaceOffset = new Vector2();

    /// <summary>
    /// Offset of Galaxyspace
    /// </summary>
    private Vector2 galaxySpaceOffset = new Vector2();

    /// <summary>
    /// Offset of the Galaxy
    /// </summary>
    private Vector2 galaxyOffset = new Vector2();

    /// <summary>
    /// backgroundcolor for changing alpha
    /// </summary>
    private Color color = new Color();

    /// <summary>
    /// Upgradesystem
    /// </summary>
    private UpgradeSystem USystem;

    /// <summary>
    /// SpawnManager
    /// </summary>
    private SpawnManager SManager;

    /// <summary>
    /// reset done?
    /// </summary>
    private bool reset = false;

    /// <summary>
    /// Alls images on bottom bar
    /// </summary>
    private Image[] bbimages;

    /// <summary>
    /// offlinebonus
    /// </summary>
    private int offlinebonus;

    /// <summary>
    /// UIManager of this game
    /// </summary>
    public UIManager uiManager;


    private float timer;

    public float MusicTimer;

    private int musicindex = 1;
    private float musicTimer = 0;

    private bool fading = false;

    private int prestigeHeight;

    // Start is called before the first frame update
    void Start()
    {
        // firebasestuff
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        });

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = FrameRate;

        // Advertisment
        MobileAds.Initialize(initStatus => { });

        CreateAndLoadRewardedAd();

        // Music
        musicindex = 1;
        uiManager = FindObjectOfType<UIManager>();

        fading = false;
        musicTimer = MusicTimer;

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        // set ScreenResolution
        Screen.SetResolution(1080, 1920, false);

        bbimages = BottomBar.GetComponentsInChildren<Image>();

        // get upgradesystem
        USystem = GetComponent<UpgradeSystem>();

        // get Spawnmanager
        SManager = GetComponent<SpawnManager>();

        // get skinmanager
        SkinMan = FindObjectOfType<SkinManager>();

        if (PC.Stats.skinIDs == null)
        {
            PC.Stats.skinIDs = new List<Skin>();
        }

        if (TestMode)
        {
            PC.Stats = new PlayerStats(PC.Stats);
            USystem.ResetPlayerStats(PC.Stats, this);
        }
        else
        {
            if (File.Exists(Application.persistentDataPath + "/Stats.save"))
            {
                PC.Stats = SaveSystem.LoadPlayerStats();
            }
            else
            {
                PC.Stats = new PlayerStats(PC.Stats);
                USystem.ResetPlayerStats(PC.Stats, this);
            }
        }

        double time = (DateTime.UtcNow - PC.Stats.time).TotalMinutes;

        if (time >= MinOfflineTime)
        {
            if (time > 1440) time = 1440;

            offlinebonus = (int)(time * OfflineBonusPerMinute * PC.Stats.OfflineBonus);

            OfflineBonusAmount.text = offlinebonus.ToString();

            if (offlinebonus > 0)
            {
                uiManager.CloseColorpicker(false);
                uiManager.CloseCurrencyChangeWindow(false);
                uiManager.CloseDataShop(false);
                uiManager.CloseOptions(false);
                uiManager.CloseShop(false);

                OfflineBonusWindow.SetActive(true);
            }
        }

        color = StarsBackground.material.color;

        prestigeHeight = (int)(PrestigeHeight * (1f + PC.Stats.prestige / 20f));
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.targetFrameRate != FrameRate)
            Application.targetFrameRate = FrameRate;

        #region Music
        double score;

        if (!PC.MusicRun)
        {
            for (int i = 0; i < PC.music.Length; i++)
            {
                if (i == 0) { continue; }
                else
                {
                    if (PC.music[i].Source.isPlaying)
                    {
                        PC.AM.FadeOut(PC.music[i].Source, AudioManager.SoundTyp.MUSIC, 2f);
                        fading = true;
                        musicindex = 1;
                        break;
                    }
                }
                fading = false;
            }
            score = 0;
        }
        else
        {
            score = PC.Stats.currentScore;
        }

        if (!fading)
        {
            // manage sound
            if (score < SManager.AsteroidStage1)
            {
                if (!PC.music[0].Source.isPlaying)
                {
                    if (musicTimer <= 0)
                    {
                        PC.music[0].Source.Play();
                        musicTimer = MusicTimer;
                    }
                    else
                    {
                        musicTimer -= Time.unscaledDeltaTime;
                    }
                }
            }
            else
            {
                if (PC.music[0].Source.isPlaying)
                {
                    PC.AM.FadeOut(PC.music[0].Source, AudioManager.SoundTyp.MUSIC, 2f);
                    musicTimer = MusicTimer;
                }
                else
                {
                    if (!PC.music[musicindex - 1].Source.isPlaying)
                    {
                        if (musicTimer <= 0)
                        {
                            if (musicindex == PC.music.Length) { musicindex = 1; }
                            PC.music[musicindex].Source.Play();
                            musicTimer = MusicTimer;
                            musicindex++;
                            PC.AM.SetVolume();
                        }
                        else
                        {
                            musicTimer -= Time.unscaledDeltaTime;
                        }
                    }
                }

            }
        }
        #endregion

        float value;
        float value2;

        if (PC.Run)
        {
            timer = 0;
            reset = false;
            value = (float)PC.Stats.currentScore / DarkestHigh;
            float speed;

            // if game is paused
            if (Time.timeScale == 0)
            {
                BottomBar.SetActive(true);
                StartButtonGO.SetActive(false);
                ShopButton.SetActive(false);
            }
            else
            {
                StartButtonGO.SetActive(true);
                ShopButton.SetActive(true);
                BottomBar.SetActive(false);
            }

            if (value > 1)
            {
                value = 1;
            }
            Score.color = ScoreColor.Evaluate(value);

            Score.text = ((int)PC.Stats.currentScore).ToString();

            // set new backgroundcolor
            if (PC.Stats.currentScore > SManager.Cloudblanket1 && PC.Stats.currentScore < (SManager.Cloudblanket2 + 200))
            {
                float tmp1 = (float)PC.Stats.currentScore - SManager.Cloudblanket1;
                float tmp2 = SManager.Cloudblanket2 - SManager.Cloudblanket1;
                value2 = tmp1 / tmp2;
                float lerp = CameraBackgroundColorCloud.Evaluate(value2).a * CBC_LerpStrength;
                MainCamera.backgroundColor = Color.Lerp(CameraBackgroundColor.Evaluate(value), CameraBackgroundColorCloud.Evaluate(value2), lerp);
            }
            else
            {
                MainCamera.backgroundColor = CameraBackgroundColor.Evaluate(value);
            }

            if (PC.Stats.currentScore < DarkestHigh && PC.Stats.currentScore >= StartStarVisibleHeight)
            {
                // calculate fadevalue for soundeffects in vacuum
                float scorevalue = (float)(PC.Stats.currentScore - StartStarVisibleHeight);
                float heightvalue = (float)(DarkestHigh - StartStarVisibleHeight);
                float soundvalue = scorevalue / heightvalue * (1f - SoundMinValue);
                PC.soundValueModifire = 1f - soundvalue;

                float alphaValue = (float)(PC.Stats.currentScore - StartStarVisibleHeight) / (DarkestHigh - StartStarVisibleHeight);

                // create new color
                color = new Color(color.r, color.g, color.b, alphaValue);

                // create new materialpropertyblock
                MaterialPropertyBlock block1 = new MaterialPropertyBlock();
                MaterialPropertyBlock block2 = new MaterialPropertyBlock();
                MaterialPropertyBlock block3 = new MaterialPropertyBlock();
                StarsBackground.GetPropertyBlock(block1);
                SpaceBackground.GetPropertyBlock(block2);
                GalaxySpaceBackground.GetPropertyBlock(block3);

                // set new color for materialpropertyblock
                block1.SetColor("_BaseColor", color);
                block2.SetColor("_BaseColor", color);
                block3.SetColor("_BaseColor", color);

                // set new materialpropertyblock
                StarsBackground.SetPropertyBlock(block1);
                SpaceBackground.SetPropertyBlock(block2);
                GalaxySpaceBackground.SetPropertyBlock(block3);
            }
            else if (PC.Stats.currentScore > DarkestHigh)
            {
                PC.soundValueModifire = SoundMinValue;
            }

            // calculate new offset
            starsOffset.y += PC.Stats.currentSpeed * StarsSpeed * Time.deltaTime;

            // set new offset for the background
            StarsBackground.material.SetTextureOffset("_BaseMap", starsOffset);

            if (Ground.transform.position.y <= -1.55f)
            {
                Ground.SetActive(false);
            }
            else
            {
                // calculate speed for Starsbackground 
                speed = PC.Stats.currentSpeed * (Time.deltaTime / 10);
                Ground.transform.position = new Vector3(0, Ground.transform.position.y - speed, 0);
            }

            // calculate speed for Spacebackground 
            speed = PC.Stats.currentSpeed * SpaceSpeed * Time.deltaTime;

            spaceOffset.y += speed;
            SpaceBackground.material.SetTextureOffset("_BaseMap", -spaceOffset);

            // calculate speed for Galaxybackground
            speed = PC.Stats.currentSpeed * GalaxySpeed * Time.deltaTime;

            if (PC.Stats.currentScore >= SManager.GalaxyStage)
            {
                Vector3 galaxyPos = GalaxyBackground.transform.position;

                if (galaxyPos.y == 2)
                {
                    // calculate galaxy offset
                    galaxyOffset.y += speed / 6;

                    // set galaxy background
                    GalaxyBackground.GetComponent<MeshRenderer>().material.SetTextureOffset("_BaseMap", galaxyOffset);
                }
                else
                {
                    if (galaxyPos.y <= 2 + PC.Stats.currentSpeed * GalaxySpeed * Time.deltaTime)
                    {
                        GalaxyBackground.transform.position = new Vector3(0, 2, 1);
                    }
                    else
                    {
                        GalaxyBackground.transform.position = new Vector3(0, galaxyPos.y - speed, 1);
                    }
                }
            }
            else
            {
                galaxySpaceOffset.y += speed;
                GalaxySpaceBackground.material.SetTextureOffset("_BaseMap", galaxySpaceOffset);
            }

            if (PC.Stats.currentScore >= prestigeHeight)
            {
                PC.Stats.isPrestigeable = true;
            }
        }
        else
        {
            if (!uiManager.GameOverScreen.gameObject.activeSelf)
                Score.text = ((int)PC.Stats.highScore).ToString();

            if (reset)
            {
                timer += Time.deltaTime;

                Transform player = PC.gameObject.transform;
                Transform ground = Ground.transform;

                if (player.position.y + FadeSpeed * Time.deltaTime > -0.5f
                    || Input.touchCount > 0 && timer >= .75f
                    || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
                {
                    player.position = new Vector3(0, -0.5f, 0);
                    ground.position = new Vector3(0, -0.887f, 0);
                    for (int i = 0; i < bbimages.Length; i++)
                    {
                        bbimages[i].color = new Color(1, 1, 1, 0);
                    }
                    tts.color = new Color(0, 0, 0, 0);
                    BottomBar.SetActive(true);
                    reset = false;
                }
                else
                {
                    player.Translate(new Vector3(0, FadeSpeed * Time.deltaTime, 0));
                    ground.Translate(new Vector3(0, FadeSpeed * Time.deltaTime, 0));
                }
            }
            else
            {
                for (int i = 0; i < bbimages.Length; i++)
                {
                    bbimages[i].color = new Color(1, 1, 1, bbimages[i].color.a + (0.5f * Time.deltaTime));
                }
                tts.color = new Color(ttsColor.r, ttsColor.g, ttsColor.b, tts.color.a + (0.5f * Time.deltaTime));
            }

            AddButtonGameOver.interactable = rewardedAd.IsLoaded();
            AddButtonOfflineBonus.interactable = rewardedAd.IsLoaded();
        }
    }

    public void RestartGame()
    {
        AM.SetVolume();

        PC.soundValueModifire = 1f;
        PC.MusicRun = false;

        // hide Gameover Window
        uiManager.GameOverScreen.gameObject.SetActive(false);

        // increase Currency
        PC.Stats.Currency.data += PC.GetReward();

        uiManager.DataAmount.text = PC.Stats.Currency.data.ToString();

        if (PC.Stats.highScore < PC.Stats.currentScore)
        {
            PC.Stats.highScore = (int)PC.Stats.currentScore;
        }

        PC.Stats.currentScore = 0;

        Score.color = ScoreColor.Evaluate(0);

        PC.Killall = true;

        #region Reset values
        // reset players position
        PC.gameObject.transform.position = ResetPosition;
        PC.gameObject.transform.rotation = ResetRotation;

        // reset slider
        PC.HealthSlider.value = 1;
        PC.FuelSlider.value = 1;

        // reset survived bonus
        PC.Survived = 0;

        // reset FuelDmgCounter
        PC.fuelDmgCount = 1;

        // save playerstats
        SaveSystem.SavePlayerStats(PC.Stats);

        // reset offset
        starsOffset = new Vector2();
        spaceOffset = starsOffset;
        galaxyOffset = starsOffset;
        galaxySpaceOffset = starsOffset;


        // set resetet offset for the background
        StarsBackground.material.SetTextureOffset("_BaseMap", starsOffset);
        SpaceBackground.material.SetTextureOffset("_BaseMap", starsOffset);
        GalaxySpaceBackground.material.SetTextureOffset("_BaseMap", starsOffset);
        GalaxyBackground.GetComponent<MeshRenderer>().material.SetTextureOffset("_BaseMap", starsOffset);
        GalaxyBackground.transform.position = new Vector3(0, 4.275f, 1);
        Ground.transform.position = new Vector3(0, -1.537f, 0);
        PC.gameObject.transform.position = new Vector3(0, -1.15f, 0);
        Ground.SetActive(true);

        // reset color
        color = new Color(color.r, color.g, color.b, 0);

        // set reset color
        // create new materialpropertyblock
        var block = new MaterialPropertyBlock();

        // set new color for materialpropertyblock
        block.SetColor("_BaseColor", color);

        // set new materialpropertyblock
        StarsBackground.SetPropertyBlock(block);
        SpaceBackground.SetPropertyBlock(block);
        GalaxySpaceBackground.SetPropertyBlock(block);

        MainCamera.backgroundColor = CameraBackgroundColor.Evaluate(0);

        Score.text = ((int)PC.Stats.highScore).ToString();

        // reset cloudblanketspawn
        SManager.cloudblanketcount = 0;

        // reset skinvalues
        SkinMan.Reset = true;
        #endregion

        reset = true;

        PC.rocketStand.Source.Play();

        CashShopButton.enabled = true;
        CurrencyChangerButton.enabled = true;
    }

    public void StartGame()
    {
        prestigeHeight = (int)(PrestigeHeight * (1f + PC.Stats.prestige / 20f));

        PC.MusicRun = true;

        // start the game
        PC.Run = true;

        // revive player
        PC.Life = true;

        PC.Killall = false;

        // hide Toolbar
        BottomBar.SetActive(false);

        // set full color
        for (int i = 0; i < bbimages.Length; i++)
        {
            bbimages[i].color = new Color(1, 1, 1, 1);
        }

        // set fuel
        PC.Stats.fuel = PC.Stats.maxFuel;

        // set health
        PC.Stats.health = PC.Stats.maxHealth;

        // reset score
        PC.Stats.currentScore = 0;

        tts.color = new Color(ttsColor.r, ttsColor.g, ttsColor.b, 0f);

        PC.rocketStand.Source.Stop();
        PC.rocketFlight.Source.Play();

        CashShopButton.enabled = false;
        CurrencyChangerButton.enabled = false;
    }

    public int GetUpgradeCost(int _target)
    {
        if (PC.Stats.Upgrades[_target].amount == 0)
        {
            PC.Stats.Upgrades[_target].cost = BaseUpgradeCost;
            return BaseUpgradeCost;
        }
        else
        {
            // set Cost on the current Upgrade
            PC.Stats.Upgrades[_target].cost = (int)((PC.Stats.Upgrades[_target].cost * 1.05f) + 0.5f);
            // return calculated cost
            return PC.Stats.Upgrades[_target].cost;
        }
    }

    public int GetResearchCost(int _target)
    {
        if (PC.Stats.Researches[_target].amount == 1)
        {
            PC.Stats.Researches[_target].cost = BaseResearchCost;
            return BaseResearchCost;
        }
        else
        {
            // set cost on the current research
            PC.Stats.Researches[_target].cost = (int)((PC.Stats.Researches[_target].cost * 1.10f) + 0.5f);
            // return calculated cost
            return PC.Stats.Researches[_target].cost;
        }
    }

    #region AddManagement

    // Implement IUnityAdsListener interface methods:
    public void AdDidFinish()
    {
        if (OfflineBonusWindow.activeSelf)
        {
            TakeOfflineBonus(2);
        }
        // gameoverwindow is open
        else
        {
            // increase Currency
            PC.Stats.Currency.data += PC.GetReward();

            // restart the game
            RestartGame();
        }

        // save playerstats
        SaveSystem.SavePlayerStats(PC.Stats);

        CreateAndLoadRewardedAd();
    }

    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        // MonoBehaviour.print("HandleRewardedAdLoaded event received");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        // MonoBehaviour.print(
        //     "HandleRewardedAdFailedToLoad event received with message: "
        //                      + args.Message);
        // 
        CreateAndLoadRewardedAd();
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        // MonoBehaviour.print("HandleRewardedAdOpening event received");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        // MonoBehaviour.print(
        //     "HandleRewardedAdFailedToShow event received with message: "
        //                      + args.Message);

        CreateAndLoadRewardedAd();
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        CreateAndLoadRewardedAd();
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        if (OfflineBonusWindow.activeSelf)
        {
            TakeOfflineBonus(2);
        }
        // gameoverwindow is open
        else
        {
            // increase Currency
            PC.Stats.Currency.data += PC.GetReward();

            // restart the game
            RestartGame();
        }

        // save playerstats
        SaveSystem.SavePlayerStats(PC.Stats);

        CreateAndLoadRewardedAd();
    }

    public void PlayAdd()
    {
        if (DeveloperMode)
        {
            if (OfflineBonusWindow.activeSelf)
            {
                TakeOfflineBonus(2);
            }
            else
            {
                PC.Stats.Currency.data += PC.GetReward();
                RestartGame();
            }
        }
        else
        {
            rewardedAd.Show();
        }
    }

    private void CreateAndLoadRewardedAd()
    {
        rewardedAd = new RewardedAd(advertismentID);
        // rewardedAd = new RewardedAd(testAdvertismentID);

        // Called when an ad request has successfully loaded.
        rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        // create an empty ad request
        AdRequest request = new AdRequest.Builder().Build();

        // load the rewarded ad with the request
        rewardedAd.LoadAd(request);
    }

    #endregion

    #region Offlinebonus

    private void OnApplicationQuit()
    {
        PC.Stats.time = DateTime.UtcNow;

        SaveSystem.SavePlayerStats(PC.Stats);
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            PC.Stats.time = DateTime.UtcNow;

            SaveSystem.SavePlayerStats(PC.Stats);
        }
        else
        {
            double time = (DateTime.UtcNow - PC.Stats.time).TotalMinutes;

            if (time >= MinOfflineTime)
            {
                if (time > 1440) time = 1440;

                offlinebonus = (int)(time * OfflineBonusPerMinute * PC.Stats.OfflineBonus);

                OfflineBonusAmount.text = offlinebonus.ToString();

                if (offlinebonus > 0)
                {
                    uiManager.CloseColorpicker(false);
                    uiManager.CloseCurrencyChangeWindow(false);
                    uiManager.CloseDataShop(false);
                    uiManager.CloseOptions(false);
                    uiManager.CloseShop(false);

                    OfflineBonusWindow.SetActive(true);
                }
            }
        }
    }

    public void TakeOfflineBonus(float _multiplier)
    {
        OfflineBonusWindow.SetActive(false);

        int bonus = offlinebonus;

        bonus = (int)(bonus * _multiplier);

        PC.Stats.Currency.data += bonus;

        uiManager.DataAmount.text = PC.Stats.Currency.data.ToString();

        offlinebonus = 0;
    }

    #endregion

    public void PrestigePlayer()
    {
        // increase prestige
        PC.Stats.prestige++;
        PC.Stats.isPrestigeable = false;

        // reset playerstats
        USystem.ResetPlayerStats(PC.Stats, this);

        SaveSystem.SavePlayerStats(PC.Stats);
    }

    public void AbortFlight()
    {
        OptionsMenu.SetActive(false);

        Time.timeScale = 1f;
        PC.DecreaseHealth(5000000000f, PlayerController.DamageTyp.FIX);

        uiManager.PauseImage.sprite = uiManager.PauseSprite;
    }

    public void OpenPatreon()
    {
        Application.OpenURL(PatreonURL);

        if (uiManager.buttonsound.Source != null)
            uiManager.buttonsound.Source.Play();
    }
}
