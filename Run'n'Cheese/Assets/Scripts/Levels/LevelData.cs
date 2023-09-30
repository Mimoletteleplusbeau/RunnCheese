using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData
{
    [Tooltip("The Name of the loaded Scene")] public string Name;
    [Tooltip("The time to beat to get 1 fish")] public float FishTimer1;
    [Tooltip("The time to beat to get 2 fishes")] public float FishTimer2;
    [Tooltip("The time to beat to get 3 fishes")] public float FishTimer3;
}
