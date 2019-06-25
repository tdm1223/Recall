using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Door : MonoBehaviour {
    public enum Arrow
    {     //방향
        Up,
        Down,
        Left,
        Right
    }

    public enum KindOfDoorWhichArrowNeed     //장애물 종류 치워야하는 방향 기준이름
    {
        UpNeed,
        DownNeed,
        LeftNeed,
        RightNeed
    }

    [Tooltip("필요로하는 방향")]
    public KindOfDoorWhichArrowNeed kindOfDoorWhichArrowNeed;

    public float scope = 4f;
    AudioSource OpenSound;

    public List<Button> buttonList;
    bool close = true;

    void Awake()
    {
        OpenSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        switch (kindOfDoorWhichArrowNeed)
        {
            case KindOfDoorWhichArrowNeed.UpNeed:
                AllButtonCheck(Arrow.Up,Arrow.Down);
                break;
            case KindOfDoorWhichArrowNeed.DownNeed:
                AllButtonCheck(Arrow.Down, Arrow.Up);
                break;
            case KindOfDoorWhichArrowNeed.LeftNeed:
                AllButtonCheck(Arrow.Left,Arrow.Right);
                break;
            case KindOfDoorWhichArrowNeed.RightNeed:
                AllButtonCheck(Arrow.Right,Arrow.Left);
                break;
        }
    }

    public void AllButtonCheck(Arrow allPress, Arrow reverse)   //버튼 모두 클릭되면 움직임, 버튼 하나라도 풀리면 반대로 다시
    {
        int i = 0;
        if (close)
        {
            while (close && (i < buttonList.Count))
            {
                bool press = buttonList[i].ReturnPress();
                if ((i == buttonList.Count - 1) && press)
                {
                    Move(allPress);
                    close = false;
                    break;
                }
                else if (press)
                {
                    i++;
                }
                else if (!press)
                {
                    break;
                }

            }
        }
        else if (!close) //오픈일때
        {
            while ((i < buttonList.Count))
            {

                bool press = buttonList[i].ReturnPress();
                if (!press)
                {
                    Move(reverse);
                    close = true;
                    break;
                }
                else if ((i == buttonList.Count - 1) && press)
                {
                    break;
                }
                else if (press)
                {
                    i++;
                }
            }
        }
    }

    public void Move(Arrow arrow)
    {
        switch (arrow)
        {
            case Arrow.Up:
                OpenSound.Play();
                StartCoroutine(MoveAction(arrow));
                break;
            case Arrow.Down:
                OpenSound.Play();
                StartCoroutine(MoveAction(arrow));
                break;
            case Arrow.Left:
                StartCoroutine(MoveAction(arrow));
                break;
            case Arrow.Right:
                StartCoroutine(MoveAction(arrow));
                break;
        }
    }
    
    IEnumerator MoveAction(Arrow arrow)
    {
        float sum = 0;
        Vector2 arrowVector= new Vector2();
        switch (arrow)
        {
            case Arrow.Up:
                arrowVector = Vector2.up;
                break;
            case Arrow.Down:
                arrowVector = Vector2.down;
                break;
            case Arrow.Left:
                arrowVector = Vector2.left;
                break;
            case Arrow.Right:
                arrowVector = Vector2.right;
                break;
        }
        arrowVector *= 0.1f;
        while (sum < scope)
        {
            sum = sum + (float)0.1;
            transform.localPosition = (Vector2)transform.localPosition + arrowVector;
            yield return null;
        }
    }
}
