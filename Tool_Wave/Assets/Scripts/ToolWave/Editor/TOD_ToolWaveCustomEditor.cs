using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EditoolsUnity;
using System;

[CustomEditor(typeof(TOD_ToolWave))]
public class TOD_ToolWaveCustomEditor : EditorCustom<TOD_ToolWave>
{
    Version version = new Version(1, 0, 3);

    protected override void OnEnable()
    {
        base.OnEnable();
        Tools.current = Tool.None;
    }

    public override void OnInspectorGUI()
    {
        LayoutEdit.HelpBoxInfo($"WAVE && SPAWNER POSITION TOOL {version}");
        LayoutEdit.Space(3);
        InfoWaveInspector();
        LayoutEdit.Space(2);
        DrawWavesInspector();
        LayoutEdit.Space(5);
        DrawSpawnPointInspector();
        SceneView.RepaintAll();
    }

    void InfoWaveInspector()
    {
        LayoutEdit.Horizontal(true);
        LayoutEdit.HelpBox("Choose difficulty about waves (more or less enemies by wave) :");
        eTarget.waveDifficulty = (WaveDifficulty)EditorGUILayout.EnumPopup(eTarget.waveDifficulty);
        CommentsDifficulty();
        LayoutEdit.Horizontal(false);
        LayoutEdit.Space();
        LayoutEdit.Horizontal(true);
        LayoutEdit.HelpBox("The time to spawn between each enemy during wave :");
        eTarget.SpawnDelayEachEnemy = EditorGUILayout.Slider(eTarget.SpawnDelayEachEnemy, .2f, 5f);
        LayoutEdit.HelpBox("secs");
        LayoutEdit.Horizontal(false);
        LayoutEdit.ToggleLeft("Use a delay between each spawner where enemies come ?", ref eTarget.UseDelay);
        if (eTarget.UseDelay)
        {
            LayoutEdit.Horizontal(true);
            LayoutEdit.HelpBox("The time between each spawner during same wave :");
            eTarget.SpawnDelayEachSpawnPos = EditorGUILayout.Slider(eTarget.SpawnDelayEachSpawnPos, .1f, 10f);
            LayoutEdit.HelpBox("secs");
            LayoutEdit.Horizontal(false);
        }
    }
    void CommentsDifficulty()
    {
        string _comment = null;
        switch (eTarget.waveDifficulty)
        {
            case WaveDifficulty.Easy:
                _comment = "/ 2";
                break;
            case WaveDifficulty.Normal:
                _comment = "x 1";
                break;
            case WaveDifficulty.Hard:
                _comment = "x 2";
                break;
        }
        LayoutEdit.HelpBox("Enemies :     " + _comment);
    }

    #region SpawnPosition
    void DrawSpawnPointInspector()
    {
        LayoutEdit.Horizontal(true);
        LayoutEdit.HelpBoxInfo("Add a spawn position");
        LayoutEdit.Vertical(true);
        ButtonEdit.Button("+", eTarget.AddSpawnPos, Color.green);
        ButtonEdit.ButtonConfirmation("Delete all", eTarget.ClearSpawnPos, Color.red, "Remove all", "Delete all spawns ?", "Yes", "No", eTarget.allSpawnPositionWave.Count > 0);
        LayoutEdit.Vertical(false);
        LayoutEdit.Horizontal(false);

        LayoutEdit.Fold(ref eTarget.IsVisbleSpawnPos, "Show/Hide spawns", true);
        if (eTarget.IsVisbleSpawnPos)
        {
            for (int i = 0; i < eTarget.allSpawnPositionWave.Count; i++)
            {
                LayoutEdit.Space();
                LayoutEdit.Horizontal(true);
                LayoutEdit.HelpBox($"Spawn points {i}");
                ButtonEdit.ButtonConfirmation("X", eTarget.RemoveSpawnPos, Color.red, i, "Remove spawn position", $"Sure to remove this spawner {i + 1} ?", "Yes", "No");
                LayoutEdit.Horizontal(false);

                if (i > eTarget.allSpawnPositionWave.Count - 1) return;
                Vector3 _pos = eTarget.allSpawnPositionWave[i];
                LayoutEdit.Vector3Field("Position", ref _pos);
            }
        }
    }

    private void OnSceneGUI()
    {
        if (eTarget.allSpawnPositionWave.Count <= 0) return;
        SpawnPointScene();
    }
    void SpawnPointScene()
    {
        for (int i = 0; i < eTarget.allSpawnPositionWave.Count; i++)
        {
            Handles.color = Color.green;
            Handles.DrawWireDisc(eTarget.allSpawnPositionWave[i], new Vector3(0, 1), 1f);
            Handles.color = Color.white;
            // Vector3 _pos = eTarget.allSpawnPositionWave[i];
            // HandlesEdit.Position(ref _pos, Quaternion.identity);
            eTarget.allSpawnPositionWave[i] = Handles.PositionHandle(eTarget.allSpawnPositionWave[i], Quaternion.identity);
        }
    }
    #endregion

