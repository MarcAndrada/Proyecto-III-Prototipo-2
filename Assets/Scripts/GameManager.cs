using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    public enum GameState { COIN_FLIP, PLAYER_TURN, AI_TURN }
    public enum ActionState { START, WHEEL_SPIN, ACTION, RESULTS }

    public List<Store.ItemType> playerItemsUsed; 
    public List<Store.ItemType> enemyItemsUsed;


    [SerializeField]
    private PlayerLookActionsController playerLookController;

    [field: Space, Header("GameState"), SerializeField]
    public GameState state { get; private set; }
    [field: SerializeField]
    public ActionState actionState { get; private set; }
    int stateOrder;


    [field: Space, Header("Slot"), SerializeField]
    public SlotMachineController playerSlot {  get; private set; }
    [field: SerializeField]
    public SlotMachineController enemySlot { get; private set; }
    [field: SerializeField]
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

    [field: Space, Header("Cigarrette"), SerializeField]
    public Transform cigarretteTransform {  get; private set; }


    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(Instance);

        Instance = this;
    }

    private void Start()
    {
        playerItemsUsed = new List<Store.ItemType>();
        enemyItemsUsed = new List<Store.ItemType>();

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

                if (UsedItem(enemyItemsUsed, Store.ItemType.INTERRUPTOR))
                {
                    ChangeToNextGameState();
                    enemyItemsUsed.Remove(Store.ItemType.INTERRUPTOR);
                }
                //Devolverle el control del player
                playerLookController.AddAction(PlayerLookActionsController.LookAtActions.NORMAL_CAMERA, 0);
                break;
            case GameState.AI_TURN:
                

                //Resetear los Items del enemigo
                if (UsedItem(playerItemsUsed, Store.ItemType.INTERRUPTOR))
                {
                    Debug.Log("Turno Skipeado");
                    playerItemsUsed.Remove(Store.ItemType.INTERRUPTOR);
                    ChangeToNextGameState();

                }

                //Hacer que el player mire en la direccion de la pantalla donde se hace la accion del enemigo
                playerLookController.AddAction(PlayerLookActionsController.LookAtActions.RESULTS, 0);

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
                //Cambiar la camara del player a la derecha
                playerLookController.AddAction(PlayerLookActionsController.LookAtActions.RESULTS, 5);                
                Invoke("FinishActionState", 5);

                actionState = ActionState.RESULTS;
                break;
            case ActionState.RESULTS:
                //Cambiar la camara del player a la central y setearle los triggers que tocan

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
        if(UsedItem(playerItemsUsed, Store.ItemType.RED_COIN))
        {
            Debug.Log("Trucado para que empieze el player");
            stateOrder = 0;
            playerItemsUsed.Remove(Store.ItemType.RED_COIN);
            ChangeToNextGameState();
            return;
        }
        else if (UsedItem(enemyItemsUsed, Store.ItemType.RED_COIN))
        {
            stateOrder = 1;
            enemyItemsUsed.Remove(Store.ItemType.RED_COIN);
            ChangeToNextGameState();
            return;
        }

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

    private bool UsedItem(List<Store.ItemType> _itemList, Store.ItemType _itemToSearch)
    {
        foreach (Store.ItemType item in _itemList)
        {
            if(item == _itemToSearch)
                return true;
        }

        return false;
    }
    public void ItemUsed(Store.ItemType _itemUsed)
    {
        if (state == GameState.PLAYER_TURN)
            playerItemsUsed.Add(_itemUsed);
        else if (state == GameState.AI_TURN)
            playerItemsUsed.Add(_itemUsed);
    }

    

}
