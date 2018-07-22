Shader "Custom/Avatar" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	
	SubShader {
		Tags { "RenderType"="Transparent"  "Queue" = "Geometry+20"}
		LOD 200
		Pass{
			ZTest Always
			ZWrite Off
			SetTexture[_MainTex] {
				constantColor (0.4,0.4,0.4,0.1)
				combine constant * texture 
			}
		}
		Pass{
			Material{
				Diffuse (1,1,1,1)
				Ambient (1,1,1,1)
			}
					
			Lighting Off
			Color(0.5,0.5,0.5,1)
			SetTexture [_MainTex]{Combine texture * primary DOUBLE}
		}
	} 
	FallBack "Diffuse"
}