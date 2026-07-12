#version 330 core
#extension GL_EXT_tessellation_shader : enable

layout (location = 0) in vec3 VertexData;
layout (location = 1) in vec2 TexData;
layout (location = 2) in vec3 NormData;

uniform mat4 Model;
uniform mat4 View;
uniform mat4 Projection;

out Material {
    vec3 VertexData;
    vec2 TexData;
    vec3 NormData;
    vec3 FragPos;
} out_material;

//https://github.com/john-chapman/im3d/blob/master/examples/OpenGL33/im3d.glsl
//https://x.com/SebAaltonen/status/1322594445548802050
void main(void) {
	out_material.VertexData = VertexData;
	out_material.TexData = TexData;
	out_material.FragPos = (Model * vec4(VertexData, 1.0F)).xyz;

    mat3 normalMatrix = transpose(inverse(mat3(Model)));
    out_material.NormData = normalize(normalMatrix * NormData);

	gl_Position =  Projection * View * Model * vec4(VertexData, 1.0F);
	//gl_ClipDistance[0] = dot(vec4(Projection[2].x, Projection[2].y, Projection[2].z, Projection[2].w), vec4(VertexData, 1.0F) * Model); 
}