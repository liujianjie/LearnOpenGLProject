#version 330 core
out vec4 FragColor;
struct Material {
    sampler2D diffuse;// 纹理单元
    sampler2D specular;//  镜面反射光用材质颜色作为反射强度
    float shininess;
}; 
// 点光源
struct Light {
    vec3 position;  

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;

    float constant; // 常数
    float linear;   // 一次项
    float quadratic;// 二次项
};

in vec3 FragPos;
in vec3 Normal;
in vec2 TexCoords;// 纹理坐标

uniform vec3 viewPos;
uniform Material material;
uniform Light light;
void main()
{
    // 环境光
    // 环境光也是采样漫反射贴图颜色作为反射强度
    vec3 ambient = light.ambient * vec3(texture(material.diffuse, TexCoords)); 

    // 漫反射
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(light.position - FragPos); // 得到光源的方向
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = light.diffuse * diff * texture(material.diffuse, TexCoords).rgb;// 不再是从设置的vec3强度

    // 镜面光照
    vec3 viewDir = normalize(viewPos - FragPos); // 是观察者方向，不是观察者看向的方向
    vec3 reflectDir = reflect(-lightDir, norm);// reflect要求第一个参数是光源指向像素点的向量
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    // 采样镜面光材质颜色作为反射强度
    //vec3 specular = light.specular * spec * vec3(texture(material.specular, TexCoords)); 
    vec3 specular = light.specular * spec * texture(material.specular, TexCoords).rgb; 

    // 得到长度
    float distance  = length(light.position - FragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance 
                        + light.quadratic * distance * distance);
    
    // 反射的光照随距离衰减
    ambient *= attenuation;
    diffuse *= attenuation;
    specular *= attenuation;

    vec3 result = (ambient + diffuse + specular) ;
    
    FragColor = vec4(result, 1.0);
}