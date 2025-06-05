using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountdownSettings : CountdownElement
{

    [SerializeField] DateTimeInfoSO dateInfo;
    [SerializeField] DateTimeInfoSO defaultDateInfo;

    [SerializeField] TMP_InputField targetDateIF;
    [SerializeField] Button startButton;

    private void Start()
    {
        elementId = ElementId.SettingsScreen;
        dateInfo.Init();
        defaultDateInfo.Init();
        targetDateIF.text = "";
        startButton.onClick.AddListener(ValidateSettings);
    }


    private void ValidateSettings()
    {
        string dateText = targetDateIF.text;

        if (dateText != "")
        {
            Debug.Log($"{GetType()} Log. date to parse: {dateText}");
            if (dateInfo.ValidateTargetFormat(dateText))
            {
                appMediator.InitCountDown();

            }
            else
            {
                Debug.LogWarning($"{GetType()} Warning. The Date target format is incorrect");
            }
        }
        else
        {
            Debug.LogWarning($"{GetType()} Warning. The input field is empty, please fill it");
        }
    }

    internal override void HideElement()
    {
        throw new System.NotImplementedException();
    }

    internal override void InitElement()
    {
        throw new System.NotImplementedException();
    }
}
