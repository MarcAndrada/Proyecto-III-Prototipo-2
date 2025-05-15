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
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Renderer renderer = hit.collider.GetComponent<Renderer>();
            if (renderer && renderer?.material.mainTexture == screenCamera.targetTexture)
            {
                Vector3 virtualPos = new Vector3(
                    hit.textureCoord.x * screenCamera.targetTexture.width ,
                    hit.textureCoord.y * screenCamera.targetTexture.height
                );
                
                eventData.position = virtualPos;

                screenCaster.Raycast(eventData, resultList);
            }
        }
    }
 
}
