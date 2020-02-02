using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionDie : MonoBehaviour
{
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
                Die();

            }
        }
    }

    public void Die()
    {
        transform.parent.gameObject.SetActive(false);
        MusicManager.Effect("Loose", 0.6f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
