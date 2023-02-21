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
    vec3 I = normalize(Position - cameraPos);
    vec3 R = reflect(I, normalize(Normal));
    // 采样镜面光贴图颜色作为反射强度，uv坐标是2维的
    vec4 specular4 = texture(texture_specular1, TexCoords); // 采样出来是4维的
    vec3 specular3 = specular4.rgb;
    // 采样天空盒的uv坐标是3维的
    FragColor = vec4(texture(skybox, R).rgb * specular3, 1.0) ;
}