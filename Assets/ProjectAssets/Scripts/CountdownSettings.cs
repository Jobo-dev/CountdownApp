using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountdownSettings : CountdownElement
{
    [SerializeField] TMP_InputField targetDateIF;
    [SerializeField] TMP_InputField descriptionIF;
    [SerializeField] Button startButton;

    private void Awake()
    {
        elementId = ElementId.SettingsScreen;
        targetDateIF.text = "";
        startButton.onClick.AddListener(ValidateSettings);
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
        appMediator.InitCountDown();
    }

    private bool ValidateDescription()
    {
        string descriptionText = descriptionIF.text;
        if (descriptionText != null && descriptionText != "")
        {
            dateInfo.description = descriptionText;
            return true;

        }
        else
        {
            Debug.LogWarning($"{GetType()} Warning. The input field is empty, please fill it");
            return false;
        }
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
        throw new System.NotImplementedException();
    }
    internal override void HideElement()
    {
        this.gameObject.SetActive( false );
    }
}
