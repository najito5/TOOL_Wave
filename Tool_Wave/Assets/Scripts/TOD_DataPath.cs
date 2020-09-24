using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TOD_DataPath
{
    public static event Action OnRefresh = null;

    public static void ReadData<T>(ref T _struct, string _directoryPath, string _filePath)
    {
        string _json = null;

        if (!Directory.Exists(_directoryPath))
            Directory.CreateDirectory(_directoryPath);

        if (!File.Exists(_filePath))
        {
            _json = JsonUtility.ToJson(default(T));
            File.WriteAllText(_filePath, _json);
        }

        _json = File.ReadAllText(_filePath);

        _struct = JsonUtility.FromJson<T>(File.ReadAllText(_filePath));
    }

    public static void WriteData<T>(T _struct, string _directoryPath, string _filePath)
    {
        if (!Directory.Exists(_directoryPath))
            Directory.CreateDirectory(_directoryPath);

        string _Json = JsonUtility.ToJson(_struct);

        File.WriteAllText(_filePath, _Json);

        OnRefresh?.Invoke();
    }
}