#version 330 core
out vec4 FragColor;
struct Material {
    sampler2D diffuse;  // 漫反射颜色分量从纹理采样
    sampler2D specular;//  镜面光照颜色分量从纹理采样
    float shininess;
}; 
// 聚光灯
struct Light {
    vec3  position; // 需要位置
    vec3  direction;// 需要照射方向
    float cutOff;// ϕ（内光切）
    float outerCutOff;// γ（外光切）

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
    // 光源的方向向量：像素点指向光源
    vec3 lightDir = normalize(light.position - FragPos);
    // 算出theta，dot(像素点指向光源，光源照射的方向取反）
    float theta = dot(lightDir, normalize(-light.direction));
    // float theta=dot(-lightDir, normalize(light.direction));// dot(光源指向像素点, 光源照射的方向)

    

    // 从漫反射纹理读取颜色分量
    vec3 ambient = light.ambient * vec3(texture(material.diffuse, TexCoords)); 

    // 漫反射光照分量
    vec3 norm = normalize(Normal);
    // vec3 lightDir = normalize(light.position - FragPos); // 得到光源的方向
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = light.diffuse * diff * texture(material.diffuse, TexCoords).rgb;// 从漫反射纹理读取颜色分量

     // 镜面光照分量
    vec3 viewDir = normalize(viewPos - FragPos);            // 是观察者方向，不是观察者看向的方向
    vec3 reflectDir = reflect(-lightDir, norm);             // reflect要求第一个参数是光源指向像素点的向量
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);// 光源对当前片段的镜面光影响
    // 采样镜面光纹理颜色作为镜面光照颜色分量
    //vec3 specular = light.specular * spec * vec3(texture(material.specular, TexCoords)); // 这句也行
    vec3 specular = light.specular * spec * texture(material.specular, TexCoords).rgb; 

    // 计算衰减
    float distance  = length(light.position - FragPos);                 // 得到光源到片段长度
    float attenuation = 1.0 / (light.constant + light.linear * distance // 根据公式
                        + light.quadratic * distance * distance);
    
    // 光照分量随距离衰减
    ambient *= attenuation;
    diffuse *= attenuation;
    specular *= attenuation;

    // 为了边缘平滑且圆锥内正常
    float epsilon = light.cutOff - light.outerCutOff;
    float intensity = clamp((theta - light.outerCutOff) / epsilon, 0.0, 1.0);

    // 将不对环境光做出影响，让它总是能有一点光
    diffuse *= intensity;
    specular *= intensity;// 聚光灯受影响

    vec3 result = (ambient + diffuse + specular) ;
    
    FragColor = vec4(result, 1.0);
}