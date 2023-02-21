#version 330 core
out vec4 FragColor;

in vec3 Normal;
in vec3 Position; // 片段的坐标-世界空间
in vec2 TexCoords;// 纹理坐标

uniform vec3 cameraPos;
uniform sampler2D texture_diffuse1;
uniform sampler2D texture_specular1;
uniform sampler2D texture_height1;

// 天空盒纹理采样
uniform samplerCube skybox;

void main(){ 
    float ratio = 1.00 / 1.52;
    vec3 I = normalize(Position - cameraPos);
    vec3 R = refract(I, normalize(Normal), ratio);// refract，第三个参数是折射率

    // 采样天空盒的uv坐标是3维的
    FragColor = vec4(texture(skybox, R).rgb, 1.0);// FragColor = texture(skybox, R); 这个效果一样
}