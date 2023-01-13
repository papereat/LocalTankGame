using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveChooserObject : MonoBehaviour
{
    public TextMeshProUGUI SaveInfoText;
    public string fileName;
    public GameSaveData GSD;

    void Start()
    {
        GSD=new GameSaveData();
        SaveControler.current.Load(GSD,"Game Save/"+fileName);
        PutInfo();
    }


    public void PutInfo()
    {
        SaveInfoText.text=GSD.Name+"("+GSD.Date+")\nTank Count:"+GSD.Tanks.Count.ToString();
    }
    public void Delete()
    {

    }
    public void Load()
    {
        MainMenuControler.current.LoadGame(fileName);
    }
}
