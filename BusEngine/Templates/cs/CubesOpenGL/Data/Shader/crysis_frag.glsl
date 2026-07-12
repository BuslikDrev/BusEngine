#version 330 core
//#extension GL_ARB_conservative_depth : enable
//#extension GL_EXT_conservative_depth : enable
#extension GL_ARB_separate_shader_objects : require

//layout (depth_greater) out float gl_FragDepth;
layout (location = 0) out vec4 Colors;
//out vec4 Colors;

uniform sampler2D Texture0; // ambient map_Ka
uniform sampler2D Texture1; // diffuse map_Kd
uniform sampler2D Texture2; // specular map_Ks
uniform sampler2D Texture3; // emissive map_Ke
uniform sampler2D Texture4; // specular highlight map_Ns
uniform sampler2D Texture5; // map_Pr
uniform sampler2D Texture6; // map_Pm
uniform sampler2D Texture7; // map_d
uniform sampler2D Texture8; // normal map_Kn map_bump map_Bump bump norm

uniform int TexturesStatus[9];
uniform mat4 Model;
uniform mat4 View;
uniform mat4 Projection;
uniform vec3 LightPos;
uniform vec4 ColorDefault0 = vec4(1.0f, 1.0f, 1.0f, 1.0f); // Ka
uniform vec4 ColorDefault1 = vec4(1.0f, 1.0f, 1.0f, 1.0f); // Kd
uniform vec4 ColorDefault2 = vec4(1.0f, 0.0f, 0.0f, 1.0f); // Ks
uniform vec4 ColorDefault3 = vec4(1.0f, 0.0f, 0.0f, 1.0f); // Ke

in Material {
	vec3 VertexData;
	vec2 TexData;
	vec3 NormData;
	vec3 FragPos;
} in_material;


/* uniform float exposure = 2.0;

vec3 Tonemap_Heable(vec3 x) {
    float A = 0.15;
    float B = 0.50;
    float C = 0.10;
    float D = 0.20;
    float E = 0.02;
    float F = 0.30;

    return ((x*(A*x+C*B)+D*E)/(x*(A*x+B)+D*F))-E/F;
}

void main(void) {
	vec3 hdrColor;

	if (TexturesStatus[1] != 0) {
		hdrColor = texture(Texture1, in_material.TexData).rgb;
	} else {
		hdrColor = ColorDefault1.rgb;
	}

    vec3 curr = Tonemap_Heable(hdrColor * exposure);
    vec3 whiteScale = 1.0 / Tonemap_Heable(vec3(11.2));
    vec3 color = curr * whiteScale;
    color = pow(color, vec3(1.0 / 2.2));
    Colors = vec4(color, 1.0);
} */









//https://godotshaders.com/shader/transparent-walls/
//https://github.com/GTruf/SolarSystem-3D
//https://learnopengl.com/Advanced-Lighting/Normal-Mapping



// --- SETTINGS ---
// 0 - Light OFF, 1 - Light ON
uniform int LightStatus = 1;
uniform float LightPower = 8000000000.0; 
float LightSoftness = 500000.0; 
float AmbientBrightness = 0.005;

// TBN calculation for Normal Mapping
vec3 getNormalFromMap() {
    vec3 tangentNormal = texture(Texture2, in_material.TexData).xyz * 2.0 - 1.0;

    vec3 Q1  = dFdx(in_material.FragPos);
    vec3 Q2  = dFdy(in_material.FragPos);
    vec2 st1 = dFdx(in_material.TexData);
    vec2 st2 = dFdy(in_material.TexData);

    vec3 N   = normalize(in_material.NormData);
    vec3 T   = normalize(Q1*st2.t - Q2*st1.t);
    vec3 B   = -normalize(cross(N, T));
    mat3 TBN = mat3(T, B, N);

    return normalize(TBN * tangentNormal);
}

void main(void) {
	vec3 baseColor = (TexturesStatus[1] != 0) ? texture(Texture1, in_material.TexData).rgb : ColorDefault1.rgb;

	if (LightStatus == 0) {
		Colors = vec4(baseColor * AmbientBrightness, 1.0);
		return;
	}

	// 1. SELECT NORMAL
	vec3 normal;
	if (TexturesStatus[2] != 0) {
		normal = getNormalFromMap();
	} else {
		normal = normalize(in_material.NormData);
	}

	vec3 lightDir = normalize(LightPos - in_material.FragPos);
	vec3 viewDir = normalize(ViewPos - in_material.FragPos);

	// 2. DISTANCE CALCULATION (Using your logic)
	float dist = length(LightPos - in_material.FragPos);
	float attenuation = LightPower / (dist * dist + LightSoftness);
	attenuation = clamp(attenuation, 0.0, 2.0); // Keep it within sane limits

	// 3. DIFFUSE
	float diff = max(dot(normal, lightDir), 0.0);
	vec3 diffuse = diff * baseColor * attenuation;
	
	// 4. SPECULAR
	vec3 reflectDir = reflect(-lightDir, normal);
	float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32.0);
	vec3 finalSpecular = vec3(0.3) * spec * attenuation;

	// 5. RIM LIGHT (Atmosphere)
	float fresnel = pow(1.0 - max(dot(normalize(in_material.NormData), viewDir), 0.0), 4.0);
	vec3 atmosphereColor = vec3(0.3, 0.5, 1.0);
	vec3 rimLight = fresnel * atmosphereColor * diff * attenuation;

	// 6. FINAL COMBINE
	vec3 ambient = baseColor * AmbientBrightness;
	vec3 result = ambient + diffuse + finalSpecular + rimLight;

	if (TexturesStatus[3] != 0) {
		result += texture(Texture3, in_material.TexData).rgb;
	}

	Colors = vec4(pow(result, vec3(1.0 / 2.2)), 1.0);
}