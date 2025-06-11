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

    private DateTime targetTime;
    private int lastDay, lastHour, lastMinute, lastSecond;

    void Awake()
    {
        elementId = ElementId.AppScreen;
        generateCountdownButton.onClick.AddListener(GenerateNewCountdownButton);
    }

    void CheckSavedInitialDateTime()
    {
        if (!PlayerPrefs.HasKey("CountDownSet"))
        {
            PlayerPrefs.SetInt("CountDownSet", 1);
            Debug.Log($"Saving date info");
        }
        else
        {
            Debug.Log($"Init date time already saved!  {PlayerPrefs.GetString("CountDownSet")}");
        }
    }
    
    #region Time Validation
    void UpdateCountdown()
    {
        TimeSpan remaining = targetTime - DateTime.Now;

        if (remaining.TotalSeconds <= 0)
        {
            daysText.text = "00";
            hoursText.text = "00";
            minutesText.text = "00";
            secondsText.text = "00";
            radialTransform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            radialFillImage.fillAmount = 0;
            CancelInvoke(nameof(UpdateCountdown));
            PlayerPrefs.DeleteAll();
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
        DateTime initialDate = dateInfo.initialDate;

        TimeSpan currentDifference = DateTime.Now - initialDate;
        TimeSpan totalDifference = targetTime - initialDate;

        float remainingDegrees = (float) (((currentDifference.TotalSeconds - totalDifference.Seconds) * 360) / totalDifference.TotalSeconds);
        float remainingRadial = (float) ((currentDifference.TotalSeconds - totalDifference.Seconds) / totalDifference.TotalSeconds);

        // Avoid negative numbers
        if (remaining.TotalSeconds < 0)
        {
            radialTransform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            radialFillImage.fillAmount = remainingRadial;
            return;
        }
        else
        {
            radialTransform.localRotation = Quaternion.Euler(0f, 0f, remainingDegrees);
            radialFillImage.fillAmount = 1 - remainingRadial;
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

        DateTime initialDateTime = dateInfo.initialDate;
        string currentDate = initialDateTime.ToString();

        CheckSavedInitialDateTime();
        
        descriptionText.text = dateInfo.description;
        
        targetTime = dateInfo.targetDate;
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
