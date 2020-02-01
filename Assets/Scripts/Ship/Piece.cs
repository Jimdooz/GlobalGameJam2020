using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    #region publicVariables
    //Représente les items dont la pièce a besoin pour être réparé
    public List<Item> itemsNeeded = new List<Item>();
    #endregion

    #region privateVariables
    //Représente les items qui ont été donné pour être réparé par le joueur, initialement vide
    List<Item> items = new List<Item>();
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Retourne si la pièce a été réparé, ou est complète ( même chose wallah )
    public bool IsComplete() {
        return items.Count == itemsNeeded.Count;
    }
}
