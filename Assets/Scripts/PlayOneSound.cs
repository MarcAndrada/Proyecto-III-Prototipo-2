using System.Collections;
using UnityEngine;

public class PlayOneSound : MonoBehaviour
{
    [SerializeField]
    private AK.Wwise.Event eventToPlay;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitEndOfFrame());

        IEnumerator WaitEndOfFrame()
        {
            yield return new WaitForEndOfFrame();
            AkUnitySoundEngine.PostEvent(eventToPlay.Id, gameObject);    
        }
    }
}
