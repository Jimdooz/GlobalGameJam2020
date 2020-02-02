using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOnStart : MonoBehaviour
{
    public string musicToPlay;

    // Start is called before the first frame update
    void Start()
    {
        MusicManager.Play(musicToPlay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
