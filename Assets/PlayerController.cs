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

    void OnApplicationQuit()
    {
        Debug.Log("QUITTING!");
        networkObject.Destroy();
    }
}
