using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SensitivitySliderController : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI sensText;
    [SerializeField] private MouseLook mouseLook;

    // Start is called before the first frame update
    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("sensitivity", 1f);
        sensText.text = (slider.value * 100f).ToString("###");
        slider.onValueChanged.AddListener(delegate { OnChanged(); });
    }

    private void OnChanged()
    {
        sensText.text = Mathf.Round(slider.value * 100f).ToString();
        PlayerPrefs.SetFloat("sensitivity", slider.value);

        if (mouseLook != null) mouseLook.SetSensitivity(Mathf.Round(slider.value * 100f));
    }
}
