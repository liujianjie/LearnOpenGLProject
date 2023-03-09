#version 330 core
out vec4 FragColor;
struct Material {
    sampler2D diffuse;  // ��������ɫ�������������
    sampler2D specular;//  ���������ɫ�������������
    float shininess;
}; 
// �۹��
struct Light {
    vec3  position; // ��Ҫλ��
    vec3  direction;// ��Ҫ���䷽��
    float cutOff;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
    float constant; // ����
    float linear;   // һ����
    float quadratic;// ������
};

in vec3 FragPos;
in vec3 Normal;
in vec2 TexCoords;// ��������

uniform vec3 viewPos;
uniform Material material;
uniform Light light;
void main()
{
    // ��Դ�ķ������������ص�ָ���Դ
    vec3 lightDir = normalize(light.position - FragPos);
    // ���theta��dot(���ص�ָ���Դ����Դ����ķ���ȡ����
    float theta = dot(lightDir, normalize(-light.direction)); 
    // float theta=dot(-lightDir, normalize(light.direction));// dot(��Դָ�����ص�, ��Դ����ķ���)
    // ִ���������ռ��㣺����theta��cosֵ��cutOffҲ��cosֵ��cos(0-90)�ݼ�������theta>��������<
    if(theta > light.cutOff){
        // Ƭ�����н���
        // �������������ȡ��ɫ����
        vec3 ambient = light.ambient * vec3(texture(material.diffuse, TexCoords)); 

        // ��������շ���
        vec3 norm = normalize(Normal);
        vec3 lightDir = normalize(light.position - FragPos); // �õ���Դ�ķ���,��һ��������Դһ��
        float diff = max(dot(norm, lightDir), 0.0);         // �õ���Դ�Ե�ǰƬ��ʵ�ʵ�������Ӱ��
        vec3 diffuse = light.diffuse * diff * texture(material.diffuse, TexCoords).rgb;// �������������ȡ��ɫ����

        // ������շ���
        vec3 viewDir = normalize(viewPos - FragPos);       // �ǹ۲��߷��򣬲��ǹ۲��߿���ķ���
        vec3 reflectDir = reflect(-lightDir, norm);         // reflectҪ���һ�������ǹ�Դָ�����ص������
        float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);// ��Դ�Ե�ǰƬ�εľ����Ӱ��
        // ���������������ɫ��Ϊ���������ɫ����
        //vec3 specular = light.specular * spec * vec3(texture(material.specular, TexCoords)); // ���Ҳ��
        vec3 specular = light.specular * spec * texture(material.specular, TexCoords).rgb; 

        // ����˥��
        float distance  = length(light.position - FragPos);                 // �õ���Դ��Ƭ�γ���
        float attenuation = 1.0 / (light.constant + light.linear * distance // ���ݹ�ʽ
                            + light.quadratic * distance * distance);
    
        // ���շ��������˥��
        ambient *= attenuation;
        diffuse *= attenuation;
        specular *= attenuation;

        vec3 result = (ambient + diffuse + specular) ;
    
        FragColor = vec4(result, 1.0);
    }else{
        // Ƭ�β����н��ڣ����㻷���⣬����ȫ��
        FragColor = vec4(light.ambient * vec3(texture(material.diffuse, TexCoords)), 1.0) ;
    }
}