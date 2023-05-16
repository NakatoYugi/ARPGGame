using System;
using UnityEngine;

[Serializable]
public class GameData
{
    public long lastUpdated;
    public Vector3 PlayerPosition;

    public GameData()
    {
        this.PlayerPosition = Vector3.zero;
    }
}


