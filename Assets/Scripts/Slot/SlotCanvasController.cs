using UnityEngine;

public class SlotCanvasController : MonoBehaviour
{
    [SerializeField]
    private SlotMachineController slotController;
    public Animator animator {  get; private set; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void RandomizeIcons()
    {
        slotController.RandomizeIcons();
    }

    public void EndSpinWheelAction()
    {
        GameManager.Instance.FinishActionState();
    }

}
