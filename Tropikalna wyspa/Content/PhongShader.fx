float4x4 WorldMatrix : WORLD;
float4x4 ViewMatrix;
float4x4 ProjectionMatrix;

float4 AmbienceColor;

// For Diffuse Lightning
float4x4 WorldInvTransMat;
float3 PointLightPosition;
float4 PointLightColor;
float3 DiffuseLightDirection;
float4 DiffuseColor;

struct VertexShaderInput
{
	float4 Position : POSITION0;
	// For Diffuse Lightning
	float4 NormalVector : NORMAL0;
	//float4 Color : COLOR0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	// For Diffuse Lightning
	float4 VertexColor : COLOR0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	float4 worldPosition = mul(input.Position, WorldMatrix);
	float4 viewPosition = mul(worldPosition, ViewMatrix);
	output.Position = mul(viewPosition, ProjectionMatrix);

	// For Diffuse Lightning
	float4 normal = normalize(mul(input.NormalVector, WorldInvTransMat));
	float lightIntensity = dot(normal, DiffuseLightDirection);
	output.VertexColor = saturate(DiffuseColor * lightIntensity);

	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	return saturate(input.VertexColor + AmbienceColor);
}

technique Diffuse
{
	pass Pass1
	{
		VertexShader = compile vs_4_0_level_9_1 VertexShaderFunction();
		PixelShader = compile ps_4_0_level_9_1 PixelShaderFunction();
	}
}
