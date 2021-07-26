using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedSpaceShuttle : MonoBehaviour
{
    public GameObject[] Parts;

    public float SpeedX;
    public float SpeedY;
    public float RotationSpeed;
    [Range(0, 1f)]
    public float PartsSpeedModifier;
    [Range(0, 1f)]
    public float PartsRotationModifier;
    [Space(10)]
    [Range(0, 1f)]
    public float FallSpeedModifier;
    [Range(0, 1f)]
    public float RotationsSpeedModifier;

    [Space(10)]
    public bool UnlockUniqueSkin;
    public string DesignName;
    public float YPos;
    private bool skinUnlocked = false;

    private List<Vector2> directions = new List<Vector2>();
    private List<int> layer = new List<int>();

    private PlayerStats ps;
    private SkinManager sm;
    private PlayerController pc;

    private void Start()
    {
        pc = FindObjectOfType<PlayerController>();

        ps = FindObjectOfType<PlayerController>().Stats;

        if (UnlockUniqueSkin)
        {
            sm = FindObjectOfType<SkinManager>();
        }

        if (Parts != null)
        {
            for (int i = 0; i < Parts.Length; i++)
            {
                Vector2 dir;

                if (Parts[i].transform.localPosition.x > 0f) { dir.x = 1; }
                else if (Parts[i].transform.localPosition.x < 0f) { dir.x = -1; }
                else { dir.x = 0; }

                if (Parts[i].transform.localPosition.y > 0f) { dir.y = 1; }
                else if (Parts[i].transform.localPosition.y < 0f) { dir.y = -1; }
                else { dir.y = 0; }

                directions.Add(dir);

                layer.Add(Parts[i].GetComponent<SpriteRenderer>().sortingOrder);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pc.Killall)
            Destroy(gameObject);

        // if palyer see full object
        if (transform.position.y <= YPos && !skinUnlocked && DesignName != "")
        {
            skinUnlocked = true;
            SkinManager.RocketSkin rocketSkin = sm.FindSkin(DesignName);

            // if skin founded by name
            if (rocketSkin.Name != "" && rocketSkin.Name != null)
            {
                // add skin
                SkinColor sc = new SkinColor
                {
                    r = Color.white.r,
                    g = Color.white.g,
                    b = Color.white.b,
                    a = Color.white.a,

                    true_r = Color.white.r,
                    true_g = Color.white.g,
                    true_b = Color.white.b,
                    true_a = Color.white.a
                };

                Skin skin = new Skin()
                {
                    id = rocketSkin.ID,
                    color = sc
                };
                ps.skinIDs.Add(skin);
            }
        }

        if (Parts != null)
        {
            for (int i = 0; i < Parts.Length; i++)
            {
                Parts[i].gameObject.transform.Translate(new Vector3(SpeedX * directions[i].x * ((1 + layer[i]) * PartsSpeedModifier) * Time.deltaTime,
                                                                    SpeedY * directions[i].y * ((1 + layer[i]) * PartsSpeedModifier) * Time.deltaTime, 0), Space.World);
                Parts[i].gameObject.transform.Rotate(0, 0, -(RotationSpeed * ((1 + layer[i]) * PartsRotationModifier) * Time.deltaTime) * directions[i].x);
            }
        }

        transform.Translate(new Vector3(0, ps.currentSpeed / 100 * FallSpeedModifier * Time.deltaTime * -1f, 0));
        transform.Rotate(0, 0, -(RotationSpeed * 2 * RotationsSpeedModifier) * Time.deltaTime);

        if (transform.position.y <= -2f)
        {
            Destroy(gameObject);
        }
    }
}
