﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;


public class DialogueTable : TableData <DialogueTable.Data>
{
    public class Data
    {
        public int Index;
        public string String;
    }
    //private Data dataDict;

    private int currentKeyValue;
    public string GetCurrentString()
    {
        if (dataDict == null)
        {
            Debug.LogError("DialogueData.GetCurrentString() : data is null!");
            return null;
        }
        if (!dataDict.ContainsKey(currentKeyValue))
        {
            Debug.LogError($"DialogueData don't have Data of key : {currentKeyValue}");
            return null;
        }
        return dataDict[currentKeyValue].String;
    }
    public string GetNextString()
    {
        currentKeyValue++;
        if (currentKeyValue >= dataDict.Count)
            return null;
        return GetCurrentString();
    }
}