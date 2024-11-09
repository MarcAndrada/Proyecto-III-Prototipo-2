using System.Collections.Generic;
using UnityEngine;
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
    [SerializeField, Range(0.01f,1f)] private float _curveStep = 0.1f;
    [SerializeField, Min(5)] private int _resolution = 10;
    
    [SerializeField] private List<Intersection> _intersections;
    public List<Intersection> Intersections => _intersections;
    
    private void Awake()
    {
        _splineSampler = gameObject.GetComponent<SplineSampler>();
        _meshFilter = gameObject.GetComponent<MeshFilter>();
        _meshRenderer = gameObject.GetComponent<MeshRenderer>();
    }

    private void OnEnable()
    {
        if (_splineSampler == null)
        {
            _splineSampler = gameObject.GetComponent<SplineSampler>();
            if (_splineSampler == null)
            {
                Debug.LogError("SplineSampler component not found!");
                return;
            }
        }

        Spline.Changed += OnSplineChanged;
        GetSplineVerts();
    }
    private void OnSplineChanged(Spline arg1, int arg2, SplineModification arg3)
    {
        Rebuild();
    }
    private void OnValidate()
    {
        Rebuild();
    }
    public void Rebuild()
    {
        GetSplineVerts();
        BuildMesh();
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
        List<Vector2> uvs = new List<Vector2>();
        
        int offset = 0;
        
        float uvOffset = 0;

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
                
                float distance = Vector3.Distance(p1, p3) / 4f;
                float uvDistance = uvOffset + distance;
                uvs.AddRange(new List<Vector2> { new Vector2(uvOffset, 0), new Vector2(uvOffset, 1), new Vector2(uvDistance, 0), new Vector2(uvDistance, 1) });

                uvOffset += distance;
            }
        }
        List<int> trisB = new List<int>();

        GetIntersectionVerts(verts, trisB, uvs);

        mesh.subMeshCount = 2;

        mesh.SetVertices(verts);

        mesh.SetTriangles(tris, 0);
        mesh.SetTriangles(trisB, 1);
        
        mesh.SetUVs(0, uvs);

        _meshFilter.mesh = mesh;
    }
    struct JunctionEdge
    {
        public Vector3 left;
        public Vector3 right;

        public Vector3 Center => (left + right)/2;

        public JunctionEdge (Vector3 p1, Vector3 p2)
        {
            this.left = p1;
            this.right = p2;
        }
    }
    private void GetIntersectionVerts(List<Vector3> verts, List<int> tris, List<Vector2> uvs)
    {
        for (int i = 0; i < _intersections.Count; i++)
        {
            Intersection intersection = _intersections[i];
            int count = 0;

            List<JunctionEdge> junctionEdges = new List<JunctionEdge>();

            Vector3 center = new Vector3();
            foreach (JunctionInfo junction in intersection.GetJunctions())
            {
                int splineIndex = junction.splineIndex;
                float t = junction.knotIndex == 0 ? 0f : 1f;
                _splineSampler.SampleSplineWidth(splineIndex, t, _width, out Vector3 p1, out Vector3 p2);

                if (junction.knotIndex == 0)
                {
                    junctionEdges.Add(new JunctionEdge(p1, p2));
                }
                else
                {
                    junctionEdges.Add(new JunctionEdge(p2, p1));

                }

                center += p1;
                center += p2;
                count++;
            }

            center /= junctionEdges.Count * 2;

            junctionEdges.Sort((x, y) => SortPoints(center, x.Center, y.Center));
            
            List<Vector3> curvePoints = new List<Vector3>();

            Vector3 mid;
            Vector3 c;
            Vector3 b;
            Vector3 a;
            BezierCurve curve;
            for (int j = 1; j <= junctionEdges.Count; j++)
            {
                a = junctionEdges[j - 1].left;
                curvePoints.Add(a);
                b = (j < junctionEdges.Count) ? junctionEdges[j].right : junctionEdges[0].right;
                mid = Vector3.Lerp(a, b, 0.5f);
                Vector3 dir = center - mid;
                mid = mid - dir;
                c = Vector3.Lerp(mid, center, intersection.curves[j - 1]);

                curve = new BezierCurve(a, c, b);
                for (float t = 0f; t < 1f; t += _curveStep)
                {
                    Vector3 pos = CurveUtility.EvaluatePosition(curve, t);
                    curvePoints.Add(pos);
                }

                curvePoints.Add(b);
            }

            curvePoints.Reverse();

            int pointsOffset = verts.Count;

            for (int j = 1; j <= curvePoints.Count; j++)
            {

                Vector3 pointA = curvePoints[j - 1];
                Vector3 pointB;
                if (j == curvePoints.Count)
                {
                    pointB = curvePoints[0];
                }
                else
                {
                    pointB = curvePoints[j];
                }

                verts.Add(center);
                verts.Add(pointA);
                verts.Add(pointB);

                tris.Add(pointsOffset + ((j - 1) * 3) + 0);
                tris.Add(pointsOffset + ((j - 1) * 3) + 1);
                tris.Add(pointsOffset + ((j - 1) * 3) + 2);

                uvs.Add(new Vector2(center.z, center.x));
                uvs.Add(new Vector2(pointA.z, pointA.x));
                uvs.Add(new Vector2(pointB.z, pointB.x));

            }
        }
    }
    private int SortPoints(Vector3 center, Vector3 x, Vector3 y)
    {
        Vector3 xDir = x - center;
        Vector3 yDir = y - center;

        float angleA = Vector3.SignedAngle(center.normalized, xDir.normalized, Vector3.up);
        float angleB = Vector3.SignedAngle(center.normalized, yDir.normalized, Vector3.up);

        if (angleA > angleB)
        {
            return -1;
        }
        if (angleA < angleB)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
    private void OnDisable()
    {
        if (_splineSampler != null)
        {
            Spline.Changed -= OnSplineChanged;
        }
    }

    public void AddJunction(Intersection junction)
    {
        if (_intersections == null)
        {
            _intersections = new List<Intersection>();
        }

        _intersections.Add(junction);
    }
}
