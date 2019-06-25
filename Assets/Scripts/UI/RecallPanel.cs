using UnityEngine;
using System.Collections;

public class RecallPanel : MonoBehaviour {

    public GameObject RecallIconPrefab;

	public void UpdateRecallIcon (int recall)
    { 
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

	    for (int i =0; i<recall; i++)
        {
            GameObject RecallIcon = (GameObject)Instantiate(
                RecallIconPrefab,
                new Vector3(i * 30 - 100, 0, 0),
                Quaternion.identity
                );
            RecallIcon.transform.SetParent(transform, false);
        }
	}
}
