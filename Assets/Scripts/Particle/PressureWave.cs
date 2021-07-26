using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PressureWave : MonoBehaviour
{
    /// <summary>
    /// is Pressurewave spawned
    /// </summary>
    public bool Life;

    /// <summary>
    /// speed of the particles
    /// </summary>
    public float ParticleSpeed = 10.0f;

    /// <summary>
    /// ammount of particle that should spawn
    /// </summary>
    public int ParticleAmount = 10;

    /// <summary>
    /// particle prefab
    /// </summary>
    public GameObject Particle;

    /// <summary>
    /// collisionwave-distance
    /// </summary>
    public float Wave;

    /// <summary>
    /// array of all particles
    /// </summary>
    private Transform[] particles;

    /// <summary>
    /// array of all velocitys for all particles
    /// </summary>
    private Vector2[] velocitys;

    /// <summary>
    /// timer until this object will destroy itself
    /// </summary>
    private float time = 2.0f;

    private void Start()
    {
        // create new array with the lenght of all childobjects
        particles = new Transform[ParticleAmount];

        // create new array with the length of all childobjects
        velocitys = new Vector2[ParticleAmount];

        float angel = 360 / ParticleAmount;

        // fill the particles array with transform from each childobject(particles)
        for (int i = 0; i < ParticleAmount; i++)
        {
            particles[i] = Instantiate(Particle, transform).transform;

            velocitys[i] = rotate(particles[i].up, angel * i);
        }

        Life = false;

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Life)
        {
            // calculate new time
            time -= Time.deltaTime;

            // if time is over, destroy this object
            if (time <= 0)
                ResetParticle();

            // go through all particles
            for (int i = 0; i < particles.Length; i++)
            {
                Vector2 speed = new Vector2(ParticleSpeed * Time.deltaTime, ParticleSpeed * Time.deltaTime);
                Vector2 velocity = velocitys[i] * speed;

                particles[i].Translate(velocity, Space.Self);
            }

            float distance = Vector3.Distance(Vector3.zero, particles[0].transform.localPosition);
            if (distance < 0)
                distance *= -1f;

            Wave = distance + 0.03f;
        }
    }

    private Vector2 rotate(Vector2 _pos, float _angel)
    {
        Vector2 result;

        result.x = _pos.x * Mathf.Cos(_angel) - _pos.y * Mathf.Sin(_angel);
        result.y = _pos.x * Mathf.Sin(_angel) + _pos.y * Mathf.Cos(_angel);

        return result;
    }

    private void ResetParticle()
    {
        Life = false;
        time = 2.0f;

        // go through all particles
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].localPosition = Vector3.zero;
        }

        transform.position = new Vector3(0f, -10f, 0f);

        Wave = 0.0f;

        gameObject.SetActive(false);
    }
}
