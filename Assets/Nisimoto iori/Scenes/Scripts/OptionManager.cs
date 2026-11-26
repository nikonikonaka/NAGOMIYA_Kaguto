using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    [Header("‰æ–ÊØ‚è‘Ö‚¦")]
    public GameObject titleUI;
    public GameObject optionPanel;

    [Header("BGM‰¹—Ê")]
    public AudioSource bgmSource;
    public Slider bgmSlider;

    [Header("SE‰¹—Ê")]
    public Slider seSlider;

    private void Start()
    {
        float savedBGM = PlayerPrefs.GetFloat("BGMVolume", 1f);

        bgmSource.volume = savedBGM;
        bgmSlider.value = savedBGM;

        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
    }

    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = volume;
        PlayerPrefs.SetFloat("BGMVolume", volume);
        PlayerPrefs.Save();
    }

    public void SetSEVolume(float volume)
    {
        // SEÀ‘•‚Éˆ—‚ğ’Ç‰Á‚·‚é
    }

    public void OpenOption()
    {
        titleUI.SetActive(false);
        optionPanel.SetActive(true);
    }

    public void CloseOption()
    {
        optionPanel.SetActive(false);
        titleUI.SetActive(true);
    }
}
