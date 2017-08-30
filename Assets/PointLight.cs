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
        //Vector3.right is the same as new Vector3(1,0,0) (x axis is constant, sun rotates around z and y).
        transform.RotateAround(Vector3.zero, Vector3.right,rotation);
        transform.LookAt(Vector3.zero);// transform at every frame to ensure light is looking at (0,0,0)
    }
}