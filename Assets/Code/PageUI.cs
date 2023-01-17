using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageUI : MonoBehaviour
{
    public int CurrentPage;
    public List<GameObject> PageObjects;

    void Update()
    {
        int x=0;
        while(x<PageObjects.Count)
        {
            PageObjects[x].SetActive(x==CurrentPage);
            x++;
        }
    }


    public void ChangePage(int Page)
    {
        CurrentPage=Page;
    }
}
