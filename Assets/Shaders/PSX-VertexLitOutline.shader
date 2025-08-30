Shader "PSX/Vertex Lit With Outline"
{
    Properties
    {
        _Color("Color (RGBA)", Color) = (1, 1, 1, 1)
        _EmissionColor("Emission Color (RGBA)", Color) = (0,0,0,0)
        _CubemapColor("Cubemap Color (RGBA)", Color) = (0,0,0,0)
        _MainTex("Texture", 2D) = "white" {}
        _EmissiveTex("Emissive", 2D) = "black" {}
        _Cubemap("Cubemap", Cube) = "" {}
        _ReflectionMap("Reflection Map", 2D) = "white" {}
        _ObjectDithering("Per-Object Dithering Enable", Range(0,1)) = 1
        _FlatShading("Flat Shading", Range(0,1)) = 0
        _CustomDepthOffset("Custom Depth Offset", Float) = 0

        // Outline settings
        _OutlineEnabled("Enable Outline", Range(0,1)) = 0
        _OutlineWidth("Outline Width", Float) = 0.02
        _OutlineColor("Outline Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        // PASS 1: Normal PSX rendering + stencil write
        Pass
        {
            Stencil
            {
                Ref 1
                Comp Always
                Pass Replace
            }

            Tags { "LightMode" = "VertexLM" }
            CGPROGRAM
            #pragma vertex vert
            #pragma geometry geom
            #pragma fragment frag
            #pragma multi_compile_fog
            #pragma multi_compile_geometry __ PSX_ENABLE_CUSTOM_VERTEX_LIGHTING
            #pragma multi_compile_geometry __ PSX_FLAT_SHADING_MODE_CENTER
            #pragma multi_compile PSX_TRIANGLE_SORT_OFF PSX_TRIANGLE_SORT_CENTER_Z PSX_TRIANGLE_SORT_CLOSEST_Z PSX_TRIANGLE_SORT_CENTER_VIEWDIST PSX_TRIANGLE_SORT_CLOSEST_VIEWDIST PSX_TRIANGLE_SORT_CUSTOM

            #include "UnityCG.cginc"
            #include "PSX-Utils.cginc"

            samplerCUBE _Cubemap;
            sampler2D _ReflectionMap;
            float4 _CubemapColor;

            #define PSX_VERTEX_LIT
            #define PSX_CUBEMAP _Cubemap
            #define PSX_CUBEMAP_COLOR _CubemapColor

            #include "PSX-ShaderSrc.cginc"
            ENDCG
        }

        // PASS 2: Standard Vertex Lit for non-lightmapped areas (also stencil write)
        Pass
        {
            Stencil
            {
                Ref 1
                Comp Always
                Pass Replace
            }

            Tags { "LightMode" = "Vertex" }
            CGPROGRAM
            #pragma vertex vert
            #pragma geometry geom
            #pragma fragment frag
            #pragma multi_compile_fog
            #pragma multi_compile_geometry __ PSX_ENABLE_CUSTOM_VERTEX_LIGHTING
            #pragma multi_compile_geometry __ PSX_FLAT_SHADING_MODE_CENTER
            #pragma multi_compile PSX_TRIANGLE_SORT_OFF PSX_TRIANGLE_SORT_CENTER_Z PSX_TRIANGLE_SORT_CLOSEST_Z PSX_TRIANGLE_SORT_CENTER_VIEWDIST PSX_TRIANGLE_SORT_CLOSEST_VIEWDIST PSX_TRIANGLE_SORT_CUSTOM

            #include "UnityCG.cginc"
            #include "PSX-Utils.cginc"

            samplerCUBE _Cubemap;
            sampler2D _ReflectionMap;
            float4 _CubemapColor;

            #define PSX_VERTEX_LIT
            #define PSX_CUBEMAP _Cubemap
            #define PSX_CUBEMAP_COLOR _CubemapColor

            #include "PSX-ShaderSrc.cginc"
            ENDCG
        }

        // PASS 3: Outline (only where stencil != 1)
        Pass
        {
            Name "Outline"
            Stencil
            {
                Ref 1
                Comp NotEqual
                Pass Keep
            }

            ZTest Always
            ZWrite Off
            Cull Front

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float4 _OutlineColor;
            float _OutlineWidth;
            float _OutlineEnabled;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                // Push vertices outwards
                v.vertex.xyz += v.normal * _OutlineWidth;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                if (_OutlineEnabled < 0.5) discard;
                return _OutlineColor;
            }
            ENDCG
        }
    }

    Fallback "PSX/Lite/Vertex Lit"
}
