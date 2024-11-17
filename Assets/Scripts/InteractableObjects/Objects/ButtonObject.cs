using UnityEngine;

public class ButtonObject : InteractableObject
{
    [SerializeField]
    private Vector2Int starterPos;
    [SerializeField]
    private Vector2Int buttonDirection;

    [Space, SerializeField]
    private TurnController turnController;

    public override void OnHover()
    {
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
        turnController.DirectionButtonPressed(starterPos, buttonDirection);
    }
}
