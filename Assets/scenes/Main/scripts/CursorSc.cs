using UnityEngine;

public class CursorSc : MonoBehaviour
{
    public GameObject rightCursor;
    public GameObject leftCursor;
    public PlayerSc playerSc;
    public GameObject FinaObject;

    public enum CursorState
    {
        None,
        Left,
        Right
    }
    void Start()
    {
        rightCursor.SetActive(false);
        leftCursor.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.currentState != GameState.Playing) return;
        if (playerSc.currentCourseIndex != GameEnums.CourseIndex.Course3) return;
        switch(checkRelation()){
            case CursorState.Left:
                rightCursor.SetActive(false);
                leftCursor.SetActive(true);
                break;
            case CursorState.Right:
                rightCursor.SetActive(true);
                leftCursor.SetActive(false);
                break;
            case CursorState.None:
                rightCursor.SetActive(false);
                leftCursor.SetActive(false);
                break;
        }
    }

    private void FixedUpdate()
    {

    }

    CursorState checkRelation()
    {
        // 車の前方ベクトルを求め、y成分を0にして正規化
        Vector3 flatCarForward = playerSc.transform.forward;
        flatCarForward.y = 0f;
        flatCarForward.Normalize();

        // lookTargetまでの方向ベクトルを求め、y成分を0にして正規化
        Vector3 dir = FinaObject.transform.position - playerSc.transform.position;
        Vector3 flatDir = dir;
        flatDir.y = 0f;
        flatDir.Normalize();

        // 外積を計算して、y成分の符号で左右を判定
        Vector3 cross = Vector3.Cross(flatCarForward, flatDir);
        float side = cross.y;    // y の符号を見る
        //角度差の絶対値を出す
        float angle = Vector3.Angle(flatCarForward, flatDir);
        //50度以内なら何もしない
        if (angle<=50)return CursorState.None;

        if (side < 0f)
        {
            // left
            return CursorState.Left;
        }
        else if (side > 0f)
        {
            // right
            Debug.Log(angle);
            return CursorState.Right;
        }

        

        return CursorState.None;
    }

}
