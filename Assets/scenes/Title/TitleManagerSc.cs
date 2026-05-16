using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Title
{

    public class TitleManagerSc : MonoBehaviour
    {
        private AsyncOperation preloadOperation;//非同期でシーンを読み込むための変数
        public Common.Fade fade;
        private bool canMove = false;
        
        void Awake()
        {
            canMove = false;
        }
        void Start()
        {
            //予めMainシーンを非同期で９割読み込んでストップしておく
            preLoad();
           //フェードインする
           fade.FadeIn(1f);
            //フェードスクリプトのイベントに、終了後の処理を登録
            fade.OnFadeInComplete += () => canMove = true;
            fade.OnFadeOutComplete += () => preloadOperation.allowSceneActivation = true;
        }

        private void preLoad()
        {
            preloadOperation = SceneManager.LoadSceneAsync("MainScene", LoadSceneMode.Single);
            preloadOperation.allowSceneActivation = false;

        }

        // Update is called once per frame
        void Update()
        {
            checkCondition();
            
        }

        private void checkCondition()
        {
            //フェードインが終わるまでは操作できないようにする
            if (!canMove) return;

            if (Keyboard.current.enterKey.wasPressedThisFrame)
            {
                //フェードアウトが始まったら念のため操作できないようにする
                canMove = false;
                fade.FadeOut(1f);
            }
        }

       
    }
}