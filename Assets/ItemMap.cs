using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMap : MonoBehaviour {

    public int activeRange;
    public int preloadRange;

    public Vector2 currentLocation;

    public float flowerRate;
    public Item p_flower;

	public Item ItemAt(int x, int y)
    {
        if (RandomNoise(x, y) <= flowerRate)
        {
            return p_flower;
        }
        else
        {
            return null;
        }
    }

    private float RandomNoise(float x, float y)
    {
        Random.InitState((int)(4223 * x));
        float a = Random.Range(0f, 1f);
        Random.InitState((int)(3229f * y));
        float b = Random.Range(0f, 1f);
        return a * b;
    }
}
