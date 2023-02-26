#version 330 core
out vec4 FragColor;
in vec2 TexCoords;

uniform sampler2D screenTexture;

const float offset = 1.0 / 300.0;

void main(){ 
    // 1.原本已经是平滑的了，不变输出即平滑的
    // vec3 col = texture(screenTexture, TexCoords).rgb;
	// FragColor = vec4(vec3(col), 1.0);

    // 2.进行后期：模糊
    vec2 offsets[9] = vec2[](
        vec2(-offset,  offset), // 左上
        vec2( 0.0f,    offset), // 正上
        vec2( offset,  offset), // 右上
        vec2(-offset,  0.0f),   // 左
        vec2( 0.0f,    0.0f),   // 中
        vec2( offset,  0.0f),   // 右
        vec2(-offset, -offset), // 左下
        vec2( 0.0f,   -offset), // 正下
        vec2( offset, -offset)  // 右下
    );
    // 改变这个数组
    float kernel[9] = float[](
        1.0 / 16, 2.0 / 16, 1.0 / 16,
        2.0 / 16, 4.0 / 16, 2.0 / 16,
        1.0 / 16, 2.0 / 16, 1.0 / 16  
    );
    vec3 sampleTex[9];
    for(int i = 0; i < 9; i++){
        sampleTex[i] = vec3(texture(screenTexture, TexCoords.st + offsets[i]));// TexCoords.st = TexCoords.xy
    }
    vec3 col = vec3(0.0);
    for(int i = 0; i < 9; i++){
        col += sampleTex[i] * kernel[i];// 周围颜色乘以相应的权重并加起来就是核效果
    }
    FragColor = vec4(col, 1.0);
}
// 后期：边缘检测可能会导致锯齿，可以再进行模糊的核滤镜可减少锯齿
