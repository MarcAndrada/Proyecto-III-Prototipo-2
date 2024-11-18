using UnityEngine;

public class LeverObject : InteractableObject
{
    [Space, Header("Lever"), SerializeField]
    private SlotMachineController controller;

    public override void ActivateObject()
    {
        if (CanInteract())
        {
            controller.SpinWheel();
            StopHovering();
        }
    }
}
