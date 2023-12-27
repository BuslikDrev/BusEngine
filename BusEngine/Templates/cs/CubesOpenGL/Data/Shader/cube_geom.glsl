#version 330 core
#extension ARB_geometry_shader4 : enable

//https://www.khronos.org/opengl/wiki/Built-in_Variable_(GLSL)
//https://www.khronos.org/opengl/wiki/Geometry_Shader
layout (TRIANGLES) in;
layout (TRIANGLE_STRIP, max_vertices = 128) out;

in vec4 gColors[];
out vec4 fColors; 

uniform mat4 VP;

int i, x = 0, y = 0;
vec4 pos = vec4(0.0F, 0.0F, 0.0F, 0.0F);

void main() {
	vec4 t = gl_in[0].gl_Position;
	vec4 h = gl_in[1].gl_Position;
	vec4 n = gl_in[2].gl_Position;

	fColors = gColors[2];

	for (i = 0; i < 16; i++) {
		pos.x = 3.0F * x;
		pos.y = 3.0F * y;

		gl_Position = pos * VP + t;
		//fColors = gColors[0];
		EmitVertex();

		gl_Position = pos * VP + h;
		//fColors = gColors[1];
		EmitVertex();

		gl_Position = pos * VP + n;
		//fColors = gColors[2];
		EmitVertex();

		EndPrimitive();

		if (y == 3) {
			x++;
			y = 0;
		} else {
			y++;
		}
	}
}