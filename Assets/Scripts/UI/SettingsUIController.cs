using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsUIController : MonoBehaviour
{
    [SerializeField] private GameObject objectsToShow;
    [SerializeField] private GameObject objectsToHide;

    [SerializeField] private float showTime;
    [SerializeField] private float hideTime;

    private float showXOriginal;
    private float hideXOriginal;

    public void ShowUI()
    {
        showXOriginal = objectsToShow.transform.position.x;
        hideXOriginal = objectsToHide.transform.position.x;
        
        LeanTween.moveX(objectsToHide, -Screen.width, hideTime)
            .setOnComplete(() =>
            {
                LeanTween.moveX(objectsToShow, Screen.width / 2f, showTime);
            });
    }

    public void HideUI()
    {
        LeanTween.moveX(objectsToShow, showXOriginal, hideTime)
            .setOnComplete(() =>
            {
                LeanTween.moveX(objectsToHide, hideXOriginal, showTime);
            });
    }
}
