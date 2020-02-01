using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [System.Serializable]
    public struct SoundElement {
        public string name;
        public AudioClip clip;
        public bool loop;
    }

    #region publicVariables
    public bool startOnPlay = true;
    public float maxDistance = 50f;
    public float minDistance = 20f;

    public string currentName = "";
    #endregion

    public List<SoundElement> allMusics = new List<SoundElement>();
    List<AudioSource> allSources = new List<AudioSource>();
    List<float> allSourcesBaseVolume = new List<float>();
    public float currTimeTransition = 0f; //in ms
    public float timeToTransition = 0f; //in ms
    int lastIndex = 0;
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

    SoundElement foundSound(string musicName)
    {
        for(int i = 0; i < allMusics.Count; i++)
        {
            if (allMusics[i].name == musicName) return allMusics[i];
        }
        return default(SoundElement);
    }
}
