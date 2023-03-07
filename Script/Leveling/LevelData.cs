using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LevelData : ScriptableObject
{
    public float StartTime;
    public int[] EnemySpawnNumber = new int[5];
    public float[] SpawnRate = new float[5];
}
