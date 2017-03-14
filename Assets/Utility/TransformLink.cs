using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformLink : MonoBehaviour {

    public Transform target;
    public bool x, y, z;
    public float xOffset;
    public float yOffset;
    public float zOffset;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        float x = (this.x) ? target.position.x + xOffset : transform.position.x;
        float y = (this.y) ? target.position.y + yOffset : transform.position.y;
        float z = (this.z) ? target.position.z + zOffset : transform.position.z;

        transform.position = new Vector3(x, y, z);

    }
}
