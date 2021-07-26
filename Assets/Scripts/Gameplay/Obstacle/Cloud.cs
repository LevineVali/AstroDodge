using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float Speed = 0.00015f;

    public bool col = false;

    public BoxCollider2D Bottom;
    public BoxCollider2D Left;
    public BoxCollider2D Right;

    public Vector3 ColiderPos;

    public CloudTyp Typ;

    /// <summary>
    /// Object of Lightning
    /// </summary>
    public GameObject LightningObject;

    public Color DummyColor;
    public Color DangerColor;

    public SpriteRenderer Renderer;

    public Sound ThunderSound;

    private PlayerController pc;

    // is colider active?
    private bool cactive = true;

    private float timerMax;
    private float timer;

    // is lightning spawned?
    private bool lightningIsSpawn;

    private GameObject lightning;

    // Start is called before the first frame update
    void Start()
    {
        pc = FindObjectOfType<PlayerController>();

        if (col)
        {
            col = false;
            cactive = false;
        }

        lightningIsSpawn = true;
        if (Typ == CloudTyp.DANGER)
        {
            timerMax = Random.Range(0, 2f);
            lightningIsSpawn = false;
            Renderer.color = DangerColor;
        }
        if (Typ == CloudTyp.DUMMY)
        {
            Renderer.color = DummyColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timerMax && !lightningIsSpawn)
        {
            lightning = Instantiate(LightningObject, transform.position, Quaternion.identity);
            lightning.transform.parent = transform;
            lightningIsSpawn = true;
            // play thudnersound
            ThunderSound.Source.PlayOneShot(ThunderSound.Clip);
        }

        if (pc.Killall)
            Destroy(gameObject);
        float speed = pc.Stats.currentSpeed * Speed * Time.deltaTime;
        transform.Translate(new Vector3(0, -speed, 0));

        if (transform.position.y <= -1.25f)
        {
            if (Typ == CloudTyp.DANGER)
            {
                if (lightning == null)
                    Destroy(gameObject);
            }
            else
                Destroy(gameObject);
        }

        if (col)
        {
            Bottom.enabled = false;
            Left.enabled = false;
            Right.enabled = false;

            cactive = false;
            col = false;
        }

        if (cactive)
        {
            float distanceX = transform.position.x - pc.transform.position.x;
            // if player is left of the cloud
            if (distanceX >= ColiderPos.y)
            {
                Bottom.enabled = false;
                Left.enabled = true;
                Right.enabled = false;
            }
            // if player is right of the cloud
            else if (distanceX <= ColiderPos.x)
            {
                Bottom.enabled = false;
                Left.enabled = false;
                Right.enabled = true;
            }
            // if player is under the cloud
            else
            {
                Bottom.enabled = true;
                Left.enabled = false;
                Right.enabled = false;
            }

            if (transform.position.y <= pc.transform.position.y + ColiderPos.z)
            {
                Bottom.enabled = false;
                Left.enabled = false;
                Right.enabled = false;

                cactive = false;
            }
        }
    }

    public enum CloudTyp
    {
        NORMAL,
        DANGER,
        DUMMY
    }

    public void DeactivateCollider()
    {
        Bottom.enabled = false;
        Left.enabled = false;
        Right.enabled = false;

        cactive = false;
    }
}
