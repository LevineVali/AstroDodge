using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemie : MonoBehaviour
{
    /// <summary>
    /// Sprite of the enemie
    /// </summary>
    public GameObject Sprite;

    /// <summary>
    /// Databonus for the player, if he avoid this enemie
    /// </summary>
    public int Bonus;

    /// <summary>
    /// Speed of this enemie
    /// </summary>
    public float SpeedY = 5f;

    /// <summary>
    /// Direction to fly
    /// </summary>
    public float SpeedX = 0;

    /// <summary>
    /// should this enemy follows the player?
    /// </summary>
    public bool FollowPlayer = false;

    /// <summary>
    /// Rotationspeedmultiplayer
    /// </summary>
    public float RotateMultiplayer;

    /// <summary>
    /// Color of the Tail of this enemie
    /// </summary>
    public Color StartColor;

    /// <summary>
    /// distance on x-axis between palyer and this object to get double bonus
    /// </summary>
    public float DoubleBonusRange;

    /// <summary>
    /// particlesystem of this enemie
    /// </summary>
    public ParticleSystem ps;

    /// <summary>
    /// BonusImageObject
    /// </summary>
    public GameObject BonusImage;

    /// <summary>
    /// Animation for picking up a Collectable
    /// </summary>
    public Sprite[] CollectableAnimation;

    /// <summary>
    /// Time in Seconds between each Frame
    /// </summary>
    public float FrameCounter;

    [HideInInspector]
    /// <summary>
    /// Playercontroller
    /// </summary>
    public PlayerController pc;

    /// <summary>
    /// SpriteRenderer of the childobject
    /// </summary>
    public SpriteRenderer spriteRenderer;

    /// <summary>
    /// Collectsound
    /// </summary>
    public Sound CollectSound;

    private bool soundPlayed;

    private Vector2 dir;

    private bool bonus = false;
    private bool doublebonus = false;

    private SpawnManager sm;
    private Vector2 oldDir;

    /// <summary>
    /// Is this object a collectable?
    /// </summary>
    public bool IsCollectable;
    
    /// <summary>
    /// Is this collectable collected?
    /// </summary>
    public bool IsCollected;

    public float RotationSpeed;
    private float angle;

    private float timer;
    private int count;

    private void Start()
    {
        soundPlayed = false;
        sm = FindObjectOfType<SpawnManager>();

        ps = GetComponentInChildren<ParticleSystem>();

        if (ps != null)
            ps.startColor = StartColor;

        if (IsCollectable)
        {
            angle = Random.Range(0f, 360f);
            timer = 0;
            count = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pc.Killall)
            Destroy(gameObject);

        if (FollowPlayer)
        {
            if (transform.position.y >= -.5f)
            {
                dir.x = pc.transform.position.x - transform.position.x;
                dir.y = SpeedY * Time.deltaTime / 10;
                oldDir = dir;
                dir.x *= SpeedX * Time.deltaTime / 3;
                dir.y *= -1;
            }
            else
            {
                dir.x = oldDir.x;
                dir.x *= SpeedX * Time.deltaTime / 3;
                dir.y = SpeedY * Time.deltaTime / 10;
                dir.y *= -1;
            }
        }
        else
        {
            dir = new Vector2(SpeedX * Time.deltaTime / 10, (SpeedY * Time.deltaTime) / 10);
            dir.y *= -1;
        }

        transform.up = -dir;

        if (IsCollectable)
        {
            transform.Translate(dir, Space.World);
            transform.Rotate(0, 0, angle);
            angle += RotationSpeed * Time.deltaTime;

            if (IsCollected)
            {
                if (!soundPlayed)
                {
                    CollectSound.Source.Play();
                    soundPlayed = true;
                }

                timer += Time.deltaTime;
                if (FrameCounter > 0)
                {
                    // play animation
                    if (timer >= FrameCounter)
                    {
                        // reduce timer
                        timer -= FrameCounter;

                        if (count >= CollectableAnimation.Length)
                        {
                            Destroy(gameObject);
                        }
                        else if (CollectableAnimation.Length > 0)
                        {
                            // set new sprite
                            spriteRenderer.sprite = CollectableAnimation[count];
                        }

                        // increase counter
                        count++;
                    }
                }
            }
        }
        else
        {
            transform.Translate(dir);
            if (Sprite != null)
            {
                Sprite.transform.Rotate(0, 0, (SpeedX + SpeedY) * RotateMultiplayer * Time.deltaTime);

                // if this enemie is out of screen
                if (transform.position.y <= -0.7f && !bonus && pc.Run && spriteRenderer.color != Color.clear)
                {
                    // increase players currency
                    if (doublebonus)
                    {
                        Vector3 pos = new Vector3()
                        {
                            x = transform.position.x + (pc.transform.position.x - transform.position.x) / 2,
                            y = transform.position.y + (pc.transform.position.y - transform.position.y) / 2,
                            z = transform.position.z + (pc.transform.position.z - transform.position.z) / 2
                        };

                        GameObject go = Instantiate(BonusImage, pos, Quaternion.identity);

                        AsteroidParticle ap = go.GetComponent<AsteroidParticle>();

                        Vector3 rot = new Vector3();

                        // when the bonusimage is left of the player
                        if (pos.x < pc.transform.position.x)
                        {
                            ap.minPos = new Vector3(-0.0001f, 0.0001f, 0f);
                            ap.maxPos = new Vector3(-0.0001f, 0.0001f, 0f);
                            rot.z = 50f;
                        }
                        // otherwise
                        else
                        {
                            ap.minPos = new Vector3(0.0001f, 0.0001f, 0f);
                            ap.maxPos = new Vector3(0.0001f, 0.0001f, 0f);
                            rot.z = -50f;
                        }

                        go.GetComponentInChildren<SpriteRenderer>().gameObject.transform.Rotate(rot, Space.Self);

                        pc.Survived += Bonus * 2;
                    }
                    else
                    {
                        pc.Survived += Bonus;
                    }
                    bonus = true;
                }
            }
        }
        if (transform.position.y <= -10f || transform.position.x <= -4f || transform.position.x >= 4f)
        {
            // destroy this enemie
            Destroy(gameObject);
        }

        if (transform.position.y <= -.4f && transform.position.y >= -.65f)
        {
            float distance = transform.position.x - pc.gameObject.transform.position.x;
            if (distance <= DoubleBonusRange && distance >= -DoubleBonusRange)
            {
                doublebonus = true;
            }
        }

        // if this object has a particle system
        if (ps != null)
        {
            var velocity = ps.velocityOverLifetime;

            float speed = dir.y / sm.MinSpeedY * sm.ParticleSpeedMultiplayerY;
            if (speed > 0)
            {
                speed *= -1;
            }
            velocity.yMultiplier = speed;

            speed = dir.x / sm.MinSpeedX * sm.ParticleSpeedMultiplayerX;
            velocity.xMultiplier = speed;
        }
    }
}
