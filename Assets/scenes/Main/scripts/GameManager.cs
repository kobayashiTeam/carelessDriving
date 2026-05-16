using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public enum GameState
{
    Start,
    Playing,
    GameOver
}
public class GameManager : MonoBehaviour
{
    public static GameManager instance { private set; get; }
    public GameState currentState { private set; get; }
    public startCountSc startCountSc;
    public PlayerSc playerSc;
    public EnemySc enemySc;
    //スタート前について
    public Common.Fade fade;
    //第２コースについて
    public LookAreaManagerSc lookAreaManagerSc;
    //第3コースについて
    public FamilyManagerSc familyManagerSc;
    public EyeCoverSc eyeCoverSc;
    public LightingManagerSc lightingManagerSc;
    //ゴールについて
    public GameObject goalOb;
    public GameObject enemyGoalOb;
    public GameObject toTitleOb;
    private bool canToTitle = false;
    public GameObject cup;
    //死について
    public GameObject deadOb;
    public GameObject deadCamera;
    //言語について
    public GameEnums.LanguageIndex currentLanguage = GameEnums.LanguageIndex.English;
    public MessageManagerSc messageManagerSc;
    //InputSystem
    public InputActionAsset inputActions;
    private InputAction enterAction;


    private void Awake()
    {
        instance = this;
        //テスト用
        enterAction = inputActions.FindAction("Enter");
    }
    void Start()
    {
        currentState = GameState.Start;
        
        //第3コースについて
        playerSc.OnPlayerEnter3rdCourse += handlePrepare3rdCourse;
        //Playerのゴールについて
        playerSc.OnPlayerGoal += handlePlayerGoal;
        //Playerの死について
        playerSc.OnPlayerDead += handlePlayerDead;
        //enemyのゴールについて
        enemySc.OnEnemyGoal += handleEnemyGoal;
        //EyeCoverについて
        eyeCoverSc.OnEyeCoverClosed += handleStartNightLighting;
        eyeCoverSc.OnEyeCoverOpen += handleDisplayResult;
        //Lightについて
        lightingManagerSc.changeToDayLighting();
        //Startの処理開始
        StartCoroutine(playStartSequence());
        Debug.Log("Start End");
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case GameState.Start:
                break;
            case GameState.Playing:
                break;
            case GameState.GameOver:
                checkToTitle();
                break;
        }
    }

    private IEnumerator playStartSequence()
    {
        Debug.Log("startSequence");
        //フェードインが終わるまで待機
        yield return fade.FadeInCoroutine(1f);
        //1f待機
        yield return new WaitForSeconds(1f);
        //カウントダウン開始
        yield return startCountSc.CountdownCoroutine();
        //1f待機
        yield return new WaitForSeconds(0.5f);
        currentState = GameState.Playing;

    }


    private void handlePlayerGoal()
    {
        
        familyManagerSc.FinaObject.SetActive(false);
        messageManagerSc.clearText();
        eyeCoverSc.gameObject.SetActive(true);
        lightingManagerSc.changeToDayLighting();
        currentState = GameState.GameOver;
        cup.SetActive(true);
        StartCoroutine(eyeCoverSc.OpenAndWait());
    }

    private void handleStartNightLighting()
    {
        StartCoroutine(handleStartNightLightingCoroutine());
        
    }

    public IEnumerator handleStartNightLightingCoroutine()
    {
        StartCoroutine(lookAreaManagerSc.deactivateLookAreasCoroutine());
        lightingManagerSc.changeToNightLighting();
        yield return new WaitForSeconds(0.8f);
        playerSc.carSoundSc.playSleep();
        eyeCoverSc.gameObject.SetActive(false);
        familyManagerSc.FinaObject.SetActive(true);

    }

    private void handlePrepare3rdCourse()
    {
        StartCoroutine(eyeCoverSc.CloseAndWait());
    }

    private void handleDisplayResult()
    {
        Common.AudioManager.Instance.PlaySFX(Common.AudioManager.SFX.PlayerGoal);
        goalOb.SetActive(true);
        StartCoroutine(WaitAndActiveEnterToTitle());
    }

    void checkToTitle()
    {
        if (!canToTitle) return;
        if (enterAction.WasPressedThisFrame())
        {
            SceneManager.LoadScene("TitleScene");
        }
    }

    public IEnumerator WaitAndActiveEnterToTitle()
    {
        yield return new WaitForSeconds(2f);
        toTitleOb.SetActive(true);
        canToTitle = true;
    }

    
    public void handleEnemyGoal()
    {
        currentState = GameState.GameOver;
        playerSc.carSoundSc.stopAllSounds();
        Common.AudioManager.Instance.PlaySFX(Common.AudioManager.SFX.EnemyGoal);
        enemyGoalOb.SetActive(true);
        StartCoroutine(WaitAndActiveEnterToTitle());
    }

    public void handlePlayerDead()
    {

        StartCoroutine(playerDeadSequence());
    }

    private IEnumerator playerDeadSequence()
    {
        currentState = GameState.GameOver;
        yield return new WaitForSeconds(0.5f);
        playerSc.deadEffectOb.SetActive(true);
        playerSc.carSoundSc.playExplosion();
        yield return new WaitForSeconds(0.5f);
        playerSc.afterBurningOb.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        //コース２で死んだらオブジェクトをきれいにする
        playerSc.focusSc.gameObject.SetActive(false);
        playerSc.faceCamManagerSc.offImage();
        deadCamera.SetActive(true);
        deadOb.SetActive(true);
        playerSc.carSoundSc.stopAllSounds();
        messageManagerSc.clearText();
        Common.AudioManager.Instance.PlaySFX(Common.AudioManager.SFX.EnemyGoal);
        StartCoroutine(WaitAndActiveEnterToTitle());
    }

}
