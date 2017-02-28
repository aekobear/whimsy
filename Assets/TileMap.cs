using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour {

    public float perlinSeed;

    public Sprite grass;

    void Start()
    {

    }

    public Sprite TileAt(int x, int y)
    {
        if(GrassAt(x, y))
        {
            return grass;
        }
        else
        {
            return null;
        }
    }

    public bool GrassAt(int x, int y)
    {
        float perlinNumber = Mathf.PerlinNoise(perlinSeed * x, perlinSeed * y);
        perlinNumber = (int)(perlinNumber * 10);

        float randomNumber = RandomNoise(x, y);

        if ((perlinNumber % 2 == 0 && randomNumber > 0.3f) || randomNumber < 0.1f)
        {
            return true;
        }
        else
        {
            return false;
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
