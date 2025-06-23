using System.Collections.Generic;
using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

[RequireComponent(typeof(DateTimeValidation))]
public class AppSettingsScreen : AppScreenElement
{
    [Header("Dropdown elements")]
    [SerializeField] List<TMP_Dropdown> targetDropDownList;
    [SerializeField] List<TMP_Dropdown> targetHourDropDownList;

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
    DateTimeValidation validationHelper;

    bool recalculateInitialDate = false;

    private void Awake()
    {
        validationHelper = GetComponent<DateTimeValidation>();
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
        
        if (validationHelper.ValidateDescription(descriptionIF, descriptionAlertText))
        {
            string date = validationHelper.SetDateAsString(targetDropDownList, dateAlertText);
            string hour = validationHelper.SetTimeAsString(targetHourDropDownList, timeAlertText, false);
            string dateText = validationHelper.SetDateTimeAsString(date, hour);

            if (validationHelper.ValidateDate(dateText))
                validationHelper.SaveDateTimeIntoObject(dateInfo, dateText);
        }
        else
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

    internal void NeedsCloseButton(bool needButton)
    {
        closeButton.gameObject.SetActive(needButton);
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
