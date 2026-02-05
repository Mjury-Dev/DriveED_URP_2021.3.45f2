using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerProfile : MonoBehaviour
{
    public GameObject InputCanvas;
    public GameObject DisplayCanvas;

    public TMP_InputField _name;
    public TMP_Text _nameText;

    private string Name;

    public void SetProfile()
    {
        Name = _name.text;
        SwapCanvas();
    }

    void SwapCanvas()
    {
        _nameText.text = Name;
        InputCanvas.SetActive(false);
        DisplayCanvas.SetActive(true);
    }
}
