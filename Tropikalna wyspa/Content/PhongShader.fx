#define VS_SHADERMODEL vs_4_0_level_9_3
#define PS_SHADERMODEL ps_4_0_level_9_3
#define LIGHTS_COUNT 2

matrix WorldMatrix;
matrix ViewMatrix;
matrix ProjectionMatrix;
// Light info
float3 LightPosition[LIGHTS_COUNT];
float3 La[LIGHTS_COUNT];		//Ambient light intensity
float3 Ld[LIGHTS_COUNT];		//Diffuse light intensity
float3 Ls[LIGHTS_COUNT];		//Specular light intensity



								// Material info
float3 Ka;			//Ambient reflectivity
float3 Kd;			//Diffuse reflectivity
float3 Ks;			//Specular reflectivity
float Shininess;	//Specular shininess factor


struct VertexShaderInput
{
	float4 Position : POSITION0;
	float4 Color : COLOR0;
	float3 Normal : NORMAL0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float4 Color : COLOR0;
	float3 Normal : NORMAL0;
	float3 WorldPos : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	float4 worldPosition = mul(input.Position, WorldMatrix);
	float4 viewPosition = mul(worldPosition, ViewMatrix);
	output.Position = mul(viewPosition, ProjectionMatrix);
	output.Color = input.Color;
	output.Normal = input.Normal;
	output.WorldPos = input.Position;

	return output;
}

void processLight(int i, float3 pos, float3 norm, out float3 ambient, out float3 diffuse, out float3 spec)
{
	float3 n = normalize(norm);
	float3 s = normalize(LightPosition[i] - pos);
	float3 v = normalize(-pos);
	float3 r = reflect(-s, n);

	ambient = La[i] * Ka;
	float sDotN = max(dot(s, n), 0.0);
	diffuse = Ld[i] * Kd * sDotN;
	spec = Ls[i] * Ks * pow(max(dot(r, v), 0.0), Shininess);
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float3 ambientSum = (float3)0;
	float3 diffuseSum = (float3)0;
	float3 specSum = (float3)0;
	// output params
	float3 ambient, diffuse, spec;
	for (int i = 0; i<LIGHTS_COUNT; i++)
	{
		processLight(i, input.WorldPos, input.Normal, ambient, diffuse, spec);
		ambientSum += ambient;
		diffuseSum += diffuse;
		specSum += spec;
	}
	ambientSum /= LIGHTS_COUNT;
	return float4(ambientSum + diffuseSum, 1) * input.Color + float4(specSum, 1);
	//return input.Color;
	//return float4(1,0,0,1);
}

technique NoTextureTeq
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL VertexShaderFunction();
		PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
	}
};