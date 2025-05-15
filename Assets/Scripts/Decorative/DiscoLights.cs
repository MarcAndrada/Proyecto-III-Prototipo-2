using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoLights : MonoBehaviour
{
    [SerializeField] private Light discoLight;
    [SerializeField] private float colorChangeSpeed = 5f;

    private Color[] vibrantColors = new Color[]
    {
        Color.red,
        Color.green,
        Color.blue,
        Color.yellow,
        Color.magenta,
        Color.cyan
    };

    private int currentColorIndex = 0;
    private int nextColorIndex = 1;
    private float t = 0f;

    void Start()
    {
        if (discoLight == null)
        {
            discoLight = GetComponent<Light>();
        }
    }

    void Update()
    {
        if (discoLight == null) return;

        discoLight.color = Color.Lerp(vibrantColors[currentColorIndex], vibrantColors[nextColorIndex], t);

        t += Time.deltaTime * colorChangeSpeed;

        if (t >= 1f)
        {
            t = 0f;
            currentColorIndex = nextColorIndex;
            nextColorIndex = (nextColorIndex + 1) % vibrantColors.Length;
        }
    }
}
