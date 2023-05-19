using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Local_GameSettings : MonoBehaviour
{
    [Header("Sliders & Toggles")]
    [SerializeField] private Slider mouseSensitivitySlider;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Toggle invertYAxisToggle;

    private const string _sliderMouseSensitivity = "sliderMouseSensitivity";

    private void Start()
    {
        mouseSensitivitySlider = GetComponentsInChildren<Slider>()[1];
        mouseSensitivitySlider.onValueChanged.AddListener((value) => {
            Game_RuntimeData.playerSettings.lookSensitivity = (int) value;
        });


        invertYAxisToggle = GetComponentsInChildren<Toggle>()[0];
        invertYAxisToggle.onValueChanged.AddListener((IsOn) => {
            Game_RuntimeData.playerSettings.invertMouseYAxis = IsOn;
        });

        //Volume Control
        volumeSlider = GetComponentsInChildren<Slider>()[0];
        volumeSlider.onValueChanged.AddListener((value) => {
            Game_RuntimeData.playerSettings.globalVolume = value;
        });
    }
}
