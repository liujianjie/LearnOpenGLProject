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
    vec3 I = normalize(Position - cameraPos);
    vec3 R = reflect(I, normalize(Normal));
    // �����������ͼ��ɫ��Ϊ����ǿ�ȣ�uv������2ά��
    vec4 specular4 = texture(texture_specular1, TexCoords); // ����������4ά��
    vec3 specular3 = specular4.rgb;
    // ������պе�uv������3ά��
    FragColor = vec4(texture(skybox, R).rgb * specular3, 1.0) ;
}