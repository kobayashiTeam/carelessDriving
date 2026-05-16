using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerSc : MonoBehaviour
{
    [Header("車の設定")]
    [SerializeField] private float accelPower;// 前進速度
    [SerializeField] private float steerSpeed;// Y軸回転速度
    [SerializeField] private float brakePower;// 後退やブレーキ速度
    [SerializeField] private float brakeForwardValue;
    [SerializeField] private float brakeSlideValue;
    [SerializeField] private float inCourseMaxSpeed;// 最大速度
    [SerializeField] private float outCourseMaxSpeed;// コース外最大速度

    public PlayerData pData;

    [Header("入力設定")]
    public InputActionAsset inputActions; // InspectorでActionAssetを割り当て
    private InputAction moveAction;
    private InputAction accelAction;
    private InputAction brakeAction;

    private float horizontalInput;
    private float verticalInput;
    private float brakeInput;

    private Rigidbody rb;
    public Slider hpSlider;
    public bool isTouchedEnemy = false;
    public int currentWaypointIndex = 0;
    public WayPointsManagerSc wayPointsManagerSc;
    public GameEnums.CourseIndex currentCourseIndex = 
        GameEnums.CourseIndex.Course1;
    public CarSoundSc carSoundSc;

    public enum PlayerState
    {
        phoneMode,
        normalMode,
        girlLookingMode,
        paranoiaMode
    }
    public PlayerState currentPlayerState = PlayerState.phoneMode;//現在のプレイヤーステート

    //親子関係にあるオブジェクト
    [Header("1Stage")]
    public GameObject carCamera;
    public GameObject phone;
    public PhoneScript phoneScript;
    public GameObject deadEffectOb;
    public GameObject afterBurningOb;

    //２ステージ
    [Header("2Stage")]
    public LookAreaManagerSc lookAreaManagerSc;
    public int currentLookAreaIndex = 0;
    public float lookGirlLimitAngle;
    public FocusSc focusSc;
    public FaceCamManagerSc faceCamManagerSc;

    //第３ステージ
    [Header("3Stage")]
    public int currentFamilyPointIndex = 0;
    public Collider enemyCollider;
    public event Action OnPlayerEnter3rdCourse;
    // ゴールしたときのイベント
    public event Action OnPlayerGoal;
    //hpが0になったときのイベント
    public event Action OnPlayerDead;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        moveAction = inputActions.FindAction("Move");       // Vector2 左右: 矢印左右
        accelAction = inputActions.FindAction("Accelerate"); // Button Xキー
        brakeAction = inputActions.FindAction("Brake");      // Button Zキー

    }

    private void Start()
    {
        hpSlider.value = 100f;
        initializePlayerData();
    }


    void Update()
    {
        ReadInput();
        switch (GameManager.instance.currentState)
        {
            case GameState.Start:
                break;
            case GameState.Playing:
                watchObject();
                updateCarSound();
                handleCrashEnemy();
                break;
            case GameState.GameOver:
                break;
        }
    }


    private void FixedUpdate()
    {
        switch (GameManager.instance.currentState)
        {
            case GameState.Start:
                break;
            case GameState.Playing:
                MoveCar();
                break;
            case GameState.GameOver:
                slowDown();
                break;
        }
    }
    void ReadInput()
    {
        Vector2 move = moveAction.ReadValue<Vector2>();
        horizontalInput = move.x;                          // 左右矢印
        verticalInput = accelAction.IsPressed() ? 1f : 0f; // Xキー
        brakeInput = brakeAction.IsPressed() ? 1f : 0f;    // Zキー
    }

    void MoveCar()
    {
        
        // 前進
       rb.AddForce(transform.forward * (verticalInput*accelPower)
           , ForceMode.Force);
        Debug.Log("vertical:" + transform.forward * (verticalInput * accelPower));
        // ブレーキ・後退
        if (brakeInput > 0)
        {
            Vector3 localVel = transform.InverseTransformDirection(rb.linearVelocity);
            // 前後成分だけ強くブレーキ
            localVel.z *= (1f - brakeForwardValue);
            // 横成分も少しだけ減らす（滑り止め）
            localVel.x *= (1f - brakeSlideValue);
            //最終的な速度
            rb.linearVelocity = transform.TransformDirection(localVel);
        }
        //速度制限
        if (rb.linearVelocity.magnitude > inCourseMaxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * inCourseMaxSpeed;
        }

        // Y軸回転（左右）
        float turn = horizontalInput * steerSpeed;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
    }

    void watchObject()
    {
        switch (currentPlayerState)
        {
            case PlayerState.phoneMode:
                watchPhone();
                break;
            case PlayerState.girlLookingMode:
                watchGirl();
                break;
            case PlayerState.normalMode:
                watchForward();
                break;
            case PlayerState.paranoiaMode:
                break;
        }
        
    }

    void watchPhone()
    {
        //カメラをスマホの方に向ける
        Vector3 dir = (phone.transform.localPosition - carCamera.transform.localPosition).normalized;
        Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
        // slerpで少しずつ回す
        carCamera.transform.localRotation = Quaternion.Slerp(
            carCamera.transform.localRotation,
            targetRot,
            0.05f
        );
        //スマホがカメラを見ている
        dir = (carCamera.transform.localPosition - phone.transform.localPosition).normalized;
        phone.transform.localRotation = Quaternion.LookRotation(dir, Vector3.up);
    }

    void watchGirl()
    {
        //currentAreaIndexに対応するLookTargetを見る
        Transform lookTarget = lookAreaManagerSc.lookAreaPairs[currentLookAreaIndex].
            lookTarget.transform;

        // 車の前方ベクトルを求め、y成分を0にして正規化
        Vector3 flatCarForward = transform.forward;
        flatCarForward.y = 0f;
        flatCarForward.Normalize();

        // lookTargetまでの方向ベクトルを求め、y成分を0にして正規化
        Vector3 dir = lookTarget.position - carCamera.transform.position;
        Vector3 flatDir = dir;
        flatDir.y = 0f;
        flatDir.Normalize();

        float angle = Vector3.Angle(flatCarForward, flatDir);

        if (Mathf.Abs(angle) < 100f)
        {
            //focusをかける。
            focusSc.gameObject.SetActive(true);
            //faceカメラ軌道
            faceCamManagerSc.onImage();
            Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
            // slerpで少しずつ回す
            carCamera.transform.rotation = Quaternion.Slerp(
                carCamera.transform.rotation,
                targetRot,
                0.05f
            );
        }
        else
        {
            //focusを外す
            focusSc.gameObject.SetActive(false);
            //faceカメラ非表示
            faceCamManagerSc.offImage();
            Quaternion targetRot = Quaternion.identity;
            // slerpで少しずつ回す
            carCamera.transform.localRotation = Quaternion.Slerp(
                carCamera.transform.localRotation,
                targetRot,
                0.05f
            );
        }

        //targetがカメラを見ている
        lookAreaManagerSc.updateGirlsRotation(transform);
    }

    void watchForward()
    {
        Quaternion targetRot = Quaternion.identity;
        // slerpで少しずつ回す
        carCamera.transform.localRotation = Quaternion.Slerp(
            carCamera.transform.localRotation,
            targetRot,
            0.05f
        );
    }


    void OnTriggerStay(Collider other)
    {
        int layer = other.gameObject.layer;
        if (layer == LayerMask.NameToLayer("CheckPoint"))
        {
            handleCheckPoint(other);

        }else if(layer== LayerMask.NameToLayer("LookArea"))
        {
            handleLookArea(other);
        }
    }

    void handleCheckPoint(Collider other)
    {
        wayPointSc wayPointScript = other.gameObject.GetComponent<wayPointSc>();
        if (wayPointScript.wallIndex == currentWaypointIndex)
        {
            currentWaypointIndex++;
            Debug.Log("waypointIndex: " + currentWaypointIndex);
            wayPointSpecialAction();
            updatePhone();
            tryAdvanceCourse();
        }
    }

    void tryAdvanceCourse()
    {
        int wayPointLength = wayPointsManagerSc.waypoints[(int)currentCourseIndex].Length;
        if (currentWaypointIndex > wayPointLength - 1)
        {
            currentWaypointIndex = 0;
            currentCourseIndex++;
            Debug.Log("courseIndex: " + currentCourseIndex);
            handleCourseTransition();
        }
    }

    void handleCourseTransition()
    {
        switch (currentCourseIndex)
        {
            case GameEnums.CourseIndex.Course2:
                phoneScript.setOffPhoneScreen();
                currentPlayerState = PlayerState.normalMode;
                break;
            case GameEnums.CourseIndex.Course3:
                focusSc.gameObject.SetActive(false);
                faceCamManagerSc.offImage();
                ignoreEnemy();
                currentPlayerState = PlayerState.normalMode;
                OnPlayerEnter3rdCourse?.Invoke();
                break;
            case GameEnums.CourseIndex.CourseEnd:
                carSoundSc.stopAllSounds();
                OnPlayerGoal?.Invoke();
                break;
        }
    }

    void handleLookArea(Collider other)
    {
        LookAreaSc lookAreaSc = other.gameObject.GetComponent<LookAreaSc>();
        currentLookAreaIndex = lookAreaSc.lookAreaIndex;
        faceCamManagerSc.updateLookAreaIndex(currentLookAreaIndex);

        //Index==0の時のみ、phoneModeからgirlLookingModeに変更
        if (currentLookAreaIndex == 0)
        {
            currentPlayerState = PlayerState.girlLookingMode;
        }
    }

    void slowDown()
    {
        rb.linearVelocity *= 0.99f;
    }

    void updateCarSound()
    {
        //アクセル
        if (verticalInput == 1)
        { 
            carSoundSc.playEngine();
        }
        else
        {
            carSoundSc.stopEngine();
        }

        //ブレーキ
        if (brakeInput > 0&&rb.linearVelocity.magnitude>0)
        {
            carSoundSc.playBrake();
        }
        if(brakeInput == 0|| rb.linearVelocity.magnitude == 0)
        {
            carSoundSc.stopBrake();
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Guard"))
        {
            guardDamage();
            if (hpSlider.value <= 0)
            {
                OnPlayerDead?.Invoke();
            }
            carSoundSc.playCrash();
        }else if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            carSoundSc.playTouchingEnemy();
            isTouchedEnemy = true;
        }
    }

    private void OnCollisionExit(Collision collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            carSoundSc.stopTouchingEnemy();
            isTouchedEnemy = false;
        }
    }

    void ignoreEnemy()
    {
        Physics.IgnoreCollision(GetComponent<Collider>(), enemyCollider);
    }

    public void wayPointSpecialAction()
    {
        //1コース目の2番目のウェイポイントに来たらphoneModeに変更
        if (currentCourseIndex == GameEnums.CourseIndex.Course1 &&
            currentWaypointIndex == 2)
        {
            currentPlayerState = PlayerState.phoneMode;
        }
    }

    public void updatePhone()
    {
        //1コース目に限り、phoneScriptのchangeTextureを呼び出す
        if (currentCourseIndex == GameEnums.CourseIndex.Course1)
        {
            switch (currentWaypointIndex)
            {
                case 3:
                    phoneScript.changeTexture(0);
                    break;
                case 4:
                    phoneScript.changeTexture(1);
                    break;
                case 5:
                    phoneScript.changeTexture(2);
                    break;
                case 6:
                    phoneScript.changeTexture(3);
                    break;
                case 7:
                    phoneScript.changeTexture(4);
                    break;
                case 8:
                    phoneScript.changeTexture(5);
                    break;
                case 9:
                    phoneScript.changeTexture(6);
                    break;
                case 10:
                    phoneScript.changeTexture(7);
                    break;
                case 11:
                    phoneScript.changeTexture(8);
                    break;
                case 12:
                    phoneScript.changeTexture(9);
                    break;
            }
        }
    }

    public void guardDamage()
    {
        hpSlider.value -= 10f;
    }

    public void handleCrashEnemy()
    {
        if (isTouchedEnemy)
        {
            hpSlider.value -= 0.2f;
        }
    }

    private void initializePlayerData()
    {
        //pDataから各種パラメータを初期化
        this.accelPower = pData.accelPower;
        this.steerSpeed = pData.steerSpeed;
        this.brakePower = pData.brakePower;
        this.brakeForwardValue = pData.brakeForwardValue;
        this.brakeSlideValue = pData.brakeSlideValue;
        this.inCourseMaxSpeed = pData.inCourseMaxSpeed;
        this.outCourseMaxSpeed = pData.outCourseMaxSpeed;

        
    }
}
