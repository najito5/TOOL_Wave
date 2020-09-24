using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class TOD_Wave  
{
    public List<TOD_Enemy> AllEnemies = new List<TOD_Enemy>();
    public List<int> AllQuantity = new List<int>();
    public List<Vector3> AllSpawners = new List<Vector3>();
    public List<int> Index = new List<int>();
    // string name = "defaultWave";
    public bool ShowWave = true;
    public bool IsFinishedToSpawn { get; set; } = false;
    public bool HasStartedToSpawn { get; set; } = true;


    public int ID { get; set; }

    public bool IsValid => AllEnemies.Count > 0 && AllSpawners.Count > 0;

    public bool IsEnabled { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void AddSpawnPos()
    {
        AllSpawners.Add(Vector3.zero);
        Index.Add(-1);
    }
    public void RemoveSpawnPos(int _i)
    {
        AllSpawners.RemoveAt(_i);
        Index.RemoveAt(_i);
    }
    public void ClearSpawnPos()
    {
        AllSpawners.Clear();
        Index.Clear();
    }

    public void AddEnemy()
    {
        AllEnemies.Add(null);
        AddValue();
    }
    public void AddValue() => AllQuantity.Add(1);

    public void RemoveEnemy(int _i)
    {
        AllEnemies.Remove(AllEnemies[_i]);
        RemoveValue(_i);
    }
    public void RemoveValue(int _i) => AllQuantity.Remove(_i);
    public void ClearEnemies()
    {
        AllEnemies.Clear();
        ClearValue();
    }
    public void ClearValue() => AllQuantity.Clear();

}
