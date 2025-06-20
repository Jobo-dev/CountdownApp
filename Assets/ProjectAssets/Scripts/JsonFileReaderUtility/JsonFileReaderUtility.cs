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
    const string FileName = "dateInfo.json";

    public void SaveInfoToFile(DateTimeInfoSO dateTimeInfo)
    {
        DateTimeData data = new DateTimeData
        {
            targetDateText = dateTimeInfo.dateString,
            initialDateText = dateTimeInfo.initialDateString,
            description = dateTimeInfo.description
        };

        string json = JsonUtility.ToJson(data);
        string path = Path.Combine(Application.persistentDataPath, FileName);

        File.WriteAllText(path, json);
        Debug.Log($"{GetType()} Log. The file {FileName} was saved with the next info: {json}");
    }

    public string LoadDataFromJson(DateDataType dataType)
    {
        string path = Path.Combine(Application.persistentDataPath, FileName);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            DateTimeData data = JsonUtility.FromJson<DateTimeData>(json);

            return dataType switch
            {
                DateDataType.TargetDate => data.targetDateText,
                DateDataType.InitialDate => data.initialDateText,
                DateDataType.Description => data.description,
                _ => ""
            };
        }
        else
        {
            Debug.LogWarning($"{GetType()} LogWarning. The file {FileName} doesn't exist");
            return null;
        }
    }
}