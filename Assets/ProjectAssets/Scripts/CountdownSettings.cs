using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountdownSettings : CountdownElement
{
    [SerializeField] TMP_InputField targetDateIF;
    [SerializeField] TMP_InputField descriptionIF;
    [SerializeField] Button startButton;
    [SerializeField] Button closeButton;

    private void Awake()
    {
        elementId = ElementId.SettingsScreen;
        targetDateIF.text = "";
        startButton.onClick.AddListener(ValidateSettings);
        closeButton.onClick.AddListener(() =>
        {
            this.gameObject.SetActive(false);
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
        string dateText = targetDateIF.text;

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

    internal override void InitElement()
    {
        ActivateScreenWithTransition();
    }
    internal override void HideElement()
    {
        HideScreenWithTransition();
    }
}
