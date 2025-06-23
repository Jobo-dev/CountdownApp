using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DateTimeInfo", menuName = "CustomDateTime/DateTimeInfo")]
public class DateTimeInfoSO : ScriptableObject
{
    public string dateString = "2025-06-10 12:00:00";
    public string initialDateString = "2025-01-01 12:00:00";
    
    public string description = "";

    private DateTime initialDate;

    public void Init()
    {
        SetInitialDate();
    }

    internal void SetDescription(string pDescription)
    {
        description = pDescription;
    }

    internal void SetInitialDate()
    {
        initialDate = DateTime.Now;
    }

    internal void AssignInitialDateAsString()
    {
        initialDateString = initialDate.ToString();
    }

}
