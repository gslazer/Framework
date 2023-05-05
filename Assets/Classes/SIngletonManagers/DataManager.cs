using Sirenix.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    private Dictionary<string, DialogueTable> dialogueDict = new Dictionary<string, DialogueTable>();

    //List<Dictionary<string, object>> data_Dialogue = .Read("Dialogue");
    public override void Initialize()
    {
        LoadTable("DialogueData");
    }

    public DialogueTable GetDialogueData(string dataName)
    {
        if (!dialogueDict.TryGetValue(dataName, out DialogueTable data))
            return null;
        return data;
    }

    public void LoadTable(string tableName)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath + $"/Resources/Data/{tableName}"); //todo gz : edit path later
        var fileList = directoryInfo.GetFiles("*.csv");
        fileList.ForEach(file =>
        {
            Debug.Log($"{file.FullName}");
            Debug.Log($"{file.Name}");
            var csvRawData = CSVReader.Read($"Data/{tableName}/" + file.Name);
            var dialogueData = new DialogueTable();
            //var tableData = ParseCSVData<T>(csvRawData);
            dialogueData.ParseCSVData(csvRawData);
            dialogueDict.Add(file.Name.Replace(".csv", ""), dialogueData);
        });
    }

}