Shader "Custom/Web" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
#pragma surface surf Lambert

		sampler2D _MainTex;

	struct Input {
		float2 uv_MainTex;
		half4 color : COLOR;
	};

	void surf(Input IN, inout SurfaceOutput o) {
		half4 c = tex2D(_MainTex, IN.uv_MainTex);
		// 叠加色
		//o.Albedo = c.rgb * IN.color.rgb;
		//o.Alpha = c.a * IN.color.a;
		//纯色
		//if()In.x
		o.Albedo = IN.color.rgb;
		o.Alpha = IN.color.a;
	}
	ENDCG
	}
		FallBack "Diffuse"
}