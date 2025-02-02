#version 330 core

in vec2 texCoord;

out vec4 fragColor;

uniform sampler2D texture0;
uniform vec4 textColour;

void main()
{
    //use letter texture as alpha value so we only apply colour where there is text
    fragColor = textColour * vec4(1, 1, 1, texture(texture0, texCoord).r);
}