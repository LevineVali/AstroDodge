using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketStartParticle : MonoBehaviour
{
    [Header("Particles")]
    public GameObject[] ParticlesLeft;
    public GameObject[] ParticlesRight;
    public int size;

    [Header("Speedoptions")]
    public float minSpeed = 0.15f;
    public float maxSpeed = 0.25f;
    public float startModifier;

    private float[] speeds = new float[6];

    /// <summary>
    /// positionrange for particles
    /// x = min pos
    /// y = max pos
    /// </summary>
    private Vector2 posrange = new Vector2(0.025f, 0.125f);

    private Color pcolor;

    private PlayerController pc;

    // Start is called before the first frame update
    void Start()
    {
        speeds = new float[size];
        for (int i = 0; i < speeds.Length; i++)
        {
            speeds[i] = Random.Range(minSpeed, maxSpeed);
        }
        pcolor = ParticlesLeft[0].GetComponent<SpriteRenderer>().color;
        pc = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        int r = 0;
        float value = 0;
        float speed;
        Vector2 range;

        for (int i = 0; i < ParticlesLeft.Length; i++)
        {
            // player is ready to start
            if (!pc.Run)
            {
                speed = speeds[r];
                range = posrange;
                if (i > 2)
                {
                    ParticlesLeft[i].GetComponent<SpriteRenderer>().color = new Color(pcolor.r, pcolor.g, pcolor.b, 0);
                    continue;
                }
            }
            else
            {
                range = new Vector2(posrange.x, posrange.y * startModifier);
                speed = speeds[r] * startModifier;
            }

            float posx = ParticlesLeft[i].transform.position.x;
            posx *= -1;

            // calculate transparent value
            value = 1 - (posx / range.y);

            // if transparent is not full
            if (ParticlesLeft[i].transform.position.x > -range.y)
            {
                ParticlesLeft[i].GetComponent<SpriteRenderer>().color = new Color(pcolor.r, pcolor.g, pcolor.b, value);
                ParticlesLeft[i].transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0), Space.Self);
            }
            else
            {
                ParticlesLeft[i].GetComponent<SpriteRenderer>().color = pcolor;
                ParticlesLeft[i].transform.position = new Vector3(-range.x, transform.position.y, 0);

                speeds[r] = Random.Range(minSpeed, maxSpeed);
            }

            r++;
        }
        for (int i = 0; i < ParticlesRight.Length; i++)
        {
            // player is ready to start
            if (!pc.Run)
            {
                speed = speeds[r];
                range = posrange;
                if (i > 2)
                {
                    ParticlesRight[i].GetComponent<SpriteRenderer>().color = new Color(pcolor.r, pcolor.g, pcolor.b, 0);
                    continue;
                }
            }
            else
            {
                range = new Vector2(posrange.x, posrange.y * startModifier);
                speed = speeds[r] * startModifier;
            }

            float posx = ParticlesRight[i].transform.position.x;

            // calculate transparent value
            value = 1 - (posx / range.y);

            // if transparent is not full
            if (ParticlesRight[i].transform.position.x < range.y)
            {
                ParticlesRight[i].GetComponent<SpriteRenderer>().color = new Color(pcolor.r, pcolor.g, pcolor.b, value);
                ParticlesRight[i].transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0), Space.Self);
            }
            else
            {
                ParticlesRight[i].GetComponent<SpriteRenderer>().color = pcolor;
                ParticlesRight[i].transform.position = new Vector3(range.x, transform.position.y, 0);

                speeds[r] = Random.Range(minSpeed, maxSpeed);
            }

            r++;
        }

    }
}
