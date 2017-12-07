float4x4 World;
float4x4 View;
float4x4 Projection;
float4x4 WorldInverseTranspose;

float3 CameraPosition;

float4 fogColor = float4(0.17f, 0.2f, 0.22f, 1.0f);
float  fogStart = 30.0f;
float  fogEnd = 50.0f;
float fogEnabled = 1.0f;

Texture SkyboxTexture;
samplerCUBE SkyboxSampler = sampler_state
{
	texture = <SkyboxTexture>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
	AddressU = Mirror;
	AddressV = Mirror;
};

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float4 Normal : NORMAL0;
};

struct VertexShaderOutputEnvMapping
{
	float4 Position : POSITION0;
	float3 Reflection: TEXCOORD0;
	float fogFactor : FLOAT0;
};

float ComputeFogFactor(float d)
{
	return clamp((d - fogStart) / (fogEnd - fogStart), 0, 1) * fogEnabled;
}

VertexShaderOutputEnvMapping VertexShaderEnvMapping(VertexShaderInput input)
{
	VertexShaderOutputEnvMapping output;
	float3 Normal = mul(normalize(input.Normal), WorldInverseTranspose);
	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);
	float3 ViewDir = normalize(worldPosition - CameraPosition);
	output.Reflection = reflect(ViewDir, Normal);
	output.fogFactor = ComputeFogFactor(length(viewPosition - worldPosition));

	return output;
}

float4 PixelShaderEnvMapping(VertexShaderOutputEnvMapping input) : COLOR0
{
	return texCUBE(SkyboxSampler, normalize(input.Reflection))*(1 - input.fogFactor) + fogColor*input.fogFactor;
}

technique Reflection
{
	pass Pass1
	{
		VertexShader = compile vs_4_0_level_9_1 VertexShaderEnvMapping();
		PixelShader = compile ps_4_0_level_9_1 PixelShaderEnvMapping();
	}
}
