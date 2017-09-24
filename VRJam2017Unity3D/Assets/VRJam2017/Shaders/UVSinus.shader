// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "StolenCouch/UV Sinus" { 

	Properties {  
		_MainTex ("Texture", 2D) = "white" { }
		_WaveSpeed ("Wave Speed", Float) = 50.0 
	} 

	SubShader 
	{ 
		Pass 
		{ 
			CULL Off 
			ColorMaterial AmbientAndDiffuse
			
			Tags {
				"RenderQueue"= "Geometry+1"
				"RenderType"="Opaque" 
			}

			CGPROGRAM 
			#pragma vertex vert 
			#pragma fragment frag 
			#include "UnityCG.cginc" 

			float4 _Color; 
			sampler2D _MainTex;
			float _WaveSpeed;

			// vertex input: position, normal 
			struct appdata { 
				float4 vertex : POSITION; 
				float4 texcoord : TEXCOORD0; 
			}; 

			struct v2f { 
				float4 pos : POSITION; 
				float2 uv: TEXCOORD0; 
			}; 

			v2f vert (appdata v) { 
				v2f o; 
 				
				o.uv = v.texcoord; 
				
				float sinOff = v.vertex.x + v.vertex.z / 3; 
				float t  = _Time * _WaveSpeed * 5; 
				
				float fx = v.vertex.x; 
				float fy = v.vertex.x * v.vertex.z; 

				v.vertex.x += sin(t * 5 + sinOff) * _WaveSpeed;
				v.vertex.z -= sin(t * 5 - sinOff) * _WaveSpeed;
				
				o.pos = UnityObjectToClipPos( v.vertex );
				return o; 
			} 

			float4 frag (v2f i) : COLOR { 
				half4 color = tex2D(_MainTex, i.uv); 
				return color; 
			} 

			ENDCG 

			SetTexture [_MainTex] {combine texture} 
		} 
	}
	
	Fallback "StolenCouch/Vertex Colored Diffuse" 
}