//Code from https://docs.unity3d.com/ScriptReference/Cursor-lockState.html

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
		wantedMode = CursorLockMode.Locked;
		SetCursorState ();
	}

	void Update(){
		
		if (Input.GetKeyDown (KeyCode.Escape)) {
			wantedMode = CursorLockMode.None;
			SetCursorState ();
		}

		if (Input.GetMouseButtonDown (0)) {
			wantedMode = CursorLockMode.Locked;
			SetCursorState ();
		}
	}
	
}
