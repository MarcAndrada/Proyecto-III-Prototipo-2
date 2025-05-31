using UnityEngine;

public class MedalController : MonoBehaviour
{
    [SerializeField]
    private Transform[] medalPositions;

    [SerializeField]
    private InteractableObjectsInfo[] unlockableItems;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < unlockableItems.Length; i++)
        {
            if (unlockableItems[i].unlocked)
            {
                GameObject item = Instantiate(unlockableItems[i].medalPrefab, medalPositions[i]);
                item.transform.localPosition = Vector3.zero;
                item.transform.rotation = medalPositions[i].rotation;
            }
        }
    }

}
