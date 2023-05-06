using Sirenix.Utilities;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;

public class TableData <T> where T : class, new()
{
    protected Dictionary<int, T> dataDict;

    public void ParseCSVData(List<Dictionary<string, object>> csvRawData)
    {
        dataDict = new Dictionary<int, T>();
        var fields = typeof(T).GetFields();

        csvRawData.ForEach(csvLineData =>
        {
            T data = new T();
            fields.ForEach(fieldInfo =>
            {
                var tempData = csvLineData[fieldInfo.Name].ConvertTo(fieldInfo.FieldType);
                fieldInfo.SetValue(data, tempData);
            });
            int index = csvLineData["Index"].ConvertTo<int>();
            dataDict.Add(index, data);
        });
    }
}
