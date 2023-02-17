#version 330 core
out vec4 FragColor;
struct Material {
    sampler2D diffuse;// 纹理单元
    sampler2D specular;//  镜面反射光用材质颜色作为反射强度
    sampler2D spotLightDiffuse;//  聚光灯的漫反射用此材质颜色作为反射强度
    float shininess;
}; 
// 平行光
struct DirLight  {
    vec3 direction; // 从光源出发到全局的方向

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};
// 点光源
struct PointLight  {
    vec3 position;  

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;

    float constant; // 常数
    float linear;   // 一次项
    float quadratic;// 二次项
};
// 聚光灯
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
in vec2 TexCoords;// 纹理坐标

uniform vec3 viewPos;

// 计算物体对平行光反射的颜色
vec3 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir){
    vec3 lightDir = normalize(-light.direction);
    // 漫反射
    float diff = max(dot(normal, lightDir), 0.0);
    // 镜面光着色
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    // 合并
    vec3 ambient = light.ambient * vec3(texture(material.diffuse, TexCoords));
    vec3 diffuse = light.diffuse * diff * vec3(texture(material.diffuse, TexCoords));
    vec3 specular = light.specular * spec * vec3(texture(material.specular, TexCoords));
    return (ambient + diffuse + specular);
}
// 计算物体对点光源反射的颜色
vec3 CalcPointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir){
    vec3 lightDir = normalize(light.position - fragPos);
    // 漫反射
    float diff = max(dot(normal, lightDir), 0.0);
    // 镜面光
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    // 衰减
    float distance = length(light.position - fragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance +
                        light.quadratic * distance * distance);
    // 合并结果
    vec3 ambient = light.ambient * vec3(texture(material.diffuse, TexCoords));
    vec3 diffuse = light.diffuse * diff * vec3(texture(material.diffuse, TexCoords));
    vec3 specular = light.specular * spec * vec3(texture(material.specular, TexCoords));
    
    ambient *= attenuation;
    diffuse *= attenuation;
    specular *= attenuation;
    return (ambient + diffuse + specular);
}
// 计算物体对聚光灯反射的颜色
vec3 CalcSpotLight(SpotLight light, vec3 normal, vec3 fragPos, vec3 viewDir){
    // 光源的方向向量，像素点指向光源，而不是光源指向像素点
    vec3 lightDir = normalize(light.position - fragPos);
    // 算出theta，dot（像素点指向光源的向量 与 光源的方向向量（依旧是像素点指向光源，只不过是照射方向上的像素点））
    float theta = dot(lightDir, normalize(-light.direction)); // = dot(-lightDir, normalize(light.direction));// 注释的是 dot(光源指向像素点 与 光源正前方照射方向指向像素点)
    // 执行正常光照计算：由于theta是cos值，cutoof也是cos值，cos(0-90)递减，所以theta>，而不是<

    // 为了边缘平滑
    float epsilon = light.cutOff - light.outerCutOff;
    float intensity = clamp((theta - light.outerCutOff) / epsilon, 0.0, 1.0);

    // 漫反射
    float diff = max(dot(normal, lightDir), 0.0);

    // 镜面光照
    vec3 reflectDir = reflect(-lightDir, normal);// reflect要求第一个参数是光源指向像素点的向量
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);

    vec3 ambient = light.ambient * vec3(texture(material.diffuse, TexCoords)); 
    vec3 diffuse = light.diffuse * diff * texture(material.spotLightDiffuse, TexCoords).rgb;// 不再是从设置的vec3强度
    vec3 specular = light.specular * spec * texture(material.specular, TexCoords).rgb; 

    // 计算衰减
    float distance  = length(light.position - fragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance 
                        + light.quadratic * distance * distance);
    
    // 反射的光照随距离衰减
    ambient *= attenuation;
    diffuse *= attenuation;
    specular *= attenuation;

    // 受聚光灯影响，将不对环境光做出影响，让它总是能有一点光
    diffuse *= intensity;
    specular *= intensity;
    return (ambient + diffuse + specular);
}

void main()
{
    // 属性
    vec3 norm = normalize(Normal);
    vec3 viewDir = normalize(viewPos - FragPos);

    // 定向光照
    vec3 result = CalcDirLight(dirLight, norm, viewDir);

    // 点光源
    for(int i = 0; i < NR_POINT_LIGHTS; i++){
        result += CalcPointLight(pointLights[i], norm, FragPos, viewDir);
    }
    // 聚光灯
    result += CalcSpotLight(spotLight, norm, FragPos, viewDir);
    
    FragColor = vec4(result, 1.0);
}