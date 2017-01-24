using UnityEngine;
using System.Collections;

public class RepeatSound : MonoBehaviour 
{
    public float repeatFrequency = 0f;
    public Color blinkColor = Color.red;

    private float timer = 0f;

    private Color origColor = Color.white;
    private Renderer parentRenderer = null;
    private int blink_count = 0;
    private float blink_timer = 0f;
	
    void Start()
    {
        if (repeatFrequency < 0f)
            repeatFrequency = 0f;
        timer = repeatFrequency - (UnityEngine.Random.Range(0f, repeatFrequency) + (repeatFrequency / 2f));

        parentRenderer = transform.parent.GetComponent<Renderer>();
        origColor = parentRenderer.material.color;
    }

	// Update is called once per frame
	void Update () 
    {
        if (repeatFrequency <= 0f)
            return;

        if (timer >= repeatFrequency)
        {
            if (GetComponent<AudioSource>() != null)
            {
                GetComponent<AudioSource>().Stop();
                GetComponent<AudioSource>().Play();

                blink_count = 3;
                blink_timer = 0.5f;
            }
            timer = repeatFrequency - (UnityEngine.Random.Range(0f, repeatFrequency) + (repeatFrequency / 2f));
        }

        if (blink_count > 0)
        {
            if (parentRenderer != null)
            {
                if (blink_timer > 0)
                    parentRenderer.material.color = blinkColor;
                else
                    parentRenderer.material.color = origColor;

                blink_timer -= Time.deltaTime;
                if (blink_timer < -0.5f)
                {
                    blink_timer += 1f;
                    blink_count -= 1;

                    if (blink_count == 0)
                        parentRenderer.material.color = origColor;
                }
            }
            else
                blink_count = 0;
        }

        timer += Time.deltaTime;
	}
}
