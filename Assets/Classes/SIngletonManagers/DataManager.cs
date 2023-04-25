﻿using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    private Dictionary<string, DialogueData> dialogueDict = new Dictionary<string, DialogueData>();

    //List<Dictionary<string, object>> data_Dialogue = .Read("Dialogue");
    public override void Initialize()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath+"/Resources/Data/DialogueData"); //todo gz : edit path later
        var fileList = directoryInfo.GetFiles("*.csv");
        fileList.ForEach(file =>
        {
            Debug.Log($"{file.FullName}");
            Debug.Log($"{file.Name}");
            var csvRawData = CSVReader.Read("Data/DialogueData/" + file.Name);
            var dialogueData = new DialogueData();
            dialogueData.Load(csvRawData);
            dialogueDict.Add(file.Name.Replace(".csv", ""), dialogueData);
        });
    }

    public DialogueData GetDialogueData(string dataName)
    {
        dialogueDict.TryGetValue(dataName, out DialogueData data);
        return data;
    }

    class TableLoader
    {

    }
}