using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float speed;

    private bool walking;
    private Vector2 destination;

    private Animator animator;

	// Use this for initialization
	void Start () {
        walking = false;

        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
        if (walking)
        {
            transform.position = Vector2.Lerp(transform.position, destination, speed / Vector2.Distance(transform.position, destination));
            if (destination == (Vector2)transform.position)
            {
                walking = false;
                animator.speed = 0;
            }
        }
    }

    public void MoveTo(Vector2 destination)
    {
        this.destination = destination;
        walking = true;
        float direction = AngleInDeg((Vector2)transform.position, destination);
        animator.SetFloat("direction", direction);
        if(direction > 112.5f && direction < 247.5f)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        animator.speed = 1;
    }

    public static float AngleInRad(Vector2 vec1, Vector2 vec2)
    {
        return Mathf.Atan2(vec2.y - vec1.y, vec2.x - vec1.x);
    }

    public static float AngleInDeg(Vector2 vec1, Vector2 vec2)
    {
        return (AngleInRad(vec1, vec2) * 180 / Mathf.PI) + 180.0f;
    }
}
