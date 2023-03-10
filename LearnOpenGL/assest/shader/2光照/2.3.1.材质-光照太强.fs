#version 330 core
out vec4 FragColor;

in vec3 Normal;
in vec3 FragPos;

uniform vec3 lightPos;
uniform vec3 viewPos;
uniform vec3 objectColor;
uniform vec3 lightColor;

struct Material {
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
    float shininess;
}; 

uniform Material material;

void main()
{
    // 环境光
    // float ambientStrength = 0.1;
    //vec3 ambient = ambientStrength * lightColor;
    vec3 ambient = lightColor * material.ambient;           // 环境光照分量

    // 漫反射
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(lightPos - FragPos);
    float diff = max(dot(norm, lightDir), 0.0);// 得到光源对当前片段实际的漫反射影响
    // vec3 diffuse = diff * lightColor;
    vec3 diffuse = lightColor * diff *  material.diffuse;   // 漫反射光照分量

    // 镜面光照
    float specularStrength = 0.5;
    vec3 viewDir = normalize(viewPos - FragPos);            // 是观察者方向，不是观察者看向的方向
    vec3 reflectDir = reflect(-lightDir, norm);

    float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);// 光源对当前片段的镜面光影响
    //vec3 specular = specularStrength * spec * lightColor;
    vec3 specular = lightColor * (spec * material.specular);// 镜面光光照分量

    //vec3 result = (ambient + diffuse + specular) * objectColor;
    vec3 result = (ambient + diffuse + specular) ;          // 不用乘以物体颜色，材质已经决定了物体的颜色
    
    FragColor = vec4(result, 1.0);
}
