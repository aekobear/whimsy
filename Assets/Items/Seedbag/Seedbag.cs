using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seedbag : Item {
    
    public Item p_seed;

    public override void OnInteract(Grid grid, Vector2 target)
    {
        Item item = grid.ItemAt(target);
        if(item == null)
        {
            grid.Place(target, Instantiate<Item>(p_seed));
        }
    }
}
