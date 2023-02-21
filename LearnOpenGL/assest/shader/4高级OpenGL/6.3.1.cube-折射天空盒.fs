#version 330 core
out vec4 FragColor;

in vec3 Normal;
in vec3 Position; // 片段的坐标-世界空间

uniform vec3 cameraPos;

// 天空盒纹理采样
uniform samplerCube skybox;

void main(){ 
    float ratio = 1.00 / 1.52;
    vec3 I = normalize(Position - cameraPos);
    vec3 R = refract(I, normalize(Normal), ratio);// refract，第三个参数是折射率

    // 采样天空盒的uv坐标是3维的
    FragColor = vec4(texture(skybox, R).rgb, 1.0);// FragColor = texture(skybox, R); 这个效果一样
}