    #region Waves
    void DrawWavesInspector()
    {
        LayoutEdit.Horizontal(true);
        LayoutEdit.HelpBoxInfo("Add a wave");
        LayoutEdit.Vertical(true);
        ButtonEdit.Button("+", eTarget.AddWave, Color.green);
        ButtonEdit.ButtonConfirmation("Delete all", eTarget.ClearWaves, Color.red, "Remove all", "Delete all waves ?", "Yes", "No", eTarget.allWaves.Count > 0);
        LayoutEdit.Vertical(false);
        LayoutEdit.Horizontal(false);

        LayoutEdit.Fold(ref eTarget.IsVisbleWaves, "Show/Hide waves", true);
        if (eTarget.IsVisbleWaves)
        {
            for (int i = 0; i < eTarget.allWaves.Count; i++)
            {
                LayoutEdit.Space();
                LayoutEdit.Horizontal(true);
                LayoutEdit.Fold(ref eTarget.allWaves[i].ShowWave, $" Show/ Hide Wave {i}", true);
                if (i != 0) ButtonEdit.Button<int, bool>("↑", eTarget.ChangeWaveOrder, Color.gray, i, true);
                if (i + 1 != eTarget.allWaves.Count) ButtonEdit.Button<int, bool>("↓", eTarget.ChangeWaveOrder, Color.gray, i, false);
                ButtonEdit.ButtonConfirmation("X", eTarget.RemoveWave, Color.red, i, "Remove wave", $"Sure to remove wave {i + 1} ?", "Yes", "No");
                LayoutEdit.Horizontal(false);

                if (i > eTarget.allWaves.Count - 1) return;

                DrawEnemyInspector(eTarget.allWaves[i]);
                DrawSpawnerInspector(eTarget.allWaves[i]);
            }
        }
    }
    void DrawEnemyInspector(TOD_Wave _currentWave)
    {
        if (!_currentWave.ShowWave) return;
        LayoutEdit.Horizontal(true);
        LayoutEdit.HelpBox("Add enemy");
        ButtonEdit.Button("+", _currentWave.AddEnemy, Color.gray);
        LayoutEdit.Horizontal(false);
        for (int j = 0; j < _currentWave.AllEnemies.Count; j++)
        {
            LayoutEdit.Horizontal(true);
            _currentWave.AllEnemies[j] = (TOD_Enemy)EditorGUILayout.ObjectField(_currentWave.AllEnemies[j], typeof(TOD_Enemy), false);
            if (_currentWave.AllEnemies[j] != null && _currentWave.AllEnemies[j].GetComponent<TOD_Enemy>() == null) _currentWave.AllEnemies[j] = null;
            // int _value = _currentWave.AllQuantity[j];
            // LayoutEdit.IntSlider("Quantity", ref _value, 1, 200);
            _currentWave.AllQuantity[j] = EditorGUILayout.IntSlider(_currentWave.AllQuantity[j], 1, 200);
            ButtonEdit.ButtonConfirmation("X", _currentWave.RemoveEnemy, Color.red, j, "Remove enemy", $"Remove enemy {j + 1}", "Yes", "No");
            LayoutEdit.Horizontal(false);
        }
        LayoutEdit.Space();
        ButtonEdit.ButtonConfirmation("Remove all enemies", _currentWave.ClearEnemies, Color.red, "Remove everyone", "Remove enemy ", "Yes", "No");
    }
    void DrawSpawnerInspector(TOD_Wave _currentWave)
    {
        if (!_currentWave.ShowWave) return;
        LayoutEdit.Horizontal(true);
        if (_currentWave.AllSpawners.Count < eTarget.allSpawnPositionWave.Count)
        {
            LayoutEdit.HelpBox("Add spawner");
            ButtonEdit.Button("+", _currentWave.AddSpawnPos, Color.gray);
        }
        LayoutEdit.Horizontal(false);
        if (_currentWave.AllSpawners.Count > 0)
        {
            for (int i = 0; i < _currentWave.Index.Count; i++)
            {
                LayoutEdit.Horizontal(true);
                LayoutEdit.HelpBox($"Spawner {i} :");
                _currentWave.Index[i] = EditorGUILayout.Popup(_currentWave.Index[i], eTarget.AllIndexSpawn.ToArray());
                if (CheckAlreadyExist(_currentWave.Index, i)) _currentWave.Index[i] = -1;
                ButtonEdit.ButtonConfirmation("X", _currentWave.RemoveSpawnPos, Color.red, i, "Remove Spawner", $"Remove spawner position {i}", "Yes", "No");
                LayoutEdit.Horizontal(false);
            }
        }
    }
    bool CheckAlreadyExist(List<int> _list, int _checkIndex)
    {
        for (int i = 0; i < _list.Count; i++)
        {
            if (_checkIndex == i) continue;
            if (_list[_checkIndex] == _list[i]) return true;
        }
        return false;
    }
    #endregion

    void OnDisable()
    {
        if (eTarget.allWaves.Count == 0) return;
        for (int i = 0; i < eTarget.allWaves.Count; i++)
        {
            for (int e = 0; e < eTarget.allWaves[i].AllEnemies.Count; e++)
            {
                if (eTarget.allWaves[i].AllEnemies[e] == null)
                {
                    eTarget.allWaves[i].AllEnemies.RemoveAt(e);
                    eTarget.allWaves[i].AllQuantity.RemoveAt(e);
                    e--;
                }
            }
            for (int j = 0; j < eTarget.allWaves[i].Index.Count; j++)
            {
                if (eTarget.allSpawnPositionWave.Count == 0)
                {
                    eTarget.allWaves[i].ClearSpawnPos();
                    return;
                }
                if (eTarget.allWaves[i].Index[j] == -1)
                {
                    eTarget.allWaves[i].RemoveSpawnPos(j);
                    j--;
                }
                else
                {
                    eTarget.allWaves[i].AllSpawners[j] = eTarget.allSpawnPositionWave[eTarget.allWaves[i].Index[j]];
                    // Debug.Log(eTarget.allWaves[i].AllSpawners[i]);
                }
            }
        }
    }
}
