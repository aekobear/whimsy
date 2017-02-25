using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windchime : MonoBehaviour {

    public float maxChimeVolume;
    public float maxWindVolume;
    public float maxWindSpeed;
    public float minWindSpeed;
    public float maxWindDelta;
    public float minWindDelta;
    public float windSpeed;

    public AudioClip [] chimeSounds;

    private Chime[] chimes;
    public float nextHit;
    private float timeElapsed;
    private float windDelta;

    private AudioSource asource;


	// Use this for initialization
	void Start () {

        asource = GetComponent<AudioSource>();

        windSpeed = minWindSpeed;

        Random.InitState(System.DateTime.Now.Millisecond);
        windDelta = Random.Range(minWindDelta, maxWindDelta);
        
        chimes = new Chime[chimeSounds.Length];

        for(int i = 0; i < chimeSounds.Length; i++)
        {
            GameObject g = new GameObject();
            g.name = "chime";
            g.transform.parent = this.transform;
            AudioSource asource = g.AddComponent<AudioSource>();
            asource.clip = chimeSounds[i];
            asource.volume = maxChimeVolume;
            Chime c = g.AddComponent<Chime>();
            chimes[i] = c;
        }

        nextHit = RandomTime();
		
	}
	
	// Update is called once per frame
	void Update () {
        ChangeWind(Time.deltaTime);
        asource.volume = (windSpeed / maxWindSpeed) * maxWindVolume;
        timeElapsed += Time.deltaTime;
        if(timeElapsed > nextHit)
        {
            Random.InitState(System.DateTime.Now.Millisecond);
            int r = Random.Range(0, chimes.Length);
            chimes[r].Hit();
            timeElapsed = 0f;
            nextHit = RandomTime();
        }
	}

    private float RandomTime()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        float r = Random.Range(0f, (1f/windSpeed));
        return r;
    }

    private void ChangeWind(float delta)
    {
        windSpeed += windDelta * delta;

        if(windSpeed > maxWindSpeed)
        {
            windSpeed = maxWindSpeed;
            Random.InitState(System.DateTime.Now.Millisecond);
            windDelta = -Random.Range(minWindDelta, maxWindDelta);
        }
        if(windSpeed < minWindSpeed)
        {
            windSpeed = minWindSpeed;
            Random.InitState(System.DateTime.Now.Millisecond);
            windDelta = Random.Range(minWindDelta, maxWindDelta);
        }
    }
}
