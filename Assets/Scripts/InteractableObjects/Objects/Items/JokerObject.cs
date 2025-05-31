using UnityEngine;

public class JokerObject : InteractableObject
{
    [SerializeField]
    private AK.Wwise.Event jokerLaughEvent;
    
    public override void ActivateObject()
    {
        base.ActivateObject();
        AkUnitySoundEngine.PostEvent(jokerLaughEvent.Id, gameObject);
    }

    public override void UseObject()
    {
        GameManager.Instance.ItemUsed(Store.ItemType.JOKER);
    }


}