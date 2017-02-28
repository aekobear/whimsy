using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    public float seed;
    public int width;
    public int height;
    public int cellWidth;
    public int cellheight;

    public TileMap tilemap;
    public ItemMap itemmap;

    public Player player;
    public GameObject p_tile;

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
            tile.render.sprite = tilemap.TileAt(tile.vx + world_x, tile.vy + world_y);
            Item p_item = itemmap.ItemAt(tile.vx + world_x, tile.vy + world_y);
            if(p_item != null)
            {
                Item item = Instantiate<Item>(p_item);
                item.transform.position = tile.transform.position;

            }
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
