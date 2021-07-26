using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudBlanket : MonoBehaviour
{
    /// <summary>
    /// Speed of the Cloudblanket
    /// </summary>
    public float Speed;

    private PlayerController pc;
    private void Start()
    {
        pc = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        float speed = pc.Stats.currentSpeed * 0.015f * Time.deltaTime;

        if (pc.Killall)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.Translate(0, -speed, 0);
        }

        if (transform.position.y <= -2f)
        {
            Destroy(gameObject);
        }
    }
}
