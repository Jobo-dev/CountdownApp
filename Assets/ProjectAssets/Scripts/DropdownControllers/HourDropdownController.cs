using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;

public class HourDropdownController : MonoBehaviour
{
    [Header("Dropdowns")]
    public TMP_Dropdown hoursDropdown;
    public TMP_Dropdown minutesDropdown;
    public TMP_Dropdown secondsDropdown;

    void Start()
    {
        if (hoursDropdown == null || minutesDropdown == null || secondsDropdown == null)
        {
            Debug.LogError("HourDropdownController: Some dropdowns are not assigned.");
            return;
        }
        PopulateDropdown(hoursDropdown, 23, DateTime.Now.Hour);
        PopulateDropdown(minutesDropdown, 59, DateTime.Now.Minute);
        PopulateDropdown(secondsDropdown, 59, 0);
    }

    void PopulateDropdown(TMP_Dropdown dropdown, int maxValue, int defaultValue = 0)
    {
        dropdown.ClearOptions();
        List<string> values = new List<string>();

        for (int i = 0; i <= maxValue; i++)
            values.Add(i.ToString("D2"));

        dropdown.AddOptions(values);
        dropdown.value = Mathf.Clamp(defaultValue, 0, maxValue);
    }
}
