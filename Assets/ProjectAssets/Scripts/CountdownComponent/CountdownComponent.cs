using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountdownComponent : MonoBehaviour
{
    [Header("Radial image elements")]
    [SerializeField] private Transform radialTransform;
    [SerializeField] private Image radialFillImage;

    [Header("Digits elements")]
    [SerializeField] private TextMeshProUGUI daysText;
    [SerializeField] private TextMeshProUGUI hoursText;
    [SerializeField] private TextMeshProUGUI minutesText;
    [SerializeField] private TextMeshProUGUI secondsText;

    DateTime targetTime, initialTime;
    int lastDay, lastHour, lastMinute, lastSecond;
    string dataIdKey = "CountDownSet";

    #region Monobehaviour callbacks
    private void Start()
    {
        SetDates(DateTime.Now, DateTime.Now);
    }
    #endregion

    internal void InitCountdownProcess(DateTime targetDate, DateTime initialDate)
    {
        SetDates(targetDate, initialDate);

        UpdateCountdown();
        InvokeRepeating(nameof(UpdateCountdown), 0f, 1f);
    }

    void SetDates(DateTime targetDate, DateTime initialDate)
    {
        targetTime = targetDate;
        initialTime = initialDate;
    }

    #region Time Validation
    /// <summary>
    /// Compare the current datetime with the target datetime.
    /// In case the timer reached the target, it will reset all
    /// the text elements. 
    /// If not, it will compare the remaining time against the 
    /// registered time on the counter.
    /// </summary>
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

    /// <summary>
    /// Ensure to set allk the counter text elements as zero and the 
    /// radial image to reach the desired position
    /// </summary>
    void ResetCountdownUI()
    {
        daysText.text = "00";
        hoursText.text = "00";
        minutesText.text = "00";
        secondsText.text = "00";
        radialTransform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        radialFillImage.fillAmount = 0;
    }

    /// <summary>
    /// Compare the counter value against the current reamaining time element to 
    /// check if the text needs to be animated. It saves the remaining element
    /// value for a future check.
    /// </summary>
    /// <param name="remainingElement">The remaining time element value</param>
    /// <param name="textElement">The visual text component</param>
    /// <param name="lastElement">The current time element value</param>
    void CompareTimeElements(int remainingElement, TextMeshProUGUI textElement, ref int lastElement)
    {
        if (remainingElement != lastElement)
        {
            AnimateText(textElement, remainingElement);
            lastElement = remainingElement;
        }
    }
    #endregion

    #region Countdown Animation Update Methods
    /// <summary>
    /// Animate the counter circle to set a visual helper of how many time is left 
    /// from the day that the timer has been initialized.
    /// </summary>
    /// <param name="remaining">The remaining time against the target date</param>
    void AnimateRadial(TimeSpan remaining)
    {
        TimeSpan currentDifference = DateTime.Now - initialTime;
        TimeSpan totalDifference = targetTime - initialTime;

        float remainingDegrees = (float)(((currentDifference.TotalSeconds - totalDifference.TotalSeconds) * 360) / totalDifference.TotalSeconds);
        float remainingRadial = (float)((totalDifference.TotalSeconds - currentDifference.TotalSeconds) / totalDifference.TotalSeconds);

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

    /// <summary>
    /// Animate the current text numbers when a time fraction has passed.
    /// </summary>
    /// <param name="digits">Text element representing the digits on the counter</param>
    /// <param name="value">The value to set into the TMP component</param>
    void AnimateText(TextMeshProUGUI digits, int value)
    {
        digits.text = value.ToString("D2");
        digits.transform.localScale = Vector3.one;

        digits.transform
            .DOScale(1.3f, 0.3f)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
                digits.transform.DOScale(1f, 0.2f)
            );
    }
    #endregion
}
