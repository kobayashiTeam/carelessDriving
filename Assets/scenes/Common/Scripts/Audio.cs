using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        //========================
        // enum
        //========================

        public enum BGM
        {
            Title,
            Race,
            Goal
        }

        public enum SFX
        {
            CountDown,
            Go,
            PlayerGoal,
            EnemyGoal
        }

        //========================
        // Inspector登録用
        //========================

        [Serializable]
        public class BGMData
        {
            public BGM id;
            public AudioClip clip;
        }

        [Serializable]
        public class SFXData
        {
            public SFX id;
            public AudioClip clip;
        }

        [Header("BGM List")]
        public List<BGMData> bgmList = new();

        [Header("SFX List")]
        public List<SFXData> sfxList = new();

        //========================
        // AudioSource
        //========================

        [Header("SFX Pool Size")]
        public int sfxPoolSize = 10;

        private AudioSource bgmSource;
        private List<AudioSource> sfxSources = new();

        //========================
        // Dictionary
        //========================

        private Dictionary<BGM, AudioClip> bgmDict;
        private Dictionary<SFX, AudioClip> sfxDict;

        //========================
        // Initialize
        //========================

        private void Awake()
        {
            // Singleton
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            // Dictionary化
            InitializeDictionary();

            // AudioSource生成
            CreateAudioSources();
        }

        //========================
        // Dictionary生成
        //========================

        private void InitializeDictionary()
        {
            bgmDict = new Dictionary<BGM, AudioClip>();

            foreach (var data in bgmList)
            {
                bgmDict[data.id] = data.clip;
            }

            sfxDict = new Dictionary<SFX, AudioClip>();

            foreach (var data in sfxList)
            {
                sfxDict[data.id] = data.clip;
            }
        }

        //========================
        // AudioSource生成
        //========================

        private void CreateAudioSources()
        {
            //----------------------------------
            // BGM
            //----------------------------------

            GameObject bgmObj = new GameObject("BGM_Source");

            bgmObj.transform.SetParent(transform);

            bgmSource = bgmObj.AddComponent<AudioSource>();

            bgmSource.loop = true;

            //----------------------------------
            // SFX Pool
            //----------------------------------

            GameObject sfxRoot = new GameObject("SFX_Pool");

            sfxRoot.transform.SetParent(transform);

            for (int i = 0; i < sfxPoolSize; i++)
            {
                GameObject sfxObj = new GameObject($"SFX_{i}");

                sfxObj.transform.SetParent(sfxRoot.transform);

                AudioSource source = sfxObj.AddComponent<AudioSource>();

                sfxSources.Add(source);
            }
        }

        //========================
        // BGM
        //========================

        public void PlayBGM(BGM id)
        {
            if (!bgmDict.ContainsKey(id))
            {
                Debug.LogWarning($"BGM not found : {id}");
                return;
            }

            AudioClip clip = bgmDict[id];

            // 同じ曲なら再スタートしない
            if (bgmSource.clip == clip && bgmSource.isPlaying)
            {
                return;
            }

            bgmSource.clip = clip;

            bgmSource.Play();
        }

        public void StopBGM()
        {
            bgmSource.Stop();
        }

        //========================
        // SFX
        //========================

        public void PlaySFX(SFX id)
        {
            if (!sfxDict.ContainsKey(id))
            {
                Debug.LogWarning($"SFX not found : {id}");
                return;
            }

            AudioSource source = GetAvailableSFXSource();

            if (source == null)
            {
                Debug.LogWarning("No available SFX AudioSource.");
                return;
            }

            source.PlayOneShot(sfxDict[id]);
        }

        //========================
        // Pool取得
        //========================

        private AudioSource GetAvailableSFXSource()
        {
            foreach (var source in sfxSources)
            {
                if (!source.isPlaying)
                {
                    return source;
                }
            }

            return null;
        }
    }
}