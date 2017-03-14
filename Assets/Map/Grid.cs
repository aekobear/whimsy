using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    public float seed;
    public int width;
    public int height;
    public int cellWidth;
    public int cellHeight;

    public TileMap tileMap;

    public Player player;
    public GameObject p_tile;

    public ItemMap itemMap;

    private Tile[,] tiles;
    private Item[,] items;

    private int world_x;
    private int world_y;

    private float perlinSeed;

	// Use this for initialization
	void Start () {

        world_x = 0;
        world_y = 0;

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
   
    public void Place(Vector2 target, Item item)
    {
        int x = Mathf.RoundToInt(target.x);
        int y = Mathf.FloorToInt(target.y);
        item.transform.position = new Vector2(x, y);
        var success = itemMap.Add(x, y, item);
    }

    public Item Take(Vector2 target)
    {
        int x = Mathf.RoundToInt(target.x);
        int y = Mathf.FloorToInt(target.y);
        return itemMap.Take(x, y);
    }

    public Item ItemAt(Vector2 target)
    {
        int x = Mathf.RoundToInt(target.x);
        int y = Mathf.FloorToInt(target.y);
        Item item = itemMap.ItemAt(x, y);
        return item;
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
            tile.render.sprite = tileMap.TileAt(tile.vx + world_x, tile.vy + world_y);
            /*Item item = itemMap.ItemAt(tile.vx + world_x, tile.vy + world_y);
            if(item != null)
            {
                item.transform.position = tile.transform.position;

            }*/
        }

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
