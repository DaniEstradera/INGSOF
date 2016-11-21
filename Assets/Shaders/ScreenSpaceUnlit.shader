Shader "Custom/ScreenSpaceUnlit" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_Detail ("Detail", 2D) = "gray" {}

		_Scale ("Texture Scale", Float) = 1

	}
	SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}

		CGPROGRAM
		#pragma surface surf NoLighting alpha:fade
		#pragma vertex vert//added

		struct Input {
			float2 uv_MainTex;
			float4 screenPos;
			float3 color; //added
			INTERNAL_DATA//added
		};

		sampler2D _MainTex;
		sampler2D _Detail;

		float4 _Color;
		float _Scale;

		//added
		void vert (inout appdata_full v, out Input o) {
  			UNITY_INITIALIZE_OUTPUT(Input,o);
  			o.color = v.color;
  			//o.a = v.a;
		}


		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) ;
			o.Albedo = c.rgb;
			float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
			screenUV *= float2(_ScreenParams.x,_ScreenParams.y*1.75)*_Scale*0.001;
			o.Albedo *= tex2D (_Detail, screenUV).rgb * _Color;
			o.Alpha = c.a * _Color.a * float3(IN.color.r,IN.color.g,IN.color.b);
		}

		fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			fixed4 c;
			c.rgb = s.Albedo; 
			c.a = s.Alpha;
			return c;
		}

		ENDCG
	} 
	Fallback "Diffuse"
}

