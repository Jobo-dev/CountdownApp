using System.Collections.Generic;
using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class AppSettingsScreen : AppScreenElement
{
    [Header("Dropdown elements")]
    [SerializeField] List<TMP_Dropdown> targetDropDownList;
    [SerializeField] List<TMP_Dropdown> targetHourDropDownList;
    //[SerializeField] List<TMP_Dropdown> initialDropDownList;


    [Header ("Description field elements")]
    [SerializeField] TMP_InputField descriptionIF;

    [Header("Buttons elements")]
    [SerializeField] Button startButton;
    [SerializeField] Button closeButton;

    [Header("Alert text elements")]
    [SerializeField] TextMeshProUGUI dateAlertText;
    [SerializeField] TextMeshProUGUI timeAlertText;
    [SerializeField] TextMeshProUGUI descriptionAlertText;

    JsonFileReaderUtility jsonFileReader;
    bool recalculateInitialDate = false;

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

        jsonFileReader = new JsonFileReaderUtility();

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
        if (!ValidateDate() || !ValidateDescription())
        {
            return;
        }

        if (recalculateInitialDate)
            dateInfo.SetInitialDate();
        else
            recalculateInitialDate = true;

        dateInfo.SetDescription(descriptionIF.text);

        jsonFileReader.SaveInfoToFile(dateInfo);
        
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
            StartCoroutine(ShowTextAlert(descriptionAlertText));
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
        string date = BuildDateTimeString("-", targetDropDownList);

        if (!DateTime.TryParse(date, out DateTime resultDate))
        {
            StartCoroutine(ShowTextAlert(timeAlertText));
            Debug.LogWarning($"{GetType()} Warning. The date does not have the correct format");
            return "";
        }

        string hour = "";
        if (setMidnight)
            hour = "00:00:00";
        else
            hour = BuildDateTimeString(":", targetHourDropDownList);

        if (!DateTime.TryParse(hour, out DateTime resultTime))
        {
            StartCoroutine(ShowTextAlert(timeAlertText));
            Debug.LogWarning($"{GetType()} Warning. The time does not have the correct format");
            return "";
        }
        
        string[] dateTime = { date, hour };
        return string.Join(" ", dateTime);
    }


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

    internal void SetRecalculateInitialDate()
    {
        recalculateInitialDate = true;
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
