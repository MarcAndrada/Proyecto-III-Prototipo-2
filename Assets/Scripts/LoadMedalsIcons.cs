using UnityEngine;
using UnityEngine.UI;

public class LoadMedalsIcons : MonoBehaviour
{
    [SerializeField]
    private InteractableObjectsInfo[] objectMedals;
    [SerializeField]
    private Image[] images;
    private void OnEnable()
    {
        for (int i = 0; i < objectMedals.Length; i++)
        {
            Color spriteColor = objectMedals[i].unlocked ? Color.white : Color.black;
            images[i].sprite = objectMedals[i].medalSprite;
            images[i].color = spriteColor;
        }
    }
}
