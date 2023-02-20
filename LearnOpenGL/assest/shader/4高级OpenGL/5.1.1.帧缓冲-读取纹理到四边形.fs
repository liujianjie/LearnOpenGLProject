#version 330 core
out vec4 FragColor;

in vec2 TexCoords;// ÎÆÀí×ø±ê

uniform sampler2D screenTexture;

void main(){ 

    vec4 diffuse = texture(screenTexture, TexCoords);
	FragColor = diffuse;
}