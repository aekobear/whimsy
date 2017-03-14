using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour {

    public UnityEngine.UI.Text text;
    public GameObject bubble;

    private float time;
    private float speed;
    private TransformLink link;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if(link == null)
        {
            return;
        }

        link.yOffset += Time.deltaTime * speed;

        time -= Time.deltaTime;

        if (time < 0)
        {
            Destroy(gameObject);
        }
	}

    public void Spawn(string text, Transform target, float time, float speed)
    {
        this.text.text = text;
        this.time = time;
        this.speed = speed;
        link = Whimsy.Link(transform, target, 0, 0);   
    }
}
