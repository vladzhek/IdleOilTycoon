using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageController : MonoBehaviour
{
    [SerializeField] private List<GameObject> Pages;
    [SerializeField] private List<Toggle> Toggles;
    [SerializeField] private List<GameObject> Banners;
    private int currentPageIndex = 0;

    public void Start()
    {
        for (int i = 0; i < Toggles.Count; i++)
        {
            int pageIndex = i;
            Toggles[i].onValueChanged.AddListener((isOn) => OnToggleValueChanged(pageIndex, isOn));
        }
        
        ShowPage(currentPageIndex);
    }

    public void OnToggleValueChanged(int pageIndex, bool isOn)
    {
        if (isOn)
        {
            ShowPage(pageIndex);
        }
    }

    private void ShowPage(int pageIndex)
    {
        if (pageIndex >= 0 && pageIndex < Pages.Count)
        {
            for (int i = 0; i < Pages.Count; i++)
            {
                Pages[i].SetActive(i == pageIndex);
                Banners[i].SetActive(i == pageIndex);
                
                Toggles[i].isOn = i == pageIndex;
                Toggles[i].interactable = i != pageIndex;
            }
            currentPageIndex = pageIndex;
        }
    }
}
