/*
 * Graphics and Interaction (COMP30019) Project 1
 * Team: Karim Khairat, Duy (Daniel) Vu, and Brody Taylor
 * 
 * Stops mouse from moving outside of play area and prevents rotation from being hindered by displays borders. 
 * Can be unlocked using the 'esc' key and locked again by clicking on the screen.
 * Code adapted from https://docs.unity3d.com/ScriptReference/Cursor-lockState.html
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockMouse : MonoBehaviour {


	CursorLockMode wantedMode;

	// Apply requested cursor state
	void SetCursorState()
	{
		Cursor.lockState = wantedMode;
		// Hide cursor when locking
		Cursor.visible = (CursorLockMode.Locked != wantedMode);
	}

	void Start(){
		// to start we want a locked cursor and invisible cursor
		wantedMode = CursorLockMode.Locked;
		SetCursorState ();
	}

	void Update(){
		//Pressing escape allows you to unlock cursor.
		if (Input.GetKeyDown (KeyCode.Escape)) {
			wantedMode = CursorLockMode.None;
			SetCursorState ();
		}
		//Left click allows you to (re-)lock cursor if it has been unlocked
		if (Input.GetMouseButtonDown (0)) {
			wantedMode = CursorLockMode.Locked;
			SetCursorState ();
		}
	}
	
}
