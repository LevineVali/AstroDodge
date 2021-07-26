using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
    /// <summary>
    /// Playerstats
    /// </summary>
    private PlayerController pc;

    /// <summary>
    /// SkinManager of this game
    /// </summary>
    private SkinManager skinManager;

    private void Start()
    {
        pc = FindObjectOfType<PlayerController>();

        skinManager = FindObjectOfType<SkinManager>();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Feather"))
        {
            // activate animation of burning feather
            AsteroidParticle asteroidParticles = other.GetComponentInParent<AsteroidParticle>();
            asteroidParticles.Particles[0].animated = true;

            // disable collider to save resource and no longer needed collider
            other.GetComponent<PolygonCollider2D>().enabled = false;
        }
        if (other.CompareTag("Bird"))
        {
            // activate animation of burning bird
            other.GetComponent<Bird>().grilled = true;

            // disable collider to save resource and no longer needed collider
            other.GetComponent<BoxCollider2D>().enabled = false;

            int id = 0;
            for (int i = 0; i < skinManager.Skins.Length; i++)
            {
                if (skinManager.Skins[i].currencyType == SkinManager.CurrencyType.COOKED_BIRD) { id = i; break; }
            }

            if (pc.Stats.cookedbirdCount < skinManager.Skins[id].cost)
            {
                pc.Stats.cookedbirdCount++;
                pc.CollectableCheck(SkinManager.CurrencyType.COOKED_BIRD, id);
            }
        }
    }
}