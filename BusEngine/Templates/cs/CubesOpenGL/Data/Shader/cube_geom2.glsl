#version 330 core
#define BUSENGINE_COPY_INT
#extension GL_EXT_geometry_shader4 : enable 

layout (TRIANGLES) in;
layout (TRIANGLE_STRIP, max_vertices = BUSENGINE_COPY_INT) out;

uniform float Pozitions[BUSENGINE_COPY_INT * 2];
uniform float Distance;
uniform mat4 View;
uniform mat4 Projection;

in Material {
    vec3 VertexData;
    vec2 TexData;
    vec3 NormData;
	vec3 FragPos;
} in_material[];

out Material {
    vec3 VertexData;
    vec2 TexData;
    vec3 NormData;
	vec3 FragPos;
} out_material;

void main(void) {
	vec4 pos, p1 = gl_in[0].gl_Position, p2 = gl_in[1].gl_Position, p3 = gl_in[2].gl_Position;
	mat4 VP = View * Projection;
	int i = 0, l = BUSENGINE_COPY_INT * 2;

	for (; i < l;) {
		pos = vec4(Pozitions[i++], Pozitions[i++], Pozitions[i++], 1.0F) * VP;
		i += 3;

		gl_Position = p1 + pos;
		EmitVertex();

		gl_Position = p2 + pos;
		EmitVertex();

		gl_Position = p3 + pos;
		EmitVertex();

		EndPrimitive();
	}
}