using UnityEngine;

public class ButtonObject : InteractableObject
{
    [SerializeField]
    private Vector2Int starterPos;
    [SerializeField]
    private Vector2Int buttonDirection;
    
    [Space, SerializeField]
    private TurnController turnController;
    [Space, SerializeField]
    private AK.Wwise.Event hoverButtonEvent;
    [SerializeField]
    private AK.Wwise.Event pressButtonEvent;
    public override void OnHover()
    {
        if (!CanInteract())
            return;
        if (!outline[0].enabled)
            AkUnitySoundEngine.PostEvent(hoverButtonEvent.Id, gameObject);
        base.OnHover();
        turnController.DisplayTurnDirection(starterPos, buttonDirection);
    }

    public override void StopHovering()
    {
        base.StopHovering();
        turnController.DisableAllConnectionImages();
    }

    public override void ActivateObject()
    {
        if (CanInteract())
        {
            UseObject();
        }
    }

    public override void UseObject()
    {
        turnController.DirectionButtonPressed(starterPos, buttonDirection);
        AkUnitySoundEngine.PostEvent(pressButtonEvent.Id, gameObject);
        base.StopHovering();
    }
}
