using System.Collections.Generic;
using UnityEngine;

public class CountdownAppMediator : MonoBehaviour
{
    [SerializeField] internal DateTimeInfoSO dateInfo;
    [SerializeField] internal DateTimeInfoSO defaultDateInfo;

    [SerializeField] private List<CountdownElement> appMainElementsList;
    [SerializeField] private Dictionary<ElementId, CountdownElement> countdownElementsDictionary;

    private void Start()
    {
        countdownElementsDictionary = new Dictionary<ElementId, CountdownElement>();

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
                AssignElementToDictionary(element);
                Debug.Log($"{GetType()} Log. Add dictionary registry: {element.elementId}, {element}");
            }

            CheckIfAppHasSavedCountdown();
        }
        else
        {
            Debug.LogWarning($"{GetType()} Warning. The main elements list is empty");
        }
    }

    void AssignElementToDictionary(CountdownElement element)
    {
        if (countdownElementsDictionary != null &&
            !countdownElementsDictionary.ContainsKey(element.elementId))
        {
            countdownElementsDictionary.Add(element.elementId, element);
            element.InitElementComponents(this, dateInfo, defaultDateInfo);
        }
        else
        {
            Debug.LogWarning($"{GetType()} Warning. The key {element.elementId.ToString()} is already saved! Please check the {element.GetType()} element id");
        }
    }

    void SetInitScreen(ElementId id)
    {
        if (countdownElementsDictionary.ContainsKey(id))
            countdownElementsDictionary[id].SetInmediateScreenVisibility(true);
    }

    void CheckIfAppHasSavedCountdown()
    {
        if (!PlayerPrefs.HasKey("CountDownSet"))
        {
            //App doesn't has info saved   
            SetInitScreen(ElementId.SettingsScreen);
        }
        else
        {
            //App has info saved
            SetInitScreen(ElementId.AppScreen);
            countdownElementsDictionary[ElementId.AppScreen].InitElement();
            Debug.Log($"{GetType()} Log. Init date time already saved!");
        }
    }
    #endregion

    #region Logic methods
    internal void InitCountDown()
    {
        countdownElementsDictionary[ElementId.AppScreen].InitElement();
        countdownElementsDictionary[ElementId.SettingsScreen].HideElement();
    }

    internal void InitSettingsFromCountdown()
    {
        CountdownSettings settings = countdownElementsDictionary[ElementId.SettingsScreen] as CountdownSettings;
        CountdownTimer countdownTimer = countdownElementsDictionary[ElementId.AppScreen] as CountdownTimer;

        settings.ActivateScreenWithTransition();
        countdownTimer.HideScreenWithTransition();
        settings.NeedsCloseButton(true);
        settings.SetRecalculateInitialDate();
    }

    internal void ReturnToCountdown()
    {
        CountdownSettings settings = countdownElementsDictionary[ElementId.SettingsScreen] as CountdownSettings;
        CountdownTimer countdownTimer = countdownElementsDictionary[ElementId.AppScreen] as CountdownTimer;
        countdownTimer.ActivateScreenWithTransition();
        settings.HideScreenWithTransition();
    }
    #endregion
}
