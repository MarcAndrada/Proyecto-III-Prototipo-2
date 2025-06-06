using System.Collections.Generic;
using UnityEngine;

public class SlotMachineController : MonoBehaviour
{
    private List<List<SlotIcon>> slotIcons;

    [SerializeField]
    private GameObject slotIconsParents;
    [SerializeField]
    private GameObject slotIconsPrefab;

    [SerializeField]
    private SlotCanvasController slotCanvasController;

    [SerializeField]
    private GameObject tvObject;
    [SerializeField]
    private AK.Wwise.Event slotAnimationEvent;

    // Start is called before the first frame update
    void Start()
    {
        InitializeSlot();
        RandomizeIcons();
    }

    private void InitializeSlot()
    {
        slotIcons = new List<List<SlotIcon>>();
        for (int i = 0; i < GameManager.Instance.slotWidth; i++) 
        {
            slotIcons.Add(new List<SlotIcon>());
            for (int j = 0; j < GameManager.Instance.slotHeight; j++)
            {
                SlotIcon newIcon = Instantiate(slotIconsPrefab, slotIconsParents.transform).GetComponent<SlotIcon>();
                slotIcons[i].Add(newIcon);
            }
        }

    }

    public void SpinWheel()
    {
        //Poner la animacion
        slotCanvasController.animator.SetTrigger("SpinWheel");
        
        GameManager.Instance.FinishActionState(); //Acabar con el estado previo y empezar el de SPIN_WHEEL
        GameObject soundObject = tvObject;
        if (!soundObject)
            soundObject = gameObject;
        AkUnitySoundEngine.PostEvent(slotAnimationEvent.Id, soundObject);

    }

    public void RandomizeIcons()
    {
        for (int i = 0; i < GameManager.Instance.slotWidth; i++)
        {
            for (int j = 0; j < GameManager.Instance.slotHeight; j++)
            {
                slotIcons[i][j].RandomizeIconType(i, j);
                slotIcons[i][j].iconImage.sprite = GameManager.Instance.iconSprite[slotIcons[i][j].type];

                float x = GameManager.Instance.iconXOffset * i;
                float y = -GameManager.Instance.iconYOffset * j;
                slotIcons[i][j].transform.localPosition = new Vector3(x, y);

                slotIcons[i][j].backgroundImage.enabled = false;
            }
        }
    }

    public SlotIcon GetSlotIcon(int _x, int _y)
    {
        return slotIcons[_x][_y];
    }
}
