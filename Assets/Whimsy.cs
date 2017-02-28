using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Unity;

public class Whimsy : MonoBehaviour {

    public Grid p_grid;
    public Windchime p_windchime;

	// Use this for initialization
	void Start () {
        if (NetworkManager.Instance.IsServer)
        {
            return;
        }
        Transform player = NetworkManager.Instance.InstantiateNessNetworkObject().transform;

        Whimsy.Link(Camera.main.transform, player, 0, 0);

        Grid grid = Instantiate<Grid>(p_grid);
        grid.player = player.GetComponent<Player>();
        player.GetComponent<PlayerController>().grid = grid;

        Windchime windchime = Instantiate<Windchime>(p_windchime);
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

    void OnApplicationQuit()
    {
        NetworkManager.Instance.Networker.Disconnect(false);
    }
}
