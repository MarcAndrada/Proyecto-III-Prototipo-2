using UnityEngine;

public class InterruptorObject : InteractableObject
{
    public override void UseObject()
    {
        GameManager.Instance.ItemUsed(Store.ItemType.INTERRUPTOR);
    }
}
