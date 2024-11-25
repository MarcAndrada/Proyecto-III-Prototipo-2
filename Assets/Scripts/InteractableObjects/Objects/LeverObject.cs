using UnityEngine;

public class LeverObject : InteractableObject
{
    [Space, Header("Lever"), SerializeField]
    private SlotMachineController controller;

    public override void ActivateObject()
    {
        if (GameManager.Instance.state == GameManager.GameState.PLAYER_TURN 
            && CanInteract())
        {
            UseObject();
        }
    }

    public override void UseObject()
    {
        controller.SpinWheel();
        animator.SetTrigger("Move");
        StopHovering();
    }
}
