using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public string Scene;

    private GameObject StartButton;
    private GameObject HowToPlayButton;
    private GameObject OptionButton;
    private GameObject QuitButton;
    private RectTransform Title;

    private GameObject HowToPlayPanel;
    private GameObject OptionPanel;

    AudioSource buttonSound;


    void Awake()
    {
        buttonSound = GetComponent<AudioSource>();

        StartButton = transform.Find("StartButton").gameObject;
        HowToPlayButton = transform.Find("HowToPlayButton").gameObject;
        OptionButton = transform.Find("OptionButton").gameObject;
        QuitButton = transform.Find("QuitButton").gameObject;
        Title = transform.Find("Title").gameObject.GetComponent<RectTransform>();

        HowToPlayPanel = transform.Find("HowToPlayPanel").gameObject;
        OptionPanel = transform.Find("OptionPanel").gameObject;
    }


    public void OnStartButtonClicked()
    {
        buttonSound.Play();
        SceneManager.LoadScene(Scene);
    }

    public void OnHowToPlayButtonClicked()
    {
        buttonSound.Play();

        StartButton.SetActive(false);
        HowToPlayButton.SetActive(false);
        OptionButton.SetActive(false);
        QuitButton.SetActive(false);

        HowToPlayPanel.gameObject.SetActive(true);

        Title.sizeDelta = new Vector2(250,125);
        Title.anchoredPosition = new Vector3(200, -60);
    }

    public void OnCancleButtonClicked(string str)
    {
        buttonSound.Play();
        StartButton.SetActive(true);
        HowToPlayButton.SetActive(true);
        OptionButton.SetActive(true);
        QuitButton.SetActive(true);

        if(str.Equals("HowToPlayPanel"))
            HowToPlayPanel.SetActive(false);
        if(str.Equals("OptionPanel"))
            OptionPanel.gameObject.SetActive(false);

        Title.sizeDelta = new Vector2(500, 250);
        Title.anchoredPosition = new Vector3(250, -125);
    }

    public void OnOptionButton()
    {
        buttonSound.Play();

        StartButton.SetActive(false);
        HowToPlayButton.SetActive(false);
        OptionButton.SetActive(false);
        QuitButton.SetActive(false);

        OptionPanel.gameObject.SetActive(true);

        Title.sizeDelta = new Vector2(250, 125);
        Title.anchoredPosition = new Vector3(200, -60);
    }

    public void OnQuitButtonClicked()
    {
        buttonSound.Play();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
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
    private void UpdateQualityLabel()
    {
        int currentQuality = QualitySettings.GetQualityLevel();
        string qualityName = QualitySettings.names[currentQuality];
        OptionPanel.transform.Find("Quality Level").GetComponent<UnityEngine.UI.Text>().text = "Quality Level - " + qualityName;
    }
    private void UpdateVolumeLabel()
    {
        OptionPanel.transform.Find("Volume").GetComponent<UnityEngine.UI.Text>().text = "Master Volume - " + (AudioListener.volume * 100).ToString("f2") + "%";
    }
}