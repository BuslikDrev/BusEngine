#version 410 core
//#extension GL_ARB_tessellation_shader : require
//#extension GL_ARB_enhanced_layouts : enable

//https://learnopengl.com/Guest-Articles/2021/Tessellation/Tessellation
//https://habr.com/ru/articles/314532/
//https://gamedev.ru/community/ogl/articles/gl4_tesselation
//layout(triangles, equal_spacing) in;
layout (triangles, equal_spacing, cw) in;
//equal_spacing
//fractional_odd_spacing
//fractional_even_spacing

uniform sampler2D Texture2;
uniform mat4 Model;
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

in gl_PerVertex {
  vec4 gl_Position;
  float gl_PointSize;
  float gl_ClipDistance[];
  //float gl_CullDistance[];
} gl_in[];

/* out gl_PerVertex {
  vec4 gl_Position;
  float gl_PointSize;
  float gl_ClipDistance[];
  float gl_CullDistance[];
} gl_out; */

out float Height;

vec2 interpolate2D(vec2 v0, vec2 v1, vec2 v2) {
  return vec2(gl_TessCoord.x) * v0 + vec2(gl_TessCoord.y) * v1 + vec2(gl_TessCoord.z) * v2;
}

vec3 interpolate3D(vec3 v0, vec3 v1, vec3 v2) {
  return vec3(gl_TessCoord.x) * v0 + vec3(gl_TessCoord.y) * v1 + vec3(gl_TessCoord.z) * v2;
}

vec4 interpolate4D(vec4 v0, vec4 v1, vec4 v2) {
  return vec4(gl_TessCoord.x) * v0 + vec4(gl_TessCoord.y) * v1 + vec4(gl_TessCoord.z) * v2;
}

void main(void) {
	out_material.VertexData = interpolate3D(in_material[0].VertexData, in_material[1].VertexData, in_material[2].VertexData);
	out_material.TexData = interpolate2D(in_material[0].TexData, in_material[1].TexData, in_material[2].TexData);  
	out_material.NormData = normalize(interpolate3D(in_material[0].NormData, in_material[1].NormData, in_material[2].NormData));
	out_material.FragPos = normalize(interpolate3D(in_material[0].FragPos, in_material[1].FragPos, in_material[2].FragPos));
	//gl_Position = interpolate4D(gl_in[0].gl_Position, gl_in[1].gl_Position, gl_in[2].gl_Position);

	// get patch coordinate
    float u = gl_TessCoord.x;
    float v = gl_TessCoord.y;

    // ----------------------------------------------------------------------
    // retrieve control point texture coordinates
    vec2 t00 = in_material[0].TexData;
    vec2 t01 = in_material[1].TexData;
    vec2 t10 = in_material[2].TexData;
    vec2 t11 = in_material[3].TexData;

    // bilinearly interpolate texture coordinate across patch
    vec2 t0 = (t01 - t00) * u + t00;
    vec2 t1 = (t11 - t10) * u + t10;
    vec2 texCoord = (t1 - t0) * v + t0;

    // lookup texel at patch coordinate for height and scale + shift as desired
    Height = texture(Texture2, texCoord).y * 64.0 - 16.0;

    // ----------------------------------------------------------------------
    // retrieve control point position coordinates
    vec4 p00 = gl_in[0].gl_Position;
    vec4 p01 = gl_in[1].gl_Position;
    vec4 p10 = gl_in[2].gl_Position;
    vec4 p11 = gl_in[3].gl_Position;

    // compute patch surface normal
    vec4 uVec = p01 - p00;
    vec4 vVec = p10 - p00;
    vec4 normal = normalize( vec4(cross(vVec.xyz, uVec.xyz), 0) );

    // bilinearly interpolate position coordinate across patch
    vec4 p0 = (p01 - p00) * u + p00;
    vec4 p1 = (p11 - p10) * u + p10;
    vec4 p = (p1 - p0) * v + p0;

    // displace point along normal
    p += normal * Height;

	//gl_Position = Projection * View * Model * p;
	//gl_Position = p * Model * View * Projection;
}