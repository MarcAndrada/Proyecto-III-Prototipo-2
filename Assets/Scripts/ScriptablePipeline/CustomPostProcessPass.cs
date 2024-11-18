using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// Define una Render Pass personalizada para post-processing.
[System.Serializable]
public class CustomPostProcessPass : ScriptableRenderPass
{
    // Datos de la cámara y materiales para efectos
    private CameraData _cameraData;
    private Material _bloomMaterial;
    private Material _compositeMaterial;

    private RTHandle _cameraColorTarget;
    private RTHandle _cameraDepthTarget;
    
    // Esta estructura contiene toda la información necesaria para crear una RenderTexture. Se puede copiar, almacenar en caché y reutilizar para crear fácilmente RenderTextures que compartan todas las mismas propiedades.
    private RenderTextureDescriptor _descriptor; 

    private const int k_MaxPyramidSize = 16;   // Tamaño máximo de la pirámide de mipmaps para el efecto de bloom
    private int[] _IDbloomMipUp;                 // IDs para mipmaps ascendentes de bloom
    private int[] _IDbloomMipDown;               // IDs para mipmaps descendentes de bloom
    private RTHandle[] _bloomMipUp;           // Mipmaps ascendentes para bloom
    private RTHandle[] _bloomMipDown;         // Mipmaps descendentes para bloom
    private GraphicsFormat hdrFormat;          // Formato HDR para efectos de alta calidad
    
    private BenDayBloomEffectComponent _bloomEffect; // Componente que contiene la configuración del bloom

    public CustomPostProcessPass(Material bloomMaterial, Material compositeMaterial)
    {
        _bloomMaterial = bloomMaterial;
        _compositeMaterial = compositeMaterial;
        
        renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;

        // Inicializa los arrays de IDs y RTHandles para los mipmaps.
        _IDbloomMipUp = new int[k_MaxPyramidSize];
        _IDbloomMipDown = new int[k_MaxPyramidSize];
        
        _bloomMipUp = new RTHandle[k_MaxPyramidSize];
        _bloomMipDown = new RTHandle[k_MaxPyramidSize];

        for (int i = 0; i < k_MaxPyramidSize; i++)
        {
            _IDbloomMipUp[i] = Shader.PropertyToID("_BloomMipUp" + i);
            _IDbloomMipDown[i] = Shader.PropertyToID("_BloomMipDown" + i);
            
            _bloomMipUp[i] = RTHandles.Alloc(_bloomMipUp[i], name: "_BloomMipUp" + i);
            _bloomMipDown[i] = RTHandles.Alloc(_bloomMipDown[i], name: "_BloomMipDown" + i);
        }

        // Define el formato de renderizado HDR si es compatible, con un fallback si no.
        const FormatUsage usage = FormatUsage.Linear | FormatUsage.Render;
        if (SystemInfo.IsFormatSupported(GraphicsFormat.B10G11R11_UFloatPack32, usage)) // HDR fallback
        {
            hdrFormat = GraphicsFormat.B10G11R11_UFloatPack32;
        }
        else
        {
            hdrFormat = QualitySettings.activeColorSpace == ColorSpace.Linear
                ? GraphicsFormat.R8G8B8A8_SRGB
                : GraphicsFormat.R8G8B8A8_UNorm;
        }
    }
    
