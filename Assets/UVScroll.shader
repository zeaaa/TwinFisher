Shader "Custom/Scroll"
{
	Properties{
		_MainTint("Diffuse Tint" , Color) = (1,1,1,1)
		_MainTex("Base (RGB)", 2D) = "white" {}
	//x轴滚动速度 
		_ScrollXSpeed("X Scroll Speed" , Range(0,10)) = 2
		//y轴滚动速度 
		_ScrollYSpeed("Y Scroll Speed" , Range(0,10)) = 2
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
#pragma surface surf Lambert 
		fixed4 _MainTint;
	fixed _ScrollXSpeed;
	fixed _ScrollYSpeed;
	sampler2D _MainTex;

	struct Input {
		float2 uv_MainTex;
	};

	void surf(Input IN, inout SurfaceOutput o) {

		//获取UV坐标 
		fixed2 scrolledUV = IN.uv_MainTex;

		//根据时间和x,y轴的速度 计算当前UV坐标的偏移量 
		fixed xScrollValue = _ScrollXSpeed * _Time;
		fixed yScrollValue = _ScrollYSpeed * _Time;
		scrolledUV += fixed2(xScrollValue,yScrollValue);

		half4 c = tex2D(_MainTex, scrolledUV);
		o.Albedo = c.rgb;
		o.Alpha = c.a;
	}
	ENDCG
	}
	FallBack "Diffuse"
}