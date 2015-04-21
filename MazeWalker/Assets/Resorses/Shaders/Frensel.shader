Shader "Custom/Frensel" {
	Properties {
		_MainTint("Diffuse",Color) = (1,1,1,1)
		_MainTex("RGB",2D) = "white"{}
		_Cubemap("Cibe",CUBE) = ""{}
		_Reflection("Reflect",Range(0,1)) = 1
		_RimPower("FalloFF",Range(0.1,3)) = 1
		_SpecColor("Spec",Color) = (1,1,1,1)
		_SpecPower("SpecPower",Range(0,1)) = 0.5
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf BlinnPhong

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		samplerCUBE _Cubemap;
		float4 _MainTint;
		float _Reflection;
		float _RimPower;
		float _SpecPower;

		struct Input {
			float2 uv_MainTex;
			float3 worldRefl;
			float3 viewDir;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutput o) {

			half4 c = tex2D(_MainTex,IN.uv_MainTex);
			float rim = 1.0 - saturate(dot(o.Normal,normalize(IN.viewDir)));
			rim = pow(rim,_RimPower);



			o.Albedo = c.rgb * _MainTint;
			o.Emission = rim * (_Reflection);
			o.Specular = _SpecPower;
			o.Gloss = 1.0;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
