#version 330 core
out vec4 FragColor;

in vec3 Normal;
in vec3 Position; // Ƭ�ε�����-����ռ�
in vec2 TexCoords;// ��������

uniform vec3 cameraPos;
uniform sampler2D texture_diffuse1;
uniform sampler2D texture_specular1;
uniform sampler2D texture_height1;

// ��պ��������
uniform samplerCube skybox;

void main(){ 
    float ratio = 1.00 / 1.52;
    vec3 I = normalize(Position - cameraPos);
    vec3 R = refract(I, normalize(Normal), ratio);// refract��������������������

    // ������պе�uv������3ά��
    FragColor = vec4(texture(skybox, R).rgb, 1.0);// FragColor = texture(skybox, R); ���Ч��һ��
}