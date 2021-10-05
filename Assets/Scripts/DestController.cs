using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class DestController : MonoBehaviour
{

    public string str;
    public GameObject ClearUIPrefab;
    private bool clear = false;
    AudioSource ClearSound;
    public void Start()
    {
        ClearSound = GetComponent<AudioSource>();
    }
    public void Update()
    {
        if (clear)
        {
            if (Input.GetButtonDown("Fire1") || CrossPlatformInputManager.GetButtonDown("Jump"))
                SceneManager.LoadScene(str);

            GetComponent<SpriteRenderer>().enabled = false;
        }
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !clear)
        {
            ClearSound.Play();
            Instantiate(ClearUIPrefab, Vector3.zero, Quaternion.identity);
            clear = true;
        }
    }
}
