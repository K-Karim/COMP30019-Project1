/*
 * Graphics and Interaction (COMP30019) Project 1
 * Team: Karim Khairat, Duy (Daniel) Vu, and Brody Taylor
 * 
 * Class dictates the cameras movement and rotation through input keys and mouse movement. 
 * All rotation and movement is done relative to the cameras orientation within restricted boundaries.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

	//Movement settings
	public float acceleration;
	public float decceleration;
	public float maxSpeed;

	//Rotation Settings
	public float rollSpeed;
	public float mouseSensitivityX;
	public float mouseSensitivityY;

	//Initial values
	private float forwardSpeed = 0f;
	private float sidewaysSpeed = 0f;

	// reference of terrain (plane) object
	private DiamondSquare1 planeObject;

	// Camera original position
	private Vector3 origPosition;

	private float gap = 1f;		// gap for detecting camera out of bound
	float skyBound;				// upper limit for camera
	float boundSize;			// surrounding limit for camera


	void Start() {

		// Set intial position
		planeObject = GameObject.Find ("Plane").GetComponent<DiamondSquare1>();
		origPosition = new Vector3(-planeObject.mSize/2 ,planeObject.mHeight+ planeObject.mHeight/10, -planeObject.mSize/2); 
		transform.localPosition = origPosition;

		// Set boundaries:
		this.boundSize = planeObject.mSize / 2;
		this.skyBound = planeObject.mHeight + planeObject.mHeight/5;

	}

	void Update() {

		//Applys position transformations based on user input
		movement ();

		//Applys rotational tranformations based on user input
		mouseLook ();

	}


	/// <summary>
	///		Project new position of camera by the movement 'movePos' and restrain within bound
	/// </summary>
	private Vector3 bound(Vector3 movePos) {
		// new position of camera after the move
		Vector3 newPos = movePos + gameObject.transform.localPosition;

		// Keep the camera inside the terrain's bounds
		if (newPos.x < -boundSize + gap) {
			newPos.x = -boundSize + gap;
		}
		if (newPos.z < -boundSize + gap) {
			newPos.z = -boundSize + gap;
		}
		if (newPos.x > boundSize - gap) {
			newPos.x = boundSize - gap;
		}
		if (newPos.z > boundSize - gap) {
			newPos.z = boundSize - gap;
		}

		// Keep the camera below sky bound
		if (newPos.y > skyBound - gap) {
			newPos.y = skyBound - gap;	
		}

		return newPos;
	}

	/// <summary>
	///		moves current transform based on keyboard inputs
	/// </summary>
	private void movement(){
		//Gets forward movement based on user input
		if (Input.GetKey (KeyCode.W)) {
			forwardSpeed += acceleration;
		} else if (Input.GetKey (KeyCode.S)) {
			forwardSpeed -= acceleration;
		} else {
			//Linearly decelerates speed as no keys being pressed
			forwardSpeed = Mathf.Lerp (forwardSpeed, 0, Time.deltaTime*decceleration);
		}

		//Gets sidesways movement bas
		if (Input.GetKey (KeyCode.D)) {
			sidewaysSpeed+= acceleration;
		} else if (Input.GetKey (KeyCode.A)) {
			sidewaysSpeed-= acceleration;
		} else {
			//Linearly decelerates speed
			sidewaysSpeed = Mathf.Lerp (sidewaysSpeed, 0, Time.deltaTime*decceleration);
		}

		// Keeps speed within maximum defined limits
		forwardSpeed = Mathf.Clamp (forwardSpeed, -maxSpeed, maxSpeed);
		sidewaysSpeed = Mathf.Clamp (sidewaysSpeed, -maxSpeed, maxSpeed);

		//Apply position tranformations 
		this.transform.localPosition = bound( (this.transform.forward * forwardSpeed + this.transform.right * sidewaysSpeed) * Time.deltaTime);
	}

	/// <summary>
	///		Rotates current transform based on mouse and keyboard inputs
	/// </summary>
	private void mouseLook(){
		//Inital value
		Quaternion rotation = Quaternion.identity;

		// Roll
		if (Input.GetKey (KeyCode.E)) {
			rotation *= Quaternion.AngleAxis (-rollSpeed * Time.deltaTime, Vector3.forward);
		} else if (Input.GetKey (KeyCode.Q)) {
			rotation *= Quaternion.AngleAxis (rollSpeed * Time.deltaTime, Vector3.forward);
		}

		// Mouse Look
		float rotX = Input.GetAxis("Mouse X")*mouseSensitivityX;
		float rotY = Input.GetAxis("Mouse Y")*mouseSensitivityY;
		rotation *= Quaternion.AngleAxis (rotX, Vector3.up);
		rotation *= Quaternion.AngleAxis (rotY, -Vector3.right);

		//Apply rotation tranformations 
		transform.rotation *= rotation;
	}

}
