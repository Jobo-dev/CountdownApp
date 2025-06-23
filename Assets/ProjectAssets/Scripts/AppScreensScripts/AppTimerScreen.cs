using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AppTimerScreen : AppScreenElement
{
    [Header("Generate new countdown button")]
    [SerializeField] private Button generateCountdownButton;

    [Header("Description text elements")]
    [SerializeField] private TextMeshProUGUI descriptionText;

    [Header("Countdown component")]
    [SerializeField] private CountdownComponent countdown;

    
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

        DateTime targetTime = DateTime.Parse(jsonFileReader.LoadDataFromJson(DateDataType.TargetDate));
        DateTime initialTime = DateTime.Parse(jsonFileReader.LoadDataFromJson(DateDataType.InitialDate));
        descriptionText.text = jsonFileReader.LoadDataFromJson(DateDataType.Description);

        countdown.InitCountdownProcess(targetTime, initialTime);

        CheckSavedInitialDateTime();
    }

    internal override void HideElement()
    {
        CancelInvoke();
        HideScreenWithTransition();
    }
    #endregion
}
