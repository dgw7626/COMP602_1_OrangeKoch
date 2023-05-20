using UnityEngine;
using UnityEngine.UI;

public class Menu_OptionsController : MonoBehaviour
{
    [Header("Sliders & Toggles")]
    [SerializeField] private Slider mouseSensitivitySlider;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Toggle invertYAxisToggle;


    /// <summary>
    /// Starts a listener when the Options Menu is opened to listen for changes on Sliders and Toggles
    /// </summary>
    private void Start()
    {
        //Mouse Sensitivity Slider
        mouseSensitivitySlider = GetComponentsInChildren<Slider>()[1];
        mouseSensitivitySlider.onValueChanged.AddListener((value) => {
            Game_RuntimeData.playerSettings.setMouseSpeed(value);
        });

        //Invert Mouse Y Axis Toggle 
        invertYAxisToggle = GetComponentsInChildren<Toggle>()[0];
        invertYAxisToggle.onValueChanged.AddListener((IsOn) => {
            Game_RuntimeData.playerSettings.setInvertMouseYAxis(IsOn);
        });

        //Global Volume Control
        volumeSlider = GetComponentsInChildren<Slider>()[0];
        volumeSlider.onValueChanged.AddListener((value) => {
            Game_RuntimeData.playerSettings.setGlobalVolume(value);
        });
    }
}
