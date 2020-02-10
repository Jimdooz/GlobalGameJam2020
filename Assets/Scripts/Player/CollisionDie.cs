using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionDie : MonoBehaviour
{
    public SpriteRenderer playerSprite;
    public Animator animator;
    public PlayerController player;

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
            EnemyAI agent = collision.gameObject.GetComponent<EnemyAI>();

            if (agent != null && agent.state == EnemyAI.states.Angry)
            {
                Debug.Log("JE MEURS");
                MusicManager.Effect("Loose", 0.6f);
                StartCoroutine("Die",transform.parent.parent);
                player.stop();
                animator.SetBool("DIE", true);
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
