using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountdownSettings : MonoBehaviour
{

    [SerializeField] DateTimeInfoSO dateInfo;
    [SerializeField] DateTimeInfoSO defaultDateInfo;

    [SerializeField] TMP_InputField targetDateIF;
    [SerializeField] Button startButton;

    private void Start()
    {
        dateInfo.Init();
        defaultDateInfo.Init();
        targetDateIF.text = "";
        startButton.onClick.AddListener(ValidateSettings);
    }


    public void ValidateSettings()
    {
        string dateText = targetDateIF.text;

        if (dateText != "")
        {
            Debug.Log($"{GetType()} Log. date to parse: {dateText}");
            dateInfo.ValidateTargetFormat(dateText);
        }
        else
        {
            Debug.LogWarning($"{GetType()} Warning. The input field is empty, please fill it");
        }
    }
}
