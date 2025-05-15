using UnityEngine;

public class LeverObject : InteractableObject
{
    [Space, Header("Lever"), SerializeField]
    private SlotMachineController controller;

    private AudioSource source;

    protected override void Awake()
    {
        base.Awake();
        source = GetComponent<AudioSource>();
    }
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
        source.Play();
        StopHovering();
    }
}
