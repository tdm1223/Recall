using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BGMController : MonoBehaviour
{
    bool isCreated = false;
    void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
    [System.Obsolete]
    void Update()
    {
        if (Application.loadedLevelName != "Main")
        {
            isCreated = true;
        }
        if (Application.loadedLevelName == "Main" && isCreated)
        {
            Destroy(transform.gameObject);
        }
    }
}
