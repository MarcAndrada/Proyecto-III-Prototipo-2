using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject objectToFollow;
    [SerializeField]
    private float YOffset;
    [SerializeField]
    private float ZOffset;


    private void Update()
    {
        transform.position = CalculateObjectOffset();

        transform.forward = (objectToFollow.transform.position - transform.position).normalized;

    }
    private Vector3 CalculateObjectOffset()
    {
        Vector3 destinyPos = objectToFollow.transform.position;

        destinyPos += (-objectToFollow.transform.forward * ZOffset) + (Vector3.up * YOffset);

        return destinyPos;
    }
}
