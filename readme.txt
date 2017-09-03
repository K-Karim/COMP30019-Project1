Implementation:
1. DiamondSquare1.cs: 
Class is used to create a terrain as a mesh object with the Diamond-Square Algorithm

2. CameraMovement.cs

3. LockMouse.cs

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

