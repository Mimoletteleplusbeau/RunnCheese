using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seagull : Enemy
{
    public static Seagull Instance;

    private void Awake()
    {
        Instance = this;
    }
}
