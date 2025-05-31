using UnityEngine;

public class LeverObject : InteractableObject
{
    [Space, Header("Lever"), SerializeField]
    private SlotMachineController controller;

    [SerializeField]
    private AK.Wwise.Event pullLeverEvent;
    
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
        AkUnitySoundEngine.PostEvent(pullLeverEvent.Id, gameObject);
        StopHovering();
    }
}
