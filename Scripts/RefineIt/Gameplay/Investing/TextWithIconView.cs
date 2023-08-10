using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.Extensions;

public class TextWithIconView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private GameObject _icon;
    [SerializeField] private TMP_Text _amount;

    public void SetTextView(string text)
    {
        _text.text = text;
    }
    
    public void SetAmountView(string amount)
    {
        _amount.text = amount;
    }
    
    public void SetIcon(Sprite sprite)
    {
        _icon.GetComponent<Image>().sprite = sprite;
    }

    public void SetActiveIcon(bool isActive)
    {
        _icon.SetActive(isActive);
    }
}
