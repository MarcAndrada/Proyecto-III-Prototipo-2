using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JokerPathCanvasController : MonoBehaviour
{
    [SerializeField]
    private GameObject jokerCanvas;

    void Update()
    {
        jokerCanvas.SetActive(GameManager.Instance.playerItemsUsed.Contains(Store.ItemType.JOKER));  
    }
}
