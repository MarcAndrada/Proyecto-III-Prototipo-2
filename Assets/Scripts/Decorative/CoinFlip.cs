using UnityEngine;
public class CoinFlip : MonoBehaviour
{

    private Animator animator;

    [Space, Header("Audio"), SerializeField]
    private GameObject TV_Object;
    [SerializeField]
    private GameObject ambienceObject;
    [SerializeField]
    private AK.Wwise.Event turnCoinFlipEvent;
    [SerializeField]
    private AK.Wwise.Event coinFlipResultEvent;
    [SerializeField]
    private string coinFlipResultSwitch;
    [SerializeField]
    private string playerWinState;
    [SerializeField]
    private string enemyWinState;
    [SerializeField]
    private string redCoinState;

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
                AkUnitySoundEngine.SetSwitch(coinFlipResultSwitch, playerWinState, ambienceObject);
                break;
            case "EnemyCoin":
                AkUnitySoundEngine.SetSwitch(coinFlipResultSwitch, enemyWinState, ambienceObject);
                break;
            case "RedCoin":
                AkUnitySoundEngine.SetSwitch(coinFlipResultSwitch, redCoinState, ambienceObject);
                break;

            default:
                break;
        }
        AkUnitySoundEngine.PostEvent(coinFlipResultEvent.Id, ambienceObject);


    }

    public void PlayAnim(string _animString)
    {
        lastAnim = _animString;
        animator.SetTrigger(_animString);

        AkUnitySoundEngine.PostEvent(turnCoinFlipEvent.Id, TV_Object);
    }
}
