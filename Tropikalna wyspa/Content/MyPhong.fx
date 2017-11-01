// Global-Level Semantics
float4x4 WorldMatrix : WORLD;
float4x4 ViewMatrix;
float4x4 ProjectionMatrix;
float4x4 WorldInvTransMat : WORLDINVERSETRANSPOSE;

float4 surfaceColor;
float3 ViewPosition;
float3 dirLightDir		: LIGHTDIR0_DIRECTION;
float3 dirLightColor	: LIGHTDIR0_COLOR;
float3 materialEmissive : EMISSIVE;
float3 materialAmbient	: AMBIENT;
float4 materialDiffuse	: DIFFUSE;
float3 materialSpecular : SPECULAR;
float  materialPower	: SPECULARPOWER;

// Vertex Shader Input Structure
struct VS_INPUT {
	float4 position : POSITION;		// Vertex position in object space
	float3 normal	: NORMAL;       // Vertex normal in object space
	float4 color	: COLOR0;		
};

// Vertex Shader Output Structure
struct VS_OUTPUT 
{
	float4 position : POSITION;		// Pixel position in clip space	
	float3 normal	: TEXCOORD0;    // Pixel normal vector
	float3 view		: TEXCOORD1;      // Pixel view vector
	float4 color	: COLOR0;
};
#define	PS_INPUT VS_OUTPUT            // What comes out of VS goes into PS!

// Vertex Shader Function
VS_OUTPUT VS(VS_INPUT IN) 
{
	VS_OUTPUT OUT;
	// Basic transformation of untransformed vertex into clip-space
	float4 worldPosition = mul(IN.position, WorldMatrix);
	float4 viewPosition = mul(worldPosition, ViewMatrix);
	OUT.position = mul(viewPosition, ProjectionMatrix);

	// Calculate the normal vector
	OUT.normal = mul(WorldInvTransMat, IN.normal);

	// Calculate the view vector
	float3 worldPos = mul(IN.position, WorldMatrix).xyz;
	OUT.view = ViewPosition - worldPos;

	OUT.color = IN.color;

	return OUT;
}

// Pixel Shader Function
float4 PS(PS_INPUT IN) : COLOR{
	// Normalize all vectors in pixel shader to get phong shading
	// Normalizing in vertex shader would provide gouraud shading
	float3 light = normalize(-dirLightDir);
	float3 view = normalize(IN.view);
	float3 normal = normalize(IN.normal);

	// Calculate the half vector
	float3 halfway = normalize(light + view);

	// Calculate the emissive lighting
	float3 emissive = materialEmissive;
	// Calculate the ambient reflection
	float3 ambient = materialAmbient;

	// Calculate the diffuse reflection
	float3 diffuse = saturate(dot(normal, light)) * materialDiffuse.rgb;

	// Calculate the specular reflection
	float3 specular = pow(saturate(dot(normal, halfway)), materialPower) * materialSpecular;

	// Sample the texture

	// Combine all the color components
	float3 pixColor = (saturate(ambient + diffuse) * IN.color + specular) * dirLightColor + emissive;
	// Calculate the transparency
	float alpha = materialDiffuse.a * surfaceColor.a;
	// Return the pixel's color
	return float4(pixColor, alpha);
}

technique TSM3 
{
	pass P 
	{
		VertexShader = compile vs_4_0_level_9_1 VS();
		PixelShader = compile ps_4_0_level_9_1 PS();
	}
}

