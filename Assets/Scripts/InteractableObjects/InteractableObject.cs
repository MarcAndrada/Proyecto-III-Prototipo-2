using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    [SerializeField]
    protected InteractableObjectsInfo objectInfo;
    [SerializeField]
    private List<GameManager.ActionState> interactableActions;

    protected List<Outline> outline;
    protected Animator animator;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();

        outline = new List<Outline>();
        Outline[] objectOutlines = GetComponents<Outline>();

        Outline[] childsOutlines = GetComponentsInChildren<Outline>();

        foreach (Outline obj in objectOutlines) 
        { 
            outline.Add(obj);
        }
        foreach (Outline obj in childsOutlines)
        {
            outline.Add(obj);
        }


    }

    public virtual void OnHover()
    {
        if (!CanInteract())
            return;

        if (outline.Count > 0)
        {
            foreach (Outline obj in outline)
            {
                obj.enabled = true;
            }
        }
    }
    public virtual void StopHovering()
    {
        if (outline.Count > 0)
        {
            foreach (Outline obj in outline)
            {
                obj.enabled = false;
            }
        }
    }


    public InteractableObjectsInfo GetObjectInfo()
    {
        return objectInfo;
    }

    public virtual void ActivateObject()
    {
        transform.parent = null;

        animator.enabled = true;
        //Play a la animacion
        animator.SetTrigger("DoAction");
        gameObject.layer = LayerMask.NameToLayer("Default");
    }
    public abstract void UseObject();
    public void DestroyObject()
    {
        Destroy(gameObject);
    }

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
