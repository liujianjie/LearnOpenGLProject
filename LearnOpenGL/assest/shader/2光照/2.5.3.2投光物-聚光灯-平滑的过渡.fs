#version 330 core
out vec4 FragColor;
struct Material {
    sampler2D diffuse;// ����Ԫ
    sampler2D specular;//  ���淴����ò�����ɫ��Ϊ����ǿ��
    float shininess;
}; 
// �۹��
struct Light {
    vec3  position;
    vec3  direction;
    float cutOff;
    float outerCutOff;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
    float constant;
    float linear;
    float quadratic;
};

in vec3 FragPos;
in vec3 Normal;
in vec2 TexCoords;// ��������

uniform vec3 viewPos;
uniform Material material;
uniform Light light;
void main()
{
    // ��Դ�ķ������������ص�ָ���Դ�������ǹ�Դָ�����ص�
    vec3 lightDir = normalize(light.position - FragPos);
    // ���theta��dot�����ص�ָ���Դ������ �� ��Դ�ķ������������������ص�ָ���Դ��ֻ���������䷽���ϵ����ص㣩��
    float theta = dot(lightDir, normalize(-light.direction)); // = dot(-lightDir, normalize(light.direction));// ע�͵��� dot(��Դָ�����ص� �� ��Դ��ǰ�����䷽��ָ�����ص�)
    // ִ���������ռ��㣺����theta��cosֵ��cutoofҲ��cosֵ��cos(0-90)�ݼ�������theta>��������<

    // Ϊ�˱�Եƽ��
    float epsilon = light.cutOff - light.outerCutOff;
    float intensity = clamp((theta - light.outerCutOff) / epsilon, 0.0, 1.0);

    // ������Ҳ�ǲ�����������ͼ��ɫ��Ϊ����ǿ��
    vec3 ambient = light.ambient * vec3(texture(material.diffuse, TexCoords)); 

    // ������
    vec3 norm = normalize(Normal);
    // vec3 lightDir = normalize(light.position - FragPos); // �õ���Դ�ķ���
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = light.diffuse * diff * texture(material.diffuse, TexCoords).rgb;// �����Ǵ����õ�vec3ǿ��

    // �������
    vec3 viewDir = normalize(viewPos - FragPos); // �ǹ۲��߷��򣬲��ǹ۲��߿���ķ���
    vec3 reflectDir = reflect(-lightDir, norm);// reflectҪ���һ�������ǹ�Դָ�����ص������
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    // ��������������ɫ��Ϊ����ǿ��
    //vec3 specular = light.specular * spec * vec3(texture(material.specular, TexCoords)); 
    vec3 specular = light.specular * spec * texture(material.specular, TexCoords).rgb; 

    // �õ�����
    float distance  = length(light.position - FragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance 
                        + light.quadratic * distance * distance);
    
    // ����Ĺ��������˥��
    ambient *= attenuation;
    diffuse *= attenuation;
    specular *= attenuation;

    // �����Ի���������Ӱ�죬������������һ���
    diffuse *= intensity;
    specular *= intensity;// �ܾ۹��Ӱ��

    vec3 result = (ambient + diffuse + specular) ;
    
    FragColor = vec4(result, 1.0);
}