using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    public int vx;
    public int vy;

    public SpriteRenderer render;

	// Use this for initialization
	void Start () {
        render = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
