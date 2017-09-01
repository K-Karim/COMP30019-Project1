//Code from https://docs.unity3d.com/ScriptReference/Cursor-lockState.html
// Adapted for COMP30019 by Brody Taylor, Karim Khairat and Duy (Daniel) Vu
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
