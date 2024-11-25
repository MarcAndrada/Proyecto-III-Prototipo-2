using UnityEngine;

public class BlackCoinObject : InteractableObject
{

    public override void UseObject()
    {
        GameManager.Instance.ItemUsed(Store.ItemType.RED_COIN);

    }
}
