using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdFloatingEffect : MonoBehaviour
{
    [SerializeField] private float amplitude = 0.5f; 
    [SerializeField] private  float frequency = 1f;  

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
