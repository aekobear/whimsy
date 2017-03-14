using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float speed;

    private bool walking;
    private Vector2 destination;
    private Rigidbody2D body;
    private SpriteRenderer sprite;

    private Animator animator;

	// Use this for initialization
	void Start () {
        walking = false;

        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void LateUpdate()
    {
        sprite.sortingOrder = (int)Camera.main.WorldToScreenPoint(sprite.bounds.min).y * -1;
    }

    void FixedUpdate()
    {
        if (walking)
        {
            if ((destination - (Vector2)transform.position).magnitude < 0.1f)
            {
                walking = false;
                animator.speed = 0;
                body.velocity = Vector2.zero;
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
        body.velocity = (Vector2)(Quaternion.Euler(0, 0, animator.GetFloat("direction")) * Vector2.left * speed);
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
