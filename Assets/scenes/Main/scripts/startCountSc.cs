using UnityEngine;
using System.Collections;
using TMPro;
using System;

public class startCountSc : MonoBehaviour
{
    private TextMeshProUGUI countDownText;
    public event Action OnCountdownEnd;
    public event Action OnEachCounting;

    private void Awake()
    {
        countDownText = GetComponent<TextMeshProUGUI>();
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playCountDown()
    {
        StartCoroutine(CountdownCoroutine());
    }

    public IEnumerator CountdownCoroutine()
    {
        for (int i = 3; i > 0; i--)
        {
            
            countDownText.text = i.ToString();
            Common.AudioManager.Instance.PlaySFX(Common.AudioManager.SFX.CountDown);
            yield return new WaitForSeconds(1f);
        }

        countDownText.text = "GO!";
        Common.AudioManager.Instance.PlaySFX(Common.AudioManager.SFX.Go);
        yield return new WaitForSeconds(0.5f);

        gameObject.SetActive(false); // é©ìÆÇ≈è¡Ç¶ÇÈ
    }
}
