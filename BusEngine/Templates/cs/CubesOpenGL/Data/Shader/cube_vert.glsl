#version 330 core

layout(location = 0) in vec3 p;
layout(location = 1) in vec4 c;

out vec4 b;
uniform mat4 MVP;

void main() {
	gl_Position = vec4(p, 1) * MVP;
	b = c;
}