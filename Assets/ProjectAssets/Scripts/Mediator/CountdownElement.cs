using UnityEngine;

public enum ElementId
{
    None,
    AppScreen,
    SettingsScreen
}

public abstract class CountdownElement : MonoBehaviour
{
    internal CountdownAppMediator appMediator;
    internal ElementId elementId = ElementId.None; 

    internal void SetMediator(CountdownAppMediator mediator)
    {
        appMediator = mediator;

    }

    internal abstract void InitElement();
    internal abstract void HideElement();

}
