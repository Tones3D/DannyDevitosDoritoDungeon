 
Shader "Custom/Glass"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 0)
        _Gloss ("Gloss", Range (0.01, 1)) = 0.5
        _Shininess ("Shininess", Range (0.01, 1)) = 0.078125
        _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
        _BumpMap ("Normalmap", 2D) = "bump" {}
    }
 
    SubShader
    {
        Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
        LOD 400
       
        CGPROGRAM
        #pragma surface surf Lambert alpha
       
        sampler2D _MainTex;
        sampler2D _BumpMap;
        fixed4 _Color;
       
        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpMap;
        };
       
        void surf(Input IN, inout SurfaceOutput o)
        {
            fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = tex.rgb * _Color.rgb;
            o.Alpha = tex.a * _Color.a;
            o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
        }
        ENDCG
       
        CGPROGRAM
        #pragma surface surf BlinnPhong finalcolor:final decal:add
       
        sampler2D _BumpMap;
        half _Gloss;
        half _Shininess;
       
        struct Input
        {
            float2 uv_BumpMap;
        };
       
        void surf(Input IN, inout SurfaceOutput o)
        {
            o.Gloss = _Gloss;
            o.Specular = _Shininess;
            o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
        }
       
        void final(Input IN, SurfaceOutput o, inout fixed4 color)
        {
            // Need to set alpha here rather than in surf to make this work with all lights
            color.a = 1;
        }
        ENDCG
    }
 
    FallBack "Transparent/VertexLit"
}