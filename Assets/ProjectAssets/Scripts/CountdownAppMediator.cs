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
        dateInfo.Init();
        defaultDateInfo.Init();

        if (appMainElementsList != null || appMainElementsList.Count != 0)
        {
            countdownElementsDictionary = new Dictionary<ElementId, CountdownElement>();
            foreach (var element in appMainElementsList)
            {
                element.SetMediator(this);
                countdownElementsDictionary.Add(element.elementId, element);
                element.dateInfo = dateInfo;
                element.defaultDateInfo = defaultDateInfo;
                Debug.Log($"{GetType()} Log. Add dictionary registry: {element.elementId}, {element}");
            }
        }
        else
        {
            Debug.LogWarning($"{GetType()} Warning. The main elements list is empty");
        }
    }


    internal void InitCountDown()
    {
        countdownElementsDictionary[ElementId.AppScreen].InitElement();
        countdownElementsDictionary[ElementId.SettingsScreen].HideElement();
    }

    internal void InitSettingsFromCountdown()
    {
        CountdownSettings settings = countdownElementsDictionary[ElementId.SettingsScreen] as CountdownSettings;

        settings.gameObject.SetActive(true);
        settings.NeedsCloseButton(true);
    }

}
