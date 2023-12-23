#version 330 core

//https://www.khronos.org/opengl/wiki/Built-in_Variable_(GLSL)
//https://www.khronos.org/opengl/wiki/Geometry_Shader
layout (TRIANGLES) in;
layout (TRIANGLE_STRIP, max_vertices = 48) out;

in vec4 gColors[];
out vec4 fColors; 

uniform mat4 VP;

int i, x = 0, y = 0;
vec4 pos = vec4(0.0F, 0.0F, 0.0F, 0.0F);

void main() {
	for (i = 0; i < 16; i++) {
		pos.x = 3.0F * x;
		pos.y = 3.0F * y;

		gl_Position = gl_in[0].gl_Position + pos * VP;
		fColors = gColors[0];
		EmitVertex();

		gl_Position = gl_in[1].gl_Position + pos * VP;
		fColors = gColors[1];
		EmitVertex();

		gl_Position = gl_in[2].gl_Position + pos * VP;
		fColors = gColors[2];
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