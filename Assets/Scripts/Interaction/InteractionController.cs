using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static GameObject generateInteraction(InteractionButton interaction) {
        GameObject worldUI = GameObject.Find("WORLD_UI");
        GameObject instanciate = Instantiate(interaction.gameObject);
        instanciate.transform.SetParent(worldUI.transform);
        return instanciate;
    }
}
