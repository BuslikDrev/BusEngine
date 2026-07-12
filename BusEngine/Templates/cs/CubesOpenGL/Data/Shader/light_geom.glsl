#version 330 core
#define BUSENGINE_COPY_INT

layout (TRIANGLES) in;
layout (TRIANGLE_STRIP, max_vertices = BUSENGINE_COPY_INT) out;

uniform float Pozitions[BUSENGINE_COPY_INT * 2];
uniform mat4 View;
uniform mat4 Projection;
uniform vec3 LightPos;

in Material {
    vec3 VertexData;
    vec2 TexData;
    vec3 NormData;
} in_material[];

out Material {
    vec3 VertexData;
    vec2 TexData;
    vec3 NormData;
} out_material;

vec4 pos, p1 = gl_in[0].gl_Position, p2 = gl_in[1].gl_Position, p3 = gl_in[2].gl_Position;
mat4 vp = View * Projection;
int i, l = Pozitions.length();

void main() {
	for (i = 0; i < l; i++) {
		pos = vec4(Pozitions[i++] + LightPos.x, Pozitions[i++] + LightPos.y, Pozitions[i++] + LightPos.z, 0.0F) * vp;
		i++;
		i++;

		out_material.TexData = in_material[0].TexData;
		gl_Position = pos + p1;
		EmitVertex();

		out_material.TexData = in_material[1].TexData;
		gl_Position = pos + p2;
		EmitVertex();

		out_material.TexData = in_material[2].TexData;
		gl_Position = pos + p3;
		EmitVertex();

		EndPrimitive();
	}
}