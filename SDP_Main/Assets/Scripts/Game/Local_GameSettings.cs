using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Local_GameSettings : MonoBehaviour
{
    [Header("Sliders & Toggles")]
    [SerializeField] private Slider mouseSensitivitySlider;
    //[SerializeField] private TextMeshProUGUI mouseSensitivityText;

    private const string _sliderMouseSensitivity = "sliderMouseSensitiviy";

    private void Start()
    {
        //mouseSensitivityText = GetComponentInParent<TextMeshProUGUI>();
        mouseSensitivitySlider = GetComponent<Slider>();
        mouseSensitivitySlider.onValueChanged.AddListener((v) => {
            //mouseSensitivityText.text = v.ToString("0.00");
            Game_RuntimeData.playerSettings.mouseMovementSpeed = (int) v;
            
        });
    }
}
