using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualScreen : GraphicRaycaster
{
    [SerializeField] private Camera screenCamera;
    [SerializeField] private GraphicRaycaster screenCaster;

    public override void Raycast(PointerEventData eventData, List<RaycastResult> resultList)
    {
        Ray ray = eventCamera.ScreenPointToRay(eventData.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.transform == transform)
            {
                // Figure out where the pointer would be in the second camera based on texture position or RenderTexture.
                Vector3 virtualPos = new Vector3(hit.textureCoord.x, hit.textureCoord.y);
                virtualPos.x *= screenCamera.targetTexture.width;
                virtualPos.y *= screenCamera.targetTexture.height;

                eventData.position = virtualPos;

                screenCaster.Raycast(eventData, resultList);
            }
        }
    }
 
}
