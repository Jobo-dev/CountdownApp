using System;
using System.IO;
using UnityEngine;

public enum DateDataType
{
    TargetDate,
    InitialDate,
    Description
}

public class DateTimeData
{
    public string targetDateText;
    public string initialDateText;
    public string description;
}
public class JsonFileReaderUtility
{
    string fileName = "dateInfo.json";

    public void SaveInfoToFile(DateTimeInfoSO dateTimeInfo)
    {
        DateTimeData data = new DateTimeData
        {
            targetDateText = dateTimeInfo.dateString,
            initialDateText = dateTimeInfo.initialDateString,
            description = dateTimeInfo.description
        };

        string json = JsonUtility.ToJson(data);
        string path = Path.Combine(Application.persistentDataPath, fileName);

        File.WriteAllText(path, json);
        Debug.Log($"{GetType()} Log. The file {fileName} was saved with the next info: {json}");
    }

    public string LoadDataFromJson(DateDataType dataType)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);
        string targetDataText;
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            DateTimeData data = JsonUtility.FromJson<DateTimeData>(json);

            switch (dataType)
            {
                case DateDataType.TargetDate:
                    targetDataText = data.targetDateText;
                    break;
                case DateDataType.InitialDate: 
                    targetDataText = data.initialDateText;
                    break;
                case DateDataType.Description:
                    targetDataText = data.description;
                    break;
                default:
                    targetDataText = "";
                    break;
            }

            Debug.Log($"{GetType()} Log. Loaded data: {targetDataText}");
            return targetDataText;
        }
        else
        {
            Debug.LogWarning($"{GetType()} LogWarning. The file {fileName} doesn't exist");
            return null;
        }
    }
}