using UnityEngine;

public class JokerObject : InteractableObject
{


    public override void UseObject()
    {
        GameManager.Instance.ItemUsed(Store.ItemType.JOKER);
    }


}