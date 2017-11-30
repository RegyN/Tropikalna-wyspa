float4x4 World;
float4x4 View;
float4x4 Projection;
float4x4 WorldInverseTranspose;

float4 TintColor = float4(1, 1, 1, 1);
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

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float4 Normal	: NORMAL0;
	float3 ViewDirection : VIEWDIRECTION0;
	float3 Reflection : TEXCOORD0;
	float fogFactor : FLOAT0;
};

float ComputeFogFactor(float d)
{
	return clamp((d - fogStart) / (fogEnd - fogStart), 0, 1) * fogEnabled;
}

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);

	float3 viewDirection = CameraPosition - worldPosition;
	output.ViewDirection = viewDirection;

	float4 normal = normalize(input.Normal);//normalize(mul(input.Normal, WorldInverseTranspose)); 
	output.Normal = normal;
	output.Reflection = reflect(-normalize(viewDirection), normalize(normal));

	output.fogFactor = ComputeFogFactor(length(viewPosition - worldPosition));

	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float3 Reflection = reflect(-normalize(input.ViewDirection), normalize(input.Normal));
	float4 kolor = texCUBE(SkyboxSampler, normalize(input.Reflection));
	return kolor*(1-input.fogFactor) + fogColor*input.fogFactor;
}

technique Reflection
{
	pass Pass1
	{
		VertexShader = compile vs_4_0_level_9_1 VertexShaderFunction();
		PixelShader = compile ps_4_0_level_9_1 PixelShaderFunction();
	}
}
