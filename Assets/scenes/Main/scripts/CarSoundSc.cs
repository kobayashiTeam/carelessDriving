using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using static Unity.VisualScripting.Member;

public class CarSoundSc : MonoBehaviour
{
    enum CarSounds
    {
        Engine,
        Brake,
        Crash,
        Sleep,
        Explosion,
        TouchingEnemy
    }

    public AudioSource[] carSounds;
    public float stopEngineVolumeDuration;
    public float stopBrakeVolumeDuration;
    void Start()
    {
        carSounds = GetComponentsInChildren<AudioSource>(true)
            .Where(a => a.gameObject != this.gameObject)
            .ToArray();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void playEngine()
    {
        //‰‚ß‚Ä‚ÌÄ¶
        if (!carSounds[(int)CarSounds.Engine].isPlaying)
        {
            carSounds[(int)CarSounds.Engine].volume = 1;
            carSounds[(int)CarSounds.Engine].Play();
        }

        //ˆÈ~‚Íí‚ÉÄ¶A‰¹—Ê‚ð•Ï‚¦‚é‚¾‚¯
        carSounds[(int)CarSounds.Engine].volume = 1;
        
    }

    public void stopEngine()
    {
        carSounds[(int)CarSounds.Engine].volume = 0;
    }

    public void playBrake()
    {
        if (!carSounds[(int)CarSounds.Brake].isPlaying)
        {
            carSounds[(int)CarSounds.Brake].volume = 1;
            carSounds[(int)CarSounds.Brake].Play();
        }

        //ˆÈ~‚Íí‚ÉÄ¶A‰¹—Ê‚ð•Ï‚¦‚é‚¾‚¯
        carSounds[(int)CarSounds.Brake].volume = 0.6f;
        if (carSounds[(int)CarSounds.Brake].time >=
            carSounds[(int)CarSounds.Brake].clip.length - 0.1f)
        {
            carSounds[(int)CarSounds.Brake].time = 0.1f;
        }
    }

    public void playCrash()
    {
        carSounds[(int)CarSounds.Crash].Play();
    }

    public void stopBrake()
    {
        carSounds[(int)CarSounds.Brake].volume = 0;
    }
    public void stopAllSounds()
    {
        stopEngine();
        stopBrake();
        stopSleep();
    }

    public void playSleep()
    {
        carSounds[(int)CarSounds.Sleep].volume = 1;
        carSounds[(int)CarSounds.Sleep].Play();
    }

    public void stopSleep()
    {
        carSounds[(int)CarSounds.Sleep].volume=0;
    }   

    public void playExplosion()
    {
        carSounds[(int)CarSounds.Explosion].Play();
    } 

    public void playTouchingEnemy()
    {
        carSounds[(int)CarSounds.TouchingEnemy].volume = 1;
        carSounds[(int)CarSounds.TouchingEnemy].Play();
    }

    public void stopTouchingEnemy()
    {
        carSounds[(int)CarSounds.TouchingEnemy].volume = 0;
    }
}
