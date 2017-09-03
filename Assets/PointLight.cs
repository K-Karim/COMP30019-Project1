/*
 * Graphics and Interaction (COMP30019) Project 1
 * Team: Karim Khairat, Duy (Daniel) Vu, and Brody Taylor
 * 
 * Class is used to create and control behaviour of the sun (point light)
 * Code adapted from COMP30019: Workshop 4. Added update and rotation.
 */

using UnityEngine;
using System.Collections;

public class PointLight : MonoBehaviour {

    public Color color;

    // Rotation will be used as a speed element for the rotation (aka sunsetting and rising)
    private float rotation; 

    // Multiplier value to modify rotation speed!
    private static int multiplier = 10;

    public Vector3 GetWorldPosition()
    {
        return this.transform.position;
    }
	/// <summary>
	///		Initialize
	/// </summary>
	void Start()
	{
		// Set initial rotation radius
		DiamondSquare1 planObject = GameObject.Find ("Plane").GetComponent<DiamondSquare1>();
		gameObject.transform.position = new Vector3(0, planObject.mSize/2, -planObject.mSize/2);
	}

	/// <summary>
	///		Perform Rotation and keep the sun facing the origin
	/// </summary>
    {
        // Time.deltaTime gets the time between frames
        rotation = Time.deltaTime * multiplier; //The greater the multiplier the faster the speed.

        // Vector3.right is the same as new Vector3(1,0,0) (x axis is constant, sun rotates around z and y).
        transform.RotateAround(Vector3.zero, Vector3.right, rotation);

	// Transform at every frame to ensure light is looking at (0,0,0)
        transform.LookAt(Vector3.zero);
    }
}
