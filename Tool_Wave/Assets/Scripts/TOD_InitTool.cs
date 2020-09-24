using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TOD_InitTool
{
    [MenuItem("TOD/Wave Tool/InitWaveTool")]
    public static void InitWaveTool()
    {
        TOD_ToolWave[] _allSpawn = Object.FindObjectsOfType<TOD_ToolWave>();
        if (_allSpawn.Length > 0) return;
        GameObject _tool = new GameObject("Wave tool", typeof(TOD_ToolWave));
        Selection.activeObject = _tool;
    }
}
