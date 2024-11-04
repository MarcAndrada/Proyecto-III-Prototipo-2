using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// Esta clase define una característica de renderizado personalizada para el pipeline de renderizado URP en Unity.
[System.Serializable]
public class CustomPostProcessRenderFeature : ScriptableRendererFeature
{
    // Shaders personalizados para efectos de post-processing.
    [SerializeField] private Shader _bloomShader;
    [SerializeField] private Shader _compositeShader;
    [SerializeField] private Shader _ssaoShader;

    // Materiales que se crearán a partir de los shaders anteriores.
    private Material _bloomMaterial;
    private Material _compositeMaterial;
    private Material _ssaoMaterial;

    // Render Pass personalizada que aplicará los efectos de post-procesamiento.
    private CustomPostProcessPass m_customPass;
    
    // Añade la render pass al renderizador. Unity llama a este método durante el ciclo de renderizado.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(m_customPass);
    }

    // Inicializa los materiales y la render pass personalizada.
    public override void Create()
    {
        // Crea los materiales
        _bloomMaterial = CoreUtils.CreateEngineMaterial(_bloomShader);
        _compositeMaterial = CoreUtils.CreateEngineMaterial(_compositeShader);

        // Inicializa la render pass personalizada con los materiales creados.
        m_customPass = new CustomPostProcessPass(_bloomMaterial, _compositeMaterial);
    }
    
    // Configura las entradas y salidas de la render pass antes de que se ejecute.
    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        if (renderingData.cameraData.cameraType == CameraType.Game)
        {
            // Configura las entradas de la render pass.
            m_customPass.ConfigureInput(ScriptableRenderPassInput.Depth);
            m_customPass.ConfigureInput(ScriptableRenderPassInput.Color);
            
            // Define los objetivos de renderizado.
            m_customPass.SetTarget(renderer.cameraColorTargetHandle, renderer.cameraDepthTargetHandle);
        }
    }
    
    protected override void Dispose(bool disposing)
    {
        CoreUtils.Destroy(_bloomMaterial);
        CoreUtils.Destroy(_compositeMaterial);
        CoreUtils.Destroy(_ssaoMaterial);

    }
}

