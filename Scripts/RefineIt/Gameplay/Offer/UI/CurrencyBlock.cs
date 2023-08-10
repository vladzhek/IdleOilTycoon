using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyBlock : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Image _image;

    public void SetCurrency(Sprite sprite, string text)
    {
        _text.text = text;
        _image.sprite = sprite;
    }
}
