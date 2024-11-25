using System;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    public enum GameState { COIN_FLIP, PLAYER_TURN, AI_TURN }
    public enum ActionState { START, WHEEL_SPIN, ACTION, SHOWING_ACTION, RESULTS }

    [HideInInspector]
    public List<Store.ItemType> playerItemsUsed;
    [HideInInspector]
    public List<Store.ItemType> enemyItemsUsed;

    [Space, Header("Item Prefabs"), SerializedDictionary("Item", "Prefab")]
    public SerializedDictionary<Store.ItemType, GameObject> itemsPrefabs;

    [field: SerializeField]
    public PlayerLookActionsController playerLookController {  get; private set; }

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
    [field: SerializeField]
    public int actionResultIconDuration { get; private set; }
    private bool playerTurnSkipped;
    private bool enemyTurnSkipped;
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
    [field: SerializeField]
    public ParticleSystem segarroSmoke { get; private set; }
    
    [field: Space, Header("Coin"), SerializeField]
    public CoinFlip turnCoin { get; private set; }
    [field: SerializeField]
    private ScrollingText scrollingText;
    private bool waitingForCoinFlip = false;
    private bool gameEnded = false;

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
        AddRandomItem(true);
        AddRandomItem(false);

        FlipCoin();
    }

    private void Update()
    {
        if (waitingForCoinFlip && !turnCoin.GetIsFlipping())
        {
            HandleCoinFlipResult();
        }
    }

    private void ChangeGameState(GameState _nextState)
    {
        if (gameEnded) return;

        switch (state)
        {
            case GameState.COIN_FLIP:
                break;
            case GameState.PLAYER_TURN:
                playerSlot.GetComponent<ItemsFeedbackController>().ClearItemScreen();
                break;
            case GameState.AI_TURN:
                enemySlot.GetComponent<ItemsFeedbackController>().ClearItemScreen();
                break;
            default:
                break;
        }

        state = _nextState;

        switch (_nextState)
        {
            case GameState.COIN_FLIP:
                //Dar nuevos items
                AddRandomItem(true);
                AddRandomItem(false);

                for (int i = 0; i < 10; i++)
                {
                    playerItemsUsed.Remove(Store.ItemType.BALANCE);
                    playerItemsUsed.Remove(Store.ItemType.JOKER);
                    playerItemsUsed.Remove(Store.ItemType.CIGARRETTE);

                    enemyItemsUsed.Remove(Store.ItemType.BALANCE);
                    enemyItemsUsed.Remove(Store.ItemType.JOKER);
                    enemyItemsUsed.Remove(Store.ItemType.CIGARRETTE);
                }

                if (playerTurnSkipped)
                    enemySlot.GetComponent<ItemsFeedbackController>().TurnOnRivalScreen();

                if (enemyTurnSkipped)
                    playerSlot.GetComponent<ItemsFeedbackController>().TurnOnRivalScreen();
                
                //Mirar hacia donde se haga el giro de moneda
                FlipCoin();
                break;
            case GameState.PLAYER_TURN:
                if (playerTurnSkipped)
                    enemySlot.GetComponent<ItemsFeedbackController>().TurnOnRivalScreen();

                if (UsedItem(enemyItemsUsed, Store.ItemType.INTERRUPTOR))
                {
                    Debug.Log("Skip player turn");
                    playerTurnSkipped = true;
                    enemyItemsUsed.Remove(Store.ItemType.INTERRUPTOR);
                    ChangeToNextGameState();
                }
                break;
            case GameState.AI_TURN:
                if (enemyTurnSkipped)
                    playerSlot.GetComponent<ItemsFeedbackController>().TurnOnRivalScreen();

                //Resetear los Items del enemigo
                if (UsedItem(playerItemsUsed, Store.ItemType.INTERRUPTOR))
                {
                    Debug.Log("Skip enemy turn");
                    enemyTurnSkipped = true;
                    playerItemsUsed.Remove(Store.ItemType.INTERRUPTOR);
                    ChangeToNextGameState();
                }

                enemySlot.GetComponent<EnemyBehaviour>().StartTurn();
                break;
            default:
                break;
        }
    }

    public void FinishActionState()
    {
        if (gameEnded) return;
        
        switch (actionState)
        {
            case ActionState.START:
                actionState = ActionState.WHEEL_SPIN;
                if(state == GameState.PLAYER_TURN)
                    playerLookController.AddAction(PlayerLookActionsController.LookAtActions.LOCK_MAIN, 1);
                else
                    playerLookController.AddAction(PlayerLookActionsController.LookAtActions.ENEMY_TURN, 1);
                break;
            case ActionState.WHEEL_SPIN:
                if (state == GameState.PLAYER_TURN)
                    playerLookController.AddAction(PlayerLookActionsController.LookAtActions.NORMAL_CAMERA, 1);
                else
                    playerLookController.AddAction(PlayerLookActionsController.LookAtActions.ENEMY_TURN, 1);
                actionState = ActionState.ACTION;
                break;
            case ActionState.ACTION:
                actionState = ActionState.SHOWING_ACTION;
                if (state == GameState.PLAYER_TURN)
                    playerLookController.AddAction(PlayerLookActionsController.LookAtActions.LOCK_MAIN, 1);
                else
                    playerLookController.AddAction(PlayerLookActionsController.LookAtActions.ENEMY_TURN, 1);
                break;
            case ActionState.SHOWING_ACTION:
                actionState = ActionState.RESULTS;
                playerLookController.AddAction(PlayerLookActionsController.LookAtActions.RESULTS, 1);
                break;
            case ActionState.RESULTS:
                //Cambiar la camara del player a la central y setearle los triggers que tocan
                playerLookController.AddAction(PlayerLookActionsController.LookAtActions.NORMAL_CAMERA, 1);
                //Cambiar de turno y empezar de nuevo las acciones
                WinLoseCondition();
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
        playerLookController.AddAction(PlayerLookActionsController.LookAtActions.ENEMY_TURN, 1);
        if (enemyTurnSkipped)
        {
            scrollingText.SetText("Your Turn");
            turnCoin.SetResult(true);
        }
        else if(playerTurnSkipped)
        {
            scrollingText.SetText("Enemy Turn");
            turnCoin.SetResult(false);
        }
        else
        {
            if (UsedItem(playerItemsUsed, Store.ItemType.RED_COIN))
            {
                turnCoin.FlipCoinRed();
                turnCoin.SetResult(true);
            }
            else if (UsedItem(enemyItemsUsed, Store.ItemType.RED_COIN))
            {
                turnCoin.FlipCoinRed();
                turnCoin.SetResult(false);
            }
            else
            {
                turnCoin.FlipCoin();
            }
        }
        
        playerTurnSkipped = false;
        enemyTurnSkipped = false;
        
        waitingForCoinFlip = true;

    }

    private void HandleCoinFlipResult()
    {
        waitingForCoinFlip = false;

        stateOrder = turnCoin.Result() ? 0 : 1;
        
        if(UsedItem(playerItemsUsed, Store.ItemType.RED_COIN))
        {
            stateOrder = 0;
            playerItemsUsed.Remove(Store.ItemType.RED_COIN);
            scrollingText.SetText("Your Turn");
            playerLookController.AddAction(PlayerLookActionsController.LookAtActions.NORMAL_CAMERA, 1);
            ChangeToNextGameState();
            return;
        }
        else if (UsedItem(enemyItemsUsed, Store.ItemType.RED_COIN))
        {
            stateOrder = 1;
            enemyItemsUsed.Remove(Store.ItemType.RED_COIN);
            scrollingText.SetText("Enemy Turn");
            ChangeToNextGameState();
            return;
        }

        if (stateOrder == 0)
        {
            scrollingText.SetText("Your Turn");
            playerLookController.AddAction(PlayerLookActionsController.LookAtActions.NORMAL_CAMERA, 1);
        }
        else
        {
            scrollingText.SetText("Enemy Turn");
        }
        
        ChangeToNextGameState();
    }
    public void ChangeHealth(bool _isPlayer, int _healthChange)
    {
        if (_isPlayer)
            playerHangedManHealth = Mathf.Clamp(playerHangedManHealth + _healthChange, 0, maxHealth);
        else
            enemyHangedManHealth = Mathf.Clamp(enemyHangedManHealth + _healthChange, 0, maxHealth);
    }

    private void WinLoseCondition()
    {
        if (playerHangedManHealth <= 0)
        {
            gameEnded = true;
            playerLookController.AddAction(PlayerLookActionsController.LookAtActions.NORMAL_CAMERA, 1);
            scrollingText.SetTextLoop("YOU LOSE");
        }
        if (enemyHangedManHealth <= 0)
        {
            gameEnded = true;
            playerLookController.AddAction(PlayerLookActionsController.LookAtActions.NORMAL_CAMERA, 1);
            scrollingText.SetTextLoop("YOU WIN");
        }
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
        {
            playerItemsUsed.Add(_itemUsed);
            playerSlot.GetComponent<ItemsFeedbackController>().ItemUsed(_itemUsed);
        }
        else if (state == GameState.AI_TURN)
        {
            enemyItemsUsed.Add(_itemUsed);
            enemySlot.GetComponent<ItemsFeedbackController>().ItemUsed(_itemUsed);
        }
    }

    public void AddRandomItem(bool _toPlayer)
    {
        Store.ItemType randomItem = (Store.ItemType)Random.Range(0, (int)Store.ItemType.BALANCE + 1);
        if (_toPlayer)
        {
            playerSlot.GetComponentInChildren<InventoryManager>().AddItem(randomItem);
        }
        else
        {
            enemySlot.GetComponentInChildren<InventoryManager>().AddItem(randomItem);
        }
    }
}
