#version 330 core

in vec2 texCoord;
in vec3 normal;
in vec3 fragPosition;

out vec4 fragColor;

uniform sampler2D texture0;

void main()
{
    //sample texture at half illumination
    vec3 result = vec3(0.5) * vec3(texture(texture0, texCoord));
    fragColor = vec4(result, 1.0);
}