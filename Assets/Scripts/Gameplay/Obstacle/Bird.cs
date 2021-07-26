using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    /// <summary>
    /// Speed of the bird on X-Axis
    /// </summary>
    public float SpeedX;

    /// <summary>
    /// Speed of the Animation iun Frontlayer
    /// </summary>
    public float FrameCounter_Front;

    /// <summary>
    /// Speed of the Animation iun backlayer
    /// </summary>
    public float FrameCounter_Back;

    /// <summary>
    /// Image of grilled bird
    /// </summary>
    public Sprite GrilledBird;

    /// <summary>
    /// Sprites for Front-Animations
    /// </summary>
    public Sprite[] AnimationsFront;

    /// <summary>
    /// Sprites for Back-Animations
    /// </summary>
    public Sprite[] AnimationsBack;

    /// <summary>
    /// particlesystem of dead bird
    /// </summary>
    public GameObject Particle;

    /// <summary>
    /// speed of the bird on the y-axis
    /// </summary>
    private float speedY;

    /// <summary>
    /// Front-Spriterender of the bird
    /// </summary>
    public SpriteRenderer srFront;

    /// <summary>
    /// Back-Spriterender of the bird which is colorable
    /// </summary>
    public SpriteRenderer srBack;

    /// <summary>
    /// is this object a decoration
    /// </summary>
    public bool isDecoration;

    /// <summary>
    /// type of decoration
    /// </summary>
    public DecorationTyp decorationTyp;

    /// <summary>
    /// name of the decoration
    /// </summary>
    public string decorationName;

    /// <summary>
    /// count for animation in frontlayer
    /// </summary>
    private float timer_front;

    /// <summary>
    /// count for animation in backlayer
    /// </summary>
    private float timer_back;

    /// <summary>
    /// counter for animation in frontlayer
    /// </summary>
    private int count_front;

    /// <summary>
    /// counter for animation in backlayer
    /// </summary>
    private int count_back;

    /// <summary>
    /// playercontroller
    /// </summary>
    private PlayerController pc;

    /// <summary>
    /// is bird grilled?
    /// </summary>
    public bool grilled = false;

    /// <summary>
    /// Collider of this object
    /// </summary>
    public BoxCollider2D boxCollider2D;

    /// <summary>
    /// is particle spawned?
    /// </summary>
    private bool spawnparticle = false;

    /// <summary>
    /// y-speedchanger
    /// </summary>
    private float speedchanger;

    private bool right;

    private float grilledspeedY;

    private SpawnManager sm;

    private float distance;


    private void Start()
    {
        if (gameObject.layer == LayerMask.NameToLayer("Decoration"))
        {
            sm = FindObjectOfType<SpawnManager>();
        }

        pc = FindObjectOfType<PlayerController>();

        if (transform.position.x < 0)
        {
            srFront.flipX = true;
            if (srBack != null)
            {
                srBack.flipX = true;
            }
        }

        speedchanger = 0.00125f;

        if (SpeedX > 0)
            right = false;
        else
            right = true;

        distance = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (pc.Killall)
        {
            if (gameObject.layer == LayerMask.NameToLayer("Decoration"))
            {
                sm.DecorationCount--;
            }
            Destroy(gameObject);
        }

        if (grilledspeedY < pc.Stats.currentSpeed)
            grilledspeedY = pc.Stats.currentSpeed;

        // move bird
        if (gameObject.layer == LayerMask.NameToLayer("Decoration"))
        {
            if (decorationTyp == DecorationTyp.SPACE)
            {
                if (decorationName == "City")
                {
                    speedY = pc.Stats.currentSpeed / 100 * .1222f * Time.deltaTime;
                }
                else
                {
                    speedY = pc.Stats.currentSpeed / 100 * .26f * Time.deltaTime;
                }
            }
            else
            {
                speedY = pc.Stats.currentSpeed * 0.015f / 2.5f * Time.deltaTime;
            }
        }
        else
        {
            speedY = pc.Stats.currentSpeed * 0.015f * Time.deltaTime;
        }

        // count timer
        timer_front += Time.deltaTime;
        timer_back += Time.deltaTime;

        if (grilled)
        {
            speedY = grilledspeedY * 0.015f * Time.deltaTime;

            if (!spawnparticle)
            {
                for (int i = 0; i < 4; i++)
                {
                    // spawn birdparticle
                    GameObject go = Instantiate(Particle, transform.position, Quaternion.identity);

                    go.GetComponent<AsteroidParticle>().Particles[0].animated = true;
                    go.GetComponentInChildren<PolygonCollider2D>().enabled = false;
                }
                spawnparticle = true;
            }

            srFront.sprite = GrilledBird;

            speedY -= speedY * Time.deltaTime + speedchanger * Time.deltaTime;

            if (right && SpeedX != 0)
            {
                SpeedX -= SpeedX * Time.deltaTime + (speedchanger / 100) * Time.deltaTime;
                if (SpeedX > 0)
                    SpeedX = 0;
            }
            else if (!right && SpeedX != 0)
            {
                SpeedX -= SpeedX * Time.deltaTime + (speedchanger / 100) * Time.deltaTime;
                if (SpeedX < 0)
                    SpeedX = 0;
            }
        }

        Vector3 pos = transform.position;

        // animation
        if (FrameCounter_Front > 0)
        {
            // play animation
            if (timer_front >= FrameCounter_Front && !grilled)
            {
                // reduce timer
                timer_front -= FrameCounter_Front;

                if (AnimationsFront.Length > 0)
                {
                    // set new sprite
                    srFront.sprite = AnimationsFront[count_front];
                }

                // increase counter
                count_front++;

                // if count is too high
                if (count_front >= AnimationsFront.Length)
                {
                    // reset count
                    count_front = 0;
                }
            }
        }
        if (FrameCounter_Back > 0)
        { 
            // play animation
            if (timer_back >= FrameCounter_Back && !grilled)
            {
                // reduce timer
                timer_back -= FrameCounter_Back;

                if (AnimationsBack.Length > 0)
                {
                    // set new sprite
                    srBack.sprite = AnimationsBack[count_back];
                }

                // increase counter
                count_back++;

                // if count is too high
                if (count_back >= AnimationsBack.Length)
                {
                    // reset count
                    count_back = 0;
                }
            }
        }
        if (pos.x >= 1.5f || pos.x <= -1.5f || pos.y <= -1.5f)
        {
            if (gameObject.layer == LayerMask.NameToLayer("Decoration"))
            {
                sm.DecorationCount--;
            }
            Destroy(gameObject);
        }

        transform.Translate(new Vector3(SpeedX * Time.deltaTime / 10, -speedY, 0));

        if (!isDecoration)
        {
            if (!pc.Life)
            {
                if (!grilled)
                {
                    distance = Vector3.Distance(transform.position, pc.pressureWave.transform.position);
                    if (distance < 0f)
                        distance *= -1;

                    if (distance <= pc.pressureWave.Wave)
                        grilled = true;
                }
            }
        }
    }

    public void AddForce(Vector2 _velocity)
    {
        SpeedX += _velocity.x;
        speedY += _velocity.y;
    }

    public enum DecorationTyp
    {
        ATMOSPHERE,
        SPACE
    }
}

