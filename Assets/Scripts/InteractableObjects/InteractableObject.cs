using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    [SerializeField]
    protected InteractableObjectsInfo objectInfo;

    protected Outline outline;

    protected virtual void Awake()
    {
        outline = GetComponent<Outline>();
    }

    public virtual void OnHover()
    {
        if (outline)
            outline.enabled = true;
    }
    public virtual void StopHovering()
    {
        if (outline)
            outline.enabled = false;
    }

    public InteractableObjectsInfo GetObjectInfo()
    {
        return objectInfo;
    }

    public abstract void ActivateObject();


}
