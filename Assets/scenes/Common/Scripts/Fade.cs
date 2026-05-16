using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Common
{
    public class Fade : MonoBehaviour
    {
        public Image FadeImage;
        public event Action OnFadeInComplete;
        public event Action OnFadeOutComplete;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void FadeIn(float duration)
        {
            StartCoroutine(FadeInCoroutine(duration));
        }

        public IEnumerator FadeInCoroutine(float duration)
        {
            float elapsedTime = 0f;
            Color color = FadeImage.color;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                color.a = 1-Mathf.Clamp01(elapsedTime / duration);
                FadeImage.color = color;
                yield return null;
            }
            color.a = 0f;
            FadeImage.color = color;
            OnFadeInComplete?.Invoke();
        }

        public void FadeOut(float duration)
        {
            StartCoroutine(FadeOutCoroutine(duration));
        }

        public IEnumerator FadeOutCoroutine(float duration)
        {
            float elapsedTime = 0f;
            Color color = FadeImage.color;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                color.a = Mathf.Clamp01(elapsedTime / duration);
                FadeImage.color = color;
                yield return null;
            }
            color.a = 1f;
            FadeImage.color = color;
            OnFadeOutComplete?.Invoke();
        }
    }
}