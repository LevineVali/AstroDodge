using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// Testing TestParticle?
    /// </summary>
    public bool ParticleTest = false;

    /// <summary>
    /// Particlesystem in test
    /// </summary>
    public GameObject TestParticle;

    [Space(10)]
    /// <summary>
    /// Gradient for change particlecolor when prestigerockets are in use
    /// </summary>
    public Gradient PrestigeParticleColor;

    [Space(10)]
    /// <summary>
    /// Front of the Rocket
    /// </summary>
    public SpriteRenderer Front;

    /// <summary>
    /// Back of the rocket
    /// </summary>
    public SpriteRenderer Back;

    /// <summary>
    /// Particleobject for dmg/fuel-"dmg"
    /// </summary>
    public GameObject[] DmgParticle;

    /// <summary>
    /// Count of DmgParticle effekts for fuelsteps
    /// </summary>
    [Range(0, 10)]
    public int FuelDmgCount = 5;

    /// <summary>
    /// Particleobject for Destruction
    /// </summary>
    public GameObject[] DestructionParticle;

    /// <summary>
    /// index for DmgParticle and DestructionParticle
    /// </summary>
    public int DestructionID;

    /// <summary>
    /// fuelDmg?
    /// </summary>
    public bool FuelDmg;

    /// <summary>
    /// List of all Particletypes for Playerskins
    /// </summary>
    public GameObject[] FireParticles;

    /// <summary>
    /// UI text for showing current score at game over panel
    /// </summary>
    public Text CurrentScore;

    /// <summary>
    /// UI text for showing highscore at game over panel
    /// </summary>
    public Text HighScore;

    /// <summary>
    /// UI Text for showing that the player have made a new highscore
    /// </summary>
    public GameObject NewHighscore;

    /// <summary>
    /// Stats of the Player
    /// </summary>
    public PlayerStats Stats;

    /// <summary>
    /// Movementspeed of the Player
    /// </summary>
    public float BaseMovementSpeed = 5.0f;

    /// <summary>
    /// Game is running?
    /// </summary>
    public bool Run = false;

    /// <summary>
    /// Images of healthbar and fuelbar for fadeeffect
    /// </summary>
    public Image[] HealthFuelImages;

    /// <summary>
    /// Slider of Fuel
    /// </summary>
    public Slider FuelSlider;

    /// <summary>
    /// Slider of Health
    /// </summary>
    public Slider HealthSlider;

    /// <summary>
    /// Game over window
    /// </summary>
    public GameObject GameOverPanel;

    /// <summary>
    /// Currencybar on the top
    /// </summary>
    public GameObject TopBar;

    /// <summary>
    /// RewardText
    /// </summary>
    public Text Reward;

    /// <summary>
    /// Bonus for survived obsticle
    /// </summary>
    public int Survived;

    /// <summary>
    /// Upgradesystem
    /// </summary>
    public UpgradeSystem upgradeSystem;

    /// <summary>
    /// bool to kill all obstacle
    /// </summary>
    public bool Killall;

    /// <summary>
    /// Collider of the player
    /// </summary>
    public PolygonCollider2D Col;

    /// <summary>
    /// fallspeed for falling down without fuel
    /// </summary>
    public float FallSpeed;

    /// <summary>
    /// rotatespeed for action without fuel
    /// </summary>
    public float RotateSpeed;

    /// <summary>
    /// fade in speed for live and fuel
    /// </summary>
    public float fadespeed = 3;

    /// <summary>
    /// smoothness for vertikal moving
    /// </summary>
    public int VertikalMoving;

    /// <summary>
    /// is player alive?
    /// </summary>
    [HideInInspector]
    public bool Life = true;


    /// <summary>
    /// fadevalue for fadeeffect for live and fuel
    /// </summary>
    private float fadevalue;

    /// <summary>
    /// 1 to right, -1 to left
    /// </summary>
    private int moveDir;

    #region Sound
    /// <summary>
    /// AudioManager
    /// </summary>
    public AudioManager AM;

    /// <summary>
    /// list of all Music
    /// </summary>
    public Sound[] music;

    /// <summary>
    /// sound of the standing Rocket
    /// </summary>
    [HideInInspector]
    public Sound rocketStand;

    /// <summary>
    /// sound of the flying Rocket
    /// </summary>
    [HideInInspector]
    public Sound rocketFlight;

    /// <summary>
    /// sound of rocketexplosion
    /// </summary>
    private Sound rocketExplosion;

    /// <summary>
    /// sound of crashed Asteroid
    /// </summary>
    private Sound asteroidExplosion;

    /// <summary>
    /// sound of crashing cloud
    /// </summary>
    private Sound cloudSound;

    /// <summary>
    /// sound of colidet bird
    /// </summary>
    private Sound birdCollision;

    [HideInInspector]
    public bool MusicRun = false;

    #endregion

    /// <summary>
    /// Spawnmanager of this game
    /// </summary>
    private SpawnManager spawnManager;

    /// <summary>
    /// Gamemanader of this game
    /// </summary>
    private GameManager gm;

    /// <summary>
    /// direction of the movement
    /// </summary>
    private float dirX;

    /// <summary>
    /// Maincamera of the Scene
    /// </summary>
    private Camera cam;

    /// <summary>
    /// Is particle spawned?
    /// </summary>
    private bool particleSpawn;

    /// <summary>
    /// Gameobject of Explosionparticle
    /// </summary>
    private float deathtimer = 2f;

    /// <summary>
    /// timecounter for deathtimer
    /// </summary>
    private float timer;

    /// <summary>
    /// speed for falling down
    /// </summary>
    private float fallspeed;

    /// <summary>
    /// speed for rotating rocket
    /// </summary>
    private float rotatespeed;

    /// <summary>
    /// number of next fueldmg-particleeffekt
    /// </summary>
    [HideInInspector]
    public int fuelDmgCount = 1;

    /// <summary>
    /// how much fuel need to be used to trigger fueldmg-particleeffect
    /// </summary>
    private float fuelDmgValue;

    /// <summary>
    /// SkinManager of this game
    /// </summary>
    private SkinManager skinManager;

    /// <summary>
    /// Pressurewave object
    /// </summary>
    public PressureWave pressureWave;

    /// <summary>
    /// Is currentskin the lightningskin? (for unique events)
    /// </summary>
    [HideInInspector]
    public bool isLightning;

    [HideInInspector]
    /// <summary>
    /// index for cloudcoliderinfo
    /// </summary>
    public int CCindex = 0;

    /// <summary>
    /// value to multiplie soundeffects for fading sound, depends on the height of the palyer
    /// </summary>
    public float soundValueModifire;

    private void Awake()
    {
        soundValueModifire = 1f;
        MusicRun = false;
        Life = true;

        Stats.Upgrades = new Upgrade[(int)UpgradeType.MAX];
        Stats.Researches = new Research[(int)ResearchType.MAX];

        pressureWave = FindObjectOfType<PressureWave>();

        for (int i = 0; i < (int)UpgradeType.MAX; i++)
        {
            Stats.Upgrades[i].type = (UpgradeType)i;
            Stats.Upgrades[i].amount = 0;
            Stats.Upgrades[i].maxAmount = 10;
        }

        for (int i = 0; i < (int)ResearchType.MAX; i++)
        {
            Stats.Researches[i].type = (ResearchType)i;
            Stats.Researches[i].amount = 1;
        }

        cam = FindObjectOfType<Camera>();

        AM = FindObjectOfType<AudioManager>();

        music = AM.Music;
        rocketStand = Array.Find(AM.Sound, sound => sound.Name == "Rocket Stand");
        rocketFlight = Array.Find(AM.Sound, sound => sound.Name == "Rocket Flight");
        rocketExplosion = Array.Find(AM.Sound, sound => sound.Name == "Rocket Explosion");
        asteroidExplosion = Array.Find(AM.Sound, sound => sound.Name == "Asteroid Explosion");
        cloudSound = Array.Find(AM.Sound, sound => sound.Name == "Cloud");
        birdCollision = Array.Find(AM.Sound, sound => sound.Name == "BirdCollision");

        rocketStand.Source.Play();
        music[0].Source.Play();

        spawnManager = FindObjectOfType<SpawnManager>();

        gm = FindObjectOfType<GameManager>();

        Col = GetComponent<PolygonCollider2D>();

        skinManager = FindObjectOfType<SkinManager>();
    }

    private void Update()
    {
        // game is running
        if (Run)
        {
            if (fuelDmgCount == 0)
            {
                fuelDmgCount = 1;
            }

            // calculate fueldmgvalue
            fuelDmgValue = Stats.maxFuel / (FuelDmgCount + 1);

            // calculate fadevalues
            if (fadevalue < 1)
            {
                fadevalue += Time.deltaTime / fadespeed;
            }

            if (Stats.currentSpeed < Stats.maxSpeed && Stats.fuel > 0)
            {
                float value = Stats.acceleration * Time.deltaTime;

                if (value > Stats.maxSpeed - Stats.currentSpeed)
                {
                    Stats.currentSpeed = Stats.maxSpeed;
                }
                else
                {
                    Stats.currentSpeed += value;
                }
            }

            // show fire
            FireParticles[Stats.activeParticle].SetActive(true);

            if (Stats.fuel > 0 && Stats.currentScore >= 10)
            {
                // get %-value of maxspeed
                float mul = Stats.currentSpeed / (50 * Stats.slide);
                if (mul > 1) mul = 1;

                // touchcontrol
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    Vector3 touchPosition = cam.ScreenToWorldPoint(touch.position);

                    if (transform.position.x - touchPosition.x < BaseMovementSpeed * Stats.slide * mul * Time.deltaTime &&
                        transform.position.x - touchPosition.x > -BaseMovementSpeed * Stats.slide * mul * Time.deltaTime)
                    {
                        transform.position = new Vector3(touchPosition.x, transform.position.y, 0);
                    }
                    else if (transform.position.x - touchPosition.x > BaseMovementSpeed * Stats.slide * mul * Time.deltaTime)
                    {
                        dirX = -1 * BaseMovementSpeed * Stats.slide * mul * Time.deltaTime;
                        transform.Translate(new Vector2(dirX, 0));
                    }
                    else if (transform.position.x - touchPosition.x < -BaseMovementSpeed * Stats.slide * mul * Time.deltaTime)
                    {
                        dirX = 1 * BaseMovementSpeed * Stats.slide * mul * Time.deltaTime;
                        transform.Translate(new Vector2(dirX, 0));
                    }
                }
                // keybordcontrol
                else
                {
                    if (Input.GetKey(KeyCode.LeftArrow))
                    {
                        moveDir = -1;
                    }
                    else if (Input.GetKey(KeyCode.RightArrow))
                    {
                        moveDir = 1;
                    }
                    else
                    {
                        moveDir = 0;
                    }

                    // calculate direction of the playermovement
                    dirX = moveDir * BaseMovementSpeed * Stats.slide * mul * Time.deltaTime;

                    // if player will move left
                    if (dirX < 0)
                    {
                        // if player is on the left border
                        if (transform.position.x <= -0.5f)
                        {
                            // reset direction
                            dirX = 0;
                        }
                    }

                    // if player will move right
                    if (dirX > 0)
                    {
                        // if player is on the right border
                        if (transform.position.x >= 0.5f)
                        {
                            // reset direction
                            dirX = 0;
                        }
                    }

                    // move the player
                    transform.Translate(new Vector2(dirX, 0));
                }

                if (Life)
                {
                    // move player higher in Atmosphere
                    if (Stats.currentScore >= 0 && Stats.currentScore < spawnManager.Cloudblanket1 && transform.position.y != -.3f)
                    {
                        Vector3 pos = transform.position;
                        pos.y += Stats.currentSpeed / VertikalMoving * Time.deltaTime;

                        if (pos.y > -.3f)
                        {
                            pos.y = -.3f;
                            transform.position = pos;
                        }
                        else
                        {
                            transform.position = pos;
                        }
                    }
                    // move player back in space
                    else
                    {
                        if (Stats.currentScore >= spawnManager.AsteroidStage1 && transform.position.y != -.5f)
                        {
                            Vector3 pos = transform.position;
                            pos.y -= Stats.currentSpeed / VertikalMoving * Time.deltaTime;

                            if (pos.y < -.5f)
                            {
                                pos.y = -.5f;
                                transform.position = pos;
                            }
                            else
                            {
                                transform.position = pos;
                            }
                        }
                    }
                }
            }

            // increase the hight
            Stats.currentScore += Stats.currentSpeed * Time.deltaTime;

            if (Stats.fuel > 0)
            {
                // decrease Fuel
                DecreaseFuel();
            }

            if (Stats.currentSpeed > Stats.maxSpeedWOD && Stats.currentScore < spawnManager.AsteroidStage1)
            {
                float dmg = (Stats.currentSpeed - Stats.maxSpeedWOD) / (upgradeSystem.BaseSpeed);

                DecreaseHealth(dmg, DamageTyp.FIX);
            }

            if (Stats.health <= 0)
            {
                if (timer < deathtimer)
                {
                    Stats.currentSpeed = 0;
                    timer += Time.deltaTime;
                }
                else
                {
                    Run = false;

                    // show gameover window
                    GameOverPanel.SetActive(true);

                    CurrentScore.text = ((int)Stats.currentScore).ToString();

                    // new highscore?
                    if (Stats.currentScore > Stats.highScore)
                    {
                        NewHighscore.SetActive(true);
                        Stats.highScore = Stats.currentScore;
                    }
                    else
                    {
                        NewHighscore.SetActive(false);
                    }

                    HighScore.text = ((int)Stats.highScore).ToString();

                    // show Reward
                    Reward.text = GetReward().ToString();
                }
            }
            else if (Stats.fuel <= 0)
            {
                if (rocketStand.Source.isPlaying)
                {
                    rocketStand.Source.Stop();
                }

                if (timer < deathtimer + 3.1f)
                {
                    timer += Time.deltaTime;
                    float tmpspeed = Stats.maxSpeed / deathtimer * Time.deltaTime;
                    if (Stats.currentSpeed < tmpspeed)
                    {
                        Stats.currentSpeed = 0;
                        fallspeed += fallspeed * Time.deltaTime + FallSpeed * Time.deltaTime;
                        transform.Translate(new Vector3(0, -fallspeed, 0), Space.World);

                        rotatespeed += rotatespeed * Time.deltaTime + RotateSpeed * Time.deltaTime;

                        // if player is left from middlepoint of screen
                        if (transform.position.x < 0)
                            transform.Rotate(new Vector3(0, 0, rotatespeed));
                        // if player is right from middlepoint of screen
                        if (transform.position.x > 0)
                            transform.Rotate(new Vector3(0, 0, -rotatespeed));
                    }
                    else
                    {
                        Stats.currentSpeed -= tmpspeed;
                    }
                }
                else
                {
                    if (Run)
                    {
                        Run = false;

                        // show gameover window
                        GameOverPanel.SetActive(true);

                        CurrentScore.text = ((int)Stats.currentScore).ToString();

                        // new highscore?
                        if (Stats.currentScore > Stats.highScore)
                        {
                            Stats.highScore = Stats.currentScore;
                        }
                        else
                        {
                            NewHighscore.SetActive(false);
                        }

                        HighScore.text = ((int)Stats.highScore).ToString();

                        // show Reward
                        Reward.text = GetReward().ToString();
                    }
                }
            }
        }
        else
        {
            // calculate fadevalue
            if (fadevalue > 0)
            {
                fadevalue -= Time.deltaTime / fadespeed;
            }

            if (Input.touchCount > 0)
            {
                GameOverPanel.transform.position = new Vector3(540, 960, 0);
                TopBar.transform.position = new Vector3(540, 1860, 0);
            }

            Stats.currentSpeed = 0;
            timer = 0;
            fallspeed = 0;
            rotatespeed = 0;

            if (particleSpawn)
            {
                particleSpawn = false;
            }

            // hide fire
            FireParticles[Stats.activeParticle].SetActive(false);

            ParticleSystem ps = FireParticles[Stats.activeParticle].GetComponentInChildren<ParticleSystem>();
            var psmain = ps.main;

            ParticleSystem.MinMaxGradient g = ps.main.startColor;
            GradientAlphaKey[] alphakeys =
                    {
                        new GradientAlphaKey(1, 0),
                        new GradientAlphaKey(1, 1)
                    };
            g.gradient.SetKeys(g.gradient.colorKeys, alphakeys);
            psmain.startColor = g;
        }

        // Fadeeffect for healthbar and fuelbar
        if (fadevalue != 0 && fadevalue != fadespeed)
        {
            for (int i = 0; i < HealthFuelImages.Length; i++)
            {
                if (i < 2)
                    HealthFuelImages[i].color = new Color(1, 1, 1, fadevalue);
                else
                    HealthFuelImages[i].color = new Color(1, 1, 1, fadevalue * 0.20f);
            }
        }

        if (fadevalue > fadespeed)
        {
            fadevalue = fadespeed;
        }
        else if (fadevalue < 0)
        {
            fadevalue = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (transform.position.y > -1)
        {
            if (ParticleTest)
            {
                Instantiate(TestParticle, transform.position, Quaternion.identity);
            }

            // if player collide with asteroid or comet
            if (collision.collider.CompareTag("Asteroid") || collision.collider.CompareTag("Comet"))
            {
                // make tail invisible
                if (collision.gameObject.GetComponentInChildren<ParticleSystem>() != null)
                {
                    var main = collision.gameObject.GetComponentInChildren<ParticleSystem>().main;
                    main.startColor = new Color(0, 0, 0, 0);
                }

                // get circlecollider
                var circle = collision.gameObject.GetComponent<CircleCollider2D>();
                if (circle != null)
                {
                    // disable it
                    circle.enabled = false;
                }

                // get capsulecollider
                var capsle = collision.gameObject.GetComponent<CapsuleCollider2D>();
                if (capsle != null)
                {
                    // disable it
                    capsle.enabled = false;
                }

                if (collision.collider.CompareTag("Asteroid"))
                {
                    Vector3 pos = collision.gameObject.transform.position;
                    GameObject go = Instantiate(spawnManager.Particles[2], pos, Quaternion.identity);
                    go.GetComponent<AsteroidParticle>().pcolor = collision.gameObject.GetComponentInChildren<SpriteRenderer>().color;

                    DecreaseHealth(70, DamageTyp.NORMAL);

                    int id = 0;
                    for (int i = 0; i < skinManager.Skins.Length; i++)
                    {
                        if (skinManager.Skins[i].currencyType == SkinManager.CurrencyType.ASTEROID) { id = i; break; }
                    }

                    if (Stats.asteroidCount < skinManager.Skins[id].cost)
                    {
                        Stats.asteroidCount++;
                        CollectableCheck(SkinManager.CurrencyType.ASTEROID, id);
                    }

                    // play asteroid crash sound
                    asteroidExplosion.Source.Play();
                }
                if (collision.collider.CompareTag("Comet"))
                {
                    Vector3 pos = collision.gameObject.transform.position;
                    GameObject go = Instantiate(spawnManager.Particles[3], pos, Quaternion.identity);
                    go.GetComponent<AsteroidParticle>().pcolor = collision.gameObject.GetComponentInChildren<SpriteRenderer>().color;

                    DecreaseHealth(95, DamageTyp.NORMAL);

                    int id = 0;
                    for (int i = 0; i < skinManager.Skins.Length; i++)
                    {
                        if (skinManager.Skins[i].currencyType == SkinManager.CurrencyType.COMET) { id = i; break; }
                    }

                    if (Stats.cometCount < skinManager.Skins[id].cost)
                    {
                        Stats.cometCount++;
                        CollectableCheck(SkinManager.CurrencyType.COMET, id);
                    }
                }

                // make Asteroiod/Comet invisible
                collision.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.clear;

                // destroy gameobject with delay
                Destroy(collision.gameObject, 5f);
            }

            // if player collide with bird
            if (collision.collider.CompareTag("Bird"))
            {
                // decrase player health
                DecreaseHealth(15, DamageTyp.NORMAL);

                // play death-animation
                Vector3 pos = collision.gameObject.transform.position;

                int id = 0;
                if (isLightning)
                {
                    collision.collider.GetComponent<Bird>().grilled = true;

                    for (int i = 0; i < skinManager.Skins.Length; i++)
                    {
                        if (skinManager.Skins[i].currencyType == SkinManager.CurrencyType.COOKED_BIRD) { id = i; break; }
                    }

                    if (Stats.birdCount < skinManager.Skins[id].cost)
                    {
                        Stats.birdCount++;
                        CollectableCheck(SkinManager.CurrencyType.COOKED_BIRD, id);
                    }
                }
                else
                {
                    // spawn 6x feather particle
                    for (int i = 0; i < 6; i++)
                        Instantiate(spawnManager.Particles[4], pos, Quaternion.identity);

                    // destroy gameobject with delay
                    Destroy(collision.gameObject);

                    for (int i = 0; i < skinManager.Skins.Length; i++)
                    {
                        if (skinManager.Skins[i].currencyType == SkinManager.CurrencyType.BIRD) { id = i; break; }
                    }

                    if (Stats.birdCount < skinManager.Skins[id].cost)
                    {
                        Stats.birdCount++;
                        CollectableCheck(SkinManager.CurrencyType.BIRD, id);
                    }

                    birdCollision.Source.Play();
                }
            }

            // if player collide with clouds
            if (collision.collider.CompareTag("CloudBottom") || collision.collider.CompareTag("CloudLeft") || collision.collider.CompareTag("CloudRight") || collision.collider.CompareTag("CloudBig"))
            {
                int index;

                Cloud cloud = collision.collider.gameObject.GetComponent<Cloud>();
                if (cloud == null)
                    cloud = collision.collider.gameObject.GetComponentInParent<Cloud>();

                if (collision.collider.CompareTag("CloudBig"))
                {
                    index = 0;
                }
                else
                {
                    index = 1;
                }

                GameObject go = Instantiate(spawnManager.Particles[index], new Vector3(transform.position.x, transform.position.y + .16f, 0), Quaternion.identity);
                go.GetComponent<CloudBlanketParticle>().PlayerPosition = transform.position;
                CloudBlanketParticle cbp = go.GetComponent<CloudBlanketParticle>();

                int id = 0;
                for (int i = 0; i < skinManager.Skins.Length; i++)
                {
                    if (skinManager.Skins[i].currencyType == SkinManager.CurrencyType.CLOUD) { id = i; break; }
                }

                if (cloud != null)
                {
                    cloud.DeactivateCollider();
                    if (cloud.Typ == Cloud.CloudTyp.DANGER)
                    {
                        List<GradientColorKey> keys = new List<GradientColorKey>();
                        for (int i = 0; i < cbp.FadeColor.colorKeys.Length; i++)
                        {
                            keys.Add(new GradientColorKey(cloud.DangerColor, cbp.FadeColor.colorKeys[i].time));
                        }
                        cbp.FadeColor.SetKeys(keys.ToArray(), cbp.FadeColor.alphaKeys);
                        DecreaseHealth(10, DamageTyp.FIX);
                    }
                    else if (cloud.Typ == Cloud.CloudTyp.DUMMY)
                    {
                        List<GradientColorKey> keys = new List<GradientColorKey>();
                        for (int i = 0; i < cbp.FadeColor.colorKeys.Length; i++)
                        {
                            keys.Add(new GradientColorKey(cloud.DummyColor, cbp.FadeColor.colorKeys[i].time));
                        }
                        cbp.FadeColor.SetKeys(keys.ToArray(), cbp.FadeColor.alphaKeys);
                    }
                    else
                    {
                        if (Stats.cloudCount < skinManager.Skins[id].cost)
                        {
                            Stats.cloudCount++;
                            CollectableCheck(SkinManager.CurrencyType.CLOUD, id);
                        }
                    }
                }
                else
                {
                    if (Stats.cloudCount < skinManager.Skins[id].cost)
                    {
                        Stats.cloudCount++;
                        CollectableCheck(SkinManager.CurrencyType.CLOUD, id);
                    }
                }

                // play cloudsound
                cloudSound.Source.Play();

                // if player collide with left side of the cloud
                if (collision.collider.CompareTag("CloudLeft"))
                {
                    for (int i = 0; i < cbp.particlesLeft.Length; i++)
                    {
                        cbp.particlesLeft[i].gameObject.SetActive(false);
                    }
                }

                // if player collide with right side of the cloud
                else if (collision.collider.CompareTag("CloudRight"))
                {
                    for (int i = 0; i < cbp.particlesRight.Length; i++)
                    {
                        cbp.particlesRight[i].gameObject.SetActive(false);
                    }
                }
            }

            if (collision.collider.CompareTag("Duck"))
            {
                collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                collision.gameObject.GetComponent<Enemie>().IsCollected = true;

                int id = 0;
                for (int i = 0; i < skinManager.Skins.Length; i++)
                {
                    if (skinManager.Skins[i].currencyType == SkinManager.CurrencyType.DUCK) { id = i; break; }
                }

                if (Stats.duckCount < skinManager.Skins[id].cost)
                {
                    Stats.duckCount++;
                    CollectableCheck(SkinManager.CurrencyType.DUCK, id);
                }
            }

            if (collision.collider.CompareTag("Firework"))
            {
                collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                collision.gameObject.GetComponent<Enemie>().IsCollected = true;

                int id = 0;
                for (int i = 0; i < skinManager.Skins.Length; i++)
                {
                    if (skinManager.Skins[i].currencyType == SkinManager.CurrencyType.FIREWORK) { id = i; break; }
                }

                if (Stats.fireworksCount < skinManager.Skins[id].cost)
                {
                    Stats.fireworksCount++;
                    CollectableCheck(SkinManager.CurrencyType.FIREWORK, id);
                }
            }

            if (collision.collider.CompareTag("Ufo"))
            {
                collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                collision.gameObject.GetComponent<Enemie>().IsCollected = true;

                int id = 0;
                for (int i = 0; i < skinManager.Skins.Length; i++)
                {
                    if (skinManager.Skins[i].currencyType == SkinManager.CurrencyType.UFO) { id = i; break; }
                }

                if (Stats.ufoCount < skinManager.Skins[id].cost)
                {
                    Stats.ufoCount++;
                    CollectableCheck(SkinManager.CurrencyType.UFO, id);
                }
            }

            if (collision.collider.CompareTag("Burger"))
            {
                collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                collision.gameObject.GetComponent<Enemie>().IsCollected = true;

                int id = 0;
                for (int i = 0; i < skinManager.Skins.Length; i++)
                {
                    if (skinManager.Skins[i].currencyType == SkinManager.CurrencyType.BURGER) { id = i; break; }
                }

                if (Stats.burgerCount < skinManager.Skins[id].cost)
                {
                    Stats.burgerCount++;
                    CollectableCheck(SkinManager.CurrencyType.BURGER, id);
                }
            }

            if (collision.collider.CompareTag("Sandwich"))
            {
                collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                collision.gameObject.GetComponent<Enemie>().IsCollected = true;

                int id = 0;
                for (int i = 0; i < skinManager.Skins.Length; i++)
                {
                    if (skinManager.Skins[i].currencyType == SkinManager.CurrencyType.SANDWICH) { id = i; break; }
                }

                if (Stats.sandwichCount < skinManager.Skins[id].cost)
                {
                    Stats.sandwichCount++;
                    CollectableCheck(SkinManager.CurrencyType.SANDWICH, id);
                }
            }

            if (collision.collider.CompareTag("Present"))
            {
                collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                collision.gameObject.GetComponent<Enemie>().IsCollected = true;

                int id = 0;
                for (int i = 0; i < skinManager.Skins.Length; i++)
                {
                    if (skinManager.Skins[i].currencyType == SkinManager.CurrencyType.PRESENT) { id = i; break; }
                }

                if (Stats.presentCount < skinManager.Skins[id].cost)
                {
                    Stats.presentCount++;
                    CollectableCheck(SkinManager.CurrencyType.PRESENT, id);
                }
            }

            if (collision.collider.CompareTag("Teddy"))
            {
                collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                collision.gameObject.GetComponent<Enemie>().IsCollected = true;

                int id = 0;
                for (int i = 0; i < skinManager.Skins.Length; i++)
                {
                    if (skinManager.Skins[i].currencyType == SkinManager.CurrencyType.TEDDY) { id = i; break; }
                }

                if (Stats.teddyCount < skinManager.Skins[id].cost)
                {
                    Stats.teddyCount++;
                    CollectableCheck(SkinManager.CurrencyType.TEDDY, id);
                }
            }

            if (collision.collider.CompareTag("Lightning"))
            {
                Destroy(collision.gameObject);

                DecreaseHealth(42, DamageTyp.FIX);

                int id = 0;
                for (int i = 0; i < skinManager.Skins.Length; i++)
                {
                    if (skinManager.Skins[i].currencyType == SkinManager.CurrencyType.ELECTRIC) { id = i; break; }
                }

                if (Stats.electricCount < skinManager.Skins[id].cost)
                {
                    Stats.electricCount++;
                    CollectableCheck(SkinManager.CurrencyType.ELECTRIC, id);
                }
            }

            if (collision.collider.CompareTag("Water"))
            {
                collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                collision.gameObject.GetComponent<Enemie>().IsCollected = true;

                int id = 0;
                for (int i = 0; i < skinManager.Skins.Length; i++)
                {
                    if (skinManager.Skins[i].currencyType == SkinManager.CurrencyType.WATER) { id = i; break; }
                }

                if (Stats.waterCount < skinManager.Skins[id].cost)
                {
                    Stats.waterCount++;
                    CollectableCheck(SkinManager.CurrencyType.WATER, id);
                }
            }

            if (collision.collider.CompareTag("Soda"))
            {
                collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                collision.gameObject.GetComponent<Enemie>().IsCollected = true;

                int id = 0;
                for (int i = 0; i < skinManager.Skins.Length; i++)
                {
                    if (skinManager.Skins[i].currencyType == SkinManager.CurrencyType.SODA) { id = i; break; }
                }

                if (Stats.sodaCount < skinManager.Skins[id].cost)
                {
                    Stats.sodaCount++;
                    CollectableCheck(SkinManager.CurrencyType.SODA, id);
                }
            }
        }
    }

    private void DecreaseFuel()
    {
        Stats.fuel -= upgradeSystem.BaseWeight / Stats.weight * Stats.currentSpeed / 10 * Time.deltaTime;

        float usedFuel = Stats.fuel / Stats.maxFuel;

        if (FuelSlider.value - usedFuel > 0.05f)
            FuelSlider.value = usedFuel;

        usedFuel = Stats.maxFuel - Stats.fuel;

        if (FuelDmg)
        {
            if (usedFuel >= fuelDmgValue * fuelDmgCount && fuelDmgCount != (FuelDmgCount + 1))
            {
                GameObject go = Instantiate(DmgParticle[DestructionID], transform.position, Quaternion.identity);
                go.GetComponent<AsteroidParticle>().pcolor = Front.color;
                fuelDmgCount++;
            }
        }

        if (Stats.fuel <= 0)
        {
            ParticleSystem ps = FireParticles[Stats.activeParticle].GetComponentInChildren<ParticleSystem>();
            var psmain = ps.main;

            ParticleSystem.MinMaxGradient g = ps.main.startColor;
            GradientAlphaKey[] alphakeys =
            {
                        new GradientAlphaKey(0, 0),
                        new GradientAlphaKey(0, 1)
                    };
            g.gradient.SetKeys(g.gradient.colorKeys, alphakeys);
            psmain.startColor = g;

            rocketFlight.Source.Stop();

            FuelSlider.value = 0f;
        }
    }

    public void DecreaseHealth(float _dmg, DamageTyp _typ)
    {
        switch (_typ)
        {
            case DamageTyp.NORMAL:
                Stats.health -= _dmg * ((Stats.currentSpeed / upgradeSystem.BaseSpeed / 2) + .5f);
                break;
            case DamageTyp.FIX:
                Stats.health -= _dmg;
                break;
        }

        HealthSlider.value = Stats.health / Stats.maxHealth;

        if (Stats.health <= 0)
        {
            // player is dead :(
            Life = false;

            // play explosionsound
            rocketExplosion.Source.Play();
            rocketFlight.Source.Stop();

            GameObject go = Instantiate(DestructionParticle[DestructionID], transform.position, Quaternion.identity);
            AsteroidParticle ap = go.GetComponent<AsteroidParticle>();

            // if player is not in space
            if (Stats.currentScore < spawnManager.AsteroidStage1)
            {
                // activate pressurewave
                ap.PressureWave = true;
                ap.PressureWaveObject = pressureWave;
            }
            // otherwise
            else
            {
                // deactivate pressurewave
                ap.PressureWave = false;
            }

            if (Stats.activeSkin < 0)
            {
                float time = (-Stats.activeSkin / 10f) - 0.05f;
                ap.pcolor = PrestigeParticleColor.Evaluate(time);
            }
            else
            {
                ap.pcolor = Front.color;
            }

            int index = 0;
            for (int i = 0; i < Stats.skinIDs.Count; i++)
            {
                if (Stats.activeSkin == Stats.skinIDs[i].id)
                {
                    index = i;
                    break;
                }
            }

            if (skinManager.Skins[Stats.skinIDs[index].id].colorableExplosion)
            {
                ParticleSystem ps = go.GetComponentInChildren<ParticleSystem>();
                ps.Stop();
                var psmain = ps.main;

                ParticleSystem.ColorOverLifetimeModule pcol = ps.colorOverLifetime;
                pcol.color = skinManager.Skins[Stats.skinIDs[index].id].particleColor;

                Gradient gradient = new Gradient();
                GradientColorKey[] colorkeys = new GradientColorKey[pcol.color.gradient.colorKeys.Length];
                for (int j = 0; j < pcol.color.gradient.colorKeys.Length; j++)
                {
                    Color c = new Color()
                    {
                        r = Stats.skinIDs[index].color.r,
                        g = Stats.skinIDs[index].color.g,
                        b = Stats.skinIDs[index].color.b,
                        a = Stats.skinIDs[index].color.a
                    };

                    GradientColorKey key = new GradientColorKey()
                    {
                        color = Color.Lerp(pcol.color.gradient.colorKeys[j].color, c, skinManager.Skins[Stats.skinIDs[index].id].colorableExplosionStrength),
                        time = pcol.color.gradient.colorKeys[j].time
                    };

                    colorkeys[j] = key;
                }
                gradient.SetKeys(colorkeys, pcol.color.gradient.alphaKeys);
                pcol.color = gradient;

                pcol.enabled = true;

                ParticleSystem.MinMaxGradient g = ps.main.startColor;
                g.gradient = new Gradient();

                psmain.startColor = g;
                ps.Play();
            }

            // unique changes
            if (skinManager.Skins[Stats.activeSkin].Name == "Teddy")
            {
                for (int i = 0; i < ap.Particles.Length; i++)
                {
                    ap.Particles[i].colorChangable = true;
                }

                ap.pcolor = new Color()
                {
                    r = 105f / 255f,
                    g = 46f / 255f,
                    b = 8f / 255f,
                    a = 1
                };
            }
            if (skinManager.Skins[Stats.activeSkin].Name == "Soda")
            {
                ap.pcolor = new Color()
                {
                    r = 198f / 255f,
                    g = 62f / 255f,
                    b = 62f / 255f,
                    a = 1
                };
            }
            if (skinManager.Skins[Stats.activeSkin].Name == "Firework")
            {
                ap.pcolor = new Color()
                {
                    r = 31f / 255f,
                    g = 6f / 255f,
                    b = 47f / 255f,
                    a = 1
                };
            }

            particleSpawn = true;
            transform.position = new Vector3(0, -2, 0);
            FireParticles[Stats.activeParticle].SetActive(false);
            timer = 0;

            HealthSlider.value = 0f;
        }
    }

    public int GetReward()
    {
        if (gm.TestPlayerMode)
            return (int)(((Stats.currentScore / 10f * gm.EarnMultiplier) + (int)(Survived * gm.EarnMultiplier)) * (1f + (Stats.prestige / 10f)));
        else
            return (int)(((Stats.currentScore / 10f) + Survived) * (1f + (Stats.prestige / 10f)));
    }

    public void CollectableCheck(SkinManager.CurrencyType _currencyType, int _id)
    {
        bool complete = false;

        switch (_currencyType)
        {
            case SkinManager.CurrencyType.COMET:
                if (Stats.cometCount >= skinManager.Skins[_id].cost)
                {
                    complete = true;
                }
                break;
            case SkinManager.CurrencyType.ASTEROID:
                if (Stats.asteroidCount >= skinManager.Skins[_id].cost)
                {
                    complete = true;
                }
                break;
            case SkinManager.CurrencyType.BIRD:
                if (Stats.birdCount >= skinManager.Skins[_id].cost)
                {
                    complete = true;
                }
                break;
            case SkinManager.CurrencyType.COOKED_BIRD:
                if (Stats.cookedbirdCount >= skinManager.Skins[_id].cost)
                {
                    complete = true;
                }
                break;
            case SkinManager.CurrencyType.CLOUD:
                if (Stats.cloudCount >= skinManager.Skins[_id].cost)
                {
                    complete = true;
                }
                break;
            case SkinManager.CurrencyType.DUCK:
                if (Stats.duckCount >= skinManager.Skins[_id].cost)
                {
                    spawnManager.Duck_Done = true;
                    Stats.tracks.Remove(Track.DUCK);
                    skinManager.Tracked_Icons[_id].SetActive(false);
                    complete = true;
                }
                break;
            case SkinManager.CurrencyType.FIREWORK:
                if (Stats.fireworksCount >= skinManager.Skins[_id].cost)
                {
                    spawnManager.Firework_Done = true;
                    Stats.tracks.Remove(Track.FIREWORK);
                    skinManager.Tracked_Icons[_id].SetActive(false);
                    complete = true;
                }
                break;
            case SkinManager.CurrencyType.UFO:
                if (Stats.ufoCount >= skinManager.Skins[_id].cost)
                {
                    spawnManager.Ufo_Done = true;
                    Stats.tracks.Remove(Track.UFO);
                    skinManager.Tracked_Icons[_id].SetActive(false);
                    complete = true;
                }
                break;
            case SkinManager.CurrencyType.BURGER:
                if (Stats.burgerCount >= skinManager.Skins[_id].cost)
                {
                    spawnManager.Burger_Done = true;
                    Stats.tracks.Remove(Track.BURGER);
                    skinManager.Tracked_Icons[_id].SetActive(false);
                    complete = true;
                }
                break;
            case SkinManager.CurrencyType.SANDWICH:
                if (Stats.sandwichCount >= skinManager.Skins[_id].cost)
                {
                    spawnManager.Sandwich_Done = true;
                    Stats.tracks.Remove(Track.SANDWICH);
                    skinManager.Tracked_Icons[_id].SetActive(false);
                    complete = true;
                }
                break;
            case SkinManager.CurrencyType.PRESENT:
                if (Stats.presentCount >= skinManager.Skins[_id].cost)
                {
                    spawnManager.Present_Done = true;
                    Stats.tracks.Remove(Track.PRESENT);
                    skinManager.Tracked_Icons[_id].SetActive(false);
                    complete = true;
                }
                break;
            case SkinManager.CurrencyType.TEDDY:
                if (Stats.teddyCount >= skinManager.Skins[_id].cost)
                {
                    spawnManager.Teddy_Done = true;
                    Stats.tracks.Remove(Track.TEDDY);
                    skinManager.Tracked_Icons[_id].SetActive(false);
                    complete = true;
                }
                break;
            case SkinManager.CurrencyType.ELECTRIC:
                if (Stats.electricCount >= skinManager.Skins[_id].cost)
                {
                    complete = true;
                }
                break;
            case SkinManager.CurrencyType.WATER:
                if (Stats.waterCount >= skinManager.Skins[_id].cost)
                {
                    spawnManager.Water_Done = true;
                    Stats.tracks.Remove(Track.WATER);
                    skinManager.Tracked_Icons[_id].SetActive(false);
                    complete = true;
                }
                break;
            case SkinManager.CurrencyType.SODA:
                if (Stats.sodaCount >= skinManager.Skins[_id].cost)
                {
                    spawnManager.Soda_Done = true;
                    Stats.tracks.Remove(Track.SODA);
                    skinManager.Tracked_Icons[_id].SetActive(false);
                    complete = true;
                }
                break;
            default:
                break;
        }

        if (complete)
        {
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
                id = skinManager.Skins[_id].ID,
                color = sc
            };

            // add new skin
            Stats.skinIDs.Add(skin);

            // set new skin as bought
            skinManager.Skins[_id].bought = true;

            // set next track
            skinManager.RemoveTrack(_id);
        }
    }

    public void GetCloudParticleDetailIndex(string _skinName)
    {
        switch (_skinName)
        {
            case "StarterRocket":
                CCindex = 0;
                break;
            case "SimpleRocket":
                CCindex = 1;
                break;
            case "Barrel":
                CCindex = 2;
                break;
            case "Bullet":
                CCindex = 3;
                break;
            case "Duck":
                CCindex = 4;
                break;
            case "UFO":
                CCindex = 5;
                break;
            case "Cloud":
                CCindex = 6;
                break;
            case "Asteroid":
                CCindex = 7;
                break;
            case "Comet":
                CCindex = 8;
                break;
            case "BoneBird":
                CCindex = 9;
                break;
            case "CookedBird":
                CCindex = 10;
                break;
            case "Spaceshuttle":
                CCindex = 11;
                break;
            case "Burger":
                CCindex = 12;
                break;
            case "Sandwich":
                CCindex = 13;
                break;
            case "Present":
                CCindex = 14;
                break;
            case "Teddy":
                CCindex = 15;
                break;
            case "Firework":
                CCindex = 16;
                break;
            case "Lightning":
                CCindex = 17;
                break;
            case "Water":
                CCindex = 18;
                break;
            case "Soda":
                CCindex = 19;
                break;
            case "Bronze 1":
                CCindex = 20;
                break;
            case "Bronze 2":
                CCindex = 21;
                break;
            case "Bronze 3":
                CCindex = 22;
                break;
            case "Silver 1":
                CCindex = 23;
                break;
            case "Silver 2":
                CCindex = 24;
                break;
            case "Silver 3":
                CCindex = 25;
                break;
            case "Gold 1":
                CCindex = 26;
                break;
            case "Gold 2":
                CCindex = 27;
                break;
            case "Gold 3":
                CCindex = 28;
                break;
            case "Platin":
                CCindex = 29;
                break;
            default:
                break;
        }
    }

    public enum DamageTyp
    {
        NORMAL,
        FIX
    }
}