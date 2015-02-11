Shader "Custom/Obstacle" 
{
	Properties 
	{
		_Color ( "Colour", Color ) = (0.0,0.0,0.0,0.0)
	}

	SubShader 
	{
		Tags { "RenderType" = "Opaque" }

		CGPROGRAM
		#pragma surface surf Lambert
		
		struct Input 
		{
			float3 worldPos;
		};

		float4 _Color;
		
		void surf (Input IN, inout SurfaceOutput o) 
		{
			o.Albedo = _Color;
		}

		ENDCG
	} 

	Fallback "Diffuse"
}