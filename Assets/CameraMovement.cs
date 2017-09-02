/*
 * Graphics and Interaction (COMP30019) Project 1
 * Team: Karim Khairat, Duy (Daniel) Vu, and Brody Taylor
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

	//Movement settings
	public float acceleration;
	public float decceleration;
	public float maxSpeed;
	public bool constantSpeed;

	//Rotation Settings
	public float rollSpeed;

	//Initial values
	private float forwardSpeed = 0f;
	private float sidewaysSpeed = 0f;

	//private Quaternion origRotation;
	private DiamondSquare1 planObject;

	private Vector3 origPosition;

	private float gap = 1f;
	float skyBound;
	float boundSize;




	void Start() {
		//freeze rotation of rigidbody
		/*
		if(GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;
			*/
		//origRotation = transform.localRotation;

		// Set intial position
		planObject = GameObject.Find ("Plane").GetComponent<DiamondSquare1>();
		origPosition = new Vector3(-planObject.mSize/2 ,planObject.mHeight+ planObject.mHeight/10, -planObject.mSize/2); 
		transform.localPosition = origPosition;

		// Set boundaries:
		this.boundSize = planObject.mSize / 2;
		this.skyBound = planObject.mHeight + planObject.mHeight/5;



	}

	//originally code from Workshop 2 solutions
	void Update() {

		//Positional Movement Input------------------------------CHANGE ARROWS TO WASD ------------------------------------\\
		if (constantSpeed) {
			forwardSpeed = 0f;
			sidewaysSpeed = 0f;
		}

		if (Input.GetKey (KeyCode.W)) {
			forwardSpeed += acceleration;
		} else if (Input.GetKey (KeyCode.S)) {
			forwardSpeed -= acceleration;
		} else {
			forwardSpeed = Mathf.Lerp (forwardSpeed, 0, Time.deltaTime*decceleration);
		}

		if (Input.GetKey (KeyCode.D)) {
			sidewaysSpeed+= acceleration;
		} else if (Input.GetKey (KeyCode.A)) {
			sidewaysSpeed-= acceleration;
		} else {
			sidewaysSpeed = Mathf.Lerp (sidewaysSpeed, 0, Time.deltaTime*decceleration);
		}

		forwardSpeed = Mathf.Clamp (forwardSpeed, -maxSpeed, maxSpeed);
		sidewaysSpeed = Mathf.Clamp (sidewaysSpeed, -maxSpeed, maxSpeed);

		//Apply position tranformations 
		this.transform.localPosition = bound( (this.transform.forward * forwardSpeed + this.transform.right * sidewaysSpeed) * Time.deltaTime);

		// Rotational Movement Input
		float rotZ=0f, rotY=0f, rotX=0f;

		if (Input.GetKey (KeyCode.E)) {
			rotZ += rollSpeed;
		} else if (Input.GetKey (KeyCode.Q)) {
			rotZ -= rollSpeed;
		} else {
			rotZ = Mathf.Lerp (0, 0, Time.deltaTime*decceleration);
		}

		Quaternion zQuat = Quaternion.AngleAxis (Time.deltaTime*rotZ, -Vector3.forward);

		// Mouse Look
		rotX += Input.GetAxis("Mouse X") * 5f;
		rotY += Input.GetAxis("Mouse Y") * 5f;

		Quaternion xQuat = Quaternion.AngleAxis (rotX, Vector3.up);
		Quaternion yQuat = Quaternion.AngleAxis (rotY, -Vector3.right);

		//Apply rotation tranformations 
		transform.localRotation *= zQuat;
		transform.localRotation *= xQuat;
		transform.localRotation *= yQuat;
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

}
