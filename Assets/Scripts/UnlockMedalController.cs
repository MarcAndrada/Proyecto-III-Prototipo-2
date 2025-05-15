using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UnlockMedalController : MonoBehaviour
{
    [SerializeField]
    private Image medalImage;

    [SerializeField]
    private InteractableObjectsInfo[] objectsToUnlock;
    private void OnEnable()
    {
        Sprite currentSprite = null;
        for (int i = 0; i < objectsToUnlock.Length; i++)
        {
            if (!objectsToUnlock[i].unlocked)
            {
                objectsToUnlock[i].unlocked = true;
                currentSprite = objectsToUnlock[i].medalSprite;
                break;
            }
        }

        if (currentSprite)
            medalImage.sprite = currentSprite;
        else
            medalImage.gameObject.SetActive(false);
    }
}
