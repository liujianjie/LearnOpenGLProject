#version 330 core
out vec4 FragColor;

in vec3 Normal;
in vec3 FragPos;
in vec2 TexCoords;// ��������

uniform vec3 viewPos;

struct Material {
    sampler2D diffuse;// ����Ԫ
    //vec3 specular;// ���淴���ǿ���������ֶ�����
    sampler2D specular;//  ���淴����ò�����ɫ��Ϊ����ǿ��
    float shininess;
}; 
uniform Material material;

// ����ǿ��
struct Light {
    vec3 position;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

uniform Light light;
void main()
{
    // ������
    float ambientStrength = 0.1;
    // ������Ҳ�ǲ�����������ͼ��ɫ��Ϊ����ǿ��
    vec3 ambient = light.ambient * vec3(texture(material.diffuse, TexCoords)); 

    // ������
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(light.position - FragPos);
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = light.diffuse * diff * vec3(texture(material.diffuse, TexCoords));// �����Ǵ����õ�vec3ǿ��

    // �������
    float specularStrength = 0.5;
    vec3 viewDir = normalize(viewPos - FragPos); // �ǹ۲��߷��򣬲��ǹ۲��߿���ķ���
    vec3 reflectDir = reflect(-lightDir, norm);

    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    // vec3 specular = light.specular * (spec * material.specular); // �ı�������
    // ��������������ɫ��Ϊ����ǿ��
    vec3 specular = light.specular * spec * vec3(texture(material.specular, TexCoords)); 

    vec3 result = (ambient + diffuse + specular) ;
    
    FragColor = vec4(result, 1.0);
}