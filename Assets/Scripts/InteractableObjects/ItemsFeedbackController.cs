using UnityEngine;

public class ItemsFeedbackController : MonoBehaviour
{
    [SerializeField]
    private GameObject jokerObject;
    [SerializeField]
    private GameObject interruptorObject;
    [SerializeField]
    private GameObject rivalScreenBlack;
    [SerializeField]
    private GameObject redCoinObject;
    public void ItemUsed(Store.ItemType _item)
    {
        switch (_item)
        {
            case Store.ItemType.JOKER:
                if(jokerObject)
                    jokerObject.SetActive(true);
                break;
            case Store.ItemType.INTERRUPTOR:
                if(interruptorObject)
                    interruptorObject.SetActive(true);
                rivalScreenBlack.SetActive(true);
                break;
            case Store.ItemType.RED_COIN:
                if(redCoinObject)
                    redCoinObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void ClearItemScreen()
    {
        if(jokerObject)
            jokerObject.SetActive(false);
        if(interruptorObject)
            interruptorObject.SetActive(false);
        if(redCoinObject)
            redCoinObject.SetActive(false);
    }

    public void TurnOnRivalScreen()
    {
        rivalScreenBlack.SetActive(false);
    }
}
