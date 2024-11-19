using UnityEngine;

public class LeverObject : InteractableObject
{
    [Space, Header("Lever"), SerializeField]
    private SlotMachineController controller;

    private Animator animator;
    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }
    public override void ActivateObject()
    {
        if (GameManager.Instance.state == GameManager.GameState.PLAYER_TURN 
            && CanInteract())
        {
            controller.SpinWheel();
            animator.SetTrigger("Move");
            StopHovering();
        }
    }
}
