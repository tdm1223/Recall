using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour
{
    protected Vector2 startPosition;
    protected float downY;
    protected float upY;
    protected AudioSource DownSound;
    protected bool pressCheck = false;
    public float sec;

    IEnumerator UpDownCoroutine;

    protected virtual void Start()
    {
        sec = 5f;
        startPosition = transform.localPosition;
        downY = startPosition.y - (float)0.5;
        DownSound = GetComponent<AudioSource>();
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)     //밟았는지 체크
    {
        if (!pressCheck)
        {
            pressCheck = true;
            Down();
            Count();
        }
    }

    public void Down()      //버튼 내려가는 함수
    {
        DownSound.Play();

        if (UpDownCoroutine == null)
        {
            UpDownCoroutine = down();
            StartCoroutine(UpDownCoroutine);
        }
        else
        {
            StopCoroutine(UpDownCoroutine);
            UpDownCoroutine = down();
            StartCoroutine(UpDownCoroutine);
        }
    }

    public void Up()        //버튼 올라가는 함수
    {
        if (UpDownCoroutine == null)
        {
            UpDownCoroutine = up();
            StartCoroutine(UpDownCoroutine);
        }
        else
        {
            StopCoroutine(UpDownCoroutine);
            UpDownCoroutine = up();
            StartCoroutine(UpDownCoroutine);
        }
    }    
  
    public bool ReturnPress()
    {
        return pressCheck;
    }

    protected void Count()        //시간 체크해서 버튼 올라오는 함수
    {
        StartCoroutine(count(sec));
    }

    protected virtual IEnumerator up()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
        while (transform.localPosition.y <= startPosition.y)
        {
            transform.localPosition = transform.localPosition + new Vector3(0, +(float)0.01, 0.0f);
            yield return null;
        }
    }

    protected virtual IEnumerator down()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        while (transform.localPosition.y >= downY)
        {
            transform.localPosition = transform.localPosition + new Vector3(0, -(float)0.01, 0.0f);
            yield return null;
        }
    }

    protected IEnumerator count(float sec)
    {
        StopCoroutine("count");
        int i = 0;
        while (i <= sec)
        {
            yield return new WaitForSeconds(1.0f);
            i++;
        }
        Up();
        pressCheck = false;
    }
}
