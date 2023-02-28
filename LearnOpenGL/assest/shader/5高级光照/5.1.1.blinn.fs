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
    // ����������Ϊ������ɫ
    vec3 color = texture(floorTexture, fs_in.TexCoords).rgb;
    // ������
    float ambientStrength = 0.05;
    vec3 ambient = ambientStrength * color;
    // ������
    vec3 lightDir = normalize(lightPos - fs_in.FragPos);
    vec3 normal = normalize(fs_in.Normal);
    float diff = max(dot(normal, lightDir), 0.0);
    vec3 diffuse = diff * color;
    // ��������
    vec3 viewDir = normalize(viewPos - fs_in.FragPos); // �ǹ۲��߷��򣬲��ǹ۲��߿���ķ���
    float spec = 0.0;
    if(blinn){
        // blinn-pong����̷��������뷨�߷��������ĵ��
        // �������
        vec3 halfwayDir = normalize(lightDir + viewDir);
        spec = pow(max(dot(normal, halfwayDir), 0.0), 1);
    }
    else
    {
        // pong���۲��߷��������뷴�䷽�������ĵ��
        vec3 reflectDir = reflect(-lightDir, normal);
        spec = pow(max(dot(viewDir, reflectDir), 0.0), 1);
    }
    float specularStrength = 0.3;
    vec3 specular = specularStrength * spec * vec3(1);// ������������Ҫ����������ɫ�����ǳ���1����ʾ��������ɫΪ��ɫ
    FragColor = vec4(ambient + diffuse + specular, 1.0);
    // FragColor = vec4(diffuse + specular, 1.0);
    // FragColor = vec4(1.0);
}