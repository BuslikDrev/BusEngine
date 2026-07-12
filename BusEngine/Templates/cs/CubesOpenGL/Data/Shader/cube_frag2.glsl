#version 330 core
//#extension GL_ARB_conservative_depth : enable
//#extension GL_EXT_conservative_depth : enable
//#extension GL_ARB_separate_shader_objects : require

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
uniform vec3 ViewPos;
uniform vec3 LightPos;
uniform vec2 Resolution;
uniform vec4 ColorDefault0 = vec4(1.0f, 0.0f, 0.0f, 1.0f); // Ka
uniform vec4 ColorDefault1 = vec4(1.0f, 0.0f, 0.0f, 1.0f); // Kd
uniform vec4 ColorDefault2 = vec4(1.0f, 0.0f, 0.0f, 1.0f); // Ks
uniform vec4 ColorDefault3 = vec4(1.0f, 0.0f, 0.0f, 1.0f); // Ke

in Material {
    vec3 VertexData;
    vec2 TexData;
    vec3 NormData;
    vec3 FragPos;
} in_material;

//gl_FragCoord = vec4(0, 0, 0, 0);
//gl_FrontFacing = bool;
//https://stackoverflow.com/questions/13711252/what-does-gl-fragcoord-z-gl-fragcoord-w-represent
//gl_FragCoord.z / gl_FragCoord.w = rastoyanine pixela do object

float near = 0.1;
float far  = 100.0;

float LinearizeDepth(float depth) {
    float z = depth * 2.0 - 1.0; // back to NDC

    return (2.0 * near * far) / (far + near - z * (far - near));	
}



void main(void) {
	if (gl_FrontFacing) {
		float depth = LinearizeDepth(gl_FragCoord.z) / far;
		float dist = 100 / distance(ViewPos, in_material.FragPos) / (gl_FragCoord.z * 10);

		if (TexturesStatus[1] != 0) {
			Colors = vec4(texture(Texture1, in_material.TexData).rgb, 1.0F);
		} else {
			if (gl_FragCoord.x > (Resolution.x * 0.5 - 200) && gl_FragCoord.x < (Resolution.x * 0.5 + 200) && gl_FragCoord.y > (Resolution.y * 0.5 - 100) && gl_FragCoord.y < (Resolution.y * 0.5 + 100)) {
				if (gl_FragCoord.z / gl_FragCoord.w > 30) {
					vec4 depthVec4 = vec4(vec3(pow(depth, 1.4)), 1.0);
					Colors = vec4(vec3(depth), 1.0f) * (1 - depthVec4) + depthVec4;
				} else {
					Colors = vec4(0.0f, 0.0f, gl_FragCoord.z, 1.0f);
				}
			} else {
				if (in_material.TexData.x > 0.1f) {
					Colors = vec4(0.0f, 1.0f, 0.0f, 1.0f);
				} else {
					Colors = vec4(gl_FragCoord.z, 0.0f, 0.0f, 1.0f);
				}
			}

			vec4 depthVec4 = vec4(vec3(pow(depth, 1.4)), 1.0);
			Colors = vec4(dist, dist, dist, 1.0f);
		}
	}
	Colors = vec4(1.0f, 1.0f, 1.0f, 1.0f);
}