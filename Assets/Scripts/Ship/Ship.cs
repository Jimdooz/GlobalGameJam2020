using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Représente le bateau à réparer
public class Ship : MonoBehaviour
{
    #region publicVariables
    public List<Piece> pieces = new List<Piece>();
    public float speed;
    public Transform initialPos;
    public int nextLevel;

    public SceneHandler fader;

    #endregion

    #region privateVariables
    bool isRepaired = false; //Si le bateau est réparé
    Animator animator;
    #endregion

    void Start() {
        animator = GetComponentInChildren<Animator>();

        if (initialPos != null)
        {
            transform.position = initialPos.position;
        }

        for(int i = 0; i < pieces.Count; i++) {
            pieces[i].SetShip(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        MusicManager.Play("Intro", 5);
    }

    bool AllPiecesComplete() {
        for(int i = 0; i < pieces.Count; i++) {
            if (!pieces[i].IsComplete()) return false;
            Debug.Log("i : " + pieces[i].IsComplete());
        }

        return true;
    }

    public void CheckFinish() {
        if (AllPiecesComplete())
        {
            animator.SetTrigger("repair");
            fader.TransitionToNextScene(nextLevel);
        }
    }

    //public void MoveToShore()
    //{
    //    if (moveToShore)
    //    {
    //        Debug.Log("hahaha");
    //        transform.position = Vector2.MoveTowards(transform.position, shorePoint.position, speed * Time.deltaTime);

    //        if (Vector2.Distance(transform.position, shorePoint.position) < 0.00001)
    //        {
    //            moveToShore = false;
    //            animator.SetTrigger("break");
    //        }
    //    }
    //}

    //IEnumerator MoveTo(Vector2 destination)
    //{
    //    Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);

    //    if (Vector2.Distance(transform.position, destination) < 0.0001)
    //    {
    //        moveToShore = false;
    //        animator.SetTrigger("break");
    //    }
    //}
}
