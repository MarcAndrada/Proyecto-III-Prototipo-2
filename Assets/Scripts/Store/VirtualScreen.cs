using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualScreen : GraphicRaycaster
{
    [SerializeField] private Camera screenCamera;
    [SerializeField] private GraphicRaycaster screenCaster;
    [SerializeField] private Vector2 targetResolution;
    private Vector2 resulutionMultiplier;

    protected override void Start()
    {
        base.Start();
        resulutionMultiplier.x = 1920 * resulutionMultiplier.x;
        resulutionMultiplier.y = 1080 * resulutionMultiplier.y;
    }
    public override void Raycast(PointerEventData eventData, List<RaycastResult> resultList)
    {        
        Ray ray = eventCamera.ScreenPointToRay(eventData.position);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Renderer renderer = hit.collider.GetComponent<Renderer>();
            if (renderer && renderer?.material.mainTexture == screenCamera.targetTexture)
            {
                Vector3 virtualPos = new Vector3(
                    hit.textureCoord.x * (screenCamera.targetTexture.width * resulutionMultiplier.x),
                    hit.textureCoord.y * (screenCamera.targetTexture.height * resulutionMultiplier.y)
                );
                
                eventData.position = virtualPos;

                screenCaster.Raycast(eventData, resultList);
            }
        }
    }
 
}
