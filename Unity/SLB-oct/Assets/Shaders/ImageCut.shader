// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:34236,y:34351,varname:node_3138,prsc:2|emission-9069-OUT,alpha-3081-A;n:type:ShaderForge.SFN_Tex2d,id:2810,x:30143,y:33152,ptovrint:False,ptlb:Elevation_Texture,ptin:_Elevation_Texture,varname:_Elevation_Texture,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:7f734e49be9548243ac4d198152e6f23,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Vector1,id:5652,x:28498,y:34627,varname:node_5652,prsc:2,v1:1;n:type:ShaderForge.SFN_Multiply,id:3487,x:30600,y:33289,varname:node_3487,prsc:2|A-2810-R,B-5652-OUT;n:type:ShaderForge.SFN_Multiply,id:1484,x:30600,y:33436,varname:node_1484,prsc:2|A-3428-OUT,B-2810-G;n:type:ShaderForge.SFN_Multiply,id:690,x:30600,y:33601,varname:node_690,prsc:2|A-3459-OUT,B-2810-B;n:type:ShaderForge.SFN_Divide,id:3428,x:28708,y:34729,varname:green_ratio,prsc:2|A-1518-OUT,B-4617-OUT;n:type:ShaderForge.SFN_Vector1,id:1518,x:28498,y:34773,varname:node_1518,prsc:2,v1:2;n:type:ShaderForge.SFN_Vector1,id:4617,x:28498,y:34877,varname:rgb_count,prsc:2,v1:3;n:type:ShaderForge.SFN_Divide,id:3459,x:28708,y:34927,varname:blue_ratio,prsc:2|A-5652-OUT,B-4617-OUT;n:type:ShaderForge.SFN_Add,id:864,x:30875,y:33293,varname:e_rgb2f,prsc:2|A-3487-OUT,B-1484-OUT,C-690-OUT;n:type:ShaderForge.SFN_Vector1,id:7636,x:30875,y:33485,varname:node_7636,prsc:2,v1:0;n:type:ShaderForge.SFN_If,id:2645,x:31240,y:33273,varname:node_2645,prsc:2|A-864-OUT,B-3438-OUT,GT-7636-OUT,EQ-2810-A,LT-2810-A;n:type:ShaderForge.SFN_If,id:4887,x:31240,y:33481,varname:node_4887,prsc:2|A-864-OUT,B-9178-OUT,GT-2810-A,EQ-2810-A,LT-7636-OUT;n:type:ShaderForge.SFN_Multiply,id:1754,x:31465,y:33356,varname:node_1754,prsc:2|A-2645-OUT,B-4887-OUT;n:type:ShaderForge.SFN_Tex2d,id:3081,x:33285,y:34651,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:37f2601622a180946af899626dd1df67,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:9993,x:30150,y:33971,ptovrint:False,ptlb:Porosity_Texture,ptin:_Porosity_Texture,varname:_Porosity_Texture,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:0a315fcb216eb734c9c85d630e411a3e,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:212,x:30614,y:33992,varname:node_212,prsc:2|A-9993-R,B-5652-OUT;n:type:ShaderForge.SFN_Multiply,id:2486,x:30614,y:34139,varname:node_2486,prsc:2|A-3428-OUT,B-9993-G;n:type:ShaderForge.SFN_Multiply,id:9250,x:30614,y:34304,varname:node_9250,prsc:2|A-3459-OUT,B-9993-B;n:type:ShaderForge.SFN_Add,id:6724,x:30889,y:33996,varname:node_6724,prsc:2|A-212-OUT,B-2486-OUT,C-9250-OUT;n:type:ShaderForge.SFN_Vector1,id:4324,x:30889,y:34188,varname:node_4324,prsc:2,v1:0;n:type:ShaderForge.SFN_If,id:272,x:31217,y:33994,varname:node_272,prsc:2|A-6724-OUT,B-6412-OUT,GT-4324-OUT,EQ-9993-A,LT-9993-A;n:type:ShaderForge.SFN_If,id:4761,x:31217,y:34201,varname:node_4761,prsc:2|A-6724-OUT,B-4139-OUT,GT-9993-A,EQ-9993-A,LT-4324-OUT;n:type:ShaderForge.SFN_Multiply,id:6888,x:31460,y:33994,varname:node_6888,prsc:2|A-272-OUT,B-4761-OUT;n:type:ShaderForge.SFN_Tex2d,id:9144,x:30156,y:34671,ptovrint:False,ptlb:Thickness_Texture,ptin:_Thickness_Texture,varname:_Thickness_Texture,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:9d63f4c218b9b294bb9fc593691d3c4d,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:7895,x:30620,y:34692,varname:node_7895,prsc:2|A-9144-R,B-5652-OUT;n:type:ShaderForge.SFN_Multiply,id:8791,x:30620,y:34839,varname:node_8791,prsc:2|A-3428-OUT,B-9144-G;n:type:ShaderForge.SFN_Multiply,id:6444,x:30620,y:35004,varname:node_6444,prsc:2|A-3459-OUT,B-9144-B;n:type:ShaderForge.SFN_Add,id:9450,x:30895,y:34696,varname:node_9450,prsc:2|A-7895-OUT,B-8791-OUT,C-6444-OUT;n:type:ShaderForge.SFN_Vector1,id:8853,x:30895,y:34888,varname:node_8853,prsc:2,v1:0;n:type:ShaderForge.SFN_If,id:8455,x:31223,y:34694,varname:node_8455,prsc:2|A-9450-OUT,B-1676-OUT,GT-8853-OUT,EQ-9144-A,LT-9144-A;n:type:ShaderForge.SFN_If,id:1205,x:31223,y:34901,varname:node_1205,prsc:2|A-9450-OUT,B-6783-OUT,GT-9144-A,EQ-9144-A,LT-8853-OUT;n:type:ShaderForge.SFN_Multiply,id:3663,x:31466,y:34694,varname:node_3663,prsc:2|A-8455-OUT,B-1205-OUT;n:type:ShaderForge.SFN_Tex2d,id:8761,x:30170,y:35378,ptovrint:False,ptlb:TOC_Texture,ptin:_TOC_Texture,varname:_TOC_Texture,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:c672e012ab03e5642b8589da25f98dfc,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:5295,x:30634,y:35399,varname:node_5295,prsc:2|A-8761-R,B-5652-OUT;n:type:ShaderForge.SFN_Multiply,id:6278,x:30634,y:35546,varname:node_6278,prsc:2|A-3428-OUT,B-8761-G;n:type:ShaderForge.SFN_Multiply,id:1684,x:30634,y:35711,varname:node_1684,prsc:2|A-3459-OUT,B-8761-B;n:type:ShaderForge.SFN_Add,id:1125,x:30909,y:35403,varname:node_1125,prsc:2|A-5295-OUT,B-6278-OUT,C-1684-OUT;n:type:ShaderForge.SFN_Vector1,id:3208,x:30909,y:35595,varname:node_3208,prsc:2,v1:0;n:type:ShaderForge.SFN_If,id:3170,x:31237,y:35401,varname:node_3170,prsc:2|A-1125-OUT,B-8884-OUT,GT-3208-OUT,EQ-8761-A,LT-8761-A;n:type:ShaderForge.SFN_If,id:6223,x:31237,y:35608,varname:node_6223,prsc:2|A-1125-OUT,B-2184-OUT,GT-8761-A,EQ-8761-A,LT-3208-OUT;n:type:ShaderForge.SFN_Multiply,id:3160,x:31455,y:35282,varname:node_3160,prsc:2|A-3170-OUT,B-6223-OUT;n:type:ShaderForge.SFN_Tex2d,id:5972,x:30181,y:36074,ptovrint:False,ptlb:VShale_Texture,ptin:_VShale_Texture,varname:_VShale_Texture,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:d72670d96f18e9f4193e43e399768302,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:9034,x:30645,y:36095,varname:node_9034,prsc:2|A-5972-R,B-5652-OUT;n:type:ShaderForge.SFN_Multiply,id:7250,x:30645,y:36242,varname:node_7250,prsc:2|A-3428-OUT,B-5972-G;n:type:ShaderForge.SFN_Multiply,id:1818,x:30645,y:36407,varname:node_1818,prsc:2|A-3459-OUT,B-5972-B;n:type:ShaderForge.SFN_Add,id:4155,x:30920,y:36099,varname:node_4155,prsc:2|A-9034-OUT,B-7250-OUT,C-1818-OUT;n:type:ShaderForge.SFN_Vector1,id:2297,x:30920,y:36291,varname:node_2297,prsc:2,v1:0;n:type:ShaderForge.SFN_If,id:3986,x:31248,y:36097,varname:node_3986,prsc:2|A-4155-OUT,B-4025-OUT,GT-2297-OUT,EQ-5972-A,LT-5972-A;n:type:ShaderForge.SFN_If,id:2655,x:31248,y:36304,varname:node_2655,prsc:2|A-4155-OUT,B-1084-OUT,GT-5972-A,EQ-5972-A,LT-2297-OUT;n:type:ShaderForge.SFN_Multiply,id:2485,x:31491,y:36097,varname:node_2485,prsc:2|A-3986-OUT,B-2655-OUT;n:type:ShaderForge.SFN_Multiply,id:9578,x:31679,y:33897,varname:porosity_c,prsc:2|A-9993-RGB,B-6888-OUT,C-3894-OUT;n:type:ShaderForge.SFN_Multiply,id:4787,x:31700,y:34605,varname:thickness_c,prsc:2|A-9144-RGB,B-3663-OUT,C-3951-OUT;n:type:ShaderForge.SFN_Multiply,id:2924,x:31661,y:35081,varname:toc_c,prsc:2|A-8761-RGB,B-3160-OUT,C-4918-OUT;n:type:ShaderForge.SFN_Multiply,id:1750,x:31727,y:35964,varname:vshale_c,prsc:2|A-5972-RGB,B-2485-OUT,C-8855-OUT;n:type:ShaderForge.SFN_Multiply,id:1521,x:31682,y:33131,varname:elev_c,prsc:2|A-2810-RGB,B-1754-OUT,C-1812-OUT;n:type:ShaderForge.SFN_Vector1,id:9893,x:31859,y:34303,varname:lerp_step,prsc:2,v1:0.5;n:type:ShaderForge.SFN_Lerp,id:2233,x:32268,y:33872,varname:node_2233,prsc:2|A-1521-OUT,B-9578-OUT,T-9893-OUT;n:type:ShaderForge.SFN_Lerp,id:8278,x:32109,y:35042,varname:node_8278,prsc:2|A-2924-OUT,B-1750-OUT,T-9893-OUT;n:type:ShaderForge.SFN_Lerp,id:2060,x:32402,y:34674,varname:node_2060,prsc:2|A-4787-OUT,B-8278-OUT,T-9893-OUT;n:type:ShaderForge.SFN_Lerp,id:5205,x:32957,y:34222,varname:node_5205,prsc:2|A-3309-OUT,B-2724-OUT,T-9893-OUT;n:type:ShaderForge.SFN_Multiply,id:3309,x:32638,y:34075,varname:node_3309,prsc:2|A-2233-OUT,B-5599-OUT;n:type:ShaderForge.SFN_Vector1,id:5599,x:32258,y:34389,varname:bright_ratio,prsc:2,v1:2;n:type:ShaderForge.SFN_Multiply,id:2724,x:32678,y:34484,varname:node_2724,prsc:2|A-2060-OUT,B-5599-OUT;n:type:ShaderForge.SFN_Multiply,id:7703,x:33184,y:34460,varname:node_7703,prsc:2|A-5205-OUT,B-5599-OUT;n:type:ShaderForge.SFN_Lerp,id:2540,x:33595,y:34277,varname:node_2540,prsc:2|A-7703-OUT,B-3081-RGB,T-9893-OUT;n:type:ShaderForge.SFN_Multiply,id:9069,x:33791,y:34372,varname:node_9069,prsc:2|A-2540-OUT,B-5599-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1812,x:31465,y:33579,ptovrint:False,ptlb:_ShowElevation,ptin:_ShowElevation,varname:ShowElevation,prsc:2,glob:True,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:3894,x:31460,y:34220,ptovrint:False,ptlb:_ShowPorosity,ptin:_ShowPorosity,varname:_ShowPorosity,prsc:2,glob:True,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:3951,x:31466,y:34935,ptovrint:False,ptlb:ShowThickness,ptin:_ShowThickness,varname:_ShowThickness,prsc:2,glob:True,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:4918,x:31455,y:35519,ptovrint:False,ptlb:ShowTOC,ptin:_ShowTOC,varname:_ShowTOC,prsc:2,glob:True,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:8855,x:31491,y:36338,ptovrint:False,ptlb:ShowVShale,ptin:_ShowVShale,varname:_ShowVShale,prsc:2,glob:True,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:4025,x:30920,y:36472,ptovrint:False,ptlb:TopVShale,ptin:_TopVShale,varname:_TopVShale,prsc:2,glob:True,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:1084,x:30920,y:36620,ptovrint:False,ptlb:BottomVShale,ptin:_BottomVShale,varname:_BottomVShale,prsc:2,glob:True,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:8884,x:30909,y:35745,ptovrint:False,ptlb:TopTOC,ptin:_TopTOC,varname:_TopTOC,prsc:2,glob:True,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:2184,x:30909,y:35892,ptovrint:False,ptlb:BottomTOC,ptin:_BottomTOC,varname:_BottomTOC,prsc:2,glob:True,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:1676,x:30895,y:35044,ptovrint:False,ptlb:TopThickness,ptin:_TopThickness,varname:_TopThickness,prsc:2,glob:True,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:6783,x:30895,y:35201,ptovrint:False,ptlb:BottomThickness,ptin:_BottomThickness,varname:_BottomThickness,prsc:2,glob:True,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:6412,x:30889,y:34362,ptovrint:False,ptlb:TopPorosity,ptin:_TopPorosity,varname:_TopPorosity,prsc:2,glob:True,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:4139,x:30889,y:34512,ptovrint:False,ptlb:BottomPorosity,ptin:_BottomPorosity,varname:_BottomPorosity,prsc:2,glob:True,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:3438,x:30875,y:33642,ptovrint:False,ptlb:TopElevation,ptin:_TopElevation,varname:_TopElevation,prsc:2,glob:True,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:9178,x:30875,y:33802,ptovrint:False,ptlb:BottomElevation,ptin:_BottomElevation,varname:_BottomElevation,prsc:2,glob:True,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;proporder:3081-2810-9993-9144-8761-5972;pass:END;sub:END;*/

