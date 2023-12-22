#version 330 core

layout(location = 0) in vec4 p;
layout(location = 1) in vec4 c;

out vec4 gColors;

uniform mat4 A;
uniform mat4 P;
uniform mat4 VP;

void main() {
	gl_Position = p * (A * P * VP);
	gColors = c;
}