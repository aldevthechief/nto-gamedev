Shader "Custom/Dark" {
    Properties{
        _MainTex("Texture", 2D) = "white" {}
    _Color("Color", Color) = (1,1,1,1)
    }
        SubShader{
        Tags { "RenderType" = "Opaque" }
        
        CGPROGRAM
          #pragma surface surf SimpleLambert fullforwardshadows

        struct Input {
            float2 uv_MainTex;
        };

        sampler2D _MainTex;
        fixed4 _Color;

        void surf(Input IN, inout SurfaceOutput o) {
            
            o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
            o.Albedo *= _Color ;
        }

        half4 LightingSimpleLambert(SurfaceOutput s, half3 lightDir, half atten) {
            half light = dot(s.Normal, lightDir) * atten;
            half4 c;
            c.rgb = s.Albedo * _LightColor0.rgb * light;
            c.a = s.Alpha;
            s.Albedo *= -1;
            return s.Albedo.rgbr + c;
        }

        ENDCG
    }
        Fallback "Diffuse"
}