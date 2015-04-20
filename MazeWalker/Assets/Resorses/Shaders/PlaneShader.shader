Shader "Custom/PlaneShader" {
	Properties
	{
		_DistFar ("DistFar", Range (0, 2)) = 1
		_DistClose ("DistClose", Range (0, 7)) = 1
		_TimeM ("TimeM", Range (0, 8)) = 1
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
			uniform float _DistFar;
			uniform float _DistClose;
			uniform float _TimeM;
			uniform float4 _ColorA;
			uniform float4 _ColorB;

            #include "UnityCG.cginc"
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

				float d = distance(_Pos.xyz,sp.position_in_world_space.xyz);
				float vig = (clamp(d ,_DistClose,_DistFar) - _DistClose)/(_DistFar - _DistClose);
				return (_ColorA *(1- vig)* sin(_TimeM)/2 + _ColorB * vig) ;
            }
            ENDCG
        }
    }
}