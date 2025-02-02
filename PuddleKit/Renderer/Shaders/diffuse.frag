#version 330 core

in vec2 texCoord;
in vec3 normal;
in vec3 fragPosition;

out vec4 fragColor;

uniform sampler2D texture0;

void main()
{
    //calulate diffuse modifier based on the difference between the normal direction and the light direction
    vec3 norm = normalize(normal);
    vec3 lightDir = normalize(vec3(0, 10, 0) - fragPosition); 
    float diffuse = max(dot(norm, lightDir), 0.0);

    //sample texture applying the diffuse modifier and a base illumiation modifier
    vec3 result = (vec3(0.3, 0.33, 0.36) + diffuse) * vec3(texture(texture0, texCoord));
    fragColor = vec4(result, 1.0);
}