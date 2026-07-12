#version 410 core
//#extension GL_ARB_tessellation_shader : require
//#extension GL_ARB_enhanced_layouts : enable
//https://learnopengl.com/Guest-Articles/2021/Tessellation/Tessellation
//https://habr.com/ru/articles/314532/
//https://gamedev.ru/community/ogl/articles/gl4_tesselation
//#define id gl_InvocationID

layout (vertices = 4) out;

in Material {
    vec3 VertexData;
    vec2 TexData;
    vec3 NormData;
	vec3 FragPos;
} in_material[gl_MaxPatchVertices];

out Material {
    vec3 VertexData;
    vec2 TexData;
    vec3 NormData;
	vec3 FragPos;
} out_material[];

in gl_PerVertex {
  vec4 gl_Position;
  float gl_PointSize;
  float gl_ClipDistance[];
  float gl_CullDistance[];
} gl_in[gl_MaxPatchVertices];

out gl_PerVertex {
  vec4 gl_Position;
  float gl_PointSize;
  float gl_ClipDistance[];
  float gl_CullDistance[];
} gl_out[];

int lod() {
    float dist = distance(gl_in[gl_InvocationID].gl_Position.xyz, vec3(0.0f));
    if(dist < 10.0f) {
        return 48;
    }
    if(dist < 20.0f) {
        return 24;
    }
    if(dist < 80.0f) {
        return 12;
    }
    if(dist < 800.0f) {
        return 6;
    }
    return 48;
}

void main(void) {
	gl_out[gl_InvocationID].gl_Position = gl_in[gl_InvocationID].gl_Position;
	out_material[gl_InvocationID].VertexData = in_material[gl_InvocationID].VertexData;
	out_material[gl_InvocationID].TexData = in_material[gl_InvocationID].TexData;
	out_material[gl_InvocationID].NormData = in_material[gl_InvocationID].NormData;
	out_material[gl_InvocationID].FragPos = in_material[gl_InvocationID].FragPos;

	gl_TessLevelOuter[0] = gl_in[gl_InvocationID].gl_Position.w;
	float halfspaceCull = step(dot(vec3(0.0f, 0.0f, -10.0f) - gl_in[gl_InvocationID].gl_Position.xyz, vec3(0.0f, 0.0f, 0.0f)), 0);
	gl_TessLevelOuter[1] = lod() * halfspaceCull;

    //if (gl_InvocationID == 0) {
        gl_TessLevelOuter[0] = 16;
        gl_TessLevelOuter[1] = 16;
        gl_TessLevelOuter[2] = 16;
        gl_TessLevelOuter[3] = 16;

        gl_TessLevelInner[0] = 16;
        gl_TessLevelInner[1] = 16;
    //}
}