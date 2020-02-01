using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Piece : MonoBehaviour
{
    #region publicVariables
    //Représente les items dont la pièce a besoin pour être réparé
    public List<Item> itemsNeeded = new List<Item>();
    public InfoPiece infoPiece;

    public Sprite visualBroken;
    public Sprite visualRepaired;
    #endregion

    #region privateVariables
    //Représente les items qui ont été donné pour être réparé par le joueur, initialement vide
    List<Item> items = new List<Item>();
    SpriteRenderer render;
    Animator pieceAnimator;

    Ship myShip;
    #endregion

    // Start is called before the first frame update
    void Start() {
        pieceAnimator = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();
        render.sprite = visualBroken;

        //Sécurité pour que les items ne puissent pas être pris ni affichés
        for(int i = 0; i < itemsNeeded.Count; i++) {
            itemsNeeded[i].grabbable = false;
        }
    }

    // Update is called once per frame
    void Update() {
        
    }

    //Permet d'ajouter l'item à la réparation
    public bool AddItem(Item itemToAdd) {
        if (IsComplete()) return false;
        if(!HaveItem(itemToAdd) && NeedItem(itemToAdd)) {
            items.Add(itemToAdd);
            if (IsComplete()) {
                RunCompleteAnimation();
                if (myShip) myShip.CheckFinish(); // Prévenir le bateau lorsque la pièce est finie
            }
            if (infoPiece) infoPiece.UpdateInfos(itemsNeeded, items);
            return true;
        }
        return false;
    }

    //Retourne si la pièce a été réparé, ou est complète ( même chose wallah )
    public bool IsComplete() {
        return items.Count == itemsNeeded.Count;
    }

    public void SetShip(Ship ship) {
        myShip = ship;
    }

    //Retourne vrai si la pièce possède déjà l'item
    private bool HaveItem(Item item) {
        for(int i = 0; i < items.Count; i++) {
            if (items[i].name == item.name) return true;
        }
        return false;
    }

    //Retourn vrai si la pièce à besoin de l'item pour être réparé
    private bool NeedItem(Item item) {
        for (int i = 0; i < itemsNeeded.Count; i++) {
            if (itemsNeeded[i].name == item.name) return true;
        }
        return false;
    }

    private void RunCompleteAnimation() {
        pieceAnimator.SetBool("Repaired", true);
    }
}
