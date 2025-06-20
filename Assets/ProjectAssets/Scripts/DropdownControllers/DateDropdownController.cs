using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;

public class DateDropdownController : MonoBehaviour
{
    public TMP_Dropdown yearDropdown;
    public TMP_Dropdown monthDropdown;
    public TMP_Dropdown dayDropdown;

    private int currentYear = DateTime.Now.Year;
    private int minYear, maxYear, maxYearOffset = 100;

    bool updateDay = false;

    void Start()
    {
        PopulateYearDropdown();
        PopulateMonthDropdown();
        UpdateDayDropdown();


        yearDropdown.onValueChanged.AddListener(delegate { UpdateDayDropdown(); });
        monthDropdown.onValueChanged.AddListener(delegate { UpdateDayDropdown(); });
    }

    void PopulateYearDropdown()
    {
        minYear = currentYear;
        maxYear = currentYear + maxYearOffset;
        yearDropdown.ClearOptions();
        List<string> years = new List<string>();

        for (int year = minYear; year <= maxYear; year++)
        {
            years.Add(year.ToString());
        }

        yearDropdown.AddOptions(years);
        yearDropdown.value = years.IndexOf(currentYear.ToString());
    }

    void PopulateMonthDropdown()
    {
        monthDropdown.ClearOptions();
        List<string> months = new List<string>();

        for (int month = 1; month <= 12; month++)
        {
            months.Add(month.ToString("D2"));
        }

        monthDropdown.AddOptions(months);
        monthDropdown.value = DateTime.Now.Month - 1;
    }

    void UpdateDayDropdown()
    {
        int currentValue = dayDropdown.value;
        int selectedYear = int.Parse(yearDropdown.options[yearDropdown.value].text);
        int selectedMonth = int.Parse(monthDropdown.options[monthDropdown.value].text);
        int daysInMonth = DateTime.DaysInMonth(selectedYear, selectedMonth);

        dayDropdown.ClearOptions();
        List<string> days = new List<string>();

        for (int day = 1; day <= daysInMonth; day++)
        {
            days.Add(day.ToString("D2"));
        }

        dayDropdown.AddOptions(days);
        if (updateDay)
        {
            dayDropdown.value = currentValue;
        }
        else
        {
            dayDropdown.value = Mathf.Clamp(DateTime.Now.Day - 1, 0, days.Count - 1);
            updateDay = true;
        }
    }
}
