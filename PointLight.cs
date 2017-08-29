//Base code from workshop 4. Added update and rotation.
using UnityEngine;
using System.Collections;

public class PointLight : MonoBehaviour {

    public Color color;
    // Rotation will be used as a speed element for the rotation (aka sunsetting and rising)
    private float rotation; 
    //change value below to modify rotation speed!
    private static int multiplier = 20;
    public Vector3 GetWorldPosition()
    {
        return this.transform.position;
    }
    //update this every frame
   public void Update()
    {
        //Time.deltaTime gets the time between frames
        rotation = Time.deltaTime *multiplier; //The greater the number the faster the speed.
        transform.RotateAround(Vector3.zero, new Vector3(1,0,0),rotation);
    }
}
