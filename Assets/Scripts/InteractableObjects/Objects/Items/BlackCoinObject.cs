using UnityEngine;

public class BlackCoinObject : InteractableObject
{
    [SerializeField]
    private AK.Wwise.Event redCoinFlipEvent;
    public override void UseObject()
    {
        GameManager.Instance.ItemUsed(Store.ItemType.RED_COIN);
        AkUnitySoundEngine.PostEvent(redCoinFlipEvent.Id, gameObject);
    }
}