    // Configuración inicial de la cámara. Guarda los datos de la cámara y su descriptor.
    // Descriptor
    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        _descriptor = renderingData.cameraData.cameraTargetDescriptor;
        _cameraData = renderingData.cameraData;
    }
    
    // Ejecuta la render pass aplicando los efectos de post-procesamiento.
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        // Obtiene el efecto de bloom configurado en la cámara actual.
        VolumeStack stack = VolumeManager.instance.stack;
        _bloomEffect = stack.GetComponent<BenDayBloomEffectComponent>();

        CommandBuffer cmd = CommandBufferPool.Get();

        using (new ProfilingScope(cmd, new ProfilingSampler("Custom Post Process Effects")))
        {
            // Configura y aplica el efecto de SSAO.
            Texture ssaoTex = Shader.GetGlobalTexture("_ScreenSpaceOcclusionTexture");
            Shader.SetGlobalTexture("_SSAOTexture", ssaoTex);
            
            // Configura y aplica el efecto de bloom.
            SetupBloom(cmd, _cameraColorTarget);
            
            // Ajusta el shader.
            _compositeMaterial.SetFloat("_Cutoff", _bloomEffect.dotsCutoff.value);
            _compositeMaterial.SetFloat("_Density", _bloomEffect.dotsDensity.value);
            _compositeMaterial.SetVector("_Direction", _bloomEffect.scrollDirection.value);
            
            // Renderiza el efecto de composición en el objetivo de color de la cámara.
            if (_cameraColorTarget != null)
                Blitter.BlitCameraTexture(cmd, _cameraColorTarget, _cameraColorTarget, _compositeMaterial, 0);
        }
        
        context.ExecuteCommandBuffer(cmd);
        cmd.Clear();
        
        CommandBufferPool.Release(cmd);
    }
    
    // Configuración de mipmaps para el efecto de bloom
    private void SetupBloom(CommandBuffer cmd, RTHandle source)
    {
        // Empieza con la resolución a la mitad
        int downres = 1;
        int tw = _descriptor.width >> downres;
        int th = _descriptor.height >> downres;

        // Calcula la cantidad de iteraciones para reducir la resolución en cada paso
        int maxSize = Mathf.Max(tw, th);
        int iterations = Mathf.FloorToInt(Mathf.Log(maxSize, 2f) - 1);
        int mipCount = Mathf.Clamp(iterations, 1, _bloomEffect.maxIterations.value);
        
        // Configuración de parámetros para el bloom
        float clamp = _bloomEffect.clamp.value;
        float threshold = Mathf.GammaToLinearSpace(_bloomEffect.threshold.value);
        float thresholdKnee = threshold * 0.5f;
        
        // Configuración del material con los parámetros calculados
        float scatter = Mathf.Lerp(0.05f, 0.95f, _bloomEffect.scatter.value);
        Material bloomMaterial = _bloomMaterial;
        
        bloomMaterial.SetVector("_Params", new Vector4(scatter, clamp, threshold, thresholdKnee));
        
        // Preconfiguración de mipmaps
        RenderTextureDescriptor desc = GetCompatibleDescriptor(tw, th, hdrFormat);
        for (int i = 0; i < mipCount; i++)
        {
            RenderingUtils.ReAllocateIfNeeded(ref _bloomMipUp[i], desc, FilterMode.Bilinear, TextureWrapMode.Clamp, name: _bloomMipUp[i].name);
            RenderingUtils.ReAllocateIfNeeded(ref _bloomMipDown[i], desc, FilterMode.Bilinear, TextureWrapMode.Clamp, name: _bloomMipDown[i].name);
            
            desc.width = Mathf.Max(1, desc.width >> 1);
            desc.height = Mathf.Max(1, desc.height >> 1);
        }
        
        // Aplica el efecto de bloom en los mipmaps
        if (source != null)
        {
            Blitter.BlitCameraTexture(cmd, source, _bloomMipDown[0], RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, bloomMaterial, 0);
        
            RTHandle lastDown = _bloomMipDown[0];
            for (int i = 1; i < mipCount; i++)
            {
                Blitter.BlitCameraTexture(cmd, lastDown, _bloomMipUp[i], 
                    RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, bloomMaterial, 1);
            
                Blitter.BlitCameraTexture(cmd, _bloomMipUp[i], _bloomMipDown[i], 
                    RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, bloomMaterial, 2);
            
                lastDown = _bloomMipDown[i];
            }

            for (int i = mipCount - 2; i >= 0; i--)
            {
                var lowMip = (i == mipCount - 2) ? _bloomMipDown[i + 1] : _bloomMipUp[i + 1];
                var highMip = _bloomMipDown[i];
                var dst = _bloomMipUp[i];
            
                cmd.SetGlobalTexture("_SourceTexLowMip", lowMip);
                Blitter.BlitCameraTexture(cmd, highMip, dst, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, bloomMaterial, 3);
            }
        
            cmd.SetGlobalTexture("_BloomTexture", _bloomMipUp[0]);
            cmd.SetGlobalFloat("_BloomIntensity", _bloomEffect.intenisty.value);
        }
    }
    
    // Genera un descriptor de textura compatible con los parámetros actuales.
    private RenderTextureDescriptor GetCompatibleDescriptor() => 
        GetCompatibleDescriptor(_descriptor.width, _descriptor.height, _descriptor.graphicsFormat);

    private RenderTextureDescriptor GetCompatibleDescriptor(int width, int height, GraphicsFormat format, DepthBits depthBufferBits = DepthBits.None) =>
        GetCompatibleDescriptor(_descriptor, width, height, format, depthBufferBits);

    internal static RenderTextureDescriptor GetCompatibleDescriptor(RenderTextureDescriptor desc, int width, int height,
        GraphicsFormat format, DepthBits depthBufferBits = DepthBits.None)
    {
        desc.depthBufferBits = (int)depthBufferBits;
        desc.msaaSamples = 1;
        desc.width = width;
        desc.height = height;
        desc.graphicsFormat = format;
        return desc;
    }
    internal void SetTarget(RTHandle cameraColorTargetHandle, RTHandle cameraDepthTargetHandle)
    {
        _cameraColorTarget = cameraColorTargetHandle;
        _cameraDepthTarget = cameraDepthTargetHandle;
    }

}
