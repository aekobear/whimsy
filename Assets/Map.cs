using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    public float seed;
    public int width;
    public int height;
    public int cellWidth;
    public int cellheight;

    public Player player;
    public GameObject p_tile;

    public Sprite grass;

    private Tile[,] tiles;

    private int world_x;
    private int world_y;

    private float perlinSeed;

	// Use this for initialization
	void Start () {

        world_x = 0;
        world_y = 0;

        perlinSeed = 0.012f;
        

        tiles = new Tile[width*2, height*2];

        for (int x = 0; x < width*2; x++)
        {
            for (int y = 0; y < height*2; y++)
            {
                Tile tile = Instantiate<GameObject>(p_tile).GetComponent<Tile>();
                tile.vx = x-width;
                tile.vy = y-height;
                tile.transform.parent = this.transform;
                tile.transform.localPosition = new Vector2(x-width, y-height);
                tiles[x, y] = tile;
            }
        }

    }
	
	// Update is called once per frame
	void Update () {

        float xDistance = player.transform.position.x - transform.position.x;
        float yDistance = player.transform.position.y - transform.position.y;

        float xOffset = xDistance - (int)xDistance;
        float yOffset = yDistance - (int)yDistance;

        world_x += (int)xDistance;
        world_y += (int)yDistance;

        transform.position = new Vector2(player.transform.position.x - xOffset, player.transform.position.y - yOffset);

        foreach (Tile tile in EachTile()){
            float perlinNumber = Mathf.PerlinNoise(perlinSeed * (tile.vx + world_x), perlinSeed * (tile.vy + world_y));
            perlinNumber = (int)(perlinNumber * 10);
            float randomNumber = RandomNoise(tile.vx + world_x, tile.vy + world_y);
            if ((perlinNumber % 2 == 0 && randomNumber > 0.3f) || randomNumber < 0.1f)
            {
                tile.render.sprite = grass;
            }
            else
            {
                tile.render.sprite = null;
            }
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

    public IEnumerable<Tile> EachTile()
    {
        for(int x = 0; x < width*2; x++)
        {
            for(int y = 0; y < height*2; y++)
            {
                yield return tiles[x, y];
            }
        }
        yield break;
    }
}
