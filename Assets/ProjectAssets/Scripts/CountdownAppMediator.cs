using System.Collections.Generic;
using UnityEngine;

public class CountdownAppMediator : MonoBehaviour
{
    [SerializeField] private List<CountdownElement> appMainElementsList;
    [SerializeField] private Dictionary<ElementId, CountdownElement> countdownElementsDictionary;

    private void Start()
    {
        if (appMainElementsList != null || appMainElementsList.Count != 0)
        {
            countdownElementsDictionary = new Dictionary<ElementId, CountdownElement>();
            foreach (var element in appMainElementsList)
            {
                element.SetMediator(this);
                countdownElementsDictionary.Add(element.elementId, element);
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

}
