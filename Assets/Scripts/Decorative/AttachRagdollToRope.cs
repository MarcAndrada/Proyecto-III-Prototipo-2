using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachRagdollToRope : MonoBehaviour
{
    [SerializeField] private Rigidbody ropeEnd;
    [SerializeField] private Rigidbody[] ragdollPart;

    private void Start()
    {
        HingeJoint joint = ropeEnd.gameObject.AddComponent<HingeJoint>();
        joint.connectedBody = ragdollPart[0];
    }
}
