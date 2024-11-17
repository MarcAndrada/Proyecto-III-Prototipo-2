using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverObject : InteractableObject
{
    [Space, Header("Lever"), SerializeField]
    private SlotMachineController controller;

    public override void ActivateObject()
    {
        controller.SpinWheel();
    }
}
