using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using System.Collections;
using DG.Tweening;

public class DateTimeValidation : MonoBehaviour
{

    #region Main validation methods
    /// <summary>
    /// Validate that the input field has info
    /// </summary>
    /// <param name="inputField">The input field to analyze</param>
    /// <param name="alertText">The text to show in case the validation fails</param>
    /// <returns>The result if the field contains any info</returns>
    internal bool ValidateDescription(TMP_InputField inputField, TextMeshProUGUI alertText)
    {
        string descriptionText = inputField.text;
        if (descriptionText != null && descriptionText != "")
        {
            return true;
        }
        else
        {
            Debug.LogWarning($"{GetType()} Warning. The input field is empty, please fill it");
            StartCoroutine(ShowTextAlert(alertText));
            return false;
        }
    }

    /// <summary>
    /// Validate the Date and time to be in the correct format
    /// </summary>
    /// <param name="dateText">The date and time in the correct format</param>
    /// <returns>The result if the string has a valid format</returns>
    internal bool ValidateDate(string dateText)
    {
        if (dateText != "")
        {
            Debug.Log($"{GetType()} Log. date to parse: {dateText}");
            if (ValidateDateTimeFormat(dateText))
            {
                return true;
            }
            else
            {
                Debug.LogWarning($"{GetType()} Warning. The Date target format is incorrect");
                return false;
            }
        }
        else
        {
            Debug.LogWarning($"{GetType()} Warning. The input field is empty, please fill it");
            return false;
        }
    }
    #endregion

    #region Save data on external objects methods
    /// <summary>
    /// Save the target date to be accessed in other components if needed
    /// </summary>
    /// <param name="dateTimeInfo">Object to save the target date as string </param>
    /// <param name="dateString">The date as string</param>
    internal void SaveDateTimeIntoObject(DateTimeInfoSO dateTimeInfo, string dateString)
    {
        dateTimeInfo.dateString = dateString;
        dateTimeInfo.AssignInitialDateAsString();
    }
    #endregion

    #region Date and time single validation methods
    /// <summary>
    /// Get the formatted string of the date based on the information from the dropdown elements.
    /// </summary>
    /// <param name="targetDropDownList">Listo with the dropdowns that contains the required info</param>
    /// <param name="alertText">Text to show in case the format is incorrect</param>
    /// <returns>The formatted string of the date</returns>
    internal string SetDateAsString(List<TMP_Dropdown> targetDropDownList, TextMeshProUGUI alertText)
    {
        string date = BuildDateTimeString("-", targetDropDownList);
        if(ValidatePartialDateTimeParse(date, alertText))
            return date;
        else
            return "";
    }
    /// <summary>
    /// Get the formatted string of the time based on the information from the dropdown elements.
    /// </summary>
    /// <param name="targetDropDownList">Listo with the dropdowns that contains the required info</param>
    /// <param name="alertText">Text to show in case the format is incorrect</param>
    /// <param name="setMidnight">control to set a default midnight hour if needed</param>
    /// <returns>The formatted string of the time</returns>
    internal string SetTimeAsString(List<TMP_Dropdown> targetDropDownList, TextMeshProUGUI alertText, bool setMidnight)
    {
        string hour = "";
        if (setMidnight)
            hour = "00:00:00";
        else
            hour = BuildDateTimeString(":", targetDropDownList);

        if (ValidatePartialDateTimeParse(hour, alertText))
            return hour;
        else
            return "";
    }

    internal string SetDateTimeAsString(string date, string hour)
    {
        string[] dateTime = { date, hour };
        return string.Join(" ", dateTime);
    }
    #endregion


    #region Internal parse helper methods
    /// <summary>
    /// Set the required string format based on the info set in the dropdowns.
    /// </summary>
    /// <param name="separator">string element to set between the info get on the dropdowns</param>
    /// <param name="dropdownList">A list with the dropdowns that contains the required info</param>
    /// <returns>A formatted string</returns>
    string BuildDateTimeString(string separator, List<TMP_Dropdown> dropdownList)
    {
        List<string> dataList = new List<string>();

        foreach (var dropdown in dropdownList)
        {
            dataList.Add(dropdown.options[dropdown.value].text);
        }

        string dateText = string.Join(separator, dataList);
        return dateText;
    }

    /// <summary>
    /// Validate that a part of a date obtained from the dropdown elements is on the correct format.
    /// If not, shows an error message on the inputs.
    /// </summary>
    /// <param name="dateTimeString">The string to be analized</param>
    /// <param name="alertText">The text to show in case the format is wrong</param>
    /// <returns>The validation status result</returns>
    bool ValidatePartialDateTimeParse(string dateTimeString, TextMeshProUGUI alertText)
    {
        if (!DateTime.TryParse(dateTimeString, out DateTime resultDate))
        {
            StartCoroutine(ShowTextAlert(alertText));
            Debug.LogWarning($"{GetType()} Warning. The element does not have the correct format");
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// Verify the string to include the date and time on the correct format.
    /// </summary>
    /// <param name="dateString">the date as string</param>
    /// <returns>The validation status result</returns>
    bool ValidateDateTimeFormat(string dateString)
    {
        if (!string.IsNullOrEmpty(dateString))
        {            
            if (DateTime.TryParse(dateString, out DateTime resultDate))
            {
                return true;
            }
            else
            {
                Debug.LogWarning($"{GetType()} Warning. The element is in a wrong format");
                return false;
            }
        }
        else
        {
            Debug.LogWarning($"{GetType()} Warning. The element string is null or empty");
            return false;
        }
    }
    #endregion

    #region Alert text methods
    /// <summary>
    /// Coroutine to show the selected alert text and hide it after a certain time
    /// </summary>
    /// <param name="textElement">Alert text element from the UI</param>
    /// <returns></returns>
    IEnumerator ShowTextAlert(TextMeshProUGUI textElement)
    {
        textElement.gameObject.SetActive(true);
        yield return new WaitForSeconds(5);
        textElement.DOFade(0, 1).OnComplete(() =>
        {
            textElement.gameObject.SetActive(false);
            textElement.DOFade(1, 0);
        });
    }
    #endregion
}
