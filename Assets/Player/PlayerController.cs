using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using System;

public class PlayerController : NessBehavior {

    public SpeechInput p_speechInput;
    public Grid grid;

    private Player player;
    private SpeechInput speechInput;

    private Item held;

    public override void MoveTo(RpcArgs args)
    {
        player.MoveTo(args.GetNext<Vector2>());
    }

    // Use this for initialization
    void Start () {
        player = GetComponent<Player>();
        speechInput = Instantiate<SpeechInput>(p_speechInput);
        speechInput.pc = this;
        Whimsy.Link(speechInput.transform, player.transform, -0.35f, 1.55f);
        held = null;
    }
	
	// Update is called once per frame
	void Update () {

        if (!networkObject.IsOwner)
        {
            return;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector2 destination = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            networkObject.SendRpc("MoveTo", Receivers.All, destination);
            //player.MoveTo(destination);
        }

        if (Input.GetMouseButtonUp(1))
        {
            Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (held == null)
            {
                if (grid.ItemAt(target) != null)
                {
                    networkObject.SendRpc("Take", Receivers.All, target);
                }
            }
            else
            {
                if (grid.ItemAt(target) == null)
                {
                    networkObject.SendRpc("Place", Receivers.All, target);
                }
            }
        }
        if (Input.GetMouseButtonUp(2))
        {
            Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            networkObject.SendRpc("Interact", Receivers.All, target);
        }

        if (Input.GetKeyDown(KeyCode.Return)){
            speechInput.Toggle();
        }
		
	}

    public void MakeSpeechBubble(string text)
    {
        networkObject.SendRpc("SpawnSpeechBubble", Receivers.All, text);
    }

    public override void SpawnSpeechBubble(RpcArgs args)
    {
        Instantiate<GameObject>(speechInput.p_speechBubble).GetComponent<SpeechBubble>().Spawn(args.GetNext<String>(), speechInput.transform, speechInput.displayTime, speechInput.driftSpeed);
    }

    public override void Take(RpcArgs args)
    {
        held = grid.Take(args.GetNext<Vector2>());
        if(held != null)
        {
            Whimsy.Link(held.transform, this.transform, 0, 1.50f);
        }
    }

    public override void Place(RpcArgs args)
    {
        Whimsy.Unlink(held.transform);
        if(grid.Place(args.GetNext<Vector2>(), held))
        {
            held = null;
        }
    }

    public override void Interact(RpcArgs args)
    {
        if(held != null)
        {
            held.OnInteract(grid, args.GetNext<Vector2>());
        }
        else
        {
            Item item = grid.ItemAt(args.GetNext<Vector2>());
            if(item != null)
            {
                item.OnInteract();
            }
        }
    }

    void OnApplicationQuit()
    {
        Debug.Log("QUITTING!");
        networkObject.Destroy();
    }
}
