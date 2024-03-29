Implementation:
1. DiamondSquare1.cs: 
Class is used to create a terrain as a mesh object with the Diamond-Square Algorithm
Code adapted from other sources: https://www.youtube.com/watch?v=1HV8GbFnCik

2. CameraMovement.cs
Class dictates the cameras movement and rotation through input keys and mouse movement. All rotation and movement is done relative to the cameras orientation within restricted boundaries.

3. LockMouse.cs
Stops mouse from moving outside of play area and prevents rotation from being hindered by displays borders. Can be unlocked using the 'esc' key and locked again by clicking on the screen.

4. PointLight.cs
Adapted from Lab04 to allow for rotation. This deals with the sun's movement and light source

5. PhongShader.shader
Adapted from Lab04, added terrain height colouring. Blue for water, yellow for sand, green for grass, brown mountains, white for snow

Generate terrain in Unity:
- Create a new plane (or terrain) object 
- Attach DiamondSquare1.cs as a component script to the object 
- Set 5 first value in script component: (set none for default value)
  + M Divisions: 	maximum number of faces (or lines) on one dimension: has to be 2^n
  + M Size: 		maximum size of terrain
  + M Height: 		maximum height of terrain
  + Roughness: 		rate of change of height for every iteration  
  + Seed: 		seed for random value
- Set Point light to be: PointLight.cs (PointLight)
- Set Shader to be: PhongShader.shader (Unlit/PhongShader)
- Run the Scene!

