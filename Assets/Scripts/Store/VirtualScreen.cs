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
        if (screenCamera == null || screenCamera.targetTexture == null)
        {
            Debug.LogWarning("Screen Camera or Render Texture not set.");
            return;
        }
        
        Ray ray = eventCamera.ScreenPointToRay(eventData.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Renderer renderer = hit.collider.GetComponent<Renderer>();
            if (renderer && renderer?.material.mainTexture == screenCamera.targetTexture)
            {
                Vector3 virtualPos = new Vector3(hit.textureCoord.x, hit.textureCoord.y);
                virtualPos.x *= screenCamera.targetTexture.width;
                virtualPos.y *= screenCamera.targetTexture.height;
                
                eventData.position = virtualPos;

                screenCaster.Raycast(eventData, resultList);
            }
        }
    }
 
}
