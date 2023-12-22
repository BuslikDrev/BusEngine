#version 330 core

//https://www.khronos.org/opengl/wiki/Built-in_Variable_(GLSL)
//https://www.khronos.org/opengl/wiki/Geometry_Shader
layout (TRIANGLES) in;
layout (TRIANGLE_STRIP, max_vertices = 75) out;

in vec4 gColors[];
out vec4 fColors; 

uniform mat4 VP;

void main() {
	int x, y;
	float right = 3.0F, top = 3.0F;
	vec4 pos = vec4(0.0F, 0.0F, 0.0F, 0.0F);

	for (x = 0; x < 5; x++)  {
		for (y = 0; y < 5; y++)  {
			pos.x = right * x;
			pos.y = top * y;

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
		}
	}
}