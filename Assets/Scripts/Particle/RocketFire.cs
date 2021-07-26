using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketFire : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        if (other.layer == 8 /*Bird Layer*/)
        {
            other.GetComponent<Bird>().grilled = true;
        }

        if (other.layer == 9 /*Feather Layer*/)
        {
            var particles = other.GetComponent<AsteroidParticle>().Particles;

            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].animated = true;
            }
        }
    }
}
