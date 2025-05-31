using UnityEngine;

public class InterruptorObject : InteractableObject
{
    [SerializeField]
    private AK.Wwise.Event interruptorPressEvent;
    public override void UseObject()
    {
        GameManager.Instance.ItemUsed(Store.ItemType.INTERRUPTOR);
        AkUnitySoundEngine.PostEvent(interruptorPressEvent.Id, gameObject);
    }
}
