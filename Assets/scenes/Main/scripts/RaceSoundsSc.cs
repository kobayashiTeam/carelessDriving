using System.Linq;
using UnityEngine;

public class RaceSoundsSc : MonoBehaviour
{
    public enum RaceSoundsIndex
    {
        Start,
        Goal,
        EnemyGoal
    }
    public AudioClip coundDownClip;
    public AudioClip goClip;
    public AudioClip goalClip;
    public AudioClip enemyGoalClip;
    public AudioSource[] raceSounds;
    void Start()
    {
        raceSounds = GetComponentsInChildren<AudioSource>(true)
            .Where(a => a.gameObject != this.gameObject)
            .ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playCoundDown()
    {
        raceSounds[(int)RaceSoundsIndex.Start].clip = coundDownClip;
        raceSounds[(int)RaceSoundsIndex.Start].Play();
    }

    public void playGo()
    {
        raceSounds[(int)RaceSoundsIndex.Start].clip = goClip;
        raceSounds[(int)RaceSoundsIndex.Start].Play();
    }

    public void playGoal()
    {
        raceSounds[(int)RaceSoundsIndex.Goal].clip = goalClip;
        raceSounds[(int)RaceSoundsIndex.Goal].Play();
    }

    public void playEnemyGoal()
    {
        raceSounds[(int)RaceSoundsIndex.EnemyGoal].clip = enemyGoalClip;
        raceSounds[(int)RaceSoundsIndex.EnemyGoal].Play();
    }
}
