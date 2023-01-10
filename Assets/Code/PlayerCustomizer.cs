using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerCustomizer : MonoBehaviour
{
    public string Name;
    public Color color;
    public Slider R;
    public Slider G;
    public Slider B;
    public TMP_InputField NameInput;
    public TextMeshProUGUI NameText;


    string aplhabet="abcdefghijklmnopqrstuvwxyz";
    
    public void Start()
    {
        NameInput.text=aplhabet[Random.Range(0,26)].ToString()+aplhabet[Random.Range(0,26)].ToString()+aplhabet[Random.Range(0,26)].ToString()+aplhabet[Random.Range(0,26)].ToString()+aplhabet[Random.Range(0,26)].ToString()+aplhabet[Random.Range(0,26)].ToString();
        color=Color.HSVToRGB(Random.Range(0,1f),Random.Range(0,1f),Random.Range(0,0.8f));

        R.value=color.r;
        B.value=color.b;
        G.value=color.g;
    }
    void Update()
    {
        Name=NameInput.text;

        color=new Color(R.value,G.value,B.value);
        
        NameInput.GetComponent<Image>().color=color;
    }
}
