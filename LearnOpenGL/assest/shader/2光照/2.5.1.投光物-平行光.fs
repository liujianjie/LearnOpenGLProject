#version 330 core
out vec4 FragColor;

in vec3 Normal;
in vec3 FragPos;
in vec2 TexCoords;// ��������

uniform vec3 viewPos;

struct Material {
    sampler2D diffuse;  // ����Ԫ
    sampler2D specular;//  ���������ɫ�������������
    float shininess;
}; 
uniform Material material;

// ����ǿ��
struct Light {
    // vec3 position; // ʹ��ƽ�й�Ͳ�����Ҫλ����
    vec3 direction; // �ӹ�Դ������ȫ�ֵķ���

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

uniform Light light;
void main()
{
    // ȡ����Ϊ��Դ�ķ������������ڼ���������;�������ʱ
    vec3 lightDir = normalize(-light.direction);
    // ��������շ���
    float ambientStrength = 0.1;
    // �������������ȡ��ɫ����
    vec3 ambient = light.ambient * vec3(texture(material.diffuse, TexCoords)); 

    // ��������շ���
    vec3 norm = normalize(Normal);
    // vec3 lightDir = normalize(light.position - FragPos); // һ��������Դ����Ҫ�����������Ƭ�ε���Դ�ķ�������
    float diff = max(dot(norm, lightDir), 0.0);             // �õ���Դ�Ե�ǰƬ��ʵ�ʵ�������Ӱ��
    vec3 diffuse = light.diffuse * diff * vec3(texture(material.diffuse, TexCoords));// �������������ȡ��ɫ����

    // ������շ���
    float specularStrength = 0.5;
    vec3 viewDir = normalize(viewPos - FragPos);            // �ǹ۲��߷��򣬲��ǹ۲��߿���ķ���
    vec3 reflectDir = reflect(-lightDir, norm);

    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);// ��Դ�Ե�ǰƬ�εľ����Ӱ��
    // vec3 specular = light.specular * (spec * material.specular); // �ı�������
    // ���������������ɫ��Ϊ���������ɫ����
    vec3 specular = light.specular * spec * vec3(texture(material.specular, TexCoords)); 

    vec3 result = (ambient + diffuse + specular) ;
    
    FragColor = vec4(result, 1.0);
}