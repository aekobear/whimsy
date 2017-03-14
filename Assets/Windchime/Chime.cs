using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chime : MonoBehaviour {

    private AudioSource asource;

	// Use this for initialization
	void Start () {
        asource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Hit()
    {
        asource.Play();
    }
}
