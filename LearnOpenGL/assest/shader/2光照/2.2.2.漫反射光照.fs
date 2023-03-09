#version 330 core
out vec4 FragColor;

in vec3 Normal;
in vec3 FragPos;

uniform vec3 lightPos;
uniform vec3 objectColor;
uniform vec3 lightColor;

void main()
{

    // 环境光
    float ambientStrength = 0.1;
    vec3 ambient = ambientStrength * lightColor;// 环境光分量=常量（光照）颜色

    // 漫反射
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(lightPos - FragPos);
    float diff = max(dot(norm, lightDir), 0.0);// 得到光源对当前片段实际的**漫反射影响**
    vec3 diffuse = diff * lightColor;// 漫反射分量


    vec3 result = (ambient + diffuse) * objectColor;
    FragColor = vec4(result, 1.0);
    // FragColor = vec4(1.0);
}