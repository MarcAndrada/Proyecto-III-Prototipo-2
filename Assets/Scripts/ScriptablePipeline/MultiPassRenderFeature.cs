using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class MultiPassRenderFeature : ScriptableRendererFeature
{
    public List<string> lightModePasses;
    private MultiPassPass _mainPass;

    public override void Create()
    {
        _mainPass = new MultiPassPass(lightModePasses);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(_mainPass);
    }
}
