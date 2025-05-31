using UnityEngine;

public class PhysicalCoinController : MonoBehaviour
{
    private bool hasCollided;

    [SerializeField]
    private AK.Wwise.Event coinFallEvent;

    private void Awake()
    {
        hasCollided = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!hasCollided)
        {
            AkUnitySoundEngine.PostEvent(coinFallEvent.Id, gameObject);
            hasCollided = true;
        }
    }

}
