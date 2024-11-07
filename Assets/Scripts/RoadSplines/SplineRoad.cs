using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Splines;


[RequireComponent(typeof(SplineSampler))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode()]
public class SplineRoad : MonoBehaviour
{
    private SplineSampler _splineSampler;
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;

    private List<Vector3> _vertsP1;
    private List<Vector3> _vertsP2;
    
    [SerializeField] private float _width;
    [SerializeField, Min(5)] private int _resolution = 10;
    
    private void Awake()
    {
        _splineSampler = gameObject.GetComponent<SplineSampler>();
        _meshFilter = gameObject.GetComponent<MeshFilter>();
        _meshRenderer = gameObject.GetComponent<MeshRenderer>();
    }

    private void OnEnable()
    {
        if (_splineSampler == null)
            _splineSampler = GetComponent<SplineSampler>();
        if (_meshFilter == null)
            _meshFilter = GetComponent<MeshFilter>();
        if (_meshRenderer == null)
            _meshRenderer = GetComponent<MeshRenderer>();
        
        Spline.Changed += OnSplineChanged;
        GetSplineVerts();
    }
    private void OnSplineChanged(Spline arg1, int arg2, SplineModification arg3)
    {
        Rebuild();
    }
    public void Rebuild()
    {
        GetSplineVerts();
        BuildMesh();
    }
    private void OnValidate()
    {
        if (_splineSampler == null)
            _splineSampler = GetComponent<SplineSampler>();
        if (_meshFilter == null)
            _meshFilter = GetComponent<MeshFilter>();
        if (_meshRenderer == null)
            _meshRenderer = GetComponent<MeshRenderer>();
        
        Rebuild();
    }
    private void GetSplineVerts()
    {
        if (_splineSampler == null)
        {
            Debug.LogError("SplineSampler is not assigned or initialized.");
            return;
        }
        
        _vertsP1 = new List<Vector3>();
        _vertsP2 = new List<Vector3>();

        float step = 1f / (float)_resolution;
        Vector3 p1;
        Vector3 p2;
        for (int j = 0; j < _splineSampler.NumSplines; j++)
        {
            for (int i = 0; i < _resolution; i++)
            {
                float t = step * i;
                _splineSampler.SampleSplineWidth(j, t, _width, out p1, out p2);
                _vertsP1.Add(p1);
                _vertsP2.Add(p2);
            }

            _splineSampler.SampleSplineWidth(j, 1f, _width, out p1, out p2);
            _vertsP1.Add(p1);
            _vertsP2.Add(p2);
        }
    }

    private void BuildMesh()
    {
        Mesh mesh = new Mesh();

        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();
        int offset = 0;
        
        int length = _vertsP2.Count;

        for (int currentSplineIndex = 0; currentSplineIndex < _splineSampler.NumSplines; currentSplineIndex++)
        {
            int splineOffset = _resolution * currentSplineIndex;
            splineOffset += currentSplineIndex;
            for (int currentSplinePoint = 1; currentSplinePoint < _resolution + 1; currentSplinePoint++)
            {
                int vertoffset = splineOffset + currentSplinePoint;
                Vector3 p1 = _vertsP1[vertoffset - 1];
                Vector3 p2 = _vertsP2[vertoffset - 1];
                Vector3 p3 = _vertsP1[vertoffset];
                Vector3 p4 = _vertsP2[vertoffset];

                offset = 4 * _resolution * currentSplineIndex;
                offset += 4 * (currentSplinePoint - 1);
            
                int t1 = offset + 0;
                int t2 = offset + 2;
                int t3 = offset + 3;

                int t4 = offset + 3;
                int t5 = offset + 1;
                int t6 = offset + 0;

                verts.AddRange(new List<Vector3> { p1, p2, p3, p4 });
                tris.AddRange(new List<int> { t1, t2, t3, t4, t5, t6 });
            }
        }
        
        mesh.SetVertices(verts);
        mesh.SetTriangles(tris, 0);
        _meshFilter.mesh = mesh;
    }
}
