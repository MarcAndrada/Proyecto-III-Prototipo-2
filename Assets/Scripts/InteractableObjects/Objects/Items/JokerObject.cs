using UnityEngine;

public class JokerObject : InteractableObject
{
    private AudioSource source;

    protected override void Awake()
    {
        base.Awake();
        source = GetComponent<AudioSource>();
    }


    public override void ActivateObject()
    {
        base.ActivateObject();
        source.Play();
    }

    public override void UseObject()
    {
        GameManager.Instance.ItemUsed(Store.ItemType.JOKER);
    }


}