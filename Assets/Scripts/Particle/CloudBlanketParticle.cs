using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudBlanketParticle : MonoBehaviour
{
    /// <summary>
    /// Particlesprite for this particleeffect
    /// </summary>
    public Sprite[] Particles;

    /// <summary>
    /// Gameobjects of the particles flying to the left
    /// </summary>
    public GameObject[] particlesLeft;

    /// <summary>
    /// Gameobjects of the particles flying to the right
    /// </summary>
    public GameObject[] particlesRight;

    /// <summary>
    /// Colorgradient to fade out 
    /// </summary>
    public Gradient FadeColor;

    /// <summary>
    /// time for fade
    /// </summary>
    public float FadeTimer;

    /// <summary>
    /// physical interactable?
    /// </summary>
    public bool Physic = false;

    /// <summary>
    /// Position of the player
    /// </summary>
    public Vector3 PlayerPosition;

    /// <summary>
    /// Percentvalue of the distance depends on collider-radius
    /// </summary>
    public float DistancePercentFromRadius;

    /// <summary>
    /// move raycast target position on the y-axis
    /// </summary>
    public float HeightY;

    // speed for parentgameobject
    private float speedY = 0.07f;
    private float speedincreaserY = 1.05f;

    // speed for particleobjects on X-Axis
    private float p_speedXmin = 0.015f;
    private float p_speedXmax = 0.055f;
    private float[] p_speedX = new float[10];
    private float p_speedincreaserX = 1.05f;

    // speed for particleobjects on Y-Axis
    private float p_speedYmin = 0.055f;
    private float p_speedYmax = 0.155f;
    private float[] p_speedY = new float[10];
    private float p_speedincreaserY = 1.05f;

    private float rotationMin = 400;
    private float rotationMax = 800;
    private float[] rotationspeed = new float[10];

    private float timer;
    private float fadetimer;

    private int r;
    private int i;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPosition.y += HeightY;

        for (int i = 0; i < particlesLeft.Length + particlesRight.Length; i++)
        {
            p_speedX[i] = Random.Range(p_speedXmin, p_speedXmax);
            p_speedY[i] = Random.Range(p_speedYmin, p_speedYmax);
            rotationspeed[i] = Random.Range(rotationMin, rotationMax);
        }

        RaycastHit2D hit;
        Vector3 direction;
        float pushStrength;
        LayerMask layerMask = 1 << 14;
        Vector2 force;
        for (int i = 0; i < particlesLeft.Length; i++)
        {
            // set random sprites
            particlesLeft[i].GetComponent<SpriteRenderer>().sprite = Particles[Random.Range(0, Particles.Length)];
            particlesRight[i].GetComponent<SpriteRenderer>().sprite = Particles[Random.Range(0, Particles.Length)];

            // set new position for each particle depend on players rocket
            // left particle
            direction = (PlayerPosition - particlesLeft[i].transform.position).normalized;
            hit = Physics2D.Raycast(particlesLeft[i].transform.position, direction, 1f, layerMask);
            if (hit.collider.CompareTag("Player"))
            {
                pushStrength = hit.distance - particlesLeft[i].GetComponent<CircleCollider2D>().radius * DistancePercentFromRadius;
                particlesLeft[i].transform.Translate(direction * pushStrength, Space.Self);
            }
            // right particle
            direction = (PlayerPosition - particlesRight[i].transform.position).normalized;
            hit = Physics2D.Raycast(particlesRight[i].transform.position, direction, 1f, layerMask);
            if (hit.collider.CompareTag("Player"))
            {
                pushStrength = hit.distance - particlesRight[i].GetComponent<CircleCollider2D>().radius * DistancePercentFromRadius;
                particlesRight[i].transform.Translate(direction * pushStrength, Space.Self);
            }

            force = new Vector2(Random.Range(p_speedXmin, p_speedXmax), Random.Range(p_speedYmin, p_speedYmax));
            particlesLeft[i].GetComponent<Rigidbody2D>().AddForce(-force, ForceMode2D.Impulse);
            force = new Vector2(Random.Range(p_speedXmin, p_speedXmax), -Random.Range(p_speedYmin, p_speedYmax));
            particlesRight[i].GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
        }
    }

    // Update is called once per frame
    void Update()
    {
        fadetimer += Time.deltaTime;

        if (fadetimer > FadeTimer)
            fadetimer = FadeTimer;

        transform.Translate(new Vector3(0, -(speedY * Time.deltaTime), 0));

        r = 0;

        for (i = 0; i < particlesRight.Length; i++)
        {
            // particlesRight[i].transform.Translate(new Vector3(p_speedX[r] * Time.deltaTime, -p_speedY[r] * Time.deltaTime, 0), Space.World);
            particlesRight[i].transform.Rotate(0, 0, (p_speedX[r] + p_speedY[r]) * -rotationspeed[r] * Time.deltaTime);
            particlesRight[i].GetComponent<SpriteRenderer>().color = FadeColor.Evaluate(fadetimer / FadeTimer);
            r++;
        }

        for (i = 0; i < particlesLeft.Length; i++)
        {
            // particlesLeft[i].transform.Translate(new Vector3(-(p_speedX[r] * Time.deltaTime), -p_speedY[r] * Time.deltaTime, 0), Space.World);
            particlesLeft[i].transform.Rotate(0, 0, (p_speedX[r] + p_speedY[r]) * rotationspeed[r] * Time.deltaTime);
            particlesLeft[i].GetComponent<SpriteRenderer>().color = FadeColor.Evaluate(fadetimer / FadeTimer);
            r++;
        }

        timer += Time.deltaTime;

        if (timer >= 0.05f)
        {
            for (i = 0; i < r; i++)
            {
                p_speedX[i] /= p_speedincreaserX;
                p_speedY[i] *= p_speedincreaserY;
            }
            speedY *= speedincreaserY;
            timer -= 0.05f;
        }

        if (transform.position.y <= -1f)
        {
            Destroy(gameObject);
        }
    }
}
