#version 330 core
out vec4 FragColor;
struct Material {
    sampler2D diffuse;// 纹理单元
    sampler2D specular;//  镜面反射光用材质颜色作为反射强度
    float shininess;
}; 
// 聚光灯
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
in vec2 TexCoords;// 纹理坐标

uniform vec3 viewPos;
uniform Material material;
uniform Light light;
void main()
{
    // 光源的方向向量，像素点指向光源，而不是光源指向像素点
    vec3 lightDir = normalize(light.position - FragPos);
    // 算出theta，dot（像素点指向光源的向量 与 光源的方向向量（依旧是像素点指向光源，只不过是照射方向上的像素点））
    float theta = dot(lightDir, normalize(-light.direction)); // = dot(-lightDir, normalize(light.direction));// 注释的是 dot(光源指向像素点 与 光源正前方照射方向指向像素点)
    // 执行正常光照计算：由于theta是cos值，cutoof也是cos值，cos(0-90)递减，所以theta>，而不是<

    // 为了边缘平滑
    float epsilon = light.cutOff - light.outerCutOff;
    float intensity = clamp((theta - light.outerCutOff) / epsilon, 0.0, 1.0);

    // 环境光也是采样漫反射贴图颜色作为反射强度
    vec3 ambient = light.ambient * vec3(texture(material.diffuse, TexCoords)); 

    // 漫反射
    vec3 norm = normalize(Normal);
    // vec3 lightDir = normalize(light.position - FragPos); // 得到光源的方向
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

    // 将不对环境光做出影响，让它总是能有一点光
    diffuse *= intensity;
    specular *= intensity;// 受聚光灯影响

    vec3 result = (ambient + diffuse + specular) ;
    
    FragColor = vec4(result, 1.0);
}