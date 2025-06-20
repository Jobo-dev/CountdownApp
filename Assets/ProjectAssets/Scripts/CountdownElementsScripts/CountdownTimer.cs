using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : CountdownElement
{
    [Header("Generate new countdown button")]
    [SerializeField] internal Button generateCountdownButton;

    [Header("Description text elements")]
    [SerializeField] internal TextMeshProUGUI descriptionText;

    [Header("Radial image elements")]
    [SerializeField] internal Transform radialTransform;
    [SerializeField] internal Image radialFillImage;

    [Header("Digits elements")]
    [SerializeField] internal TextMeshProUGUI daysText;
    [SerializeField] internal TextMeshProUGUI hoursText;
    [SerializeField] internal TextMeshProUGUI minutesText;
    [SerializeField] internal TextMeshProUGUI secondsText;

    DateTime targetTime, initialTime;
    int lastDay, lastHour, lastMinute, lastSecond;
    JsonFileReaderUtility jsonFileReader;
    string dataIdKey = "CountDownSet";

    void Awake()
    {
        elementId = ElementId.AppScreen;
        jsonFileReader = new JsonFileReaderUtility();
        generateCountdownButton.onClick.AddListener(GenerateNewCountdownButton);
    }

    void CheckSavedInitialDateTime()
    {
        if (!PlayerPrefs.HasKey(dataIdKey))
        {
            PlayerPrefs.SetInt(dataIdKey, 1);
            Debug.Log($"Saving date info");
        }
        else
        {
            Debug.Log($"Init date time already saved!  {PlayerPrefs.GetString(dataIdKey)}");
        }
    }
    
    #region Time Validation
    void UpdateCountdown()
    {
        TimeSpan remaining = targetTime - DateTime.Now;

        if (remaining.TotalSeconds <= 0)
        {
            ResetCountdownUI();
            CancelInvoke(nameof(UpdateCountdown));
            PlayerPrefs.DeleteKey(dataIdKey);
            return;
        }

        int remainingDays = remaining.Days;
        int remainingHours = remaining.Hours;
        int remainingMinutes = remaining.Minutes;
        int remainingSeconds = remaining.Seconds;


        CompareTimeElements(remainingDays, daysText, ref lastDay);
        CompareTimeElements(remainingHours, hoursText, ref lastHour);
        CompareTimeElements(remainingMinutes, minutesText, ref lastMinute);
        CompareTimeElements(remainingSeconds, secondsText, ref lastSecond);

        AnimateRadial(remaining);
    }

    void ResetCountdownUI()
    {
        daysText.text = "00";
        hoursText.text = "00";
        minutesText.text = "00";
        secondsText.text = "00";
        radialTransform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        radialFillImage.fillAmount = 0;
    }

    void CompareTimeElements(int remainingElement, TextMeshProUGUI textElement, ref int lastElement)
    {
        if (remainingElement != lastElement)
        {
            AnimateText(textElement, remainingElement);
            lastElement = remainingElement;
        }
    }
    #endregion

    #region Countdown Update Methods
    void AnimateRadial(TimeSpan remaining)
    {

        TimeSpan currentDifference = DateTime.Now - initialTime;
        TimeSpan totalDifference = targetTime - initialTime;

        float remainingDegrees = (float) (((currentDifference.TotalSeconds - totalDifference.TotalSeconds) * 360) / totalDifference.TotalSeconds);
        float remainingRadial = (float) ((totalDifference.TotalSeconds - currentDifference.TotalSeconds) / totalDifference.TotalSeconds);

        // Avoid negative numbers
        if (remaining.TotalSeconds < 0)
        {
            radialTransform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            radialFillImage.fillAmount = 0;
            return;
        }
        else
        {
            radialTransform.localRotation = Quaternion.Euler(0f, 0f, remainingDegrees);
            radialFillImage.fillAmount = remainingRadial;
        }
    }

    void AnimateText(TextMeshProUGUI tmp, int value)
    {
        tmp.text = value.ToString("D2");
        tmp.transform.localScale = Vector3.one;

        tmp.transform
            .DOScale(1.3f, 0.3f)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
                tmp.transform.DOScale(1f, 0.2f)
            );
    }
    #endregion

    #region Buttons Methods
    internal void GenerateNewCountdownButton()
    {
        appMediator.InitSettingsFromCountdown();
    }
    #endregion

    #region Mediator access methods

    internal override void InitElement()
    {
        ActivateScreenWithTransition();

        targetTime = DateTime.Parse(jsonFileReader.LoadDataFromJson(DateDataType.TargetDate));
        initialTime = DateTime.Parse(jsonFileReader.LoadDataFromJson(DateDataType.InitialDate));
        descriptionText.text = jsonFileReader.LoadDataFromJson(DateDataType.Description);

        CheckSavedInitialDateTime();

        UpdateCountdown();
        InvokeRepeating(nameof(UpdateCountdown), 0f, 1f);
    }

    internal override void HideElement()
    {
        CancelInvoke();
        HideScreenWithTransition();
    }
    #endregion
}
