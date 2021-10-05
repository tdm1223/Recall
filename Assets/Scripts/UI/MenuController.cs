using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;
public class MenuController : MonoBehaviour
{
    GameObject escMenu;
    GameObject optionMenu;
    AudioSource buttonSound;
    void Start()
    {
        escMenu = transform.Find("MenuPanel").gameObject;
        optionMenu = transform.Find("OptionPanel").gameObject;
        buttonSound = GetComponent<AudioSource>();

        UpdateQualityLabel();
        UpdateVolumeLabel();
    }
    void Update()
    {
        if (Input.GetButtonDown("Cancel") || CrossPlatformInputManager.GetButtonDown("Menu"))
        {
            if (escMenu.activeSelf)
            {
                escMenu.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                escMenu.SetActive(true);
                Time.timeScale = 0;
            }
            if (optionMenu.activeSelf)
            {
                OpenMenuPanel();
            }
        }
    }
    public void OnResumeButtonClicked()
    {
        buttonSound.Play();
        transform.Find("MenuPanel").gameObject.SetActive(false);
        Time.timeScale = 1;
    }
    public void OnMainButtonClicked()
    {
        buttonSound.Play();
        SceneManager.LoadScene("Main");
        Time.timeScale = 1;
    }
    public void OnQuitButtonClicked()
    {
        buttonSound.Play();
        Application.Quit();
    }
    public void IncreaseQuality()
    {
        buttonSound.Play();
        QualitySettings.IncreaseLevel();
        UpdateQualityLabel();
    }
    public void DecreaseQuality()
    {
        buttonSound.Play();
        QualitySettings.DecreaseLevel();
        UpdateQualityLabel();
    }
    public void SetVolume(float value)
    {
        AudioListener.volume = value;
        UpdateVolumeLabel();
    }
    public void OpenOption()
    {
        buttonSound.Play();
        optionMenu.SetActive(true);
        escMenu.SetActive(false);
    }
    public void OpenMenuPanel()
    {
        buttonSound.Play();
        optionMenu.SetActive(false);
        escMenu.SetActive(true);
    }
    private void UpdateQualityLabel()
    {
        int currentQuality = QualitySettings.GetQualityLevel();
        string qualityName = QualitySettings.names[currentQuality];
        optionMenu.transform.Find("Quality Level").GetComponent<UnityEngine.UI.Text>().text = "Quality Level - " + qualityName;
    }
    private void UpdateVolumeLabel()
    {
        optionMenu.transform.Find("Volume").GetComponent<UnityEngine.UI.Text>().text = "Master Volume - " + (AudioListener.volume * 100).ToString("f2") + "%";
    }
}
