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
    [SerializeField] private int _splineIndex;
    [SerializeField] [Range(0f, 1f)] private float _time;
    [SerializeField] private float _width;

    public SplineContainer Container => _splineContainer;
    public int NumSplines => _splineContainer != null ? _splineContainer.Splines.Count : 0;
    
    private float3 _position;
    private float3 _forward;
    private float3 _upVector;

    private Vector3 _p1;
    private Vector3 _p2;

    private void Update()
    {
        if (_splineContainer == null)
        {
            Debug.LogWarning("SplineContainer is not assigned.", this);
            return;
        }

        if (_splineIndex < 0 || _splineIndex >= NumSplines)
        {
            Debug.LogWarning("Spline index out of range.", this);
            return;
        }

        _splineContainer.Evaluate(_splineIndex, _time, out _position, out _forward, out _upVector);
        float3 right = Vector3.Cross(_forward, _upVector).normalized;

        _p1 = _position + (right * _width);
        _p2 = _position + (-right * _width);
    }

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

    private void OnDrawGizmos()
    {
        if (_splineContainer == null) return;

        Handles.matrix = transform.localToWorldMatrix;
        Handles.SphereHandleCap(0, _p1, Quaternion.identity, .5f, EventType.Repaint);
        Handles.DrawDottedLine(_p1, _p2, .5f);
        Handles.SphereHandleCap(0, _p2, Quaternion.identity, .5f, EventType.Repaint);
    }
}
