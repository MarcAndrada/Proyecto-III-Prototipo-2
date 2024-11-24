using UnityEngine;

public class FeedbackObjectsController : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        animator.SetTrigger("Fly");
    }
}
