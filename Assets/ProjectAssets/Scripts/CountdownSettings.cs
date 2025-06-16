using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountdownSettings : CountdownElement
{
    
    [SerializeField] List<TMP_Dropdown> targetDropDownList;
    [SerializeField] List<TMP_Dropdown> targetHourDropDownList;
    //[SerializeField] List<TMP_Dropdown> initialDropDownList;


    [SerializeField] TMP_InputField descriptionIF;
    [SerializeField] Button startButton;
    [SerializeField] Button closeButton;

    private void Awake()
    {
        if (targetDropDownList.Count == 0)
        {
            Debug.LogWarning($"{GetType()} Warning. The target date dropdown list is empty, please assign the corresponding elements");
            return;
        }
        if (targetHourDropDownList.Count == 0)
        {
            Debug.LogWarning($"{GetType()} Warning. The target hour dropdown list is empty, please assign the corresponding elements");
            return;
        }

        elementId = ElementId.SettingsScreen;
        startButton.onClick.AddListener(ValidateSettings);
        closeButton.onClick.AddListener(() =>
        {
            appMediator.ReturnToCountdown();
        });
        closeButton.gameObject.SetActive(false);
    }


    private void ValidateSettings()
    {
        if (!ValidateDate())
        {
            return;
        }
        if (!ValidateDescription())
        {
            return;
        }

        dateInfo.SetDescription(descriptionIF.text);
        dateInfo.SetTargetDateString();
        appMediator.InitCountDown();
    }


    private bool ValidateDescription()
    {
        string descriptionText = descriptionIF.text;
        if (descriptionText != null && descriptionText != "")
        {
            return true;
        }
        else
        {
            Debug.LogWarning($"{GetType()} Warning. The input field is empty, please fill it");
            return false;
        }
    }

    internal void NeedsCloseButton(bool needButton)
    {
        closeButton.gameObject.SetActive(needButton);
    }

    private bool ValidateDate()
    {
        string dateText = SetDateTimeAsString(false);

        if (dateText != "")
        {
            Debug.Log($"{GetType()} Log. date to parse: {dateText}");
            if (dateInfo.ValidateTargetFormat(dateText))
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

    string SetDateTimeAsString(bool setMidnight)
    {
        string hour = "";
        
        if (setMidnight)
            hour = "00:00:00";
        else
            hour = BuildDateTimeString(":", targetHourDropDownList);        
        
        string date = BuildDateTimeString("-", targetDropDownList);
        
        string[] dateTime = { date, hour };
        return string.Join(" ", dateTime);
    }

    internal override void InitElement()
    {
        ActivateScreenWithTransition();
    }
    internal override void HideElement()
    {
        HideScreenWithTransition();
    }
}