Shader "Shader Forge/ImageCut" {
	Properties{
		_MainTex("MainTex", 2D) = "white" {}
		_Elevation_Texture("Elevation_Texture", 2D) = "black" {}
		_Porosity_Texture("Porosity_Texture", 2D) = "black" {}
		_Thickness_Texture("Thickness_Texture", 2D) = "black" {}
		_TOC_Texture("TOC_Texture", 2D) = "black" {}
		_VShale_Texture("VShale_Texture", 2D) = "black" {}

		_E_Start_Color("Elevation start color", Color) = (0,0,1,1)
		_E_End_Color("Elevation end color", Color) = (1,0,0,1)
		_Porosity_Start_Color("Porosity start color", Color) = (0,0,1,1)
		_Porosity_End_Color("Porosity end color", Color) = (1,0,0,1)
		_Thickness_Start_Color("Thickness start color", Color) = (0,0,1,1)
		_Thickness_End_Color("Thickness end color", Color) = (1,0,0,1)
		_TOC_Start_Color("TOC start color", Color) = (0,0,1,1)
		_TOC_End_Color("TOC end color", Color) = (1,0,0,1)
		_VShale_Start_Color("Vshale start color", Color) = (0,0,1,1)
		_VShale_End_Color("Vshale end color", Color) = (1,0,0,1)

		[HideInInspector]_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
	}
		SubShader{
			Tags {
				"IgnoreProjector" = "True"
				"Queue" = "Geometry"
				"RenderType" = "Transparent"
			}
			Pass {
				Name "FORWARD"
				Tags {
					"LightMode" = "ForwardBase"
				}
				Blend SrcAlpha OneMinusSrcAlpha
				ZWrite Off

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"
				#pragma multi_compile_fwdbase
				#pragma only_renderers d3d9 d3d11 glcore gles gles3 
				#pragma target 3.0
				uniform sampler2D _Elevation_Texture; uniform float4 _Elevation_Texture_ST;
				uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
				uniform sampler2D _Porosity_Texture; uniform float4 _Porosity_Texture_ST;
				uniform sampler2D _Thickness_Texture; uniform float4 _Thickness_Texture_ST;
				uniform sampler2D _TOC_Texture; uniform float4 _TOC_Texture_ST;
				uniform sampler2D _VShale_Texture; uniform float4 _VShale_Texture_ST;
				uniform float _ShowElevation;
				uniform float _ShowPorosity;
				uniform float _ShowThickness;
				uniform float _ShowTOC;
				uniform float _ShowVShale;
				uniform float _TopVShale;
				uniform float _BottomVShale;
				uniform float _TopTOC;
				uniform float _BottomTOC;
				uniform float _TopThickness;
				uniform float _BottomThickness;
				uniform float _TopPorosity;
				uniform float _BottomPorosity;
				uniform float _TopElevation;
				uniform float _BottomElevation;

				uniform float4 _E_Start_Color;
				uniform float4 _E_End_Color;
				uniform float4 _Porosity_Start_Color;
				uniform float4 _Porosity_End_Color;
				uniform float4 _Thickness_Start_Color;
				uniform float4 _Thickness_End_Color;
				uniform float4 _TOC_Start_Color;
				uniform float4 _TOC_End_Color;
				uniform float4 _VShale_Start_Color;
				uniform float4 _VShale_End_Color;

				float3 _Origin;
				float3 _BoxSize;

				struct VertexInput {
					float4 vertex : POSITION;
					float2 texcoord0 : TEXCOORD0;
				};

				struct VertexOutput {
					float4 pos : SV_POSITION;
					float2 uv0 : TEXCOORD0;
					float3 worldPos : TEXCOORD1;
				};

				VertexOutput vert(VertexInput v) {
					VertexOutput o = (VertexOutput)0;
					o.uv0 = v.texcoord0;

					o.pos = UnityObjectToClipPos(v.vertex);
					o.worldPos = mul(unity_ObjectToWorld, v.vertex);

					return o;
				}


				float4 frag(VertexOutput i) : COLOR {
					////// Lighting:
					////// Emissive:

									float4 _Elevation_Texture_var = tex2D(_Elevation_Texture,TRANSFORM_TEX(i.uv0, _Elevation_Texture));
									float4 es = (_E_End_Color - _E_Start_Color);
									float4 ec = (_Elevation_Texture_var - _E_Start_Color);
									float rgb2f = dot(ec, es) / dot(es, es); 
									float is_over_top = step(rgb2f,_TopElevation); //filter from top

									float is_under_bottom = step(_BottomElevation, rgb2f);
									float is_filtered = is_over_top * is_under_bottom * _Elevation_Texture_var.a;
									float3 elev_c = _Elevation_Texture_var.rgb * is_filtered * _ShowElevation;
									
									float4 _Porosity_Texture_var = tex2D(_Porosity_Texture,TRANSFORM_TEX(i.uv0, _Porosity_Texture));
									es = (_Porosity_End_Color - _Porosity_Start_Color);
									ec = (_Porosity_Texture_var - _Porosity_Start_Color);
									rgb2f = dot(ec, es) / dot(es, es);
									is_over_top = step(rgb2f, _TopPorosity); //filter from top
									is_under_bottom = step(_BottomPorosity, rgb2f);
									is_filtered = is_over_top * is_under_bottom * _Porosity_Texture_var.a;
									float3 porosity_c = (_Porosity_Texture_var.rgb * is_filtered*_ShowPorosity);
									
									float4 _Thickness_Texture_var = tex2D(_Thickness_Texture,TRANSFORM_TEX(i.uv0, _Thickness_Texture));
									es = (_Thickness_End_Color - _Thickness_Start_Color);
									ec = (_Thickness_Texture_var - _Thickness_Start_Color);
									rgb2f = dot(ec, es) / dot(es, es);
									is_over_top = step(rgb2f, _TopThickness); //filter from top
									is_under_bottom = step(_BottomThickness, rgb2f);
									is_filtered = is_over_top * is_under_bottom * _Thickness_Texture_var.a;
									float3 thickness_c = (_Thickness_Texture_var.rgb * is_filtered * _ShowThickness);

									float4 _TOC_Texture_var = tex2D(_TOC_Texture,TRANSFORM_TEX(i.uv0, _TOC_Texture));
									es = (_TOC_End_Color - _TOC_Start_Color);
									ec = (_TOC_Texture_var - _TOC_Start_Color);
									rgb2f = dot(ec, es) / dot(es, es);
									is_over_top = step(rgb2f, _TopTOC); //filter from top
									is_under_bottom = step(_BottomTOC, rgb2f);
									is_filtered = is_over_top * is_under_bottom * _TOC_Texture_var.a;
									float3 toc_c = (_TOC_Texture_var.rgb * is_filtered  *_ShowTOC);

									float4 _VShale_Texture_var = tex2D(_VShale_Texture,TRANSFORM_TEX(i.uv0, _VShale_Texture));
									es = (_VShale_End_Color - _VShale_Start_Color);
									ec = (_VShale_Texture_var - _VShale_Start_Color);
									rgb2f = dot(ec, es) / dot(es, es);
									is_over_top = step(rgb2f, _TopVShale); //filter from top
									is_under_bottom = step(_BottomVShale, rgb2f);
									is_filtered = is_over_top * is_under_bottom * _VShale_Texture_var.a;
									float3 vshale_c = (_VShale_Texture_var.rgb * is_filtered * _ShowVShale);

									//float3 emissive = (lerp((lerp((lerp(elev_c, porosity_c, lerp_step)*bright_ratio), (lerp(thickness_c, lerp(toc_c, vshale_c, lerp_step), lerp_step)*bright_ratio), lerp_step)*bright_ratio), _MainTex_var.rgb, lerp_step)*bright_ratio);

									float4 _MainTex_var = tex2D(_MainTex, TRANSFORM_TEX(i.uv0, _MainTex));
									float available_colors = _ShowElevation + _ShowPorosity +_ShowThickness + _ShowTOC + _ShowVShale ;

								    // https://gamedev.stackexchange.com/questions/158173/does-cause-branching-in-glsl
									available_colors = (available_colors == 0.0 ? 1 : available_colors); //is it actual branching or GPU can optimize it ?
									
									float3 emissive = ((elev_c + porosity_c + thickness_c + toc_c + vshale_c ) ) ;
									float3 finalColor = emissive + _MainTex_var.rgb;

									float3 dir = i.worldPos - _Origin; 
									half3 dist = half3(
										abs(dir.x), // no negatives
										abs(dir.y), // no negatives
										abs(dir.z)  // no negatives
										);

									float3 offset;
									offset.x = dist.x - _BoxSize.x * 0.5;
									offset.y = dist.y - _BoxSize.y * 0.5;
									offset.z = dist.z - _BoxSize.z * 0.5;
									
									float isClipped = max(offset.x, offset.z);  //avoiding branching
									//clip(isClipped);

									return fixed4(finalColor,1);
								}
								ENDCG
							}
		}
			FallBack "Diffuse"
CustomEditor "ShaderForgeMaterialInspector"
}
