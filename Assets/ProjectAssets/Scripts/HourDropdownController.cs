using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;

public class HourDropdownController : MonoBehaviour
{
    public TMP_Dropdown hoursDropdown;
    public TMP_Dropdown minutesDropdown;
    public TMP_Dropdown secondsDropdown;

    private Time currentTime;

    void Start()
    {
        PopulateHoursAndMinutesDropdown(minutesDropdown, true);
        PopulateHoursAndMinutesDropdown(secondsDropdown, false);
        PopulatehoursDropdown();
    }

    void PopulateHoursAndMinutesDropdown(TMP_Dropdown dropdown, bool isMinutes)
    {
        dropdown.ClearOptions();
        List<string> years = new List<string>();

        for (int year = 0; year <= 59; year++)
        {
            years.Add(year.ToString("D2"));
        }

        dropdown.AddOptions(years);
        if(isMinutes)
            dropdown.value = DateTime.Now.Minute;
        else
            dropdown.value = 0;
    }

    void PopulatehoursDropdown()
    {
        hoursDropdown.ClearOptions();
        List<string> hours = new List<string>();

        for (int month = 0; month <= 23; month++)
        {
            hours.Add(month.ToString("D2"));
        }

        hoursDropdown.AddOptions(hours);
        hoursDropdown.value = DateTime.Now.Hour;
    }
}
