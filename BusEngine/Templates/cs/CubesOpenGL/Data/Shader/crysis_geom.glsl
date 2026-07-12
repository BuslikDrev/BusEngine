#version 410 core
#extension GL_ARB_geometry_shader4 : require
#extension GL_ARB_gpu_shader5 : require
//#extension GL_ARB_arrays_of_arrays : require
//#extension GL_EXT_gpu_shader4 : require
//#extension GL_EXT_geometry_shader4 : require
//#pragma optionNV unroll all
#define BUSENGINE_COPY_INT

//https://habr.com/ru/articles/533932/
//https://www.khronos.org/opengl/wiki/Built-in_Variable_(GLSL)
//https://www.khronos.org/opengl/wiki/Geometry_Shader
layout (TRIANGLES) in;
layout (TRIANGLE_STRIP, max_vertices = BUSENGINE_COPY_INT) out;
//layout (location = 5) in vec3 Pozitions[];

/* layout(std430) buffer Buffer1
{
    vec3[][2] multidim; // legal
}; */

uniform float Pozitions[BUSENGINE_COPY_INT * 2];
uniform float Distance;
uniform mat4 View;
uniform mat4 Projection;

/* out gl_PerVertex {
  vec4 gl_Position;
  float gl_PointSize;
  float gl_ClipDistance[];
  float gl_CullDistance[];
};

in gl_PerVertex {
  vec4 gl_Position;
  float gl_PointSize;
  float gl_ClipDistance[];
} gl_in[]; */

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

vec4 pos, p1 = gl_in[0].gl_Position, p2 = gl_in[1].gl_Position, p3 = gl_in[2].gl_Position;
//float d1 = gl_in[0].gl_ClipDistance[0], d2 = gl_in[1].gl_ClipDistance[0], d3 = gl_in[2].gl_ClipDistance[0];
mat4 vp = View * Projection;

void main(void) {
	//float d1 = gl_in[0].gl_ClipDistance[2], d2 = gl_in[0].gl_ClipDistance[0], d3 = gl_in[0].gl_ClipDistance[0];
	//float dist, f1;

		//dist = dot((pos + p1), View[0]);
		//f1 = dot(p1, Projection[0].xyzw);
		//dist = distance(p1.xyw, vec3(View[0].x, View[1].y, View[2].z));
//if (gl_in[0].gl_ClipDistance[0] > 0.0f && (gl_PrimitiveIDIn % 1) == 0) {
//if (dot(p1.xyzw, View[3].xyzw) >= 0.0f && dot(p2.xyzw, View[3].xyzw) >= 0.0f && dot(p3.xyzw, View[3].xyzw) >= 0.0f) {
	for (int i = 0; i < Pozitions.length(); i++) {
		pos = vec4(Pozitions[i++], Pozitions[i++], Pozitions[i++], 0.0F) * vp;
		//mat3 rot = mat3(vec3(cos(radians(Pozitions[i])), sin(radians(Pozitions[i])), 0), vec3(-sin(radians(Pozitions[i])), cos(radians(Pozitions[i])), 0), vec3(0, 0, 1));
		//pos = vec4(pos.xyz * rot, pos.w) * (View * Projection);
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
//}
}