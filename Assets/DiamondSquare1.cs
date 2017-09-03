/*
 * Graphics and Interaction (COMP30019) Project 1
 * Team: Karim Khairat, Duy (Daniel) Vu, and Brody Taylor
 *
 * Class is used to create a terrain as a mesh object with the Diamond-Square Algorithm
 * Code adapted from other sources: https://www.youtube.com/watch?v=1HV8GbFnCik
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondSquare1 : MonoBehaviour {

    public int mDivisions = 128;    // maximum number of faces (or lines) on one dimension: has to be 2^n
	public float mSize = 200;       // maximum size of terrain
    public float mHeight = 50;      // maximum height of terrain
	public float roughness = 0.4f;  // rate of change of height for every iteration of the Diamond-Square algorithm
	public int seed= 50;			// seed for random value

	public PointLight pointLight;
	public Shader shader;

	float minHeight = 0f;	// Minimum height of terrain


	Vector3[] mVerts;		// Array for all vertices of the terrain mesh
	Vector2[] uvs;			// Base texture coordinates of the terrain mesh
    int mVertCount;			// Maximum numnber of vertices


    void Start()
    {
		// Set initial seed for random value
		Random.InitState(seed);

		// Set middle point of the terrain to be at (0,0,0)
		gameObject.transform.position = new Vector3 (0, 0, 0);

		// Create mesh object for terrain
		MeshFilter planeMesh = this.gameObject.GetComponent<MeshFilter>();
		planeMesh.mesh = this.CreateTerrain();

		// Add a MeshRenderer component. This component actually renders the mesh that
		// is defined by the MeshFilter component.
		MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();
		renderer.material.shader = shader;


		// Add MeshCollider for mesh object to avoid camera object
		this.gameObject.AddComponent(typeof(MeshCollider));
		transform.gameObject.AddComponent<MeshCollider>();
		GetComponent<MeshCollider>().sharedMesh = planeMesh.mesh;

		// Set frame rate of application
		Application.targetFrameRate = 30;

    }

	// Called each frame
	void Update()
	{

		// Update point light (sun)
		pointLight.Update ();

		// Get renderer component (in order to pass params to shader)
		MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();

		// Pass updated light positions to shader
		renderer.material.SetColor("_PointLightColor", this.pointLight.color);
		renderer.material.SetVector("_PointLightPosition", this.pointLight.GetWorldPosition());
	}



	/// <summary>
	/// 	Create terrain object as a mesh object
	/// </summary>
	/// <returns> mesh object of terrain </returns>
	Mesh CreateTerrain()
    {
		// Number of vertices = number of faces +1. Need total (2^n + 1) vertices => (2^n) faces
        mVertCount = (mDivisions + 1) * (mDivisions + 1);
        mVerts = new Vector3[mVertCount];
        uvs = new Vector2[mVertCount];

		// Array of all points creating triangles for the terrain:
		// Total number of squares = mDivisions * mDivisions
		// One square = 2 * triangles = 2 * ( 3 * points) = 6 points (or numbers)
        int[] tris = new int[mDivisions * mDivisions * 6];
		int triOffset = 0;	// index of triangle for drawing squares

        float halfSize = mSize * 0.5f;			 // half the maximum size of the terrain
		float divisionSize = mSize / mDivisions; // size of each division (face)

        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;


		/* Plot vertices for mesh in order of left to right, top to bottom with center to be (0,0,0)
		i and j corresponding to row and column respectively in the 0xz plane
		Starting from topleft position to be (-halfsize, 0, halfsize)
		*/
        for (int i = 0; i <= mDivisions; i++)
        {
            for (int j = 0; j <= mDivisions; j++)
            {
                mVerts[i * (mDivisions + 1) + j] = new Vector3(-halfSize + j * divisionSize, 0.0f, halfSize - i * divisionSize);
                uvs[i * (mDivisions + 1) + j] = new Vector2((float)i / mDivisions, (float)j / mDivisions);

				/* Draw triangles for mesh */
                if (i < mDivisions && j < mDivisions)
				{
					/* At each vertex (except those positioned at row (or col) = mDivisions), draw 2 triangles to represent a square.
					* Square contains 4 vertices, top left & right, bottom left & right.
					* Index of Top left and Bottom left vertices of the current square (face) are: */
                    int topLeft = i * (mDivisions + 1) + j;
                    int botLeft = (i + 1) * (mDivisions + 1) + j;

					/* Order of vertices drawing: Top left -> Top right -> Bottom right */
                    tris[triOffset] = topLeft;
                    tris[triOffset + 1] = topLeft + 1;
                    tris[triOffset + 2] = botLeft + 1;

					/* Order of vertices drawing: Top left --> Bottom right --> Bottom left */
                    tris[triOffset + 3] = topLeft;
                    tris[triOffset + 4] = botLeft + 1;
                    tris[triOffset + 5] = botLeft;

					/* Increase index for next square drawing */
                    triOffset += 6;
                }
            }
        }


		/*
		 * Initialize for corner with random value and normalise the value to be higher or equal to 'minHeight'
		 */
		mVerts[0].y = Mathf.Max(minHeight, Random.Range(-mHeight, mHeight));
		mVerts[mDivisions].y = Mathf.Max(minHeight, Random.Range(-mHeight, mHeight));
		mVerts[mVerts.Length - 1].y = Mathf.Max(minHeight, Random.Range(-mHeight, mHeight));
		mVerts[mVerts.Length - 1 - mDivisions].y = Mathf.Max(minHeight, Random.Range(-mHeight, mHeight));

		/* Diamond-Square Algorithms */
		int iterations = (int)Mathf.Log(mDivisions, 2); 	// total number of iteration to run (Diamond-Square) pairs
		int numSquares = 1;				// number of squares in the iteration (first iteration has 1 big square)
		int squareSize = mDivisions;	// size of the square in the iteration (first iteration uses smallest squares)

		for (int i = 0; i < iterations; i++)
        {
            int row = 0;

            for (int j = 0; j < numSquares; j++)
            {
                int col = 0;

                for (int k = 0; k < numSquares; k++)
                {
					// Perform the Diamond-Square algorithm on the current square
                    DiamondSquare(row, col, squareSize, mHeight);
                    col += squareSize;
                }
                row += squareSize;
            }
            numSquares *= 2;		// double number of square in next iteration
            squareSize /= 2;		// square size half in next iteration
			mHeight *= roughness;	// magnitude of the random height value should be reduced at each iteration
        }

		/* setup mesh parameters */
        mesh.vertices = mVerts;
		mesh.uv = uvs;
        mesh.triangles = tris;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

		return mesh;
    }



	/// <summary>
	///		Perform Diamond step and Square step in one square
	/// </summary>
	/// <param name="row"> row the square at</param>
	/// <param name="col"> column the square at </param>
	/// <param name="size"> size of one side of the square (also step size to iterate)</param>
	/// <param name="offset"> offset value that height can be added </param>
    void DiamondSquare(int row, int col, int size, float offset)
    {
        int halfSize = (int)(size * 0.5f);

		// Index of the Top-left point of the square in triangle array
        int topLeft = row * (mDivisions + 1) + col;

		// Index of the Bottom-left point of the square in triangle array (step size between 4 corners is 'size')
		int botLeft = (row + size) * (mDivisions + 1) + col;
		 
		/* 
		 * Diamond step: find the midpoint of the square 
		 * and set the height value to be average of four corners'value plus a random value
		 * Extra: normalise the value to be higher or equal to 'minHeight'
		 */
		// Index of the Middle point of the square in triangle array
        int mid = (int)(row + halfSize) * (mDivisions + 1) + (int)(col + halfSize);

		// Calculate height of midpoint
		mVerts[mid].y = Mathf.Max((mVerts[topLeft].y + mVerts[topLeft + size].y + mVerts[botLeft].y + mVerts[botLeft + size].y) * 0.25f + Random.Range(-offset, offset), minHeight);

		/* 
		 * Square step: find mid point of '3-point-diamond' made from different composition of 4 square's corners and midpoint 
		 * and set the height value to be the average of just the three adjacent values
		 * Extra: normalise the value to be higher or equal to 'minHeight'
		 */
		mVerts[topLeft + halfSize].y = Mathf.Max((mVerts[topLeft].y + mVerts[topLeft + size].y + mVerts[mid].y) / 3 + Random.Range(-offset, offset), minHeight);
		mVerts[mid - halfSize].y = Mathf.Max((mVerts[topLeft].y + mVerts[botLeft].y + mVerts[mid].y) / 3 + Random.Range(-offset, offset),minHeight);
		mVerts[mid + halfSize].y = Mathf.Max((mVerts[topLeft + size].y + mVerts[botLeft + size].y + mVerts[mid].y) / 3 + Random.Range(-offset, offset),minHeight);
		mVerts[botLeft + halfSize].y = Mathf.Max((mVerts[botLeft].y + mVerts[botLeft + size].y + mVerts[mid].y) / 3 + Random.Range(-offset, offset),minHeight);
    }


}
