// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Hidden/BOXOPHOBIC/The Vegetation Engine/Helpers/Impostors Baker"
{
	Properties
	{
		_IsPropShader("IsPropShader", Float) = 0
		_IsBarkShader("IsBarkShader", Float) = 0
		_IsGrassShader("IsGrassShader", Float) = 0
		_IsLeafShader("IsLeafShader", Float) = 0
		_IsCrossShader("IsCrossShader", Float) = 0
		_HasEmissive("_HasEmissive", Float) = 0
		_HasOcclusion("_HasOcclusion", Float) = 1
		_HasGradient("_HasGradient", Float) = 1
		[HDR]_MainColor("Main Color", Color) = (1,1,1,1)
		[NoScaleOffset]_MainAlbedoTex("Main Albedo", 2D) = "white" {}
		[NoScaleOffset]_MainNormalTex("Main Normal", 2D) = "gray" {}
		[NoScaleOffset]_MainMaskTex("Main Mask", 2D) = "white" {}
		[Space(10)]_MainUVs("Main UVs", Vector) = (1,1,0,0)
		_MainNormalValue("Main Normal", Range( -8 , 8)) = 0
		[HDR]_VertexOcclusionColor("Vertex Occlusion Color", Color) = (1,1,1,1)
		[HideInInspector]_VertexOcclusionMinValue("Vertex Occlusion Min Value", Range( 0 , 1)) = 0
		[HideInInspector]_VertexOcclusionMaxValue("Vertex Occlusion Max Value", Range( 0 , 1)) = 1
		_Cutoff("_Cutoff", Float) = 0
		_MainSmoothnessValue("Main Smoothness", Range( 0 , 1)) = 1
		_MainMetallicValue("Main Metallic", Range( 0 , 1)) = 1
		[HDR]_GradientColorOne("Gradient Color One", Color) = (1,1,1,1)
		[HDR]_GradientColorTwo("Gradient Color Two", Color) = (1,1,1,1)
		[StyledRemapSlider(_GradientMinValue, _GradientMaxValue, 0, 1)]_GradientMaskRemap("Gradient Mask", Vector) = (0,0,0,0)
		[HideInInspector]_GradientMinValue("Gradient Mask Min", Range( 0 , 1)) = 0
		[HideInInspector]_GradientMaxValue("Gradient Mask Max ", Range( 0 , 1)) = 1
		_RenderClip("Render Clip", Float) = 1
		[Enum(UV 0,0,UV 2,1)]_DetailCoordMode("Detail Coord", Float) = 0
		[NoScaleOffset]_SecondAlbedoTex("Detail Albedo", 2D) = "white" {}
		[NoScaleOffset]_SecondNormalTex("Detail Normal", 2D) = "gray" {}
		[NoScaleOffset]_SecondMaskTex("Detail Mask", 2D) = "white" {}
		[Space(10)]_SecondUVs("Detail UVs", Vector) = (1,1,0,0)
		[HDR]_SecondColor("Detail Color", Color) = (1,1,1,1)
		_SecondNormalValue("Detail Normal", Range( -8 , 8)) = 1
		_SecondMetallicValue("Detail Metallic", Range( 0 , 1)) = 0
		_SecondSmoothnessValue("Detail Smoothness", Range( 0 , 1)) = 1
		[Enum(Top,0,Bottom,1)]_DetailProjectionMode("Detail Projection", Float) = 0
		[Space(10)]_DetailNormalValue("Detail Use Main Normal", Range( 0 , 1)) = 0.5
		[Enum(Main Mask,0,Detail Mask,1)][Space(10)]_DetailMaskMode("Detail Mask", Float) = 0
		_DetailMeshValue("Detail Offset", Range( -1 , 1)) = 0
		[StyledRemapSlider(_DetailBlendMinValue, _DetailBlendMaxValue,0,1)]_DetailBlendRemap("Detail Blend", Vector) = (0,0,0,0)
		[HideInInspector]_DetailBlendMinValue("Detail Blend Min Value", Range( 0 , 1)) = 0.2
		[HideInInspector]_DetailBlendMaxValue("Detail Blend Max Value", Range( 0 , 1)) = 0.3
		[HideInInspector]_render_normals_options("_render_normals_options", Vector) = (1,1,1,0)
		[Enum(Off,0,On,1)]_DetailMaskInvertMode("Detail Mask Invert", Float) = 0
		[NoScaleOffset][StyledTextureSingleLine]_EmissiveTex("Emissive Texture", 2D) = "white" {}
		[HDR]_EmissiveColor("Emissive Color", Color) = (0,0,0,0)
		[ASEEnd][Space(10)]_EmissiveUVs("Emissive UVs", Vector) = (1,1,0,0)
		[HideInInspector]_render_cull("_render_cull", Float) = 0

	}

	SubShader
	{
		Tags { "RenderType"="Opaque" }
	LOD 100
		CGINCLUDE
		#pragma target 4.0
		ENDCG
		Cull [_render_cull]
		

		Pass
		{
			Name "Unlit"
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#pragma multi_compile_fwdbase
			#include "UnityStandardUtils.cginc"
			#include "UnityShaderVariables.cginc"
			#define ASE_NEEDS_FRAG_COLOR
			#define ASE_NEEDS_VERT_POSITION
			#pragma shader_feature_local TVE_DETAIL_MODE_OFF TVE_DETAIL_MODE_ON
			#pragma shader_feature_local TVE_DETAIL_TYPE_VERTEX_BLUE TVE_DETAIL_TYPE_PROJECTION
			#pragma shader_feature_local TVE_DETAIL_BLEND_OVERLAY TVE_DETAIL_BLEND_REPLACE


			struct appdata
			{
				float4 vertex : POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_color : COLOR;
				float3 ase_normal : NORMAL;
				float4 ase_tangent : TANGENT;
			};

			struct v2f
			{
				UNITY_POSITION(pos);
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_color : COLOR;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_texcoord4 : TEXCOORD4;
				float4 ase_texcoord5 : TEXCOORD5;
				float4 ase_texcoord6 : TEXCOORD6;
			};

			uniform half _render_cull;
			uniform float4 _GradientMaskRemap;
			uniform half4 _DetailBlendRemap;
			uniform sampler2D _MainAlbedoTex;
			uniform half4 _MainUVs;
			uniform float4 _MainColor;
			uniform half4 _SecondColor;
			uniform sampler2D _SecondAlbedoTex;
			uniform float _IsBarkShader;
			uniform half _DetailCoordMode;
			uniform float _IsPropShader;
			uniform half4 _SecondUVs;
			uniform half _DetailMeshValue;
			uniform half _DetailProjectionMode;
			uniform sampler2D _MainMaskTex;
			uniform sampler2D _SecondMaskTex;
			uniform half _DetailMaskMode;
			uniform half _DetailMaskInvertMode;
			uniform half _DetailBlendMinValue;
			uniform half _DetailBlendMaxValue;
			uniform half4 _VertexOcclusionColor;
			uniform half _VertexOcclusionMinValue;
			uniform half _VertexOcclusionMaxValue;
			uniform float _HasOcclusion;
			uniform half4 _GradientColorTwo;
			uniform half4 _GradientColorOne;
			uniform half _GradientMinValue;
			uniform half _GradientMaxValue;
			uniform float _HasGradient;
			uniform float _IsLeafShader;
			uniform float _IsGrassShader;
			uniform sampler2D _MainNormalTex;
			uniform float _MainNormalValue;
			uniform sampler2D _SecondNormalTex;
			uniform half _SecondNormalValue;
			uniform half _DetailNormalValue;
			uniform half3 _render_normals_options;
			uniform float _MainMetallicValue;
			uniform half _SecondMetallicValue;
			uniform float _IsCrossShader;
			uniform float _MainSmoothnessValue;
			uniform half _SecondSmoothnessValue;
			uniform half4 _EmissiveColor;
			uniform sampler2D _EmissiveTex;
			uniform half4 _EmissiveUVs;
			uniform float _HasEmissive;
			uniform float _Cutoff;
			uniform float _RenderClip;


			v2f vert(appdata v )
			{
				v2f o = (v2f)0;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				float3 ase_worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.ase_texcoord2.xyz = ase_worldPos;
				float3 ase_worldNormal = UnityObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord3.xyz = ase_worldNormal;
				
				float3 ase_worldTangent = UnityObjectToWorldDir(v.ase_tangent);
				o.ase_texcoord4.xyz = ase_worldTangent;
				float ase_vertexTangentSign = v.ase_tangent.w * unity_WorldTransformParams.w;
				float3 ase_worldBitangent = cross( ase_worldNormal, ase_worldTangent ) * ase_vertexTangentSign;
				o.ase_texcoord5.xyz = ase_worldBitangent;
				float3 objectToViewPos = UnityObjectToViewPos(v.vertex.xyz);
				float eyeDepth = -objectToViewPos.z;
				o.ase_texcoord1.z = eyeDepth;
				
				float2 vertexToFrag11_g50675 = ( ( v.ase_texcoord.xy * (_EmissiveUVs).xy ) + (_EmissiveUVs).zw );
				o.ase_texcoord6.xy = vertexToFrag11_g50675;
				
				o.ase_texcoord = v.ase_texcoord;
				o.ase_texcoord1.xy = v.ase_texcoord1.xy;
				o.ase_color = v.ase_color;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.w = 0;
				o.ase_texcoord2.w = 0;
				o.ase_texcoord3.w = 0;
				o.ase_texcoord4.w = 0;
				o.ase_texcoord5.w = 0;
				o.ase_texcoord6.zw = 0;

				v.vertex.xyz +=  float3(0,0,0) ;
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}


			void frag(v2f i , half ase_vface : VFACE,
				out half4 outGBuffer0 : SV_Target0, 
				out half4 outGBuffer1 : SV_Target1, 
				out half4 outGBuffer2 : SV_Target2, 
				out half4 outGBuffer3 : SV_Target3,
				out half4 outGBuffer4 : SV_Target4,
				out half4 outGBuffer5 : SV_Target5,
				out half4 outGBuffer6 : SV_Target6,
				out half4 outGBuffer7 : SV_Target7,
				out float outDepth : SV_Depth
			) 
			{
				UNITY_SETUP_INSTANCE_ID( i );
				float2 Main_UVs247_g50667 = ( ( i.ase_texcoord.xy * (_MainUVs).xy ) + (_MainUVs).zw );
				float4 tex2DNode14_g50667 = tex2D( _MainAlbedoTex, Main_UVs247_g50667 );
				float3 Main_Albedo79_g50667 = ( (tex2DNode14_g50667).rgb * (_MainColor).rgb );
				float2 appendResult498_g50667 = (float2(i.ase_texcoord.z , i.ase_texcoord.w));
				float _IsBarkShader78_g50667 = _IsBarkShader;
				float2 lerpResult461_g50667 = lerp( i.ase_texcoord.xy , i.ase_texcoord1.xy , _DetailCoordMode);
				float3 ase_worldPos = i.ase_texcoord2.xyz;
				#if defined(TVE_DETAIL_TYPE_VERTEX_BLUE)
				float2 staticSwitch479_g50667 = lerpResult461_g50667;
				#elif defined(TVE_DETAIL_TYPE_PROJECTION)
				float2 staticSwitch479_g50667 = (ase_worldPos).xz;
				#else
				float2 staticSwitch479_g50667 = lerpResult461_g50667;
				#endif
				float _IsPropShader89_g50667 = _IsPropShader;
				half2 Second_UVs495_g50667 = ( ( ( ( appendResult498_g50667 * _IsBarkShader78_g50667 ) + ( staticSwitch479_g50667 * _IsPropShader89_g50667 ) ) * (_SecondUVs).xy ) + (_SecondUVs).zw );
				float4 tex2DNode491_g50667 = tex2D( _SecondAlbedoTex, Second_UVs495_g50667 );
				half3 Second_Albedo469_g50667 = (( _SecondColor * tex2DNode491_g50667 )).rgb;
				#ifdef UNITY_COLORSPACE_GAMMA
				float staticSwitch1_g50673 = 2.0;
				#else
				float staticSwitch1_g50673 = 4.594794;
				#endif
				float3 ase_worldNormal = i.ase_texcoord3.xyz;
				float3 lerpResult573_g50667 = lerp( float3(0,1,0) , float3(0,-1,0) , _DetailProjectionMode);
				float dotResult556_g50667 = dot( ase_worldNormal , lerpResult573_g50667 );
				#if defined(TVE_DETAIL_TYPE_VERTEX_BLUE)
				float staticSwitch555_g50667 = ( ( i.ase_color.b - 0.5 ) + _DetailMeshValue );
				#elif defined(TVE_DETAIL_TYPE_PROJECTION)
				float staticSwitch555_g50667 = ( ( dotResult556_g50667 * 0.5 ) + _DetailMeshValue );
				#else
				float staticSwitch555_g50667 = ( ( i.ase_color.b - 0.5 ) + _DetailMeshValue );
				#endif
				half Blend_Source550_g50667 = staticSwitch555_g50667;
				float4 tex2DNode30_g50667 = tex2D( _MainMaskTex, Main_UVs247_g50667 );
				float Main_Mask64_g50667 = tex2DNode30_g50667.b;
				float4 tex2DNode451_g50667 = tex2D( _SecondMaskTex, Second_UVs495_g50667 );
				half Second_Mask437_g50667 = tex2DNode451_g50667.b;
				float lerpResult563_g50667 = lerp( Main_Mask64_g50667 , Second_Mask437_g50667 , _DetailMaskMode);
				float lerpResult611_g50667 = lerp( lerpResult563_g50667 , ( 1.0 - lerpResult563_g50667 ) , _DetailMaskInvertMode);
				float temp_output_7_0_g50674 = _DetailBlendMinValue;
				half Detail_Mask535_g50667 = saturate( ( ( saturate( ( Blend_Source550_g50667 + ( Blend_Source550_g50667 * lerpResult611_g50667 ) ) ) - temp_output_7_0_g50674 ) / ( _DetailBlendMaxValue - temp_output_7_0_g50674 ) ) );
				float3 lerpResult560_g50667 = lerp( Main_Albedo79_g50667 , ( Main_Albedo79_g50667 * Second_Albedo469_g50667 * staticSwitch1_g50673 ) , Detail_Mask535_g50667);
				float3 lerpResult536_g50667 = lerp( Main_Albedo79_g50667 , Second_Albedo469_g50667 , Detail_Mask535_g50667);
				#if defined(TVE_DETAIL_BLEND_OVERLAY)
				float3 staticSwitch620_g50667 = lerpResult560_g50667;
				#elif defined(TVE_DETAIL_BLEND_REPLACE)
				float3 staticSwitch620_g50667 = lerpResult536_g50667;
				#else
				float3 staticSwitch620_g50667 = lerpResult560_g50667;
				#endif
				#if defined(TVE_DETAIL_TYPE_VERTEX_BLUE)
				float3 staticSwitch634_g50667 = staticSwitch620_g50667;
				#elif defined(TVE_DETAIL_TYPE_PROJECTION)
				float3 staticSwitch634_g50667 = Main_Albedo79_g50667;
				#else
				float3 staticSwitch634_g50667 = staticSwitch620_g50667;
				#endif
				#if defined(TVE_DETAIL_MODE_OFF)
				float3 staticSwitch591_g50667 = Main_Albedo79_g50667;
				#elif defined(TVE_DETAIL_MODE_ON)
				float3 staticSwitch591_g50667 = staticSwitch634_g50667;
				#else
				float3 staticSwitch591_g50667 = Main_Albedo79_g50667;
				#endif
				half3 Blend_Albedo540_g50667 = staticSwitch591_g50667;
				float3 temp_cast_0 = (1.0).xxx;
				float3 temp_cast_1 = (1.0).xxx;
				float temp_output_7_0_g50670 = _VertexOcclusionMinValue;
				float3 lerpResult260_g50667 = lerp( (_VertexOcclusionColor).rgb , temp_cast_1 , saturate( ( ( i.ase_color.g - temp_output_7_0_g50670 ) / ( _VertexOcclusionMaxValue - temp_output_7_0_g50670 ) ) ));
				float3 lerpResult421_g50667 = lerp( temp_cast_0 , lerpResult260_g50667 , _HasOcclusion);
				float3 Vertex_Occlusion_Color261_g50667 = lerpResult421_g50667;
				float3 temp_cast_2 = (1.0).xxx;
				float temp_output_7_0_g50668 = _GradientMinValue;
				float3 lerpResult339_g50667 = lerp( (_GradientColorTwo).rgb , (_GradientColorOne).rgb , saturate( ( ( i.ase_color.a - temp_output_7_0_g50668 ) / ( _GradientMaxValue - temp_output_7_0_g50668 ) ) ));
				float3 lerpResult419_g50667 = lerp( temp_cast_2 , lerpResult339_g50667 , _HasGradient);
				half3 Gradient_Tint335_g50667 = lerpResult419_g50667;
				float _IsLeafShader81_g50667 = _IsLeafShader;
				float _IsGrassShader255_g50667 = _IsGrassShader;
				half3 Final_Albedo606_g50667 = ( ( Blend_Albedo540_g50667 * Vertex_Occlusion_Color261_g50667 * Gradient_Tint335_g50667 * _IsLeafShader81_g50667 ) + ( Blend_Albedo540_g50667 * Vertex_Occlusion_Color261_g50667 * Gradient_Tint335_g50667 * _IsGrassShader255_g50667 ) + ( Blend_Albedo540_g50667 * Vertex_Occlusion_Color261_g50667 * _IsBarkShader78_g50667 ) + ( Blend_Albedo540_g50667 * _IsPropShader89_g50667 ) );
				float Final_Alpha387_g50667 = 1.0;
				float4 appendResult230_g50667 = (float4(Final_Albedo606_g50667 , Final_Alpha387_g50667));
				
				float4 tex2DNode18_g50667 = tex2D( _MainNormalTex, Main_UVs247_g50667 );
				float2 appendResult88_g50671 = (float2(tex2DNode18_g50667.a , tex2DNode18_g50667.g));
				float2 temp_output_90_0_g50671 = ( (appendResult88_g50671*2.0 + -1.0) * _MainNormalValue );
				float3 appendResult91_g50671 = (float3(temp_output_90_0_g50671 , 1.0));
				float3 Main_Normal27_g50667 = appendResult91_g50671;
				float4 tex2DNode459_g50667 = tex2D( _SecondNormalTex, Second_UVs495_g50667 );
				float2 appendResult88_g50669 = (float2(tex2DNode459_g50667.a , tex2DNode459_g50667.g));
				float2 temp_output_90_0_g50669 = ( (appendResult88_g50669*2.0 + -1.0) * _SecondNormalValue );
				float3 appendResult91_g50669 = (float3(temp_output_90_0_g50669 , 1.0));
				half3 Second_Normal443_g50667 = appendResult91_g50669;
				float3 lerpResult514_g50667 = lerp( float3( 0,0,1 ) , Second_Normal443_g50667 , Detail_Mask535_g50667);
				float3 lerpResult554_g50667 = lerp( float3( 0,0,1 ) , Main_Normal27_g50667 , _DetailNormalValue);
				float3 lerpResult502_g50667 = lerp( Main_Normal27_g50667 , BlendNormals( lerpResult554_g50667 , Second_Normal443_g50667 ) , Detail_Mask535_g50667);
				#if defined(TVE_DETAIL_BLEND_OVERLAY)
				float3 staticSwitch621_g50667 = BlendNormals( Main_Normal27_g50667 , lerpResult514_g50667 );
				#elif defined(TVE_DETAIL_BLEND_REPLACE)
				float3 staticSwitch621_g50667 = lerpResult502_g50667;
				#else
				float3 staticSwitch621_g50667 = BlendNormals( Main_Normal27_g50667 , lerpResult514_g50667 );
				#endif
				#if defined(TVE_DETAIL_TYPE_VERTEX_BLUE)
				float3 staticSwitch625_g50667 = staticSwitch621_g50667;
				#elif defined(TVE_DETAIL_TYPE_PROJECTION)
				float3 staticSwitch625_g50667 = Main_Normal27_g50667;
				#else
				float3 staticSwitch625_g50667 = staticSwitch621_g50667;
				#endif
				#if defined(TVE_DETAIL_MODE_OFF)
				float3 staticSwitch577_g50667 = Main_Normal27_g50667;
				#elif defined(TVE_DETAIL_MODE_ON)
				float3 staticSwitch577_g50667 = staticSwitch625_g50667;
				#else
				float3 staticSwitch577_g50667 = Main_Normal27_g50667;
				#endif
				float3 temp_output_13_0_g50672 = staticSwitch577_g50667;
				float3 switchResult12_g50672 = (((ase_vface>0)?(temp_output_13_0_g50672):(( temp_output_13_0_g50672 * _render_normals_options ))));
				half3 Blend_Normal582_g50667 = switchResult12_g50672;
				float3 ase_worldTangent = i.ase_texcoord4.xyz;
				float3 ase_worldBitangent = i.ase_texcoord5.xyz;
				float3 tanToWorld0 = float3( ase_worldTangent.x, ase_worldBitangent.x, ase_worldNormal.x );
				float3 tanToWorld1 = float3( ase_worldTangent.y, ase_worldBitangent.y, ase_worldNormal.y );
				float3 tanToWorld2 = float3( ase_worldTangent.z, ase_worldBitangent.z, ase_worldNormal.z );
				float3 tanNormal3_g50667 = Blend_Normal582_g50667;
				float3 worldNormal3_g50667 = float3(dot(tanToWorld0,tanNormal3_g50667), dot(tanToWorld1,tanNormal3_g50667), dot(tanToWorld2,tanNormal3_g50667));
				float3 Final_NormalsWS91_g50667 = (worldNormal3_g50667*0.5 + 0.5);
				float eyeDepth = i.ase_texcoord1.z;
				float temp_output_7_0_g50667 = ( -1.0 / UNITY_MATRIX_P[2].z );
				float Final_Depth92_g50667 = ( ( eyeDepth + temp_output_7_0_g50667 ) / temp_output_7_0_g50667 );
				float4 appendResult235_g50667 = (float4(Final_NormalsWS91_g50667 , Final_Depth92_g50667));
				
				float Main_Metallic62_g50667 = ( tex2DNode30_g50667.r * _MainMetallicValue );
				half Second_Metallic465_g50667 = ( tex2DNode451_g50667.r * _SecondMetallicValue );
				float lerpResult505_g50667 = lerp( Main_Metallic62_g50667 , Second_Metallic465_g50667 , Detail_Mask535_g50667);
				#if defined(TVE_DETAIL_BLEND_OVERLAY)
				float staticSwitch622_g50667 = Main_Metallic62_g50667;
				#elif defined(TVE_DETAIL_BLEND_REPLACE)
				float staticSwitch622_g50667 = lerpResult505_g50667;
				#else
				float staticSwitch622_g50667 = Main_Metallic62_g50667;
				#endif
				#if defined(TVE_DETAIL_TYPE_VERTEX_BLUE)
				float staticSwitch627_g50667 = staticSwitch622_g50667;
				#elif defined(TVE_DETAIL_TYPE_PROJECTION)
				float staticSwitch627_g50667 = Main_Metallic62_g50667;
				#else
				float staticSwitch627_g50667 = staticSwitch622_g50667;
				#endif
				#if defined(TVE_DETAIL_MODE_OFF)
				float staticSwitch590_g50667 = Main_Metallic62_g50667;
				#elif defined(TVE_DETAIL_MODE_ON)
				float staticSwitch590_g50667 = staticSwitch627_g50667;
				#else
				float staticSwitch590_g50667 = Main_Metallic62_g50667;
				#endif
				half Blend_Metallic543_g50667 = staticSwitch590_g50667;
				float _IsCrossShader254_g50667 = _IsCrossShader;
				float _IsVegetationShader86_g50667 = saturate( ( _IsBarkShader78_g50667 + _IsLeafShader81_g50667 + _IsCrossShader254_g50667 + _IsGrassShader255_g50667 ) );
				float Final_VariationOrMetallic67_g50667 = ( ( Blend_Metallic543_g50667 * _IsPropShader89_g50667 ) + ( i.ase_color.r * _IsVegetationShader86_g50667 ) );
				float Final_Leaves372_g50667 = ( ( 1.0 * _IsLeafShader81_g50667 ) + ( 1.0 * _IsGrassShader255_g50667 ) + ( 0.0 * _IsBarkShader78_g50667 ) + ( 0.0 * _IsPropShader89_g50667 ) );
				half Detail_MaskTex636_g50667 = lerpResult611_g50667;
				float Final_Mask208_g50667 = ( ( Main_Mask64_g50667 * _IsLeafShader81_g50667 ) + ( i.ase_color.a * _IsGrassShader255_g50667 ) + ( 0.0 * _IsBarkShader78_g50667 ) + ( Detail_MaskTex636_g50667 * _IsPropShader89_g50667 ) );
				float Main_Smoothness65_g50667 = ( tex2DNode30_g50667.b * _MainSmoothnessValue );
				half Second_Smoothness472_g50667 = ( tex2DNode451_g50667.a * _SecondSmoothnessValue );
				float lerpResult539_g50667 = lerp( Main_Smoothness65_g50667 , Second_Smoothness472_g50667 , Detail_Mask535_g50667);
				#if defined(TVE_DETAIL_BLEND_OVERLAY)
				float staticSwitch623_g50667 = Main_Smoothness65_g50667;
				#elif defined(TVE_DETAIL_BLEND_REPLACE)
				float staticSwitch623_g50667 = lerpResult539_g50667;
				#else
				float staticSwitch623_g50667 = Main_Smoothness65_g50667;
				#endif
				#if defined(TVE_DETAIL_TYPE_VERTEX_BLUE)
				float staticSwitch629_g50667 = staticSwitch623_g50667;
				#elif defined(TVE_DETAIL_TYPE_PROJECTION)
				float staticSwitch629_g50667 = Main_Smoothness65_g50667;
				#else
				float staticSwitch629_g50667 = staticSwitch623_g50667;
				#endif
				#if defined(TVE_DETAIL_MODE_OFF)
				float staticSwitch519_g50667 = Main_Smoothness65_g50667;
				#elif defined(TVE_DETAIL_MODE_ON)
				float staticSwitch519_g50667 = staticSwitch629_g50667;
				#else
				float staticSwitch519_g50667 = Main_Smoothness65_g50667;
				#endif
				half Blend_Smoothness553_g50667 = staticSwitch519_g50667;
				float Final_Smoothness214_g50667 = Blend_Smoothness553_g50667;
				float4 appendResult236_g50667 = (float4(Final_VariationOrMetallic67_g50667 , Final_Leaves372_g50667 , Final_Mask208_g50667 , Final_Smoothness214_g50667));
				
				float2 vertexToFrag11_g50675 = i.ase_texcoord6.xy;
				half2 Emissive_UVs655_g50667 = vertexToFrag11_g50675;
				half3 Final_Emissive661_g50667 = ( (( _EmissiveColor * tex2D( _EmissiveTex, Emissive_UVs655_g50667 ) )).rgb * _HasEmissive );
				float4 appendResult672_g50667 = (float4(Final_Emissive661_g50667 , 0.0));
				
				float Albedo_Alpha90_g50667 = tex2DNode14_g50667.a;
				float Alpha_Cutoff406_g50667 = _Cutoff;
				float Render_Clip390_g50667 = _RenderClip;
				float lerpResult389_g50667 = lerp( 1.0 , ( Albedo_Alpha90_g50667 - Alpha_Cutoff406_g50667 ) , Render_Clip390_g50667);
				float Final_Clip393_g50667 = lerpResult389_g50667;
				

				outGBuffer0 = appendResult230_g50667;
				outGBuffer1 = appendResult235_g50667;
				outGBuffer2 = appendResult236_g50667;
				outGBuffer3 = appendResult672_g50667;
				outGBuffer4 = 0;
				outGBuffer5 = 0;
				outGBuffer6 = 0;
				outGBuffer7 = 0;
				float alpha = Final_Clip393_g50667;
				clip( alpha );
				outDepth = i.pos.z;
			}
			ENDCG
		}
	}
	
	
	
}
/*ASEBEGIN
Version=18909
1920;0;1920;1029;-1514.045;851.5616;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;268;2304,-512;Half;False;Property;_render_cull;_render_cull;52;1;[HideInInspector];Create;True;0;0;0;True;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;281;2304,-384;Inherit;False;Base Baker;0;;50667;fd1de57b6831b5a4a975edc31360e4cc;0;0;5;FLOAT4;0;FLOAT4;19;FLOAT4;20;FLOAT4;673;FLOAT;323
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;199;2688,-384;Float;False;True;-1;2;;100;9;Hidden/BOXOPHOBIC/The Vegetation Engine/Helpers/Impostors Baker;f53051a8190f7044fa936bd7fbe116c1;True;Unlit;0;0;Unlit;10;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;2;True;268;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;RenderType=Opaque=RenderType;True;4;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;199;0;281;0
WireConnection;199;1;281;19
WireConnection;199;2;281;20
WireConnection;199;3;281;673
WireConnection;199;8;281;323
ASEEND*/
//CHKSM=6D0CE68F07C33CA0D45A34AEFD5CA78DD8C6AC0E