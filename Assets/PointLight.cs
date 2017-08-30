using UnityEngine;
using System.Collections;

public class PointLight : MonoBehaviour {

    public Color color;
	private float rotation;
    public Vector3 GetWorldPosition()
    {
        return this.transform.position;
    }
	public void Update(){
	



		rotation = Time.deltaTime*15;
		transform.RotateAround (Vector3.zero, new Vector3(1,0,0), rotation);
		transform.LookAt (Vector3.zero);
	}

}
