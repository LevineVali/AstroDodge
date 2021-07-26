using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lightning : MonoBehaviour
{
    /// <summary>
    /// Time in seconds for first lightning shining
    /// </summary>
    public float Duration1;

    /// <summary>
    /// Time in seconds for waiting until 2nd shining
    /// </summary>
    public float WaitTime;

    /// <summary>
    /// Time in seconds for second shining in full strength
    /// </summary>
    public float Duration2;

    /// <summary>
    /// Time in seconds for fading shining
    /// </summary>
    public float FadeDuration;

    /// <summary>
    /// Effect for lightning
    /// </summary>
    public Image LightningEffect;

    public Sprite[] Sprites;

    public GameObject[] Colliders;

    public SpriteRenderer Renderer;

    private int state;

    private float timer;

    private int index;

    private float maxAlpha = 128f / 255f;
    private Color color;
    // Start is called before the first frame update
    void Start()
    {
        index = Random.Range(0, Sprites.Length);

        Renderer.sprite = Sprites[index];
        Colliders[index].SetActive(true);

        state = 0;
        color = LightningEffect.color;
        color.a = maxAlpha;

        LightningEffect.color = color;

        index = Random.Range(0, 2);
        if (index == 0)
        {
            Renderer.gameObject.transform.Rotate(Vector3.down, 180f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (state == 0)
        {
            if (timer >= Duration1)
            {
                color.a = 0;
                LightningEffect.color = color;

                Renderer.color = new Color(1, 1, 1, 0);
                timer -= Duration1;
                state++;
            }
        }
        else if (state == 1)
        {
            if (timer >= WaitTime)
            {
                color.a = maxAlpha;
                LightningEffect.color = color;

                Renderer.color = new Color(1, 1, 1, 1);
                timer -= WaitTime;
                state++;
            }
        }
        else if (state == 2)
        {
            if (timer >= Duration2)
            {
                timer -= WaitTime;
                state++;
            }
        }
        else
        {
            Renderer.color = new Color(1, 1, 1, 1f - (timer / FadeDuration));

            color.a = Renderer.color.a * maxAlpha;
            LightningEffect.color = color;

            if (Renderer.color.a <= 0)
                Destroy(gameObject);
        }
    }
}
