Shader "Custom/TwoFaceTransparent" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("MainTex", 2D) = "white" {}
	_AlphaScale("Alpha Scale", Range(0, 1)) = 1
	}
		SubShader{
		Tags{ "Queue" = "Transparent" "IngnoreProjector" = "True" "RenderType" = "Transparent" }
		LOD 200

		Pass{
		Tags{ "LightMode" = "ForwardBase" }

		Cull Front

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM

#pragma vertex vert
#pragma fragment frag

#include "Lighting.cginc"

		fixed4 _Color;
	sampler2D _MainTex;
	float4 _MainTex_ST;
	fixed _AlphaScale;

	struct a2v {
		float4 vertex : POSITION;
		float4 normal : NORMAL;
		float4 texcoord : TEXCOORD0;
	};

	struct v2f {
		float4 position : SV_POSITION;
		float3 worldNormal : TEXCOORD0;
		float3 worldPos : TEXCOORD1;
		float2 uv : TEXCOORD2;
	};

	v2f vert(a2v v)
	{
		v2f f;
		f.position = UnityObjectToClipPos(v.vertex);

		//计算世界空间下的法线
		f.worldNormal = UnityObjectToWorldNormal(v.normal);

		//计算世界空间下的顶点
		f.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

		//计算变换后的纹理坐标
		f.uv = TRANSFORM_TEX(v.texcoord, _MainTex);

		return f;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		//归一
		fixed3 worldNormal = normalize(i.worldNormal);
	fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));

	//纹理颜色
	fixed4 textColor = tex2D(_MainTex, i.uv);


	//反射颜色
	fixed3 albedo = textColor.rgb * _Color.rgb;

	//环境光
	fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;

	//漫反射
	fixed3 diffuse = _LightColor0.rgb * albedo * max(0, dot(worldNormal, worldLightDir));

	return fixed4(ambient + diffuse, textColor.a * _AlphaScale);
	}

		ENDCG
	}
		Pass{
		Tags{ "LightMode" = "ForwardBase" }

		Cull Back

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM

#pragma vertex vert
#pragma fragment frag

#include "Lighting.cginc"

		fixed4 _Color;
	sampler2D _MainTex;
	float4 _MainTex_ST;
	fixed _AlphaScale;

	struct a2v {
		float4 vertex : POSITION;
		float4 normal : NORMAL;
		float4 texcoord : TEXCOORD0;
	};

	struct v2f {
		float4 position : SV_POSITION;
		float3 worldNormal : TEXCOORD0;
		float3 worldPos : TEXCOORD1;
		float2 uv : TEXCOORD2;
	};

	v2f vert(a2v v)
	{
		v2f f;
		f.position = UnityObjectToClipPos(v.vertex);

		//计算世界空间下的法线
		f.worldNormal = UnityObjectToWorldNormal(v.normal);

		//计算世界空间下的顶点
		f.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

		//计算变换后的纹理坐标
		f.uv = TRANSFORM_TEX(v.texcoord, _MainTex);

		return f;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		//归一
		fixed3 worldNormal = normalize(i.worldNormal);
	fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));

	//纹理颜色
	fixed4 textColor = tex2D(_MainTex, i.uv);


	//反射颜色
	fixed3 albedo = textColor.rgb * _Color.rgb;

	//环境光
	fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;

	//漫反射
	fixed3 diffuse = _LightColor0.rgb * albedo * max(0, dot(worldNormal, worldLightDir));

	return fixed4(ambient + diffuse, textColor.a * _AlphaScale);
	}

		ENDCG
	}
	}
		
}