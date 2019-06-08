Shader "Custom/TestCarPaintShader" {
		Properties{
			_Color("Color", Color) = (1,1,1,1)
			_MainTex("Albedo (RGB)", 2D) = "white" {}
			_Glossiness("Smoothness", Range(0,1)) = 0.5
			_Metallic("Metallic", Range(0,1)) = 0.0

			_StartX("start", Float) = 0.0
			_EndX("end", Float) = 0.0
			_Value("value",Float) = 0.5

			//1.以下四个属性为新加入的实现渐变色属性（参见学习小结（七））
			_Center("Center", range(-120,420)) = 0
			_R("R",range(0,400)) = 0
			_MainColor("Main Color", color) = (0,0,1,1)
			_SecColor("Second Color", color) = (1,0,0,1)
		}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		//#pragma surface surf Standard fullforwardshadows     //2.
#pragma surface surf Standard vertex:vert                       //3.

		// Use shader model 3.0 target, to get nicer looking lighting
#pragma target 3.0

		sampler2D _MainTex;
		float _Center;
		float _R;



		struct Input {
			float2 uv_MainTex;
			float x;                       //4.
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		fixed4 _SecColor;
		fixed4 _MainColor;
		float _StartX;
		float _EndX;
		float _Value;

		void vert(inout appdata_full v, out Input o)
		{
			o.uv_MainTex = v.texcoord.xy;
			o.x = v.vertex.x;
		}

		void surf(Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			float range = _EndX -_StartX;
			float target = _StartX + _Value * range;
			_Center = target;
			float s = IN.x - (_Center - _R / 2);
			float f = saturate(s / _R);
			fixed4 col = lerp(_MainColor, _SecColor, f);
			o.Albedo *= col.rgb;
			o.Alpha = c.a;
		}
		ENDCG
		}
		FallBack "Diffuse"
}