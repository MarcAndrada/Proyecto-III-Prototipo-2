using UnityEngine;
using AYellowpaper.SerializedCollections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [field: Space, Header("Slot"), SerializeField]
    public int slotWidth {  get; private set; }
    [field: SerializeField] 
    public int slotHeight {  get; private set; }

    [Space, Header("Slot Icons"), SerializedDictionary("Type", "Sprite")]
    public SerializedDictionary<SlotIcon.IconType, Sprite> iconSprite;
    [field: SerializeField]
    public float iconXOffset {  get; private set; }
    [field: SerializeField]
    public float iconYOffset {  get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(Instance);

        Instance = this;
    }



}
