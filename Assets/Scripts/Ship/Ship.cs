﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Représente le bateau à réparer
public class Ship : MonoBehaviour
{
    #region publicVariables
    public List<Piece> pieces = new List<Piece>();
    #endregion

    #region privateVariables
    bool isRepaired = false; //Si le bateau est réparé
    Animator animator;
    #endregion

    void Start() {
        animator = GetComponent<Animator>();

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
        }
    }
}
