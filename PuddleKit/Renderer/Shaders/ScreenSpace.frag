#version 330 core

in vec2 texCoord;

out vec4 fragColor;

uniform sampler2D texture0;

void main()
{
    //directly sample texture
    fragColor = texture(texture0, texCoord);
}