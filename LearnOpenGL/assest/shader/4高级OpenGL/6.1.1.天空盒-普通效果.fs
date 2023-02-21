#version 330 core
out vec4 FragColor;

// ����������3ά��
in vec3 TexCoords;// ��������

// ��պ��������
uniform samplerCube skybox;

void main(){ 
    FragColor = texture(skybox, TexCoords);
}