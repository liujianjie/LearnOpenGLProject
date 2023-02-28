#version 330 core
out vec4 FragColor;

in VS_OUT{
    vec3 FragPos;
    vec3 Normal;
    vec2 TexCoords;
}fs_in;

uniform sampler2D floorTexture;
uniform vec3 lightPos;
uniform vec3 viewPos;
uniform bool blinn;

void main()
{
    // 采样纹理作为光照颜色
    vec3 color = texture(floorTexture, fs_in.TexCoords).rgb;
    // 环境光
    float ambientStrength = 0.05;
    vec3 ambient = ambientStrength * color;
    // 漫反射
    vec3 lightDir = normalize(lightPos - fs_in.FragPos);
    vec3 normal = normalize(fs_in.Normal);
    float diff = max(dot(normal, lightDir), 0.0);
    vec3 diffuse = diff * color;
    // 环境光照
    vec3 viewDir = normalize(viewPos - fs_in.FragPos); // 是观察者方向，不是观察者看向的方向
    float spec = 0.0;
    if(blinn){
        // blinn-pong：半程方向向量与法线方向向量的点积
        // 半程向量
        vec3 halfwayDir = normalize(lightDir + viewDir);
        spec = pow(max(dot(normal, halfwayDir), 0.0), 1);
    }
    else
    {
        // pong：观察者方向向量与反射方向向量的点积
        vec3 reflectDir = reflect(-lightDir, normal);
        spec = pow(max(dot(viewDir, reflectDir), 0.0), 1);
    }
    float specularStrength = 0.3;
    vec3 specular = specularStrength * spec * vec3(1);// 不像漫反射需要乘以纹理颜色，而是乘以1，表示镜面光的颜色为白色
    FragColor = vec4(ambient + diffuse + specular, 1.0);
    // FragColor = vec4(diffuse + specular, 1.0);
    // FragColor = vec4(1.0);
}