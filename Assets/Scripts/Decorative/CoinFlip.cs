using UnityEngine;
using UnityEngine.Rendering;

public class CoinFlip : MonoBehaviour
{

    private Animator animator;

    [Space, Header("Audio"), SerializeField]
    private AudioClip coinFlip;
    [SerializeField]
    private AudioClip[] playerWin;
    [SerializeField]
    private AudioClip[] enemyWin;
    [SerializeField]
    private AudioClip redCoinEnd;

    private string lastAnim;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void GoNextState()
    {
        GameManager.Instance.ChangeToNextGameState();

        if (GameManager.Instance.stateOrder == 0)
            GameManager.Instance.playerLookController.AddAction(PlayerLookActionsController.LookAtActions.NORMAL_CAMERA, 1);
        else
            GameManager.Instance.playerLookController.AddAction(PlayerLookActionsController.LookAtActions.ENEMY_TURN, 1);

        switch (lastAnim)
        {
            case "PlayerCoin":
                AmbientSoundController.instance.PlaySound(playerWin[Random.Range(0, playerWin.Length)], 1, Random.Range(0.4f, 0.7f));
                break;
            case "EnemyCoin":
                AmbientSoundController.instance.PlaySound(enemyWin[Random.Range(0, enemyWin.Length)], 1, Random.Range(0.4f, 0.7f));
                break;
            case "RedCoin":
                AmbientSoundController.instance.PlaySound(redCoinEnd, 1, Random.Range(0.4f, 0.7f));
                break;

            default:
                break;
        }

    }

    public void PlayAnim(string _animString)
    {
        lastAnim = _animString;
        animator.SetTrigger(_animString);
        AmbientSoundController.instance.PlaySound(coinFlip, 1, 1);
    }
}
