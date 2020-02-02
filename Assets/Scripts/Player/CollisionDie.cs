﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionDie : MonoBehaviour
{
    public SpriteRenderer playerSprite;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("AgentBody"))
        {
            Agent agent = collision.transform.parent.GetComponent<Agent>();

            if (agent != null && agent.angry)
            {
                Debug.Log("JE MEURS");
                MusicManager.Effect("Loose", 0.6f);
                playerSprite.enabled = false;
                StartCoroutine("Die",transform.parent.parent);

            }
        }
    }

    public IEnumerator Die()
    {
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        transform.parent.gameObject.SetActive(false);
    }
}
