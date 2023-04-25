using Sirenix.Utilities;
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
        directoryInfo.GetFiles(".csv").ForEach(File =>
        {
            Debug.Log($"{File.FullName}");
            Debug.Log($"{File.Name}");
            var csvRawData = CSVReader.Read(File.FullName);

            var dialogueData = new DialogueData();
            dialogueData.Load(csvRawData);
            dialogueDict.Add(File.Name, dialogueData);
        });
    }

    class TableLoader
    {

    }
}