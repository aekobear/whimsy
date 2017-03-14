using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : Item {

    public Item p_plant;
    public float germinationTime;
    private WhimsyTime.Stamp sewedTime;
    private Grid grid;
    private Vector2 location;

    void Update()
    {
        if( (WhimsyTime.Now() - sewedTime).InDays() > germinationTime ){
            grid.Replace(location, Instantiate<Item>(p_plant));
            Destroy(gameObject);
        }
    }

    public override void OnInteract()
    {

    }

    public override void OnInteract(Grid grid, Vector2 target)
    {

    }

    public override bool OnPlace(Grid grid, Vector2 target)
    {
        sewedTime = WhimsyTime.Now();
        this.grid = grid;
        this.location = target;
        return base.OnPlace(grid, target);
    }

}
