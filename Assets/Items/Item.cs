using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour, IPoint {

    public int X {
        set { transform.position = new Vector2((int)value, (int)transform.position.y); }
        get { return (int)transform.position.x; }
    }

    public int Y
    {
        set { transform.position = new Vector2((int)transform.position.x, (int)value); }
        get { return (int)transform.position.y; }
    }

    private SpriteRenderer sprite;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        sprite.sortingOrder = (int)Camera.main.WorldToScreenPoint(sprite.bounds.min).y * -1;
    }

    public virtual void OnInteract() { }

    public virtual void OnInteract(Grid grid, Vector2 target) { }

    public virtual bool OnPlace(Grid grid, Vector2 target)
    {
        return true;
    }

    public virtual bool OnTake(Grid grid)
    {
        return true;
    }
}
