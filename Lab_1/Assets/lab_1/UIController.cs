using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [Header("Cube Generator Settings")]
    public Slider radiusSlider;
    public TextMeshProUGUI radiusValueText;
    public Slider maxAngleSlider;
    public TextMeshProUGUI maxAngleValueText;
    public Slider numberOfCubesSlider;
    public TextMeshProUGUI numberOfCubesValueText;
    public Toggle toggleVoidCubes;
    public Button spawnCubesButton;
    public CubeGenerator cubeGenerator;

    [Header("Cube Rotation Settings")]
    public Slider rotationSpeedSlider;
    public TextMeshProUGUI rotationSpeedValueText;
    public Slider spinSpeedSlider;
    public TextMeshProUGUI spinSpeedValueText;
    public CubeRotation cubeRotation;

    void Start()
    {
        // Initialize CubeGenerator sliders and toggle
        radiusSlider.minValue = 15.0f;
        radiusSlider.maxValue = 75.0f;
        radiusSlider.value = cubeGenerator.radius;
        radiusValueText.text = cubeGenerator.radius.ToString("F1");
        radiusSlider.onValueChanged.AddListener((value) => {
            cubeGenerator.radius = value;
            radiusValueText.text = value.ToString("F1");
        });

        maxAngleSlider.minValue = 2.0f;
        maxAngleSlider.maxValue = 50.0f;
        maxAngleSlider.value = cubeGenerator.maxAngle;
        maxAngleValueText.text = cubeGenerator.maxAngle.ToString("F1");
        maxAngleSlider.onValueChanged.AddListener((value) => {
            cubeGenerator.maxAngle = value;
            maxAngleValueText.text = value.ToString("F1");
        });

        numberOfCubesSlider.minValue = 10.0f;
        numberOfCubesSlider.maxValue = 1000.0f;
        numberOfCubesSlider.value = cubeGenerator.numberOfCubes;
        numberOfCubesValueText.text = cubeGenerator.numberOfCubes.ToString();
        numberOfCubesSlider.onValueChanged.AddListener((value) => {
            cubeGenerator.numberOfCubes = (int)value;
            numberOfCubesValueText.text = ((int)value).ToString();
        });

        toggleVoidCubes.isOn = cubeGenerator.ToggleVoidCubes;
        toggleVoidCubes.onValueChanged.AddListener((isOn) => cubeGenerator.ToggleVoidCubes = isOn);

        // Initialize CubeRotation sliders
        rotationSpeedSlider.minValue = 10.0f;
        rotationSpeedSlider.maxValue = 60.0f;
        rotationSpeedSlider.value = cubeRotation.rotationSpeed;
        rotationSpeedValueText.text = cubeRotation.rotationSpeed.ToString("F1");
        rotationSpeedSlider.onValueChanged.AddListener((value) => {
            cubeRotation.rotationSpeed = value;
            rotationSpeedValueText.text = value.ToString("F1");
        });

        spinSpeedSlider.minValue = 60.0f;
        spinSpeedSlider.maxValue = 120.0f;
        spinSpeedSlider.value = cubeRotation.spinSpeed;
        spinSpeedValueText.text = cubeRotation.spinSpeed.ToString("F1");
        spinSpeedSlider.onValueChanged.AddListener((value) => {
            cubeRotation.spinSpeed = value;
            spinSpeedValueText.text = value.ToString("F1");
        });
    }
}
