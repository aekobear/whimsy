using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IPoint {

    public int X {
        set { transform.position = new Vector2((int)value, (int)transform.position.y); }
        get { return (int)transform.position.x; }
    }

    public int Y
    {
        set { transform.position = new Vector2((int)transform.position.x, (int)value); }
        get { return (int)transform.position.y; }
    }

    public virtual void Interact()
    {

    }

    public virtual void Interact(Grid grid, Vector2 target)
    {

    }
}
