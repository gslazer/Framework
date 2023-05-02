using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;

public class DialogueData
{
    private int currentKeyValue;
    private Dictionary<int, string> data;

    public string GetCurrentString()
    {
        if (data == null)
        {
            Debug.LogError("DialogueData.GetCurrentString() : data is null!");
            return null;
        }
        if (!data.ContainsKey(currentKeyValue))
        {
            Debug.LogError($"DialogueData don't have Data of key : {currentKeyValue}");
            return null;
        }
        return data[currentKeyValue];
    }
    public string GetNextString()
    {
        currentKeyValue++;
        if (currentKeyValue >= data.Count)
            return null;
        return GetCurrentString();
    }

    public void Load(List<Dictionary<string, object>> csvRawData)
    {
        data = new Dictionary<int, string>();
        csvRawData.ForEach(csvLineDict =>{
            int index = Convert.ToInt32(csvLineDict["Index"]);
            string str = csvLineDict["String"].ToString();
            data.Add(index, str);
        });

        //첫번쨰 Index를 currentKeyValue로
        currentKeyValue = data.First().Key;
    }
}