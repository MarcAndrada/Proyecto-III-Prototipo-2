using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    [SerializeField] private List<Light> spectatorsLights;
    [SerializeField] private List<Light> bodyLights;
    [SerializeField] private Light playerLight;
    [SerializeField] private Light enemyLight;

    public void DisableLights()
    {
        playerLight.enabled = false;
        enemyLight.enabled = false;
        foreach (Light light in bodyLights)
        {
            light.enabled = false;
        }
        foreach (Light light in spectatorsLights)
        {
            light.enabled = false;
        }
    }
    public void EnableSpectatorLights()
    {
        foreach (Light light in spectatorsLights)
        {
            light.enabled = true;
        }
    }

    public void EnableBodyLights()
    {
        foreach (Light light in bodyLights)
        {
            light.enabled = true;
        }
    }

    public void EnablePlayerLights()
    {
        playerLight.enabled = true;
    }

    public void EnableEnemyLights()
    {
        enemyLight.enabled = true;
    }
}
