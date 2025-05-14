using UnityEngine;
using UnityEngine.UI;

public class UnlockMedalController : MonoBehaviour
{
    [SerializeField]
    private Image medalImage;

    [SerializeField]
    private InteractableObjectsInfo[] objectsToUnlock;
    [SerializeField]
    private Sprite[] sprites;
    private void OnEnable()
    {
        Sprite currentSprite = null;
        for (int i = 0; i < objectsToUnlock.Length; i++)
        {
            if (!objectsToUnlock[i].unlocked)
            {
                objectsToUnlock[i].unlocked = true;
                currentSprite = sprites[i];
                break;
            }
        }

        if (currentSprite)
            medalImage.sprite = currentSprite;
        else
            medalImage.gameObject.SetActive(false);
    }
}
