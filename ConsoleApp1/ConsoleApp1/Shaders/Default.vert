#version 330 core
layout (location = 0) in vec3 aPosition; // vertex coordinates

void main()
{
	glPosition = vec4(aPosition, 1.0); // coordinates
}