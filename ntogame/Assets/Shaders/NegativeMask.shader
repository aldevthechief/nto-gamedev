Shader "Custom/NegativeMask"
{
    Properties
    {
      [HideInInspector]  _MainTex ("Texture", 2D) = "white" {}
    _Color("Color", Color) = (1,1,1,1)
        _Multiplier("Multiplier", Range(0, 5)) = 1
    }
    SubShader
    {
        // No culling or depth
        Cull Off
        ZWrite Off
        ZTest Less

        Tags{"Queue"="Transparent-1"}

          GrabPass
        {
            "_BackgroundTexture"
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct mesh
            {
                float4 vertex : POSITION;
            };

            struct pixel
            {
                float4 vertex : SV_POSITION;
                float4 grabPos : TEXCOORD0;
            };

            pixel vert (mesh v)
            {
                pixel o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                o.grabPos = ComputeGrabScreenPos(o.vertex);
                return o;
            }

            sampler2D _BackgroundTexture;
            fixed4 _Color;
            fixed _Multiplier;

            fixed4 frag (pixel i) : SV_Target
            {
                fixed4 color = 1 - tex2Dproj(_BackgroundTexture, i.grabPos) * _Multiplier;
                color.a = 1;
                return color * _Color;
            }
            ENDCG
        }
    }
}
