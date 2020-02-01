using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 
 * Created by Romain Saclier @2020
 *
 */

public class MusicManager : MonoBehaviour
{
    [System.Serializable]
    public struct SoundElement {
        public string name;
        public AudioClip clip;
        public bool loop;
    }

    [System.Serializable]
    public struct EffectElement
    {
        public string name;
        public List<AudioClip> variants;
    }

    #region publicVariables
    public bool startOnPlay = true;
    public float maxDistance = 50f;
    public float minDistance = 20f;
    #endregion

    public List<SoundElement> allMusics = new List<SoundElement>();
    List<AudioSource> allSources = new List<AudioSource>();
    List<float> allSourcesBaseVolume = new List<float>();
    float currTimeTransition = 0f; //in ms
    float timeToTransition = 0f; //in ms
    int lastIndex = 0;

    public List<EffectElement> allSoundsEffects = new List<EffectElement>();
    List<AudioSource> allSourcesSoundsEffects = new List<AudioSource>();

    string currentName = ""; //Current Sound Play
    // Start is called before the first frame update
    void Start()
    {
        //Run main sound
        if(startOnPlay && allMusics.Count > 0) {
            PlaySound(allMusics[0].name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (allSources.Count > 0)
        {
            if (currTimeTransition > 0) currTimeTransition -= Time.deltaTime;
            else currTimeTransition = 0f;
            // Others
            for (int i = 0; i < allSources.Count - 1; i++) {
                allSources[i].volume = Mathf.Lerp(0f, allSourcesBaseVolume[i], currTimeTransition/timeToTransition);
                if(allSources[i].volume == 0f)
                {
                    //Remove AudioSource
                    Destroy(allSources[i]);
                    allSources.RemoveAt(i);
                    allSourcesBaseVolume.RemoveAt(i);
                    i--;
                }
            }
            //Last
            lastIndex = allSources.Count - 1;
            if(allSources[lastIndex]) allSources[lastIndex].volume = Mathf.Lerp(1f, allSourcesBaseVolume[lastIndex], currTimeTransition/timeToTransition);
        }
        for(int i = 0; i < allSourcesSoundsEffects.Count; i++)
        {
            if (!allSourcesSoundsEffects[i].isPlaying)
            {
                Destroy(allSourcesSoundsEffects[i]);
                allSourcesSoundsEffects.RemoveAt(i);
                i--;
            }
        }
    }

    public void PlaySound(string musicName, float speedTime = 10f)
    {
        SoundElement found = foundSound(musicName);
        if(found.name != null && found.name != currentName)
        {
            //We have found a sound
            currentName = found.name;
            timeToTransition = speedTime;
            currTimeTransition = timeToTransition;
            AudioSource newAudio = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
            Sound2D.Adapt2DSound(newAudio);
            newAudio.maxDistance = maxDistance;
            newAudio.minDistance = minDistance;
            newAudio.clip = found.clip;
            newAudio.volume = 0f;
            newAudio.Play();
            newAudio.loop = found.loop;
            allSources.Add(newAudio);

            allSourcesBaseVolume.Clear();
            for(int i = 0; i < allSources.Count; i++)
            {
                allSourcesBaseVolume.Add(allSources[i].volume);
            }
        }
    }

    public void PlayEffect(string effectName)
    {
        EffectElement found = foundEffect(effectName);
        if (found.name != null)
        {
            AudioSource newAudio = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
            Sound2D.Adapt2DSound(newAudio);
            newAudio.maxDistance = maxDistance;
            newAudio.minDistance = minDistance;
            newAudio.clip = found.variants[Random.Range(0,found.variants.Count)];
            newAudio.volume = 1f;
            newAudio.Play();
            newAudio.loop = false;
            allSourcesSoundsEffects.Add(newAudio);

        }
    }

    SoundElement foundSound(string musicName)
    {
        for(int i = 0; i < allMusics.Count; i++)
        {
            if (allMusics[i].name == musicName) return allMusics[i];
        }
        return default(SoundElement);
    }

    EffectElement foundEffect(string effectName)
    {
        for (int i = 0; i < allSoundsEffects.Count; i++)
        {
            if (allSoundsEffects[i].name == effectName) return allSoundsEffects[i];
        }
        return default(EffectElement);
    }

    public static MusicManager GetMusicManager()
    {
        GameObject MAIN_MUSIC = GameObject.Find("MAIN_MUSIC");
        if (!MAIN_MUSIC)
        {
            MAIN_MUSIC = Instantiate(Resources.Load("Prefabs/MAIN_MUSIC", typeof(GameObject))) as GameObject;
            MAIN_MUSIC.name = "MAIN_MUSIC";
            DontDestroyOnLoad(MAIN_MUSIC);
        }
        MusicManager musicManager = MAIN_MUSIC.GetComponent<MusicManager>();
        return musicManager;
    }

    public static void Play(string musicName, float speedTime = 10f)
    {
        MusicManager musicManager = MusicManager.GetMusicManager();
        musicManager.PlaySound(musicName, speedTime);
    }

    public static void Effect(string effectName)
    {
        MusicManager musicManager = MusicManager.GetMusicManager();
        musicManager.PlayEffect(effectName);
    }
}
