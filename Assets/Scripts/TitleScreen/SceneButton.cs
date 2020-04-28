using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SceneButton : MonoBehaviour, IPointerClickHandler
{
    public OptionsManager manager;
    public LoadingPanel loadingPanel;
    public string sceneToLoad;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (manager.locked) return;
        manager.Lock();
        manager.gameObject.SetActive(false);
        loadingPanel.gameObject.SetActive(true);
        loadingPanel.BeginLoading(sceneToLoad);
    }

    

}
