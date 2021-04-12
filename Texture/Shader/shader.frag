#version 330

out vec4 outputColor;

in vec2 uv;

uniform sampler2D ourTexA;
uniform sampler2D ourTexB;
void main()
{
    outputColor = mix(texture(ourTexA,uv),texture(ourTexB,uv),0.2);
}