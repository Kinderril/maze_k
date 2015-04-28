Shader "Custom/TimeDependesColor" {
	Properties {
		_TimeM ("TimeM", Range (0, 1)) = 1
		_Pos ("Pos", vector) = (1,1,1)
		_ColorA ("ColorA", Color) = (1,1,1,1)
		_ColorB ("ColorB", Color) = (1,1,1,1)
	}
	SubShader {
        Pass {
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
		
			uniform float3 _Pos;
			uniform float _TimeM;
			uniform float4 _ColorA;
			uniform float4 _ColorB;
		
			struct vertexInput {
				float4 vertex : POSITION;
			};
		
			struct vertexOutput {
				float4 pos : SV_POSITION;
				float4 position_in_world_space : TEXCOORD0;
			};

			vertexOutput vert(vertexInput input : POSITION) : POSITION {
				vertexOutput output; 
 
				output.pos =  mul(UNITY_MATRIX_MVP, input.vertex);
				output.position_in_world_space = 
					mul(_Object2World, input.vertex);
				return output;
			}

			float4 frag(vertexOutput sp:POSITION) : COLOR {

				return (_ColorA *(1- _TimeM) + _ColorB * _TimeM) ;
			}
			ENDCG
		} 
	}
}

