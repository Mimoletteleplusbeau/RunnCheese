using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New LevelsList", menuName = "LevelsList")]
public class LevelsList : ScriptableObject
{
    public string MainMenu;

    public LevelData[] Levels;
}
