using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    private Dictionary<string, DialogueData> dialogueDict = new Dictionary<string, DialogueData>();

    //List<Dictionary<string, object>> data_Dialogue = .Read("Dialogue");
    public override void Initialize()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath+ "/ResourceBundle/DialogueData"); //todo gz : edit path later
        foreach(FileInfo File in directoryInfo.GetFiles(file => file.Name.EndsWith(".csv")))
        {
            Debug.Log($"{File.FullName}");
            Debug.Log($"{File.Name}");
            CSVReader.Read(File.Full).ForEach();

        }
        foreach(DirectoryInfo sub_Dir in directoryInfo.GetDirectories())
        {
            Debug.Log($"{sub_Dir.FullName}");
            Debug.Log($"{sub_Dir.Name}");
        }
    }

    class TableLoader
    {

    }
}