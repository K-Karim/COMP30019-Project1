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
	private Quaternion origRotation;

	void Start() {
		//freeze rotation of rigidbody
		/*
		if(GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;
			*/

		origRotation = transform.localRotation;
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



		//Rotational Movement Input

		float rotZ=0f;
		if (Input.GetKey (KeyCode.E)) {
			rotZ += rollSpeed;
		} else if (Input.GetKey (KeyCode.Q)) {
			rotZ -= rollSpeed;
		}

		Quaternion zQuat = Quaternion.AngleAxis (Time.deltaTime*rotZ, -Vector3.forward);

		//Apply tranformations 
		this.transform.localPosition += (this.transform.forward * forwardSpeed + this.transform.right * sidewaysSpeed) * Time.deltaTime;
		transform.localRotation *= zQuat;


		float rotY=0f;
		float rotX=0f;

		//Mouse Look
		rotX += Input.GetAxis("Mouse X") * 5f;
		rotY += Input.GetAxis("Mouse Y") * 5f;

		Quaternion xQuat = Quaternion.AngleAxis (rotX, Vector3.up);
		Quaternion yQuat = Quaternion.AngleAxis (rotY, -Vector3.right);


		//Apply tranformations 
		transform.localRotation *= xQuat;
		transform.localRotation *= yQuat;
	}
}
