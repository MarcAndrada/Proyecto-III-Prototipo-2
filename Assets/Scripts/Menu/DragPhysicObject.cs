using System;
using Unity.VisualScripting;
using UnityEngine;

public class DragPhysicObject : MonoBehaviour
{
    [SerializeField] private LayerMask dragLayers;

    [Header("Joint Configuration")]
    [Range(0.0f, 10.0f)][SerializeField] private float damping = 1.0f;
    [Range(0.0f, 10.0f)][SerializeField] private float frequency = 5.0f;

    private TargetJoint2D targetJoint;
    private Vector3 worldPos;
    private bool isMouseDown;

    private bool isInWorkSpace;
    private Vector2 initScale;

    private Rigidbody2D rb;

    private void Awake()
    {
        initScale= transform.localScale;
        isInWorkSpace = false;

        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (targetJoint != null)
        {
            targetJoint.target = worldPos;
        }
    }

    private void OnMouseDown()
    {
        isMouseDown = true;

        Collider2D collider = Physics2D.OverlapPoint(worldPos, dragLayers);
        if (!collider)
        {
            return;
        }

        targetJoint = gameObject.AddComponent<TargetJoint2D>();
        targetJoint.dampingRatio = damping;
        targetJoint.frequency = frequency;

        targetJoint.anchor = targetJoint.transform.InverseTransformPoint(worldPos);

        rb.bodyType = RigidbodyType2D.Dynamic;
    }
    private void OnMouseUp()
    {
        isMouseDown = false;
        Destroy(targetJoint);
        targetJoint = null;
    }
    public bool GetMouseDown()
    {
        return isMouseDown;
    }

    public bool GetIsInWorkSpace()
    {
        return isInWorkSpace;
    }
}
