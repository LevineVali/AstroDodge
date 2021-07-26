using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidParticle : MonoBehaviour
{
    /// <summary>
    /// all Particles of asteroids
    /// </summary>
    public Particle[] Particles;

    [Space(20)]
    /// <summary>
    /// color of particle
    /// </summary>
    public Color pcolor;

    public Vector3 minPos = new Vector3(-0.0351f, -0.0392f, 0);
    public Vector3 maxPos = new Vector3(0.0351f, 0.0392f, 0);

    // speed for parentgameobject
    public float speedY = 0.07f;
    public float speedincreaserY = 1.05f;

    // speed for particleobjects on X-Axis
    public float p_speedXmin = 0.15f;
    public float p_speedXmax = 0.25f;
    public float p_speedincreaserX = 1.0125f;

    // speed for particleobjects on Y-Axis
    public float p_speedYmin = 0.015f;
    public float p_speedYmax = 0.025f;
    public float p_speedincreaserY = 1.05f;

    public float rotationMin = 400;
    public float rotationMax = 800;

    /// <summary>
    /// size for speeds, roations and positions
    /// </summary>
    public int size;

    [Header("Pressure Wave")]
    /// <summary>
    /// is pressure wave aviable?
    /// </summary>
    public bool PressureWave = false;

    /// <summary>
    /// Pressurwave object
    /// </summary>
    public PressureWave PressureWaveObject;

    private float[] p_speedY = new float[7];
    private float[] p_speedX = new float[7];
    private float[] rotationspeed = new float[7];

    /// <summary>
    /// List of all particlepositions
    /// </summary>
    private Vector3[] ppos = new Vector3[14];

    private SpriteRenderer[] sprenderer;

    private float timer;

    private int i;

    // Start is called before the first frame update
    void Start()
    {
        p_speedY = new float[size];
        p_speedX = new float[size];
        rotationspeed = new float[size];
        ppos = new Vector3[size];

        Vector3 pos;

        // for all particles
        for (i = 0; i < Particles.Length; i++)
        {
            // get random position
            pos = new Vector3(UnityEngine.Random.Range(minPos.x, maxPos.x), UnityEngine.Random.Range(minPos.y, maxPos.y), 0);

            p_speedX[i] = UnityEngine.Random.Range(p_speedXmin, p_speedXmax);
            p_speedY[i] = UnityEngine.Random.Range(p_speedXmin, p_speedYmax);
            rotationspeed[i] = UnityEngine.Random.Range(rotationMin, rotationMax);

            Particles[i].gameObject.transform.position = pos + transform.position;

            if (Particles[i].colorChangable)
                Particles[i].gameObject.GetComponent<SpriteRenderer>().color = pcolor;

            ppos[i] = pos;
        }

        // create new array
        sprenderer = new SpriteRenderer[Particles.Length];

        // go through all particles
        for (i = 0; i < sprenderer.Length; i++)
        {
            // get there spriterenderers
            sprenderer[i] = Particles[i].gameObject.GetComponent<SpriteRenderer>();
        }

        if (PressureWave)
        {
            PressureWaveObject.gameObject.SetActive(true);
            PressureWaveObject.transform.position = transform.position;
            PressureWaveObject.Life = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, -(speedY * Time.deltaTime), 0));

        for (i = 0; i < Particles.Length; i++)
        {
            // if particle is top left
            if (ppos[i].x < 0 && ppos[i].y > 0)
            {
                Particles[i].gameObject.transform.Translate(new Vector3(-p_speedX[i] * Time.deltaTime, p_speedY[i] * Time.deltaTime, 0), Space.World);
                Particles[i].gameObject.transform.Rotate(0, 0, (p_speedX[i] + p_speedY[i]) * rotationspeed[i] * Time.deltaTime);
            }

            // if particle is top right
            if (ppos[i].x > 0 && ppos[i].y > 0)
            {
                Particles[i].gameObject.transform.Translate(new Vector3(p_speedX[i] * Time.deltaTime, p_speedY[i] * Time.deltaTime, 0), Space.World);
                Particles[i].gameObject.transform.Rotate(0, 0, -(p_speedX[i] + p_speedY[i]) * rotationspeed[i] * Time.deltaTime);
            }

            // if particle is bottom left
            if (ppos[i].x < 0 && ppos[i].y < 0)
            {
                Particles[i].gameObject.transform.Translate(new Vector3(-p_speedX[i] * Time.deltaTime, -p_speedY[i] * Time.deltaTime, 0), Space.World);
                Particles[i].gameObject.transform.Rotate(0, 0, (p_speedX[i] + p_speedY[i]) * rotationspeed[i] * Time.deltaTime);
            }

            // if particle is bottom right
            if (ppos[i].x > 0 && ppos[i].y < 0)
            {
                Particles[i].gameObject.transform.Translate(new Vector3(p_speedX[i] * Time.deltaTime, -p_speedY[i] * Time.deltaTime, 0), Space.World);
                Particles[i].gameObject.transform.Rotate(0, 0, -(p_speedX[i] + p_speedY[i]) * rotationspeed[i] * Time.deltaTime);
            }

            if (Particles[i].animated)
            {
                if (Particles[i].frames.Length > 0)
                {
                    Particles[i].frameTimer += Time.deltaTime;
                    if (Particles[i].frameTimer > Particles[i].frameTime)
                    {
                        Particles[i].frameTimer -= Particles[i].frameTime;
                        Particles[i].frameCounter++;
                    }

                    if (Particles[i].looped)
                    {
                        if (Particles[i].frameCounter >= Particles[i].frames.Length)
                            Particles[i].frameCounter = 0;
                    }
                    if (Particles[i].frameCounter < Particles[i].frames.Length)
                    {
                        sprenderer[i].sprite = Particles[i].frames[Particles[i].frameCounter];
                    }
                }
            }
        }

        timer += Time.deltaTime;

        if (timer >= 0.05f)
        {
            for (i = 0; i < Particles.Length; i++)
            {
                p_speedX[i] /= p_speedincreaserX;
                p_speedY[i] /= p_speedincreaserY;
            }
            speedY *= speedincreaserY;
            timer -= 0.05f;
        }

        if (transform.position.y <= -2f)
        {
            Destroy(gameObject);
        }
    }

    public void AddForce(Vector2 _velocity)
    {
        p_speedX[0] += _velocity.x;
        p_speedY[0] += _velocity.y;
    }

    [Serializable]
    public struct Particle
    {
        public GameObject gameObject;
        public bool colorChangable;
        [Header("Animation of Particles")]
        public bool animated;
        public bool looped;
        public float frameTime;
        public Sprite[] frames;
        [HideInInspector]
        public float frameTimer;
        [HideInInspector]
        public int frameCounter;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Pressurewave") && gameObject.layer == 9 /*Feather*/)
        {
            for (int i = 0; i < Particles.Length; i++)
            {
                Particles[i].animated = true;
            }
        }
    }

    private void OnCollisionStay(Collision collisionInfo)
    {
        if (collisionInfo.collider.gameObject.CompareTag("Pressurewave") && gameObject.layer == 9 /*Feather*/)
        {
            for (int i = 0; i < Particles.Length; i++)
            {
                Particles[i].animated = true;
            }
        }
    }
}
