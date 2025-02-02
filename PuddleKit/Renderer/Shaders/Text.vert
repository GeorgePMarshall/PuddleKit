#version 330 core

layout (location = 0) in vec4 vertex; //xy are position, zw are texcoords

out vec2 texCoord;

uniform mat4 objectTransform;
uniform mat4 viewProjectionTransform;

void main()
{
    //apply vertex transformations 
    gl_Position = vec4(vertex.xy, 0.0f, 1.0f) * viewProjectionTransform;
    
    //pass texture coords
    texCoord = vertex.zw;
}