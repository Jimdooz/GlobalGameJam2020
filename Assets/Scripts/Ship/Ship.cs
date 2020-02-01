using System.Collections;
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
    #endregion

    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    bool AllPiecesComplete() {
        for(int i = 0; i < pieces.Count; i++) {
            if (!pieces[i].IsComplete()) return false;
        }

        return true;
    }
}
