using System;
using UnityEngine;
using System.Collections;

public class EyeCoverSc : MonoBehaviour
{
    //â¸ëPî≈
    public GameObject upCover;
    public GameObject downCover;
    private RectTransform upRect;
    private RectTransform downRect;

    public bool isClose = false;

    public RectTransform eyeCoverRect;
    public float closeSpeed;
    private float closeY = 0f;
    private float openY = 720f;
    //ï¬Ç∂ÇΩèuä‘Ç…ímÇÁÇπÇÈ
    public event Action OnEyeCoverClosed;
    public event Action OnEyeCoverOpen;
    void Start()
    {
        upRect = upCover.GetComponent<RectTransform>();
        downRect = downCover.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (isClose)
        {
            
            upRect.anchoredPosition= Vector2.MoveTowards(
                upRect.anchoredPosition,
                new Vector2(0, 270),//180
                closeSpeed
            );
            downRect.anchoredPosition= Vector2.MoveTowards(
                downRect.anchoredPosition,
                new Vector2(0, -270),//-180
                closeSpeed
            );

        }
        else
        {
            upRect.anchoredPosition= Vector2.MoveTowards(
                upRect.anchoredPosition,
                new Vector2(0, 810),//720
                closeSpeed
            );
            downRect.anchoredPosition = Vector2.MoveTowards(
                downRect.anchoredPosition,
                new Vector2(0, -810),//-720
                closeSpeed
            );
        }
    }

    public void close()
    {
        isClose = true;
    }

    public void open()
    {
        isClose = false;
    }

    public IEnumerator CloseAndWait()
    {
        //isClose = true;
        //yield return new WaitUntil(() =>
        //    Mathf.Approximately(eyeCoverRect.anchoredPosition.y, closeY));//
        //OnEyeCoverClosed?.Invoke();
        isClose = true;
        yield return new WaitUntil(() =>
            Mathf.Approximately(upRect.anchoredPosition.y, 270));//
        OnEyeCoverClosed?.Invoke();

    }

    public IEnumerator OpenAndWait()
    {
        //yield return new WaitForSeconds(1f);
        //isClose = false;
        //yield return new WaitUntil(() =>
        //    Mathf.Approximately(eyeCoverRect.anchoredPosition.y, openY));
        //OnEyeCoverOpen?.Invoke();
        yield return new WaitForSeconds(1f);
        isClose = false;
        yield return new WaitUntil(() =>
            Mathf.Approximately(upRect.anchoredPosition.y, 810));
        OnEyeCoverOpen?.Invoke();
    }
}
