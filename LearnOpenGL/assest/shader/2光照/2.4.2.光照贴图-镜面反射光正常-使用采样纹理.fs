#version 330 core
out vec4 FragColor;

in vec3 Normal;
in vec3 FragPos;
in vec2 TexCoords;// 纹理坐标

uniform vec3 viewPos;

struct Material {
    sampler2D diffuse;// 纹理单元
    //vec3 specular;// 镜面反射光强度依旧是手动设置
    sampler2D specular;//  镜面反射光用材质颜色作为反射强度
    float shininess;
}; 
uniform Material material;

// 光照强度
struct Light {
    vec3 position;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

uniform Light light;
void main()
{
    // 环境光
    float ambientStrength = 0.1;
    // 环境光也是采样漫反射贴图颜色作为反射强度
    vec3 ambient = light.ambient * vec3(texture(material.diffuse, TexCoords)); 

    // 漫反射
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(light.position - FragPos);
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = light.diffuse * diff * vec3(texture(material.diffuse, TexCoords));// 不再是从设置的vec3强度

    // 镜面光照
    float specularStrength = 0.5;
    vec3 viewDir = normalize(viewPos - FragPos); // 是观察者方向，不是观察者看向的方向
    vec3 reflectDir = reflect(-lightDir, norm);

    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    // vec3 specular = light.specular * (spec * material.specular); // 改变在这里
    // 采样镜面光材质颜色作为反射强度
    vec3 specular = light.specular * spec * vec3(texture(material.specular, TexCoords)); 

    vec3 result = (ambient + diffuse + specular) ;
    
    FragColor = vec4(result, 1.0);
}