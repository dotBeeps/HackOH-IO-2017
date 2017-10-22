// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// original source: http://demo.bkcore.com/threejs/webgl_tron_godrays.html
// converted to surface shader : http://unitycoder.com/blog/2012/10/02/fake-godrays-shader/
// converted to image effect : 

Shader "Hidden/VolumetricLightApproximation"
{
    Properties
	{
        _MainTex ("Texture", 2D) = "white" {
            }

        fExposure ("fExposure", Float) = 0.6
        fDecay ("fDecay", Float) = 0.93
        fDensity ("fDensity", Float) = 0.96
        fWeight ("fWeight", Float) = 0.4
        fClamp ("fClamp", Float) = 1.0
        fSamples ("fSamples", Int) = 32


	}
	SubShader
	{
        // No culling or depth
		Cull Off ZWrite Off ZTest Always

		Blend SrcAlpha OneMinusSrcAlpha // Traditional transparency

		Pass
		{
            CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
			{
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
			{
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

			sampler2D _MainTex;
            float fX,fY,fExposure,fDecay,fDensity,fWeight,fClamp;
			int fSamples;


            fixed4 frag (v2f i) : SV_Target
			{
                float2 vUv = i.uv;
				float2 deltaTextCoord = float2(vUv - float2(0.5,0.5));
				deltaTextCoord *= 1.0 /  float(fSamples) * fDensity;
				float2 coord = vUv;

                float illuminationDecay = 1.0;
                float4 FragColor = float4(0,0,0,0);

                for(int i=0; i < fSamples ; i++)
				{
                    coord -= deltaTextCoord;
                    float4 texel = tex2D(_MainTex, coord);
                    texel *= illuminationDecay * fWeight;
                    FragColor += texel;
                    illuminationDecay *= fDecay;
                }

				FragColor *= fExposure;
                FragColor = clamp(FragColor, 0.0, fClamp);
                float4 c = FragColor;

				return c;
            }

			ENDCG
		}
	}
}
