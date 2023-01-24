using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class DialogueData
{
    private int currentKeyValue;
    private Dictionary<int, string> data = new Dictionary<int, string>
    {
        { 0, "0입니다~~" },
        { 1, "ㅁㄴㅇㄹ" },
        { 2, "두번쨰라인"},
        { 3, "33333"},
        { 4, "44번쨰라인"},
        { 5, "555번쨰라인"},
        { 6, "666번쨰라인"}
    };

    public void SetData(Dictionary<int, string> data)
    {
        this.data = data;
    }
    public string GetCurrentString()
    {
        if (!data.ContainsKey(currentKeyValue))
        {
            Debug.LogError($"DialogueData don't have Data of key : {currentKeyValue}");
            return "";
        }
        return data[currentKeyValue];
    }
    public string GetNextString()
    {
        currentKeyValue++;
        return GetCurrentString();
    }

    public void Load(List<Dictionary<string, object>> csvRawData)
    {
        csvRawData.ForEach(csvDict =>{
            data.Add(csvDict["ID"], csvDict["DialogueString"]);
        });
    }
}