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
} out_material;

void main(void) {
	out_material.VertexData = VertexData;
	out_material.TexData = TexData;
	out_material.NormData = NormData;

	gl_Position = Projection * View * Model * vec4(VertexData, 1.0F);
}