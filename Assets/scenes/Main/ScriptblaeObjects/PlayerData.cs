using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Player Data"  )]
public class PlayerData : ScriptableObject
{
    [Header("車の設定")]

    public float accelPower;         // 前進速度
    public float steerSpeed;         // Y軸回転速度
    public float brakePower;         // ブレーキ速度

    public float brakeForwardValue;
    public float brakeSlideValue;

    public float inCourseMaxSpeed;   // コース内最大速度
    public float outCourseMaxSpeed;  // コース外最大速度
}