using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;

[ExecuteInEditMode()]
public class SplineSampler : MonoBehaviour
{
    [SerializeField] private SplineContainer _splineContainer;
    private int _splineIndex;
    
    public SplineContainer Container => _splineContainer;
    public int NumSplines => _splineContainer != null ? _splineContainer.Splines.Count : 0;
    
    private float3 _position;
    private float3 _forward;
    private float3 _upVector;

    private Vector3 _p1;
    private Vector3 _p2;

    public void SampleSplineWidth(int splineIndex, float t, float width, out Vector3 p1, out Vector3 p2)
    {
        if (_splineContainer == null)
        {
            p1 = p2 = Vector3.zero;
            Debug.LogWarning("SplineContainer is not assigned.");
            return;
        }

        _splineContainer.Evaluate(splineIndex, t, out _position, out _forward, out _upVector);
        float3 right = Vector3.Cross(_forward, _upVector).normalized;

        p1 = _position + (right * width);
        p2 = _position + (-right * width);
    }

    public void SampleSplineWidth(float t, float width, out Vector3 p1, out Vector3 p2)
    {
        SampleSplineWidth(_splineIndex, t, width, out p1, out p2);
    }

}
