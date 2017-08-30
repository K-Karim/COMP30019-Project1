using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondSquare1 : MonoBehaviour {

    public int mDivisions;
    public float mSize;
    public float mHeight;

	public float roughness = 0.4f;
	float minHeight = 0f;

	public PointLight pointLight;
	public Shader shader;

	public int seed;




    Vector3[] mVerts;
    int mVertCount;

    void Start()
    {
//        CreateTerrain();

		Random.InitState(seed);

		MeshFilter planeMesh = this.gameObject.AddComponent<MeshFilter>();
		planeMesh.mesh = this.CreateTerrain();

		// Add a MeshRenderer component. This component actually renders the mesh that
		// is defined by the MeshFilter component.
		MeshRenderer renderer = this.gameObject.AddComponent<MeshRenderer>();
		renderer.material.shader = shader;

		Application.targetFrameRate = 30;

    }

	// Called each frame
	void Update()
	{
		pointLight.Update ();

		// Get renderer component (in order to pass params to shader)
		MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();


		// Pass updated light positions to shader
		renderer.material.SetColor("_PointLightColor", this.pointLight.color);
		renderer.material.SetVector("_PointLightPosition", this.pointLight.GetWorldPosition());
	}

	Mesh CreateTerrain()
    {


        mVertCount = (mDivisions + 1) * (mDivisions + 1);
        mVerts = new Vector3[mVertCount];
        Vector2[] uvs = new Vector2[mVertCount];
        int[] tris = new int[mDivisions * mDivisions * 6];

        float halfSize = mSize * 0.5f;
        float divisionSize = mSize / mDivisions;

        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        int triOffset = 0;


        for (int i = 0; i <= mDivisions; i++)
        {
            for (int j = 0; j <= mDivisions; j++)
            {
                mVerts[i * (mDivisions + 1) + j] = new Vector3(-halfSize + j * divisionSize, 0.0f, halfSize - i * divisionSize);
                uvs[i * (mDivisions + 1) + j] = new Vector2((float)i / mDivisions, (float)j / mDivisions);

                if (i < mDivisions && j < mDivisions)
                {
                    int topLeft = i * (mDivisions + 1) + j;
                    int botLeft = (i + 1) * (mDivisions + 1) + j;

                    tris[triOffset] = topLeft;
                    tris[triOffset + 1] = topLeft + 1;
                    tris[triOffset + 2] = botLeft + 1;

                    tris[triOffset + 3] = topLeft;
                    tris[triOffset + 4] = botLeft + 1;
                    tris[triOffset + 5] = botLeft;

                    triOffset += 6;
                }
            }
        }

		mVerts[0].y = Mathf.Max(minHeight, Random.Range(-mHeight, mHeight));
		mVerts[mDivisions].y = Mathf.Max(minHeight, Random.Range(-mHeight, mHeight));
		mVerts[mVerts.Length - 1].y = Mathf.Max(minHeight, Random.Range(-mHeight, mHeight));
		mVerts[mVerts.Length - 1 - mDivisions].y = Mathf.Max(minHeight, Random.Range(-mHeight, mHeight));

        int iterations = (int)Mathf.Log(mDivisions, 2);
        int numSquares = 1;
        int squareSize = mDivisions;
        for (int i = 0; i < iterations; i++)
        {
            int row = 0;

            for (int j = 0; j < numSquares; j++)
            {
                int col = 0;

                for (int k = 0; k < numSquares; k++)
                {
                    DiamondSquare(row, col, squareSize, mHeight);
                    col += squareSize;
                }
                row += squareSize;
            }
            numSquares *= 2;
            squareSize /= 2;
            mHeight *= roughness;
        }





        mesh.vertices = mVerts;


        mesh.uv = uvs;
        mesh.triangles = tris;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        /*MeshCollider meshc = gameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;
        meshc.sharedMesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;*/
//        gameObject.AddComponent(typeof(MeshCollider));
//        transform.gameObject.AddComponent<MeshCollider>();
//        GetComponent<MeshCollider>().sharedMesh = mesh    ;

		return mesh; 
    }

    void DiamondSquare(int row, int col, int size, float offset)
    {
        int halfSize = (int)(size * 0.5f);
        int topLeft = row * (mDivisions + 1) + col;
        int botLeft = (row + size) * (mDivisions + 1) + col;

        int mid = (int)(row + halfSize) * (mDivisions + 1) + (int)(col + halfSize);
		mVerts[mid].y = Mathf.Max((mVerts[topLeft].y + mVerts[topLeft + size].y + mVerts[botLeft].y + mVerts[botLeft + size].y) * 0.25f + Random.Range(-offset, offset), minHeight);


		mVerts[topLeft + halfSize].y = Mathf.Max((mVerts[topLeft].y + mVerts[topLeft + size].y + mVerts[mid].y) / 3 + Random.Range(-offset, offset), minHeight);
		mVerts[mid - halfSize].y = Mathf.Max((mVerts[topLeft].y + mVerts[botLeft].y + mVerts[mid].y) / 3 + Random.Range(-offset, offset),minHeight);
		mVerts[mid + halfSize].y = Mathf.Max((mVerts[topLeft + size].y + mVerts[botLeft + size].y + mVerts[mid].y) / 3 + Random.Range(-offset, offset),0);
		mVerts[botLeft + halfSize].y = Mathf.Max((mVerts[botLeft].y + mVerts[botLeft + size].y + mVerts[mid].y) / 3 + Random.Range(-offset, offset),0);
    }
		
}
