using UnityEngine;
using System.Collections;

public class SpringController : MonoBehaviour {

    AudioSource SpringSound;

	void Start ()
    {
        SpringSound = GetComponent<AudioSource>();
	}

	void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            SpringSound.Play();
            
        }
	}
}
