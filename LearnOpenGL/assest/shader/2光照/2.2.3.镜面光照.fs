#version 330 core
out vec4 FragColor;

in vec3 Normal;
in vec3 FragPos;

uniform vec3 lightPos;
uniform vec3 viewPos;
uniform vec3 objectColor;
uniform vec3 lightColor;

void main()
{

    // 环境光
    float ambientStrength = 0.1;
    vec3 ambient = ambientStrength * lightColor;

    // 漫反射
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(lightPos - FragPos);
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = diff * lightColor;// 漫反射分量

    // 镜面光
    float specularStrength = 0.5;
    vec3 viewDir = normalize(viewPos - FragPos); // 是观察者方向，不是观察者看向的方向
    vec3 reflectDir = reflect(-lightDir, norm);

    float spec = pow(max(dot(viewDir, reflectDir), 0.0), 256);// 光源对当前片段的镜面光影响。32是反光度
    vec3 specular = specularStrength * spec * lightColor;// 镜面光照分量 = 镜面光照强度*光源对当前片段的镜面光影响*光源的颜色
    // 由上一节2.1颜色所说，光源的颜色（冯氏）与物体的颜色值相乘 = 物体的颜色
    vec3 result = (ambient + diffuse + specular) * objectColor;
    FragColor = vec4(result, 1.0);
    // FragColor = vec4(1.0);
}