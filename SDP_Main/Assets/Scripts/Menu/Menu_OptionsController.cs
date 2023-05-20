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
        mouseSensitivitySlider = GetComponentsInChildren<Slider>()[1]; // Get Mouse Sensitivity Slider Object
        mouseSensitivitySlider.value = Game_RuntimeData.playerSettings.lookSensitivity; //Set Initial Value
        mouseSensitivitySlider.onValueChanged.AddListener((value) => { //Start Active Listener for Changes
            Game_RuntimeData.playerSettings.setMouseSpeed(value); //Set new Value upon Change
        });

        //Invert Mouse Y Axis Toggle
        invertYAxisToggle = GetComponentsInChildren<Toggle>()[0]; //Get Invert Y Axis Toggle Object
        invertYAxisToggle.isOn = Game_RuntimeData.playerSettings.invertMouseYAxis; //Set Initial Value
        invertYAxisToggle.onValueChanged.AddListener((IsOn) => { //Start Active Listener for Changes
            Game_RuntimeData.playerSettings.setInvertMouseYAxis(IsOn); //Set new Value upon Change
        });

        //Global Volume Control
        volumeSlider = GetComponentsInChildren<Slider>()[0]; //Get Volume Slider Object
        volumeSlider.value = Game_RuntimeData.playerSettings.globalVolume; //Set Initial Value
        volumeSlider.onValueChanged.AddListener((value) => { //Start Active Listener for Changes
            Game_RuntimeData.playerSettings.setGlobalVolume(value); //Set new Value upon Change
        });
    }
}
