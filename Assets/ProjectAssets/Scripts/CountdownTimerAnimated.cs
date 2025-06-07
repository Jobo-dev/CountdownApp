using DG.Tweening;
using System;
using System.CodeDom.Compiler;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimerAnimated : CountdownElement
{
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
    }

    void CheckSavedInitialDateTime()
    {
        DateTime initialDateTime = dateInfo.initialDate;
        string currentDate = initialDateTime.ToString();
        

        if (!PlayerPrefs.HasKey("InitDateTime"))
        {
            PlayerPrefs.SetString("InitDateTime", currentDate);
            Debug.Log($"Saving init date time: {currentDate}");
        }
        else
        {
            Debug.Log($"Init date time already saved!  {PlayerPrefs.GetString("InitDateTime")}");
        }
    }

    void UpdateCountdown()
    {
        TimeSpan remaining = targetTime - DateTime.Now;

        if (remaining.TotalSeconds <= 0)
        {
            daysText.text = "00";
            hoursText.text = "00";
            minutesText.text = "00";
            secondsText.text = "00";
            CancelInvoke(nameof(UpdateCountdown));
            return;
        }

        int remainingDays = remaining.Days;
        int remainingHours = remaining.Hours;
        int remainingMinutes = remaining.Minutes;
        int remainingSeconds = remaining.Seconds;

        if (remainingDays != lastDay)
        {
            AnimateText(daysText, remainingDays);
            lastDay = remainingDays;
        }
        if (remainingHours != lastHour)
        {
            AnimateText(hoursText, remainingHours);
            lastHour = remainingHours;
        }
        if (remainingMinutes != lastMinute)
        {
            AnimateText(minutesText, remainingMinutes);
            lastMinute = remainingMinutes;
        }
        if (remainingSeconds != lastSecond)
        {
            AnimateText(secondsText, remainingSeconds);
            lastSecond = remainingSeconds;
        }
        AnimateRadial(remaining);
    }

    void AnimateRadial(TimeSpan remaining)
    {
        DateTime initialDate = DateTime.Parse(PlayerPrefs.GetString("InitDateTime"));

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


    internal override void InitElement()
    {
        CheckSavedInitialDateTime();
        descriptionText.text = dateInfo.description;
        targetTime = dateInfo.targetDate;
        UpdateCountdown();
        InvokeRepeating(nameof(UpdateCountdown), 0f, 1f);
    }

    internal override void HideElement()
    {
        CancelInvoke();
        this.gameObject.SetActive(false);

    }
}
