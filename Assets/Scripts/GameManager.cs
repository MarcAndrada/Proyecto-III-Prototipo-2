using UnityEngine;
using AYellowpaper.SerializedCollections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    public enum GameState { COIN_FLIP, PLAYER_TURN, AI_TURN }
    public enum ActionState { START, WHEEL_SPIN, ACTION, RESULTS }

    [field: Space, Header("GameState"), SerializeField]
    public GameState state { get; private set; }
    [field: SerializeField]
    public ActionState actionState { get; private set; }
    int stateOrder;


    [field: Space, Header("Slot"), SerializeField]
    public int slotWidth { get; private set; }
    [field: SerializeField]
    public int slotHeight { get; private set; }

    [Space, Header("Slot Icons"), SerializedDictionary("Type", "Sprite")]
    public SerializedDictionary<SlotIcon.IconType, Sprite> iconSprite;
    [field: SerializeField]
    public float iconXOffset { get; private set; }
    [field: SerializeField]
    public float iconYOffset { get; private set; }

    [Space, Header("Hanged mans"), SerializeField]
    private int maxHealth;
    [field: SerializeField]
    public int playerHangedManHealth { get; private set; }
    [field: SerializeField]
    public int enemyHangedManHealth { get; private set; }

    [field: SerializeField]
    public float ropeOffset;
    [field: SerializeField]
    public float moveUpSpeed { get; private set; }
    [field: SerializeField]
    public float moveDownSpeed { get; private set; }





    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(Instance);

        Instance = this;
    }

    private void Start()
    {
        state = GameState.COIN_FLIP;
        actionState = ActionState.START;

        FlipCoin();
    }
    private void ChangeGameState(GameState _nextState)
    {
        switch (state)
        {
            case GameState.COIN_FLIP:
                break;
            case GameState.PLAYER_TURN:
                //Bloquear las acciones del player
                break;
            case GameState.AI_TURN:
                break;
            default:
                break;
        }

        state = _nextState;

        switch (_nextState)
        {
            case GameState.COIN_FLIP:
                //Mirar hacia donde se haga el giro de moneda
                FlipCoin();
                break;
            case GameState.PLAYER_TURN:
                //Devolverle el control del player
                break;
            case GameState.AI_TURN:
                //Hacer que el player mire en la direccion de la pantalla donde se hace la accion del enemigo
                ChangeToNextGameState();
                break;
            default:
                break;
        }
    }

    public void FinishActionState()
    {
        switch (actionState)
        {
            case ActionState.START:
                actionState = ActionState.WHEEL_SPIN;
                break;
            case ActionState.WHEEL_SPIN:
                actionState = ActionState.ACTION;
                break;
            case ActionState.ACTION:
                actionState = ActionState.RESULTS;
                Invoke("FinishActionState", 4);
                break;
            case ActionState.RESULTS:


                //Cambiar de turno y empezar de nuevo las acciones
                ChangeToNextGameState();
                actionState = ActionState.START;

                break;
            default:
                break;
        }
    }

    private void ChangeToNextGameState()
    {
        if (stateOrder == 0) //El player tira primero
        {
            switch (state)
            {
                case GameState.COIN_FLIP:
                    ChangeGameState(GameState.PLAYER_TURN);
                    break;
                case GameState.PLAYER_TURN:
                    ChangeGameState(GameState.AI_TURN);
                    break;
                case GameState.AI_TURN:
                    //Flipear Moneda
                    ChangeGameState(GameState.COIN_FLIP);
                    break;
                default:
                    break;
            }
        }
        else //El Enemigo Tira primero
        {
            switch (state)
            {
                case GameState.COIN_FLIP:
                    ChangeGameState(GameState.AI_TURN);
                    break;
                case GameState.PLAYER_TURN:
                    //Flipear Moneda
                    ChangeGameState(GameState.COIN_FLIP);
                    break;
                case GameState.AI_TURN:
                    ChangeGameState(GameState.PLAYER_TURN);
                    break;
                default:
                    break;
            }
        }
    }

    private void FlipCoin()
    {
        stateOrder = Random.Range(0,2);
        if (stateOrder == 0)
            Debug.Log("Empieza a tirar el player");
        else
            Debug.Log("Empieza tirando el enemigo");
        ChangeToNextGameState();
    }

    public void ChangeHealth(bool _isPlayer, int _healthChange)
    {
        if (_isPlayer)
            playerHangedManHealth = Mathf.Clamp(playerHangedManHealth + _healthChange, 0, maxHealth);
        else
            enemyHangedManHealth = Mathf.Clamp(enemyHangedManHealth + _healthChange, 0, maxHealth);
    }

}
