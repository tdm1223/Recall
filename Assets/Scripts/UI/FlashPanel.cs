using UnityEngine;
using System.Collections;

public class FlashPanel : MonoBehaviour {

    public GameObject FlashIconPrefab;

    public void UpdateFlashIcon(int flash)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < flash; i++)
        {
            GameObject FlashIcon = (GameObject)Instantiate(
                FlashIconPrefab,
                new Vector3(i * 30 - 100, 0, 0),
                Quaternion.identity
                );
            FlashIcon.transform.SetParent(transform, false);
        }
    }
}
