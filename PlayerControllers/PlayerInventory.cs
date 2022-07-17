using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory
{
    public bool jetpack = false;

    // initialization functions
    public PlayerInventory() { }

    public PlayerInventory(bool jetpack)
    {
        this.jetpack = jetpack;
    }

}
