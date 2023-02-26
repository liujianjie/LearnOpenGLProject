#version 330 core
out vec4 FragColor;

in vec3 ourColor;
in vec2 TexCoord;

uniform sampler2D texture1;
uniform sampler2D texture2;

void main()
{
    // 单个纹理
    FragColor = texture(texture1, TexCoord);
    // 纹理和颜色混合
    //FragColor = texture(texture1, TexCoord) * vec4(ourColor, 1.0);
    // 两个纹理混合
    //FragColor = mix(texture(texture1, TexCoord), texture(texture2, TexCoord), 0.2);
}