using DG.Tweening;
using UnityEngine;

public enum ElementId
{
    None,
    AppScreen,
    SettingsScreen
}

[RequireComponent(typeof(CanvasGroup))]
public abstract class CountdownElement : MonoBehaviour
{
    internal CountdownAppMediator appMediator;
    internal ElementId elementId = ElementId.None;

    internal DateTimeInfoSO dateInfo;
    internal DateTimeInfoSO defaultDateInfo;

    internal CanvasGroup elementCanvasGroup;
    
    internal void InitElementComponents(CountdownAppMediator mediator, DateTimeInfoSO targetInfo, DateTimeInfoSO defaultInfo)
    {
        SetMediator(mediator);
        dateInfo = targetInfo;
        defaultDateInfo = defaultInfo;
        elementCanvasGroup = GetComponent<CanvasGroup>();
        SetInmediateScreenVisibility(false);
    }

    void SetMediator(CountdownAppMediator mediator)
    {
        appMediator = mediator;
    }

    internal void SetInmediateScreenVisibility(bool isVisible)
    {
        if(isVisible) 
            elementCanvasGroup.alpha = 1;
        else 
            elementCanvasGroup.alpha = 0;

        elementCanvasGroup.blocksRaycasts = isVisible;
        elementCanvasGroup.interactable = isVisible;
        elementCanvasGroup.ignoreParentGroups = false;
    }

    internal void ActivateScreenWithTransition()
    {
        elementCanvasGroup.DOFade(1f, 1f).OnStart(() =>
        {
            elementCanvasGroup.blocksRaycasts = true;
            elementCanvasGroup.interactable = true;
            elementCanvasGroup.ignoreParentGroups = false;
        });
    }
    internal void HideScreenWithTransition()
    {
        elementCanvasGroup.DOFade(0f, 1f).OnStart(() =>
        {
            elementCanvasGroup.blocksRaycasts = false;
            elementCanvasGroup.interactable = false;
            elementCanvasGroup.ignoreParentGroups = false;
        });
    }

    internal abstract void InitElement();
    internal abstract void HideElement();

}
