using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    [SerializeField]
    protected InteractableObjectsInfo objectInfo;
    [SerializeField]
    private List<GameManager.ActionState> interactableActions;

    protected Outline outline;
    
    protected virtual void Awake()
    {
        outline = GetComponent<Outline>();
    }

    public virtual void OnHover()
    {
        if (!CanInteract())
            return;

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

    public bool CanInteract()
    {
        foreach (GameManager.ActionState item in interactableActions)
        {
            if(item == GameManager.Instance.actionState)
                return true;
        }

        return false;
    }

}
