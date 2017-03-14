using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCyle : MonoBehaviour {

    public float secondRatio = 1000f;
    public float maxDarkness = 0.6f;
    public float dark;
    private HueShift hue;
    private int year = 0;
    private int month = 0;
    private int day = 0;
    private float hour = 0;
    private int monthsInyear = 12;
    private int daysInMonth = 30;
    private float hoursInDay = 24f;
    private float secondsInHour = 3600f;
   

	// Use this for initialization
	void Start () {
        hue = GetComponent<HueShift>();
	}
	
	// Update is called once per frame
	void Update () {

        hour += (Time.deltaTime * secondRatio) / secondsInHour;

        if(hour > hoursInDay)
        {
            hour -= hoursInDay;
            NextDay();
        }
        dark = getDarkness(hour);
        hue.effectMaterial.SetFloat("_Amount", getDarkness(hour));
    }

    private float getDarkness(float hour)
    {
        float third = hoursInDay / 3f;
        
        //if daytime
        if(hour > third && hour < (24f - third))
        {
            return 0.0f;
        }
        //if morning
        else if(hour < third)
        {
            return maxDarkness * ((third - hour) / third);
        }
        //if evening
        else
        {
            return maxDarkness * ((hour - (2f*third)) / third);
        }
    }

    public WhimsyTime WhimsyTime()
    {
        return new WhimsyTime(hour, day, month, year);
    }

    private void NextDay()
    {
        day += 1;
        if(day >= daysInMonth)
        {
            day = 0;
            month += 1;
        }
        if(month >= monthsInyear)
        {
            month = 0;
            year += 1;

        }
    }
}

public class WhimsyTime
{
    private float ticks;
    private float hour;
    private int day;
    private int month;
    private int year;

    public WhimsyTime(float hour, int day, int month, int year)
    {
        this.hour = hour;
        this.day = day;
        this.month = month;
        this.year = year;
    }
}
