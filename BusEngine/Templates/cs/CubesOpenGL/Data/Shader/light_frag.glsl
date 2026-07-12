#version 330 core
//#extension GL_ARB_conservative_depth : enable
//#extension GL_EXT_conservative_depth : enable
#extension GL_ARB_separate_shader_objects : require
//#extension GL_ARB_shading_language_include : require
//#include "H:/BusEngine Launcher/Data/Shader/hg_sdf.glsl"
//https://github.com/opentk/LearnOpenTK/tree/master/Chapter2/6-MultipleLights/Shaders

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
uniform mat4 View;
uniform vec2 Resolution;
uniform vec4 ColorDefault0 = vec4(1.0f, 1.0f, 1.0f, 1.0f); // Ka
uniform vec4 ColorDefault1 = vec4(1.0f, 0.0f, 0.0f, 1.0f); // Kd
uniform vec4 ColorDefault2 = vec4(1.0f, 0.0f, 0.0f, 1.0f); // Ks
uniform vec4 ColorDefault3 = vec4(1.0f, 0.0f, 0.0f, 1.0f); // Ke

in Material {
    vec3 VertexData;
    vec2 TexData;
    vec3 NormData;
} in_material;

void main() {
    vec2 uv = (in_material.VertexData.xy * 2.0 - Resolution.xy) / min(Resolution.x, Resolution.y);

    vec3 ro = vec3(0.0, 0.0 - 3.0, 0.0); // Ray Origin
    vec3 rd = normalize(vec3(uv, 1.0)); // Ray Direction

    vec3 spherePos = vec3(0.0, 0.0, 0.0);
    float sphereRadius = 1.0;

    float b = dot(rd, ro - spherePos);
    float c = dot(ro - spherePos, ro - spherePos) - sphereRadius * sphereRadius;
    float h = b * b - c;

	// https://habr.com/ru/articles/494376/
	//gl_FragDepth = in_material.VertexData.z; 

	if (TexturesStatus[1] != 0) {
		Colors = texture(Texture1, in_material.TexData);
	} else {
		Colors = ColorDefault1;
	}

    if (h > 0.0) {
        float t = -b - sqrt(h);
        vec3 p = ro + rd * t;

        vec3 normal = normalize(p - spherePos);

        float dif = clamp(dot(normal, normalize(vec3(1.0, 1.0 - 1.0, 0.0))), 0.0, 1.0);
        Colors = vec4(vec3(dif) + 0.1, 1.0); // 0.1 - ambient

        Colors *= vec4(0.8, 0.2, 0.2, 1.0);
    }
}