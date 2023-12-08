Shader "Custom/Dark2D" {
    Properties{
        _MainTex("Texture", 2D) = "white" {}
    _Color("Color", Color) = (1,1,1,1)
        _AlbedoAddSet("Addset", Float) = 0
        _LightMultiplier("LightMultiplier", Range(0, 10)) = 1
    }
        SubShader{
        Tags { "Queue"="Transparent" "RenderType" = "Transparent" }

        Blend SrcAlpha OneMinusSrcAlpha
         Cull Off
        CGPROGRAM
          #pragma surface surf SimpleLambert fullforwardshadows alpha:fade

        struct Input {
            float2 uv_MainTex;
        };

        sampler2D _MainTex;
        fixed4 _Color;
        fixed _LightMultiplier;
        fixed _AlbedoAddSet;

        void surf(Input IN, inout SurfaceOutput o) {

            half4 color = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = color.rgb;
            o.Alpha = color.a;
            o.Albedo *= _Color;
        }

        half4 LightingSimpleLambert(SurfaceOutput s, half3 lightDir, half atten) {
            half light = dot(s.Normal, lightDir) * atten * _LightMultiplier;
            half4 c;
            c.rgb = (s.Albedo + _AlbedoAddSet) * _LightColor0.rgb * light;
            s.Albedo *= -1;
            c /= 2;
            c += half4(s.Albedo.rgb, 0);
            c.a = s.Alpha;
            return c;
        }

        ENDCG
    }
        Fallback "Diffuse"
}