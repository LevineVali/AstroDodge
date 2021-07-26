using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public GameObject testObject;

    public float testtime;
    private float backuptime;

    #region Sounds

    /// <summary>
    /// Audiomanager
    /// </summary>
    public AudioManager AM;

    /// <summary>
    /// Sound of thunder
    /// </summary>
    private Sound thunderSound;

    /// <summary>
    /// Sound for collecting Collactables
    /// </summary>
    private Sound collectSound;

    #endregion

    /// <summary>
    /// Playercontroller
    /// </summary>
    public PlayerController playerController;

    /// <summary>
    /// Particleobejcts
    /// 0 = Cloudblanket,
    /// 1 = Cloudsmall,
    /// 2 = Asteroid,
    /// 3 = Comet,
    /// 4 = Bird
    /// </summary>
    public GameObject[] Particles;

    /// <summary>
    /// List of all Cloud and CloudParticle-SpawnDetails
    /// </summary>
    public CloudColliderDetail[] CloudColliderDetails;

    [Header("Spawn settings, Bird & Cloud")]
    /// <summary>
    /// Max distance player must flight for next spawn
    /// </summary>
    public int MaxDistance = 100;

    /// <summary>
    /// Min distance player must flight for next spawn
    /// </summary>
    public int MinDistance = 10;

    /// <summary>
    /// percent value for spawnrate of danger clouds
    /// </summary>
    [Range(0, 1f)]
    public float DangerCloudRate;

    /// <summary>
    /// UI Effect for lightning
    /// </summary>
    public Image LightningEffect;

    [Header("Spawn settings, general")]
    /// <summary>
    /// Max value for the timer
    /// </summary>
    public float MaxTimer = 5.0f;

    /// <summary>
    /// min value for the timer
    /// </summary>
    public float MinTimer = 1.0f;

    [Header("Spawn Settings, Asteroid & Comet")]
    /// <summary>
    /// Range of X-Position for spawning enemies
    /// </summary>
    public float MaxRangeX = .5f;

    /// <summary>
    /// Y-position for spawning enemies
    /// </summary>
    public float PosY = 1.25f;

    /// <summary>
    /// Bonus for player
    /// </summary>
    public int Bonus;

    /// <summary>
    /// Distance between player and enemie to get double bonus
    /// </summary>
    public float DoubleBonusRange;

    /// <summary>
    /// min enemie speed on y axis
    /// </summary>
    public float MinSpeedY;

    /// <summary>
    /// max enemie speed on y axis
    /// </summary>
    public float MaxSpeedY;

    /// <summary>
    /// min enemie speed on x axis
    /// </summary>
    public float MinSpeedX;

    /// <summary>
    /// max enemie speed on x axis
    /// </summary>
    public float MaxSpeedX;

    /// <summary>
    /// Speedmultiplayer for particle on y-axis
    /// </summary>
    public float ParticleSpeedMultiplayerY;

    /// <summary>
    /// Speedmultiplayer for particle on x-axis
    /// </summary>
    public float ParticleSpeedMultiplayerX;

    [Header("Spawn Settings, Bird")]
    [Range(0, 1)]
    /// <summary>
    /// PercentStageHightForMaxSpawnrate.
    /// At this percentage of stagehight, spawnrate is maximum
    /// </summary>
    public float PSHFMaxSpawnrate;

    /// <summary>
    /// Min Speed of bird on x-axis
    /// </summary>
    public float MinBirdSpeedX;

    /// <summary>
    /// Max Speed of bird on x-axis
    /// </summary>
    public float MaxBirdSpeedX;

    /// <summary>
    /// Spawnposition 1 of Bird
    /// </summary>
    public Vector2 birdPosition1;

    /// <summary>
    /// Spawnposition 2 of Bird
    /// </summary>
    public Vector2 birdPosition2;

    /// <summary>
    /// Lower Birdspawnposition
    /// </summary>
    public float BirdY;

    [Header("Cloudblanket")]
    /// <summary>
    /// Position for spawning Cloudblanket
    /// </summary>
    public float CloudblanketY;

    /// <summary>
    /// Gameobject of Cloudblanket
    /// </summary>
    public GameObject CBObject;

    [Header("Collectables")]
    /// <summary>
    /// Gameobject of collectables
    /// 0 = Duck
    /// 1 = Ufo
    /// </summary>
    public GameObject[] Collectables;

    [Header("Gameobjects of Enemies")]
    /// <summary>
    /// Gameobject of the enemie
    /// 0 = Bird,
    /// 1 = Asteroid,
    /// 2 = AsteroidOval,
    /// 3 = Comet,
    /// 4 = Cloud,
    /// 5 = CloudBackground
    /// </summary>
    public GameObject[] Enemie;

    [Header("Forms")]
    /// <summary>
    /// List of all different Asteroid Forms
    /// </summary>
    public Sprite[] FAsteroid;

    /// <summary>
    /// List of all different Comet Forms
    /// </summary>
    public Sprite[] FComet;

    /// <summary>
    /// List of all different Cloud Forms
    /// </summary>
    public Sprite[] FCloud;

    /// <summary>
    /// List of all different Cloud Forms
    /// </summary>
    public Sprite[] FCloudBack1;

    /// <summary>
    /// List of all different Cloud Forms
    /// </summary>
    public Sprite[] FCloudBack2;

    [Header("Colors")]
    /// <summary>
    /// List of all different Asteroid colors
    /// </summary>
    public Color[] CAsteroid;

    /// <summary>
    /// List of all different Comet colors
    /// </summary>
    public Color[] CComet;

    [Header("Stages")]
    public int BirdStage;
    public int BirdEndStage;
    [Space(10)]
    public int Cloudblanket1;
    public int Cloudblanket2;
    [Space(10)]
    public int AsteroidStage1;
    public int AsteroidStage2;
    public int AsteroidStage3;
    public int AsteroidStage4;
    public int AsteroidStage5;

    [Space(10)]
    /// <summary>
    /// High, when comet starts to spawn
    /// </summary>
    public int CometStage;

    /// <summary>
    /// High, when galaxy will come to screen
    /// </summary>
    public int GalaxyStage;

    [Header("Collectables Stages")]
    public int DuckStage;
    public int UfoStage;
    public int BurgerStage;
    public int SandwichStage;
    public int PresentStage;
    public int TeddyStage;
    public int FireworkStage;
    public int WaterStage;
    public int SodaStage;

    /// <summary>
    /// 1/10% for Spawnrate at begining
    /// </summary>
    public int CollectableSpawnrate;

    /// <summary>
    /// amount of 1/10% for increasing Spawnrate for collectables per Distance
    /// </summary>
    public int CollectableSpawnrateIncreaser;

    /// <summary>
    /// Distance that must be reached
    /// </summary>
    public int CollectableSpawnrateIncreaserDistance;

    /// <summary>
    /// max spawnrate for collectables
    /// </summary>
    public int CollectableSpawnrateMAX;

    /// <summary>
    /// Rotationspeed for collectables x = min, y = max
    /// </summary>
    public Vector2 RotationSpeed;

    /// <summary>
    /// 1/10% for Spawnrate
    /// </summary>
    private int collectableSpawnrate;

    /// <summary>
    /// is collectable already spawned in this frame
    /// </summary>
    private bool collectableIsSpawned;

    [Header("Spezial Changes")]
    /// <summary>
    /// Multiplayer for spawnheight for comet
    /// </summary>
    public float CometSpawnHeightMultiplayer;

    [Header("Decorations Settings")]
    public Vector2 DecorationSpeedX;
    public int DecorationCount;
    /// <summary>
    /// list of random colors for each decoration
    /// </summary>
    public Gradient[] DecorationColors;

    [Header("Decorations Atmosphere")]
    public Decoration[] DecorationsAtmosphere;

    [Header("Decorations Space")]
    public Decoration[] DecorationsSpace;

    [Header("Unique Decorations")]
    public GameObject DestroyedSpaceshuttle;
    public int DestoyedSpaceshuttle_Height;
    private bool DestroyedSpaceshuttle_IsSpawned = false;

    [Space(10)]
    public GameObject ISS;
    public int ISS_Height;
    private bool ISS_IsSpawned = false;

    [Space(10)]
    public GameObject DeathMoon;
    public int DeathMoon_Height;
    private bool DeathMoon_IsSpawned = false;



    /// <summary>
    /// Time between spawning Enemies
    /// </summary>
    private float timer = 0;

    /// <summary>
    /// Distance between spawning Enemies (Bird & Cloud Spawnsetting)
    /// </summary>
    private double distance = 0;

    /// <summary>
    /// Hight of the player of the last frame
    /// </summary>
    private double lastHight = 0;

    /// <summary>
    /// Index for spawnobject
    /// 0 = Bird |
    /// 1 = Asteroid |
    /// 2 = AsteroidOval |
    /// 3 = Comet |
    /// 4 = Cloud |
    /// 100 = Duck |
    /// 101 = Ufo |
    /// 102 = Burger |
    /// 103 = Sandwich |
    /// 104 = Present |
    /// 105 = Teddy |
    /// 106 = Firework |
    /// 107 = Water |
    /// 108 = Soda
    /// </summary>
    private int spawn;

    [HideInInspector]
    /// <summary>
    /// is cloudblanket spawned?
    /// </summary>
    public int cloudblanketcount = 0;

    private GameObject go;

    [HideInInspector]
    public SkinManager SM;
    private bool smChecked;

    public bool Ufo_Done;
    public bool Duck_Done;
    public bool Burger_Done;
    public bool Sandwich_Done;
    public bool Present_Done;
    public bool Teddy_Done;
    public bool Firework_Done;
    public bool Water_Done;
    public bool Soda_Done;
    private void Start()
    {
        go = new GameObject();

        if (BirdEndStage > Cloudblanket1)
            BirdEndStage = Cloudblanket1;

        smChecked = false;
        SM = FindObjectOfType<SkinManager>();

        backuptime = testtime;

        // get thundersound
        thunderSound = System.Array.Find(AM.Sound, sound => sound.Name == "Thunder");
        collectSound = System.Array.Find(AM.Sound, sound => sound.Name == "CollactableCollect");
    }

    // Update is called once per frame
    void Update()
    {
        // testtime -= Time.deltaTime;
        // if (testtime <= 0f)
        // {
        //     Instantiate(testObject, Vector3.up, Quaternion.identity);
        //     testtime = backuptime;
        // }

        if (SM != null && !smChecked)
        {
            smChecked = true;
            foreach (Skin skin in playerController.Stats.skinIDs)
            {
                if (skin.id >= 0)
                {
                    switch (SM.Skins[skin.id].Name)
                    {
                        case "UFO":
                            Ufo_Done = true;
                            break;
                        case "Duck":
                            Duck_Done = true;
                            break;
                        case "Burger":
                            Burger_Done = true;
                            break;
                        case "Sandwich":
                            Sandwich_Done = true;
                            break;
                        case "Present":
                            Present_Done = true;
                            break;
                        case "Teddy":
                            Teddy_Done = true;
                            break;
                        case "Firework":
                            Firework_Done = true;
                            break;
                        case "Water":
                            Water_Done = true;
                            break;
                        case "Soda":
                            Soda_Done = true;
                            break;
                    }
                }
            }
        }

        if (playerController.Stats.currentScore < Cloudblanket1)
        {
            DestroyedSpaceshuttle_IsSpawned = false;
            ISS_IsSpawned = false;
            DeathMoon_IsSpawned = false;
        }

        // if game is running and player is not dead
        if (playerController.Run && playerController.Life)
        {
            // if player reached cloudblanketstage and cloudblanket isnt spawned yet
            if (playerController.Stats.currentScore >= Cloudblanket1 && cloudblanketcount < 1)
            {
                SpawnEnemy(-1);
            }

            // if player reached cloudblanketstage and cloudblanket isnt spawned yet
            else if (playerController.Stats.currentScore >= Cloudblanket2 && cloudblanketcount < 2)
            {
                SpawnEnemy(-1);
            }

            // if player reached ISSStage and ISS isnt spawned yet
            if (playerController.Stats.currentScore >= ISS_Height && !ISS_IsSpawned)
            {
                Instantiate(ISS, ISS.transform.position, ISS.transform.rotation);
                ISS_IsSpawned = true;
            }

            // if player reached DestroyedSpaceshuttleStage and DestroyedSpaceshuttle isnt spawned yet
            if (playerController.Stats.currentScore >= DestoyedSpaceshuttle_Height && !DestroyedSpaceshuttle_IsSpawned)
            {
                Instantiate(DestroyedSpaceshuttle, DestroyedSpaceshuttle.transform.position, DestroyedSpaceshuttle.transform.rotation);
                DestroyedSpaceshuttle_IsSpawned = true;
            }

            // if player reached DeathmoonStage and Deathmoon isnt spawned yet
            if (playerController.Stats.currentScore >= DeathMoon_Height && !DeathMoon_IsSpawned)
            {
                Instantiate(DeathMoon, DeathMoon.transform.position, DeathMoon.transform.rotation);
                DeathMoon_IsSpawned = true;
            }

            // if player is between bird and cloudblanket stage
            if (playerController.Stats.currentScore <= Cloudblanket1 && playerController.Stats.currentScore >= BirdStage)
            {
                distance -= playerController.Stats.currentScore - lastHight;

                if (distance <= 0)
                {
                    // get highrange of Birdspawnstage
                    float spawnrange = BirdEndStage - BirdStage;

                    // if player reached the needed high for 100% spawnrate
                    if (playerController.Stats.currentScore >= spawnrange * PSHFMaxSpawnrate + BirdStage)
                    {
                        distance = Random.Range(MinDistance, MaxDistance + 1);
                        SpawnEnemy(0);
                    }
                    // otherwise spawnrate is increasing
                    else
                    {
                        // calculate modifre to modifire the distance between spawn
                        double modifier = spawnrange * PSHFMaxSpawnrate / playerController.Stats.currentScore;

                        if (modifier < 1)
                            modifier = 1;
                        else if (modifier > 4)
                            modifier = 4;

                        distance = Random.Range(MinDistance, MaxDistance + 1);
                        distance *= modifier;

                        SpawnEnemy(0);
                    }

                    if (playerController.Stats.currentScore >= BirdStage && playerController.Stats.currentScore <= Cloudblanket1)
                    {
                        if (DecorationCount < 2)
                        {
                            int spawnDecoration = Random.Range(1, 101);
                            if (spawnDecoration > 50)
                            {
                                SpawnDecoration(DecorationsAtmosphere, Bird.DecorationTyp.ATMOSPHERE);
                            }
                        }
                    }
                }

                lastHight = playerController.Stats.currentScore;
            }
            else if (playerController.Stats.currentScore <= Cloudblanket2 && playerController.Stats.currentScore >= Cloudblanket1)
            {
                distance -= playerController.Stats.currentScore - lastHight;

                if (distance <= 0)
                {
                    // get highrange of Birdspawnstage
                    float spawnrange = BirdEndStage - BirdStage;

                    // if player reached the needed high for 100% spawnrate
                    if (playerController.Stats.currentScore >= spawnrange * PSHFMaxSpawnrate * .75f + BirdStage)
                    {
                        distance = Random.Range(MinDistance, MaxDistance + 1);
                        SpawnEnemy(0);
                    }
                    // otherwise spawnrate is increasing
                    else
                    {
                        // calculate modifre to modifire the distance between spawn
                        double modifier = spawnrange * PSHFMaxSpawnrate / playerController.Stats.currentScore;

                        if (modifier < 1)
                            modifier = 1;
                        else if (modifier > 4)
                            modifier = 4;

                        distance = Random.Range(MinDistance, MaxDistance + 1);
                        distance *= modifier;

                        SpawnEnemy(0);
                    }
                }

                lastHight = playerController.Stats.currentScore;
            }

            // if the timer is countet
            else if (timer <= 0)
            {
                int r;

                // get new counter
                timer = Random.Range(MinTimer, MaxTimer);

                if (playerController.Stats.currentScore >= AsteroidStage5)
                {
                    r = Random.Range(1, 8);
                    timer /= 5;
                }
                else if (playerController.Stats.currentScore >= AsteroidStage4)
                {
                    r = Random.Range(1, 5);
                    timer /= 4;
                }
                else if (playerController.Stats.currentScore >= AsteroidStage3)
                {
                    r = Random.Range(1, 4);
                    timer /= 3;
                }
                else if (playerController.Stats.currentScore >= AsteroidStage2)
                {
                    r = Random.Range(1, 3);
                    timer /= 2;
                }
                else if (playerController.Stats.currentScore >= AsteroidStage1)
                {
                    r = 1;
                }
                else if (playerController.Stats.currentScore >= BirdStage)
                {
                    r = 0;
                    timer /= 6;

                    if (playerController.Stats.currentScore >= Cloudblanket1)
                    {
                        double multiplire = (playerController.Stats.currentScore / Cloudblanket1) * 2;
                        timer *= (float)multiplire;
                    }
                }
                else
                {
                    r = -1;
                }

                if (playerController.Stats.currentScore >= AsteroidStage1)
                {
                    if (DecorationCount < 2)
                    {
                        int spawnDecoration = Random.Range(1, 101);
                        if (spawnDecoration > 50)
                        {
                            SpawnDecoration(DecorationsSpace, Bird.DecorationTyp.SPACE);
                        }
                    }
                }

                // spawn enemie
                SpawnEnemy(r);
            }

            // decrease timer
            timer -= Time.deltaTime;
        }
        else
        {
            lastHight = 0;
        }
    }

    private void SpawnEnemy(int _enemy)
    {
        if (playerController.Stats.currentScore >= Cloudblanket1 && cloudblanketcount < 1)
        {
            Instantiate(CBObject, new Vector3(0, CloudblanketY, 0), Quaternion.identity);
            cloudblanketcount = 1;
        }
        else if (playerController.Stats.currentScore >= Cloudblanket2 && cloudblanketcount < 2)
        {
            Instantiate(CBObject, new Vector3(0, CloudblanketY, 0), Quaternion.identity);
            cloudblanketcount = 2;
        }

        if (_enemy == -1)
        {
            return;
        }

        int r;

        Enemie enemy;
        Color c;

        // get random spawnposition
        Vector3 pos = new Vector3(Random.Range(-MaxRangeX, MaxRangeX) + playerController.transform.position.x, PosY, 0);
        if (pos.x < -.5f)
            pos.x = -.5f;
        if (pos.x > .5f)
            pos.x = .5f;

        // get random Image for specific stage
        if (playerController.Stats.currentScore >= CometStage)
        {
            r = Random.Range(1, 12);
            if (r <= 3)
            {
                // set comet spawn
                spawn = 3;
            }
            else
            {
                // set asteroid spawn
                spawn = Random.Range(1, 3);
            }
        }
        // asteroidspawn
        else if (playerController.Stats.currentScore >= AsteroidStage1)
        {
            spawn = Random.Range(1, 3);
        }
        // birdspawn or cloud
        else
        {
            if (playerController.Stats.currentScore < BirdEndStage)
            {
                r = Random.Range(0, 3);
                if (r == 0)
                    // cloud
                    spawn = 4;
                else
                // bird
                {
                    spawn = 0;
                }
            }
            else if (playerController.Stats.currentScore <= Cloudblanket1 + (AsteroidStage1 - Cloudblanket1) / 2)
            {
                // cloud
                spawn = 4;
            }
        }

        // Duck
        if (!Duck_Done)
        {
            if (playerController.Stats.tracks.Contains(Track.DUCK))
            {
                if (playerController.Stats.currentScore >= DuckStage && !collectableIsSpawned)
                {
                    int distance = (int)playerController.Stats.currentScore - DuckStage;
                    int increaser = distance / CollectableSpawnrateIncreaserDistance * CollectableSpawnrateIncreaser;
                    collectableSpawnrate = CollectableSpawnrate + increaser;

                    if (collectableSpawnrate > CollectableSpawnrateMAX)
                        collectableSpawnrate = CollectableSpawnrateMAX;

                    r = Random.Range(0, 1000);
                    if (r < collectableSpawnrate)
                    {
                        spawn = 100;
                        collectableIsSpawned = true;
                    }
                }
            }
        }

        // Ufo
        if (!Ufo_Done)
        {
            if (playerController.Stats.tracks.Contains(Track.UFO))
            {
                if (playerController.Stats.currentScore >= UfoStage && !collectableIsSpawned)
                {
                    int distance = (int)playerController.Stats.currentScore - UfoStage;
                    int increaser = distance / CollectableSpawnrateIncreaserDistance * CollectableSpawnrateIncreaser;
                    collectableSpawnrate = CollectableSpawnrate + increaser;

                    if (collectableSpawnrate > CollectableSpawnrateMAX)
                        collectableSpawnrate = CollectableSpawnrateMAX;

                    r = Random.Range(0, 1000);
                    if (r < collectableSpawnrate)
                    {
                        spawn = 101;
                        collectableIsSpawned = true;
                    }
                }
            }
        }

        // Burger
        if (!Burger_Done)
        {
            if (playerController.Stats.tracks.Contains(Track.BURGER))
            {
                if (playerController.Stats.currentScore >= BurgerStage && !collectableIsSpawned)
                {
                    int distance = (int)playerController.Stats.currentScore - BurgerStage;
                    int increaser = distance / CollectableSpawnrateIncreaserDistance * CollectableSpawnrateIncreaser;
                    collectableSpawnrate = CollectableSpawnrate + increaser;

                    if (collectableSpawnrate > CollectableSpawnrateMAX)
                        collectableSpawnrate = CollectableSpawnrateMAX;

                    r = Random.Range(0, 1000);
                    if (r < collectableSpawnrate)
                    {
                        spawn = 102;
                        collectableIsSpawned = true;
                    }
                }
            }
        }

        // Sandwich
        if (!Sandwich_Done)
        {
            if (playerController.Stats.tracks.Contains(Track.SANDWICH))
            {
                if (playerController.Stats.currentScore >= SandwichStage && !collectableIsSpawned)
                {
                    int distance = (int)playerController.Stats.currentScore - SandwichStage;
                    int increaser = distance / CollectableSpawnrateIncreaserDistance * CollectableSpawnrateIncreaser;
                    collectableSpawnrate = CollectableSpawnrate + increaser;

                    if (collectableSpawnrate > CollectableSpawnrateMAX)
                        collectableSpawnrate = CollectableSpawnrateMAX;

                    r = Random.Range(0, 1000);
                    if (r < collectableSpawnrate)
                    {
                        spawn = 103;
                        collectableIsSpawned = true;
                    }
                }
            }
        }

        // Present
        if (!Present_Done)
        {
            if (playerController.Stats.tracks.Contains(Track.PRESENT))
            {
                if (playerController.Stats.currentScore >= PresentStage && !collectableIsSpawned)
                {
                    int distance = (int)playerController.Stats.currentScore - PresentStage;
                    int increaser = distance / CollectableSpawnrateIncreaserDistance * CollectableSpawnrateIncreaser;
                    collectableSpawnrate = CollectableSpawnrate + increaser;

                    if (collectableSpawnrate > CollectableSpawnrateMAX)
                        collectableSpawnrate = CollectableSpawnrateMAX;

                    r = Random.Range(0, 1000);
                    if (r < collectableSpawnrate)
                    {
                        spawn = 104;
                        collectableIsSpawned = true;
                    }
                }
            }
        }

        // Teddy
        if (!Teddy_Done)
        {
            if (playerController.Stats.tracks.Contains(Track.TEDDY))
            {
                if (playerController.Stats.currentScore >= TeddyStage && !collectableIsSpawned)
                {
                    int distance = (int)playerController.Stats.currentScore - TeddyStage;
                    int increaser = distance / CollectableSpawnrateIncreaserDistance * CollectableSpawnrateIncreaser;
                    collectableSpawnrate = CollectableSpawnrate + increaser;

                    if (collectableSpawnrate > CollectableSpawnrateMAX)
                        collectableSpawnrate = CollectableSpawnrateMAX;

                    r = Random.Range(0, 1000);
                    if (r < collectableSpawnrate)
                    {
                        spawn = 105;
                        collectableIsSpawned = true;
                    }
                }
            }
        }

        // Firework
        if (!Firework_Done)
        {
            if (playerController.Stats.tracks.Contains(Track.FIREWORK))
            {
                if (playerController.Stats.currentScore >= FireworkStage && !collectableIsSpawned)
                {
                    int distance = (int)playerController.Stats.currentScore - FireworkStage;
                    int increaser = distance / CollectableSpawnrateIncreaserDistance * CollectableSpawnrateIncreaser;
                    collectableSpawnrate = CollectableSpawnrate + increaser;

                    if (collectableSpawnrate > CollectableSpawnrateMAX)
                        collectableSpawnrate = CollectableSpawnrateMAX;

                    r = Random.Range(0, 1000);
                    if (r < collectableSpawnrate)
                    {
                        spawn = 106;
                        collectableIsSpawned = true;
                    }
                }
            }
        }

        // Water
        if (!Water_Done)
        {
            if (playerController.Stats.tracks.Contains(Track.WATER))
            {
                if (playerController.Stats.currentScore >= WaterStage && !collectableIsSpawned)
                {
                    int distance = (int)playerController.Stats.currentScore - WaterStage;
                    int increaser = distance / CollectableSpawnrateIncreaserDistance * CollectableSpawnrateIncreaser;
                    collectableSpawnrate = CollectableSpawnrate + increaser;

                    if (collectableSpawnrate > CollectableSpawnrateMAX)
                        collectableSpawnrate = CollectableSpawnrateMAX;

                    r = Random.Range(0, 1000);
                    if (r < collectableSpawnrate)
                    {
                        spawn = 107;
                        collectableIsSpawned = true;
                    }
                }
            }
        }

        // Firework
        if (!Soda_Done)
        {
            if (playerController.Stats.tracks.Contains(Track.SODA))
            {
                if (playerController.Stats.currentScore >= SodaStage && !collectableIsSpawned)
                {
                    int distance = (int)playerController.Stats.currentScore - SodaStage;
                    int increaser = distance / CollectableSpawnrateIncreaserDistance * CollectableSpawnrateIncreaser;
                    collectableSpawnrate = CollectableSpawnrate + increaser;

                    if (collectableSpawnrate > CollectableSpawnrateMAX)
                        collectableSpawnrate = CollectableSpawnrateMAX;

                    r = Random.Range(0, 1000);
                    if (r < collectableSpawnrate)
                    {
                        spawn = 108;
                        collectableIsSpawned = true;
                    }
                }
            }
        }

        collectableIsSpawned = false;

        // asteroid spawn
        if (spawn == 1)
        {
            // spawn asteroid
            go = Instantiate(Enemie[1], pos, Quaternion.identity);

            enemy = go.GetComponent<Enemie>();

            int colornumber = Random.Range(0, CAsteroid.Length);

            // get color
            c = CAsteroid[colornumber];

            // set startcolor for tail
            enemy.StartColor = c;

            // set bonus
            enemy.Bonus = Bonus;

            // give enmie playercontroller
            enemy.pc = playerController;

            // its not a collectable
            enemy.IsCollectable = false;

            // set color
            go.GetComponentInChildren<SpriteRenderer>().color = c;

            go.GetComponentInChildren<SpriteRenderer>().sprite = FAsteroid[0];

            r = Random.Range(0, 2);

            if (r == 0)
            {
                enemy.RotateMultiplayer = Random.Range(10, 25);
            }
            else
            {
                enemy.RotateMultiplayer = -Random.Range(10, 25);
            }
        }
        // asteroid oval spawn
        else if (spawn == 2)
        {
            // spawn asteroid
            go = Instantiate(Enemie[2], pos, Quaternion.identity);

            enemy = go.GetComponent<Enemie>();

            int colornumber = Random.Range(0, CAsteroid.Length);

            // get color
            c = CAsteroid[colornumber];

            // set startcolor for tail
            enemy.StartColor = c;

            // set bonus
            enemy.Bonus = Bonus;

            // give enmie playercontroller
            enemy.pc = playerController;

            // its not a collectable
            enemy.IsCollectable = false;

            // set color
            go.GetComponentInChildren<SpriteRenderer>().color = c;

            go.GetComponentInChildren<SpriteRenderer>().sprite = FAsteroid[1];

            r = Random.Range(0, 2);

            if (r == 0)
            {
                enemy.RotateMultiplayer = Random.Range(10, 25);
            }
            else
            {
                enemy.RotateMultiplayer = -Random.Range(10, 25);
            }
        }
        // comet spawn
        else if (spawn == 3)
        {
            pos *= CometSpawnHeightMultiplayer;

            // spawn comet
            go = Instantiate(Enemie[3], pos, Quaternion.identity);

            enemy = go.GetComponent<Enemie>();

            int colornumber = Random.Range(0, CComet.Length);

            // get color
            c = CComet[colornumber];

            // set startcolor for tail
            enemy.StartColor = c;

            // set bonus
            enemy.Bonus = Bonus;

            // give enmie playercontroller
            enemy.pc = playerController;

            // its not a collectable
            enemy.IsCollectable = false;

            r = Random.Range(0, FComet.Length);

            // set color
            go.GetComponentInChildren<SpriteRenderer>().color = c;

            go.GetComponentInChildren<SpriteRenderer>().sprite = FComet[r];

            if (r == 0)
            {
                go.GetComponent<CircleCollider2D>().radius = 0.05751846f;
            }
            else
            {
                go.GetComponent<CircleCollider2D>().radius = 0.04931723f;
            }
        }
        // spawn cloud
        else if (spawn == 4)
        {
            pos = new Vector3(Random.Range(-.5f, .5f), PosY, 0);
            go = Instantiate(Enemie[4], pos, Quaternion.identity);

            SpriteRenderer renderer = go.GetComponentInChildren<SpriteRenderer>();
            renderer.sprite = FCloud[Random.Range(0, FCloud.Length)];

            int random = Random.Range(0, 2);

            if (random == 0)
                renderer.flipX = true;

            Cloud cloud = go.GetComponent<Cloud>();
            cloud.ColiderPos = CloudColliderDetails[playerController.CCindex].CloudPosX;
            cloud.Typ = Cloud.CloudTyp.NORMAL;

            if (playerController.Stats.currentScore >= Cloudblanket1 && playerController.Stats.currentScore <= Cloudblanket2)
            {
                float value = Random.Range(0, 1f);

                if (value >= DangerCloudRate)
                {
                    cloud.Typ = Cloud.CloudTyp.DANGER;
                    cloud.LightningObject.GetComponent<Lightning>().LightningEffect = LightningEffect;
                    cloud.ThunderSound = thunderSound;
                }
                else
                {
                    cloud.Typ = Cloud.CloudTyp.DUMMY;
                }
            }

            #region Background Coulds 1

            pos = new Vector3(Random.Range(-.5f, .5f), PosY, 0);
            go = Instantiate(Enemie[5], pos, Quaternion.identity);

            renderer = go.GetComponentInChildren<SpriteRenderer>();
            renderer.sprite = FCloudBack1[Random.Range(0, FCloudBack1.Length)];
            renderer.sortingOrder = 4;

            go.GetComponent<Cloud>().Speed = go.GetComponent<Cloud>().Speed / 2;

            random = Random.Range(0, 2);

            if (random == 0)
                renderer.flipX = true;

            // 2nd spawn
            float yVariable = Random.Range(-.5f, .5f);

            pos = new Vector3(Random.Range(-.5f, .5f), PosY + yVariable, 0);
            go = Instantiate(Enemie[5], pos, Quaternion.identity);

            renderer = go.GetComponentInChildren<SpriteRenderer>();
            renderer.sprite = FCloudBack1[Random.Range(0, FCloudBack1.Length)];
            renderer.sortingOrder = 4;

            go.GetComponent<Cloud>().Speed = go.GetComponent<Cloud>().Speed / 2;

            random = Random.Range(0, 2);

            if (random == 0)
                renderer.flipX = true;

            #endregion

            #region Background Coulds 2

            pos = new Vector3(Random.Range(-.5f, .5f), PosY, 0);
            go = Instantiate(Enemie[5], pos, Quaternion.identity);

            renderer = go.GetComponentInChildren<SpriteRenderer>();
            renderer.sprite = FCloudBack2[Random.Range(0, FCloudBack2.Length)];
            renderer.sortingOrder = 1;

            go.GetComponent<Cloud>().Speed = go.GetComponent<Cloud>().Speed / 3;

            random = Random.Range(0, 2);

            if (random == 0)
                renderer.flipX = true;
            #endregion
        }



        #region Collectables

        // Duck
        else if (spawn == 100)
        {
            SpawnCollectables(0, pos, go);
        }
        // Ufo
        else if (spawn == 101)
        {
            SpawnCollectables(1, pos, go);
        }
        // Burger
        else if (spawn == 102)
        {
            SpawnCollectables(2, pos, go);
        }
        // Sandwich
        else if (spawn == 103)
        {
            SpawnCollectables(3, pos, go);
        }
        // Present
        else if (spawn == 104)
        {
            SpawnCollectables(4, pos, go);
        }
        // Teddy
        else if (spawn == 105)
        {
            SpawnCollectables(5, pos, go);
        }
        // Firework
        else if (spawn == 106)
        {
            SpawnCollectables(6, pos, go);
        }
        // Water
        else if (spawn == 107)
        {
            SpawnCollectables(7, pos, go);
        }
        // Soda
        else if (spawn == 108)
        {
            SpawnCollectables(8, pos, go);
        }

        #endregion



        // bird spawn
        else
        {
            int i = Random.Range(0, 2);
            float speed = Random.Range(MinBirdSpeedX, MaxBirdSpeedX);

            float posy = Random.Range(birdPosition1.y, birdPosition2.y) + playerController.Stats.currentSpeed / 100;

            if (posy > 1f)
            {
                pos = new Vector3(Random.Range(birdPosition1.x, birdPosition2.x), posy, 0);
            }
            else
            {
                if (i == 0)
                {
                    pos = new Vector3(birdPosition1.x, posy, 0);
                }
                else
                {
                    pos = new Vector3(birdPosition2.x, posy, 0);
                }
            }

            if (pos.x > 0)
            {
                speed *= -1;
            }

            go = Instantiate(Enemie[0], pos, Quaternion.identity);
            go.GetComponent<Bird>().SpeedX = speed;
            go.GetComponent<Bird>().isDecoration = false;

            #region Background Coulds 1

            pos = new Vector3(Random.Range(-.5f, .5f), PosY, 0);
            go = Instantiate(Enemie[5], pos, Quaternion.identity);

            SpriteRenderer renderer = go.GetComponentInChildren<SpriteRenderer>();
            renderer.sprite = FCloudBack1[Random.Range(0, FCloudBack1.Length)];
            renderer.sortingOrder = 4;

            go.GetComponent<Cloud>().Speed = go.GetComponent<Cloud>().Speed / 2;

            int random = Random.Range(0, 2);

            if (random == 0)
                renderer.flipX = true;

            // 2nd spawn
            float yVariable = Random.Range(-.5f, .5f);

            pos = new Vector3(Random.Range(-.5f, .5f), PosY + yVariable, 0);
            go = Instantiate(Enemie[5], pos, Quaternion.identity);

            renderer = go.GetComponentInChildren<SpriteRenderer>();
            renderer.sprite = FCloudBack1[Random.Range(0, FCloudBack1.Length)];
            renderer.sortingOrder = 4;

            go.GetComponent<Cloud>().Speed = go.GetComponent<Cloud>().Speed / 2;

            random = Random.Range(0, 2);

            if (random == 0)
                renderer.flipX = true;

            #endregion

            #region Background Coulds 2

            pos = new Vector3(Random.Range(-.5f, .5f), PosY, 0);
            go = Instantiate(Enemie[5], pos, Quaternion.identity);

            renderer = go.GetComponentInChildren<SpriteRenderer>();
            renderer.sprite = FCloudBack2[Random.Range(0, FCloudBack2.Length)];
            renderer.sortingOrder = 1;

            go.GetComponent<Cloud>().Speed = go.GetComponent<Cloud>().Speed / 3;

            random = Random.Range(0, 2);

            if (random == 0)
                renderer.flipX = true;
            #endregion
        }

        if (spawn != 0 && spawn != 4 && spawn < 100)
        {
            float speed = pos.x;
            if (speed < 0)
                speed *= -1;

            speed = pos.x / speed;

            if (_enemy == 1)
            {
                // set speed
                go.GetComponent<Enemie>().SpeedY = Random.Range(MinSpeedY, MaxSpeedY);
            }
            else if (_enemy == 2)
            {
                // set speed
                go.GetComponent<Enemie>().SpeedY = Random.Range(MinSpeedY, MaxSpeedY) * 3;
            }
            else if (_enemy == 3)
            {
                // set speed
                go.GetComponent<Enemie>().SpeedY = Random.Range(MinSpeedY, MaxSpeedY);
                if (pos.x > 0)
                {
                    go.GetComponent<Enemie>().SpeedX = -Random.Range(MinSpeedX, MaxSpeedX) * speed;
                    go.transform.Translate(0.5f + Random.Range(MinSpeedX, MaxSpeedX) / 5, 0, 0);
                }
                else
                {
                    go.GetComponent<Enemie>().SpeedX = Random.Range(MinSpeedX, MaxSpeedX) * speed;
                    go.transform.Translate(-(.5f + Random.Range(MinSpeedX, MaxSpeedX) / 5), 0, 0);
                }
            }
            else if (_enemy == 4)
            {
                // set speed
                go.GetComponent<Enemie>().SpeedY = Random.Range(MinSpeedY, MaxSpeedY) * 3;
                if (pos.x > 0)
                {
                    go.GetComponent<Enemie>().SpeedX = -Random.Range(MinSpeedX, MaxSpeedX) * speed;
                    go.transform.Translate(0.5f + Random.Range(MinSpeedX, MaxSpeedX) / 5, 0, 0);
                }
                else
                {
                    go.GetComponent<Enemie>().SpeedX = Random.Range(MinSpeedX, MaxSpeedX) * speed;
                    go.transform.Translate(-(0.5f + Random.Range(MinSpeedX, MaxSpeedX) / 5), 0, 0);
                }
            }
            else if (_enemy >= 5)
            {
                // set speed
                go.GetComponent<Enemie>().SpeedY = -Random.Range(MinSpeedY, MaxSpeedY) * 1.5f;
                if (pos.x > 0)
                {
                    go.GetComponent<Enemie>().SpeedX = -Random.Range(MinSpeedX, MaxSpeedX) * speed;
                    go.transform.Translate(0.5f + Random.Range(MinSpeedX, MaxSpeedX) / 5, 0, 0);
                }
                else
                {
                    go.GetComponent<Enemie>().SpeedX = Random.Range(MinSpeedX, MaxSpeedX) * speed;
                    go.transform.Translate(-(0.5f + Random.Range(MinSpeedX, MaxSpeedX) / 5), 0, 0);
                }
                go.GetComponent<Enemie>().FollowPlayer = true;
                go.GetComponentInChildren<SpriteRenderer>().flipY = true;
            }

            go.GetComponent<Enemie>().DoubleBonusRange = DoubleBonusRange;
        }
    }

    private void SpawnDecoration(Decoration[] _decorations, Bird.DecorationTyp _decorationTyp)
    {
        // get random spawnposition
        Vector3 pos = new Vector3(Random.Range(-MaxRangeX, MaxRangeX) + playerController.transform.position.x, PosY, 0);
        if (pos.x < -.5f)
            pos.x = -.5f;
        if (pos.x > .5f)
            pos.x = .5f;

        int i = Random.Range(0, 2);
        float speed = Random.Range(DecorationSpeedX.x, DecorationSpeedX.y);

        float posy = Random.Range(birdPosition1.y, birdPosition2.y) + playerController.Stats.currentSpeed / 100;

        if (posy > 1f)
        {
            pos = new Vector3(Random.Range(birdPosition1.x, birdPosition2.x), posy, 0);
        }
        else
        {
            if (i == 0)
            {
                pos = new Vector3(birdPosition1.x, posy, 0);
            }
            else
            {
                pos = new Vector3(birdPosition2.x, posy, 0);
            }
        }

        if (pos.x > 0)
        {
            speed *= -1;
        }

        int index = Random.Range(0, _decorations.Length);

        GameObject go = Instantiate(_decorations[index].Object, pos, Quaternion.identity);
        Bird bird = go.GetComponent<Bird>();

        if (_decorations[index].DecorationColor > 0)
        {
            Color c = DecorationColors[_decorations[index].DecorationColor - 1].Evaluate(Random.Range(0, 1f));
            if (_decorations[index].Front)
            {
                bird.srFront.color = c;
            }
            if (_decorations[index].Back)
            {
                bird.srBack.color = c;
            }
        }
        bird.SpeedX = speed;
        bird.isDecoration = true;
        bird.decorationTyp = _decorationTyp;
        bird.decorationName = _decorations[index].Object.name;

        DecorationCount++;
    }

    [System.Serializable]
    public struct Decoration
    {
        public GameObject Object;
        public int DecorationColor;
        public bool Front;
        public bool Back;
    }

    [System.Serializable]
    public struct CloudColliderDetail
    {
        public string Name;
        public Vector3 CloudPosX;
    }

    private void SpawnCollectables(int _index, Vector3 _pos, GameObject _go)
    {
        _go = Instantiate(Collectables[_index], _pos, Quaternion.identity);

        var enemy = _go.GetComponent<Enemie>();

        enemy.pc = playerController;

        // its a collectable
        enemy.IsCollectable = true;

        // set rotationspeed
        enemy.RotationSpeed = Random.Range(RotationSpeed.x, RotationSpeed.y);

        // set collectsound
        enemy.CollectSound = collectSound;
    }
}