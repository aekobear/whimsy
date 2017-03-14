using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMap : MonoBehaviour {

    public int activeRange;
    public int preloadRange;

    public Vector2 currentLocation;

    public float flowerRate;
    public Item p_flower;

    private QuadList _quadlist;

    void Start()
    {
        _quadlist = new QuadList();
    }

    public bool Add(int x, int y, Item item)
    {
        item.X = x;
        item.Y = y;
        return quadList().Insert(item);
    }

    public Item Take(int x, int y)
    {
        return (Item)quadList().Pop(x, y);
    }

	public Item ItemAt(int x, int y)
    {
        return (Item)quadList().Find(x, y);
    }

    private float RandomNoise(float x, float y)
    {
        Random.InitState((int)(4223 * x));
        float a = Random.Range(0f, 1f);
        Random.InitState((int)(3229f * y));
        float b = Random.Range(0f, 1f);
        return a * b;
    }

    private QuadList quadList()
    {
        _quadlist = _quadlist.Head();
        return _quadlist;
    }
}

