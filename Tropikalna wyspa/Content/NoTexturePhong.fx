// Global-Level Semantics
float4x4 WorldMatrix : WORLD;
float4x4 ViewMatrix;
float4x4 ProjectionMatrix;
float4x4 WorldInvTransMat : WORLDINVERSETRANSPOSE;

float4 surfaceColor;
float3 ViewPosition;

float3 dirLightDir		: LIGHTDIR0_DIRECTION;
float4 dirLightColor	: LIGHTDIR0_COLOR;

float3 pointLightPos;
float4 pointLightColor;
float pointLightFalloff;
float pointLightRange;

float3 materialEmissive : EMISSIVE;
float3 materialAmbient	: AMBIENT;
float4 materialDiffuse	: DIFFUSE;
float3 materialSpecular : SPECULAR;
float  specularIntensity;
float  materialPower : SPECULARPOWER;

float4 fogColor = float4(0.17f, 0.2f, 0.22f, 1.0f);
float  fogStart = 15.0f;
float  fogEnd = 30.0f;
float fogEnabled = 1.0f;

// Vertex Shader Input Structure
struct VS_INPUT {
	float4 position : POSITION;		// Vertex position in object space
	float3 normal	: NORMAL;       // Vertex normal in object space
	//float4 color	: COLOR0;
};

// Vertex Shader Output Structure
struct VS_OUTPUT
{
	float4 position2 : POSITION;
	float4 position : TEXCOORD0;		// Pixel position in clip space	
	float3 normal	: TEXCOORD1;    // Pixel normal vector
	float3 view		: TEXCOORD2;      // Pixel view vector
	float4 worldPos : TEXCOORD3;
	float4 color	: COLOR0;
	float fogFactor : FLOAT0;
};
#define	PS_INPUT VS_OUTPUT            // What comes out of VS goes into PS!

float ComputeFogFactor(float d)
{
	return clamp((d - fogStart) / (fogEnd - fogStart), 0, 1) * fogEnabled;
}
// Vertex Shader Function
VS_OUTPUT VS(VS_INPUT IN)
{
	VS_OUTPUT OUT;
	// Basic transformation of untransformed vertex into clip-space
	float4 worldPosition = mul(IN.position, WorldMatrix);
	float4 viewPosition = mul(worldPosition, ViewMatrix);
	OUT.position = IN.position;
	OUT.position2 = mul(viewPosition, ProjectionMatrix);

	// Calculate the normal vector
	OUT.normal = mul(IN.normal, WorldInvTransMat);

	// Calculate the view vector
	OUT.view = ViewPosition - worldPosition;

	OUT.worldPos = worldPosition;

	OUT.color = surfaceColor;
	OUT.fogFactor = ComputeFogFactor(length(ViewPosition - worldPosition));

	return OUT;
}

float4 WyznaczKierunkowe(float4 col, float3 norm, float3 vi)
{
	float diffuseIntensity = saturate(dot(norm, normalize(-dirLightDir)));
	float4 diffuse = diffuseIntensity * dirLightColor * col;

	float3 dirLight = normalize(-dirLightDir);
	float3 view = normalize(vi);
	float3 normal = normalize(norm);
	float3 halfway = normalize(dirLight + view);

	float4 specular = saturate(dirLightColor * col * specularIntensity
		* pow(saturate(dot(normal, halfway)), materialPower));


	return (diffuse + specular);
}

float4 WyznaczPunktowe(float4 col, float3 norm, float3 vi, float4 pos)
{
	float3 lightVector = pointLightPos - pos;
	float lightDist = length(lightVector);
	float3 directionToLight = normalize(-lightVector);
	float3 view = normalize(vi);
	float3 normal = normalize(norm);

	float baseIntensity = pow(saturate((pointLightRange - lightDist) / pointLightRange), pointLightFalloff);
	float diffuseIntensity = saturate(dot(norm, -directionToLight));
	float4 diffuse = diffuseIntensity * pointLightColor * col;
	float3 halfway = normalize(-directionToLight + view);
	float4 specular = saturate(pointLightColor * col * specularIntensity
		* pow(saturate(dot(normal, halfway)), materialPower));

	return baseIntensity * (diffuse + specular);
}

float4 WyznaczPunktowe2(float4 col, float3 norm, float3 vi, float4 pos)
{
	float3 n = normalize(norm);
	float3 s = normalize(pointLightPos - pos);
	float3 v = normalize(-pos);
	float3 r = reflect(-s, n);

	float sDotN = max(dot(s, n), 0.0);
	float4 diffuse = col * materialDiffuse * sDotN;

	return diffuse;
}

// Pixel Shader Function
float4 PS(PS_INPUT IN) : COLOR{

	float4 kolorKier = WyznaczKierunkowe(IN.color, IN.normal, IN.view);

	float4 kolorPoint = WyznaczPunktowe(IN.color, IN.normal, IN.view, IN.worldPos);
	float4 kolorEmiss = float4(materialEmissive, 1.0f);
	float4 kolor = (kolorKier + kolorPoint+ kolorEmiss)*(1 - IN.fogFactor) + IN.fogFactor * fogColor;
	kolor.a = 1.0f;// -IN.fogFactor;
	return kolor;
}

technique NoTex
{
	pass P
	{
		AlphaBlendEnable = TRUE;
		DestBlend = INVSRCALPHA;
		SrcBlend = SRCALPHA;
		VertexShader = compile vs_4_0_level_9_1 VS();
		PixelShader = compile ps_4_0_level_9_1 PS();
	}
}