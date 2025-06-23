using System.Collections.Generic;
using UnityEngine;

public class CountdownAppMediator : MonoBehaviour
{
    [SerializeField] internal DateTimeInfoSO dateInfo;
    [SerializeField] internal DateTimeInfoSO defaultDateInfo;

    [SerializeField] private List<AppScreenElement> appMainElementsList;
    private Dictionary<ElementId, AppScreenElement> countdownElementsDictionary;
    private AppSettingsScreen settings;
    private AppTimerScreen timer;
    private string countdownKey = "CountDownSet";

    private void Start()
    {
        countdownElementsDictionary = new Dictionary<ElementId, AppScreenElement>();

        InitDateTimeObjects();
        PrepareAppElements();
    }

    #region Preparation methods
    void InitDateTimeObjects()
    {
        dateInfo.Init();
        defaultDateInfo.Init();
    }
    void PrepareAppElements()
    {
        //Check that there is at least an app element on the list
        if (appMainElementsList != null && appMainElementsList.Count != 0)
        {
            //Set needed references for each App element and fill
            //the dictionary for an easier search.
            foreach (var element in appMainElementsList)
            {
                RegisterElement(element);
                InitElement(element);
                Debug.Log($"{GetType()} Log. Add dictionary registry: {element.elementId}, {element}");
            }
            settings = countdownElementsDictionary[ElementId.SettingsScreen] as AppSettingsScreen;
            timer = countdownElementsDictionary[ElementId.AppScreen] as AppTimerScreen;
            CheckIfAppHasSavedCountdown();
        }
        else
        {
            Debug.LogWarning($"{GetType()} Warning. The main elements list is empty");
        }
    }

    void RegisterElement(AppScreenElement element)
    {
        if (!countdownElementsDictionary.ContainsKey(element.elementId))
        {
            countdownElementsDictionary[element.elementId] = element;
            Debug.Log($"{GetType()} Log. Registered: {element.elementId}");
        }
        else
        {
            Debug.LogWarning($"Duplicate element ID: {element.elementId}");
        }
    }
    void InitElement(AppScreenElement element)
    {
        element.InitElementComponents(this, dateInfo, defaultDateInfo);
    }

    void SetInitScreen(ElementId id)
    {
        if (countdownElementsDictionary.ContainsKey(id))
            countdownElementsDictionary[id].SetInmediateScreenVisibility(true);
    }

    void CheckIfAppHasSavedCountdown()
    {
        if (!PlayerPrefs.HasKey(countdownKey))
        {
            //App doesn't has info saved   
            SetInitScreen(ElementId.SettingsScreen);
        }
        else
        {
            //App has info saved
            SetInitScreen(ElementId.AppScreen);
            timer.InitElement();
            Debug.Log($"{GetType()} Log. Init date time already saved!");
        }
    }
    #endregion

    #region Logic methods
    internal void InitCountDown()
    {
        timer.InitElement();
        settings.HideElement();
    }

    internal void InitSettingsFromCountdown()
    {
        

        settings.ActivateScreenWithTransition();
        timer.HideScreenWithTransition();
        settings.NeedsCloseButton(true);
        settings.SetRecalculateInitialDate();
    }

    internal void ReturnToCountdown()
    {
        timer.ActivateScreenWithTransition();
        settings.HideScreenWithTransition();
    }
    #endregion
}
