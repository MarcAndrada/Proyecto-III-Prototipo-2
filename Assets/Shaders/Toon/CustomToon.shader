Shader "Gemix/CustomToon"
{
    Properties
    {
        [MainTexture] _ColorMap ("Color Map", 2D) = "white" {}
        [MainColor] _Color ("Color", Color) = (1.0, 1.0, 1.0)
        _Smoothness ("Smoothness", Float) = 16.0
        _RimSharpness ("Rim Sharpness", Float) = 16.0
        [HDR] _RimColor ("Rim Color", Color) = (1.0, 1.0, 1.0)
        [HDR] _WorldColor ("World Color", Color) = (0.1, 0.1, 0.1)
        _OutlineThickness("Outline Thickness", Float) = 1
        _OutlineType("Outline Type", Integer) = 0
        _OutlineOffset("Outline Offset", Vector) = (0, 0, 0,0)
        _Steps("Steps", Integer) = 2
        _Smoothness("Smoothness", Range(0,1))=1
        [HDR]_SpecularColor("Specular Color", Color) = (1,1,1,0)
        [Header(Noise)]
        _NoiseScale("Noise Scale", Float) = 1
        _NoiseBias("Noise Bias", Range(0,1)) = 0.5
        _NoiseColor("Noise Color", Color) = (1,1,1,0)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }

        Cull Back
        ZWrite On
        ZTest LEqual
        ZClip Off
        
        Pass
        {
            Name "ForwardLit"
            Tags {"LightMode" = "UniversalForwardOnly"}
            
            
            HLSLPROGRAM

            #pragma multi_compile_fog
            #pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
            
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma vertex Vertex
            #pragma fragment Fragment
            
            #include "HLSL/ToonShaderPass.hlsl"
            
            ENDHLSL
        }

        Pass
        {
            Name "ShadowCaster"
            Tags {"LightMode" = "ShadowCaster"}
            
            
            HLSLPROGRAM

            #define SHADOW_CASTER_PASS
            
            #pragma vertex Vertex
            #pragma fragment FragmentDepthOnly
            
            #include "HLSL/ToonShaderPass.hlsl"
            
            ENDHLSL
        }
        Pass
        {
            Tags { "LightMode" = "Outline" }
            Name "Outline"
            Cull Front
            Blend One Zero
            ZTest LEqual
            ZWrite On

            HLSLPROGRAM
            
            #pragma vertex InverseVert;
            #pragma fragment Outline;

            #include "HLSL/Unlit.hlsl"
            ENDHLSL
        }
        /*
        Pass
        {
            Tags { "LightMode" = "Outline Two" }
            Name "Outline Two"
            Cull Front
            Blend One Zero
            ZTest LEqual
            ZWrite On

            HLSLPROGRAM
            
            #pragma vertex InverseVertTwo;
            #pragma fragment OutlineTwo;

            #include "HLSL/Unlit.hlsl"
            ENDHLSL
        }
        */
        Pass
        {
            Name "DepthOnly"
            Tags {"LightMode" = "DepthOnly"}
            
            
            HLSLPROGRAM
            
            #pragma vertex Vertex
            #pragma fragment FragmentDepthOnly
            
            #include "HLSL/ToonShaderPass.hlsl"
            
            ENDHLSL
        }
        
        Pass
        {
            Name "DepthNormalsOnly"
            Tags {"LightMode" = "DepthNormalsOnly"}
            
            
            HLSLPROGRAM
            
            #pragma vertex Vertex
            #pragma fragment FragmentDepthNormalsOnly
            
            #include "HLSL/ToonShaderPass.hlsl"
            
            ENDHLSL
        }
    }
}
