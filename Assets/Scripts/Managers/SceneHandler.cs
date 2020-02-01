using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    private Animator fader;

    [SerializeField]
    private int sceneToLoad;

    // Start is called before the first frame update
    void Start()
    {
        fader = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void TransitionToNextScene(int nextScene)
    {
        sceneToLoad = nextScene;
        fader.SetTrigger("toDark");
    }

}
