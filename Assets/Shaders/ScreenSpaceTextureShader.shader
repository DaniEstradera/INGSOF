Shader "Custom/ScreenSpaceTextureShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Main texture (RGB)", 2D) = "white" {}
		_ScreenTex("Screen space texture (RGB)", 2D) = "white" {}
		_OutlineColor("Outline Color", Color) = (0,0,0,1)
		_Outline("Outline width", Range(0.0, 0.03)) = .005
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}

	CGINCLUDE
		#include "UnityCG.cginc"
	ENDCG

	SubShader {
		Tags { "RenderType"="Opaque" "Queue" = "Transparent-10"}
		LOD 200
		Pass{
			Name "OUTLINE"
			Tags{ "LightMode" = "Always" }
			Cull Front
			ZWrite Off
			ZTest Less

			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				struct appdata {
					float4 vertex : POSITION;
					float3 normal : NORMAL;
				};

				struct v2f {
					float4 pos : POSITION;
					float4 color : COLOR;
				};

				uniform float _Outline;
				uniform float4 _OutlineColor;
				
				v2f vert(appdata v) {
					v2f o;
					o.pos = mul(UNITY_MATRIX_MVP, v.vertex);

					float3 norm = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
					norm = normalize(norm);
					float2 offset = TransformViewToProjection(norm.xy);

					o.pos.xy += offset * o.pos.z * _Outline;
					o.color = _OutlineColor;
					return o;
				}

				half4 frag(v2f i) : COLOR{
					return i.color;
				}
			ENDCG
		}

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _ScreenTex;

		struct Input {
			float2 uv_MainTex;
			float4 screenPos;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			half2 screenUV = IN.screenPos.xy / IN.screenPos.w;
			fixed4 sstc = tex2D(_ScreenTex, screenUV);
			o.Albedo = c.rgb * sstc.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
