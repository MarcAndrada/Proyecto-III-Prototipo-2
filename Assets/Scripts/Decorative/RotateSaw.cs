using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSaw : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 100f;

    void Update()
    {
        // Rotate the saw around its local Z-axis
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
