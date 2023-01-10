using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Toggle : MonoBehaviour
{
    public bool Value;

    public Image image;
    public Sprite TrueImage;
    public Sprite FalseImage;
    

    public void Press()
    {
        Value=!Value;
        image.sprite=Value?TrueImage:FalseImage;
    }
}
