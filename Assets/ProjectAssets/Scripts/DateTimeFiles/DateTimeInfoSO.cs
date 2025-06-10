using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DateTimeInfo", menuName = "CustomDateTime/DateTimeInfo")]
public class DateTimeInfoSO : ScriptableObject
{
    public string dateString = "2025-06-10 12:00:00";
    public DateTime targetDate;
    public DateTime initialDate;
    public string description = "";

    public void Init()
    {
        initialDate = DateTime.Now;
    }

    internal bool ValidateTargetFormat(string targetDateString)
    {
        if(targetDateString != null || targetDateString != "")
        {
            if (DateTime.TryParse(targetDateString, out targetDate))
            {
                dateString = targetDate.ToString();
                Debug.Log($"TargetDateString: {dateString}");
                return true;
            }
            else
            {
                Debug.LogWarning($"{GetType()} Warning. The date is in a wrong format");
                return false;
            }
        }else
        {
            Debug.LogWarning($"{GetType()} Warning. The date string is null or empty");
            return false;
        }
    }
    internal void SetTargetDateString()
    {
        if (DateTime.TryParse(dateString, out targetDate))
        {
            DateTime initialDateTime = targetDate;
        }
    }
    internal void SetDescription(string pDescription)
    {
        description = pDescription;
    }

}
