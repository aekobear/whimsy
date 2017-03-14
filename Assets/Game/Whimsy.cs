using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine.SceneManagement;

public class Whimsy : MonoBehaviour {

    public Grid p_grid;
    public Windchime p_windchime;

    public Seedbag p_seedbag;

    private Grid grid;

    private bool offlineMode = true;

	// Use this for initialization
	void Start () {

        if(NetworkManager.Instance == null)
        {
            SceneManager.LoadScene(0);
        }

        if (NetworkManager.Instance.IsServer && !offlineMode)
        {
            return;
        }
        Transform player = NetworkManager.Instance.InstantiateNessNetworkObject().transform;

        Whimsy.Link(Camera.main.transform, player, 0, 0);

        Grid grid = Instantiate<Grid>(p_grid);
        this.grid = grid;
        grid.player = player.GetComponent<Player>();
        player.GetComponent<PlayerController>().grid = grid;

        Windchime windchime = Instantiate<Windchime>(p_windchime);

        StartCoroutine(PopulateGrid());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static TransformLink Link(Transform t, Transform target, float x, float y)
    {
        TransformLink link = t.gameObject.AddComponent<TransformLink>();
        link.target = target;
        link.x = true;
        link.y = true;
        link.xOffset = x;
        link.yOffset = y;
        return link;
    }

    public static bool Unlink(Transform t)
    {
        var link = t.GetComponent<TransformLink>();
        if(link == null)
        {
            return false;
        }
        Destroy(link);
        return true;
    }

    private IEnumerator PopulateGrid()
    {
        yield return new WaitForSeconds(1);

        grid.Place(new Vector2(0, 0), Instantiate<Item>(p_seedbag));
        grid.Place(new Vector2(1, 0), Instantiate<Item>(p_seedbag));
    }

    void OnApplicationQuit()
    {
        NetworkManager.Instance.Networker.Disconnect(false);
    }
}
