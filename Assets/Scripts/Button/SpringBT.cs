using UnityEngine;
using System.Collections;

public class SpringBT : Button
{
    public GameObject spring;    

    protected override void Start()
    {
        base.Start();
        spring.GetComponent<BoxCollider2D>().enabled = false;
        sec = 6f;
    }
    
    protected override IEnumerator up()
    {
        while (transform.localPosition.y <= startPosition.y)
        {
            GetComponent<SpriteRenderer>().color = Color.white;
            spring.GetComponent<SpriteRenderer>().color = Color.white;
            spring.GetComponent<BoxCollider2D>().enabled = false;
            transform.localPosition = transform.localPosition + new Vector3(0, +(float)0.01, 0.0f);
            yield return null;
        }
    }
    protected override IEnumerator down()
    {
        while (transform.localPosition.y >= downY)
        {
            GetComponent<SpriteRenderer>().color = Color.blue;
            spring.GetComponent<SpriteRenderer>().color = Color.blue;
            spring.GetComponent<BoxCollider2D>().enabled = true;
            transform.localPosition = transform.localPosition + new Vector3(0, -(float)0.01, 0.0f);
            yield return null;
        }
    }
}
