using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSlotController : MonoBehaviour
{
    [SerializeField]
    private LayerMask interactableMask;

    [Space, Header("Display Info"), SerializeField]
    private GameObject displayInfoPivot;
    [SerializeField]
    private TextMeshProUGUI objectName;
    [SerializeField]
    private TextMeshProUGUI objectDescription;

    private Vector3 selectedObjectMouseHitPos;
    private InteractableObject hoveredObject;

    // Update is called once per frame
    void Update()
    {
        CheckMousePointer();
        ActivateHoveredObject();
        DisplayObjectInfo();
    }

    private void CheckMousePointer()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit, 1000, interactableMask))
        {
            if(hoveredObject)
            {
                hoveredObject.StopHovering();
                hoveredObject = null;
            }
            return;
        }

        Debug.Log("Hit Object: " + hit.collider.gameObject.name);
        selectedObjectMouseHitPos = hit.point;

        InteractableObject currentObject = hit.collider.gameObject.GetComponent<InteractableObject>();

        if (hoveredObject != null && hoveredObject != currentObject)
            hoveredObject.StopHovering();

        hoveredObject = currentObject;
        hoveredObject.OnHover();
    }

    private void ActivateHoveredObject()
    {
        if (!hoveredObject || !Input.GetKeyDown(KeyCode.Mouse0))
            return;

        hoveredObject.ActivateObject();
    }

    private void DisplayObjectInfo()
    {
        if (hoveredObject == null || !hoveredObject.GetObjectInfo())
        {
            displayInfoPivot.SetActive(false);
            return;
        }

        InteractableObjectsInfo objectInfo = hoveredObject.GetObjectInfo();


        displayInfoPivot.SetActive(true);
        Vector3 displayInfoPos = selectedObjectMouseHitPos + (Camera.main.transform.position - selectedObjectMouseHitPos).normalized;
        displayInfoPivot.transform.position = displayInfoPos;

        objectName.text = objectInfo.objectName;
        objectDescription.text = objectInfo.objectDescription;
    }
}
