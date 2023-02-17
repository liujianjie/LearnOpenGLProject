#version 330 core
out vec4 FragColor;
struct Material {
    sampler2D diffuse;// ����Ԫ
    sampler2D specular;//  ���淴����ò�����ɫ��Ϊ����ǿ��
    sampler2D spotLightDiffuse;//  �۹�Ƶ��������ô˲�����ɫ��Ϊ����ǿ��
    float shininess;
}; 
// ƽ�й�
struct DirLight  {
    vec3 direction; // �ӹ�Դ������ȫ�ֵķ���

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};
// ���Դ
struct PointLight  {
    vec3 position;  

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;

    float constant; // ����
    float linear;   // һ����
    float quadratic;// ������
};
// �۹��
struct SpotLight {
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
uniform DirLight dirLight;
#define NR_POINT_LIGHTS 4
uniform PointLight pointLights[NR_POINT_LIGHTS];
uniform SpotLight spotLight;

uniform Material material;

in vec3 FragPos;
in vec3 Normal;
in vec2 TexCoords;// ��������

uniform vec3 viewPos;

// ���������ƽ�йⷴ�����ɫ
vec3 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir){
    vec3 lightDir = normalize(-light.direction);
    // ������
    float diff = max(dot(normal, lightDir), 0.0);
    // �������ɫ
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    // �ϲ�
    vec3 ambient = light.ambient * vec3(texture(material.diffuse, TexCoords));
    vec3 diffuse = light.diffuse * diff * vec3(texture(material.diffuse, TexCoords));
    vec3 specular = light.specular * spec * vec3(texture(material.specular, TexCoords));
    return (ambient + diffuse + specular);
}
// ��������Ե��Դ�������ɫ
vec3 CalcPointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir){
    vec3 lightDir = normalize(light.position - fragPos);
    // ������
    float diff = max(dot(normal, lightDir), 0.0);
    // �����
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    // ˥��
    float distance = length(light.position - fragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance +
                        light.quadratic * distance * distance);
    // �ϲ����
    vec3 ambient = light.ambient * vec3(texture(material.diffuse, TexCoords));
    vec3 diffuse = light.diffuse * diff * vec3(texture(material.diffuse, TexCoords));
    vec3 specular = light.specular * spec * vec3(texture(material.specular, TexCoords));
    
    ambient *= attenuation;
    diffuse *= attenuation;
    specular *= attenuation;
    return (ambient + diffuse + specular);
}
// ��������Ծ۹�Ʒ������ɫ
vec3 CalcSpotLight(SpotLight light, vec3 normal, vec3 fragPos, vec3 viewDir){
    // ��Դ�ķ������������ص�ָ���Դ�������ǹ�Դָ�����ص�
    vec3 lightDir = normalize(light.position - fragPos);
    // ���theta��dot�����ص�ָ���Դ������ �� ��Դ�ķ������������������ص�ָ���Դ��ֻ���������䷽���ϵ����ص㣩��
    float theta = dot(lightDir, normalize(-light.direction)); // = dot(-lightDir, normalize(light.direction));// ע�͵��� dot(��Դָ�����ص� �� ��Դ��ǰ�����䷽��ָ�����ص�)
    // ִ���������ռ��㣺����theta��cosֵ��cutoofҲ��cosֵ��cos(0-90)�ݼ�������theta>��������<

    // Ϊ�˱�Եƽ��
    float epsilon = light.cutOff - light.outerCutOff;
    float intensity = clamp((theta - light.outerCutOff) / epsilon, 0.0, 1.0);

    // ������
    float diff = max(dot(normal, lightDir), 0.0);

    // �������
    vec3 reflectDir = reflect(-lightDir, normal);// reflectҪ���һ�������ǹ�Դָ�����ص������
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);

    vec3 ambient = light.ambient * vec3(texture(material.diffuse, TexCoords)); 
    vec3 diffuse = light.diffuse * diff * texture(material.spotLightDiffuse, TexCoords).rgb;// �����Ǵ����õ�vec3ǿ��
    vec3 specular = light.specular * spec * texture(material.specular, TexCoords).rgb; 

    // ����˥��
    float distance  = length(light.position - fragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance 
                        + light.quadratic * distance * distance);
    
    // ����Ĺ��������˥��
    ambient *= attenuation;
    diffuse *= attenuation;
    specular *= attenuation;

    // �ܾ۹��Ӱ�죬�����Ի���������Ӱ�죬������������һ���
    diffuse *= intensity;
    specular *= intensity;
    return (ambient + diffuse + specular);
}

void main()
{
    // ����
    vec3 norm = normalize(Normal);
    vec3 viewDir = normalize(viewPos - FragPos);

    // �������
    vec3 result = CalcDirLight(dirLight, norm, viewDir);

    // ���Դ
    for(int i = 0; i < NR_POINT_LIGHTS; i++){
        result += CalcPointLight(pointLights[i], norm, FragPos, viewDir);
    }
    // �۹��
    result += CalcSpotLight(spotLight, norm, FragPos, viewDir);
    
    FragColor = vec4(result, 1.0);
}