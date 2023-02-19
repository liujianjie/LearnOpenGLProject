#version 330 core
out vec4 FragColor;

in vec2 TexCoords;// ÎÆÀí×ø±ê

uniform sampler2D texture1;

void main(){ 

    vec4 diffuse = texture(texture1, TexCoords);
	if(diffuse.a < 0.1f){
		discard;
	}
	FragColor = diffuse;
}