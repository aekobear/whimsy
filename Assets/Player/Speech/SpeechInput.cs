using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking;
using System;

public class SpeechInput : MonoBehaviour {

    public float displayTime;
    public float driftSpeed;

    public UnityEngine.UI.Text text;
    public GameObject image;
    public GameObject p_speechBubble;

    public PlayerController pc;

    private bool active;

	// Use this for initialization
	void Start () {
        Deactivate();
	}
	
	// Update is called once per frame
	void OnGUI () {

        if (!active)
        {
            return;
        }

        Event e = Event.current;
        if (e.isKey && e.type == EventType.KeyDown)
        {
            if(e.keyCode >= KeyCode.A && e.keyCode <= KeyCode.Z)
            {
                if (e.shift)
                {
                    text.text += e.keyCode.ToString();
                }
                else
                {
                    text.text += e.keyCode.ToString().ToLower();
                }
              
            }
            else if(e.keyCode == KeyCode.Space)
            {
                text.text += " ";
            }
            else if(e.keyCode == KeyCode.Period)
            {
                text.text += ".";
            }
            else if(e.keyCode == KeyCode.Backspace)
            {
                text.text = text.text.Substring(0, text.text.Length - 1);
            }
        }
	}

    public void Toggle()
    {
        if (active)
        {
            if (!(text.text.Trim() == ""))
            {
                pc.MakeSpeechBubble(text.text);
            }
            Deactivate();
        }
        else
        {
            Activate();
        }
    }

    public void Activate()
    {
        active = true;
        image.SetActive(true);
        text.gameObject.SetActive(true);
    }

    private void Deactivate()
    {
        text.text = "";
        active = false;
        image.SetActive(false);
        text.gameObject.SetActive(false);
    }

}
