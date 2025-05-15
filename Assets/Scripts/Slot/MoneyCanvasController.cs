using TMPro;
using UnityEngine;

public class MoneyCanvasController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI coinText;
    
    public void UpdateCoins(int _coins)
    {
        coinText.text = _coins.ToString();
    }
}
