/*
 * Graphics and Interaction (COMP30019) Project 1
 * Team: Karim Khairat, Duy (Daniel) Vu, and Brody Taylor
 * 
<<<<<<< HEAD
 * Code Adapted from other sources: Workshop 2 solutions
=======
 * Class is used to control the view of camera as a flying object within restricted boundaries
>>>>>>> master
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

	private DiamondSquare1 planObject;

	private Vector3 origPosition;

	private float gap = 1f;
	float skyBound;
	float boundSize;




	void Start() {

		// Set intial position
		planObject = GameObject.Find ("Plane").GetComponent<DiamondSquare1>();
		origPosition = new Vector3(-planObject.mSize/2 ,planObject.mHeight+ planObject.mHeight/10, -planObject.mSize/2); 
		transform.localPosition = origPosition;

		// Set boundaries:
		this.boundSize = planObject.mSize / 2;
		this.skyBound = planObject.mHeight + planObject.mHeight/5;



	}

	void Update() {

		//Applys position transformations based on user input
		movement ();

		//Applys rotational tranformations based on user input
		mouseLook ();


	}


	// Project new position of camera by the movement 'movePos' and restrain within bound
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

	//Movement based on user input
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

	//Rotations based on mouse and keyboard inputs
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
