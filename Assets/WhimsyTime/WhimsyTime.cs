using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhimsyTime : MonoBehaviour {

    public float secondsInHour = 3600f;
    public float hoursInDay = 24f;
    public int daysInMonth = 30;
    public int monthsInYear = 12;

    public static WhimsyTime Instance { get; private set; }

    public float secondRatio = 1000f;
    public float maxDarkness = 0.6f;
    public float dark;
    private HueShift hue;
    private int year = 0;
    private int month = 0;
    private int day = 0;
    private float hour = 0;

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;

        //DontDestroyOnLoad(gameObject);
    }

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

    public Stamp GetStamp()
    {
        return new Stamp(hour, day, month, year);
    }

    public static Stamp Now()
    {
        return Instance.GetStamp();
    }

    private void NextDay()
    {
        day += 1;
        if(day >= daysInMonth)
        {
            day = 0;
            month += 1;
        }
        if(month >= monthsInYear)
        {
            month = 0;
            year += 1;

        }
    }

    public class Stamp
    {
        private WhimsyTime wt { get { return Instance; } }
        private int ticks;

        public Stamp(float hour, int day, int month, int year)
        {
            ticks = (int)((((year * (wt.monthsInYear) + month) * wt.daysInMonth + day) * wt.hoursInDay + hour) * wt.secondsInHour);
        }

        public Stamp(int seconds)
        {
            ticks = seconds;
        }

        public int InSeconds()
        {
            return ticks;
        }

        public float InHours()
        {
            return InSeconds() / wt.secondsInHour;
        }

        public float InDays()
        {
            return InHours() / wt.hoursInDay;
        }

        public float InMonths()
        {
            return InDays() / (float)wt.daysInMonth;
        }

        public float InYears()
        {
            return InMonths() / (float)wt.monthsInYear;
        }

        public int Year()
        {
            return Mathf.FloorToInt(InYears());
        }

        public int Month()
        {
            return Mathf.FloorToInt(InMonths() - (Mathf.FloorToInt(InYears()) * (float)wt.monthsInYear));
        }

        public int Day()
        {
            return Mathf.FloorToInt(InDays() - (Mathf.FloorToInt(InMonths()) * (float)wt.daysInMonth));
        }

        public int Hour()
        {
            return Mathf.FloorToInt(InHours() - (Mathf.FloorToInt(InDays() * wt.hoursInDay)));
        }

        public int Second()
        {
            return Mathf.FloorToInt(InSeconds() - (Mathf.FloorToInt(InHours() * wt.secondsInHour)));
        }

        public static Stamp operator -(Stamp left, Stamp right)
        {
            return new Stamp(left.InSeconds() - right.InSeconds());
        }

        public static Stamp operator +(Stamp left, Stamp right)
        {
            return new global::WhimsyTime.Stamp(left.InSeconds() + right.InSeconds());
        }
    }
}
