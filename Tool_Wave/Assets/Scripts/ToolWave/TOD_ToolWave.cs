using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TOD_ToolWave : MonoBehaviour
{
    public bool IsVisbleWaves = true;
    public bool IsVisbleSpawnPos = true;
    public List<TOD_Wave> allWaves = new List<TOD_Wave>();
    public WaveDifficulty waveDifficulty = WaveDifficulty.Normal;
    public List<Vector3> allSpawnPositionWave = new List<Vector3>();
    public List<string> AllIndexSpawn = new List<string>();

    public bool UseDelay = true;
    public float SpawnDelayEachSpawnPos = 1;
    public float SpawnDelayEachEnemy = 1;

    public void AddWave() => allWaves.Add(new TOD_Wave());
    public void AddSpawnPos()
    {
        allSpawnPositionWave.Add(Vector3.zero);
        AllIndexSpawn.Add((allSpawnPositionWave.Count - 1).ToString());
    }
    public void RemoveWave(int _i)
    {
        allWaves.RemoveAt(_i);
    }
    public void RemoveSpawnPos(int _i)
    {
        allSpawnPositionWave.RemoveAt(_i);
        AllIndexSpawn.RemoveAt(_i);
    }
    public void ClearWaves() => allWaves.Clear();
    public void ClearSpawnPos()
    {
        allSpawnPositionWave.Clear();
        AllIndexSpawn.Clear();
    }

    public void ChangeWaveOrder(int _index, bool _Up)
    {
        TOD_Wave _tmpWave = new TOD_Wave();
        int _order = 0;

        if (_Up) _order = -1;
        else _order = 1;

        _tmpWave = allWaves[_index + _order];
        allWaves[_index + _order] = allWaves[_index];
        allWaves[_index] = _tmpWave;
    }
}

public enum WaveDifficulty
{
    Easy,Normal,Hard
}
