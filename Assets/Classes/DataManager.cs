using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    //List<Dictionary<string, object>> data_Dialogue = .Read("Dialogue");
    public override void Initialize()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath+ "/ResourceBundle"); //todo gz : edit path later
        foreach(FileInfo File in directoryInfo.GetFiles())
        {
            Debug.Log($"{File.FullName}");
            Debug.Log($"{File.Name}");
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