using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogController : MonoBehaviour
{
    [SerializeField]
    private float disperseSpeed;
    private float currentFogValue;
    private ParticleSystem fog;
    private void Awake()
    {
        fog = GetComponent<ParticleSystem>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.playerItemsUsed.Contains(Store.ItemType.CIGARRETTE) && !GameManager.Instance.enemyItemsUsed.Contains(Store.ItemType.CIGARRETTE))
            DisperseFog();
        else
            UpdateFogValue();
    }

    private void DisperseFog()
    {
        currentFogValue -= Time.deltaTime * disperseSpeed / 10;

        currentFogValue = Mathf.Clamp01(currentFogValue);

        ParticleSystem.MainModule mainModule = GameManager.Instance.segarroSmoke.main;
        mainModule.startColor = new Color(1, 1, 1, currentFogValue);
    }

    private void UpdateFogValue()
    {
        currentFogValue = fog.main.startColor.color.a;
    }
}
