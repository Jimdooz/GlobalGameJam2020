using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Piece : MonoBehaviour
{
    #region publicVariables
    [Range(1,5)]
    public int powerItemRepair = 2; //Nombre de clicks nécéssaire pour réparer un item
    //Représente les items dont la pièce a besoin pour être réparé
    public List<Item> itemsNeeded = new List<Item>();
    public InfoPiece infoPiece;

    public Sprite visualBroken;
    public Sprite visualRepaired;
    #endregion

    #region privateVariables
    int nbRepair; // Nombre de click total nécéssaire à la réparation
    int repaired = 0; // Pourcentage réparé
    SpriteRenderer render;
    Animator pieceAnimator;

    Ship myShip;
    #endregion

    // Start is called before the first frame update
    void Start() {
        pieceAnimator = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();
        render.sprite = visualBroken;

        nbRepair = itemsNeeded.Count * powerItemRepair;

        //Sécurité pour que les items ne puissent pas être pris ni affichés
        for (int i = 0; i < itemsNeeded.Count; i++) {
            itemsNeeded[i].grabbable = false;
        }
    }

    // Update is called once per frame
    void Update() {
        
    }

    //Permet d'ajouter l'item à la réparation
    public bool Repair(List<Item> inventaire) {
        if (IsComplete()) return false;
        for(int i = 0; i < itemsNeeded.Count; i++) {
            if(!HaveItem(itemsNeeded[i], inventaire)) {
                //Lancement animation de refus
                return false;
            }
        }
        // Le joueur a tous les items requis
        repaired++; //On incrémente la réparation
        if (IsComplete()) {
            RunCompleteAnimation();
            if (myShip) myShip.CheckFinish(); // Prévenir le bateau lorsque la pièce est finie
            return true;
        }
        if (infoPiece) infoPiece.UpdateInfos(itemsNeeded, inventaire);
        return false;
    }

    //Retourne si la pièce a été réparé ( même chose wallah )
    public bool IsComplete() {
        //Le nombre de click total nécéssaire est égal au nombre de click donné
        return repaired == nbRepair;
    }

    public void SetShip(Ship ship) {
        myShip = ship;
    }

    //Retourn vrai si la pièce est compris dans l'inventaire
    private bool HaveItem(Item item, List<Item> inventaire) {
        for (int i = 0; i < itemsNeeded.Count; i++) {
            if (itemsNeeded[i].name == item.name) return true;
        }
        return false;
    }

    private void RunCompleteAnimation() {
        pieceAnimator.SetBool("Repaired", true);
    }
}
