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
    public InteractionButton interactionPrefab;
    public Transform positionInteraction;

    #endregion

    #region privateVariables
    float nbRepair; // Nombre de click total nécéssaire à la réparation
    int repaired = 0; // Pourcentage réparé
    SpriteRenderer render;
    Animator pieceAnimator;

    Ship myShip;
    private InteractionLoader interaction;
    #endregion

    // Start is called before the first frame update
    void Start() {
        pieceAnimator = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();
        render.sprite = visualBroken;

        nbRepair = itemsNeeded.Count * powerItemRepair;

        //Sécurité pour que les items ne puissent pas être pris ni affichés
        for (int i = 0; i < itemsNeeded.Count; i++) {
            infoPiece.CreateBubble(itemsNeeded[i]);
        }

        interaction = InteractionController.generateInteraction(interactionPrefab).GetComponent<InteractionLoader>();
        interaction.transform.position = positionInteraction.position;
    }

    // Update is called once per frame
    void Update() {
        
    }

    //Permet d'ajouter l'item à la réparation
    // 0 -> Déjà terminé
    // 1 -> Terminé
    // 2 -> Construit
    // 3 -> Items non suffisant
    public int Repair(List<Item> inventaire) {
        if (IsComplete()) return 0; // Déjà terminé
        for(int i = 0; i < itemsNeeded.Count; i++) {
            if(!HaveItem(itemsNeeded[i], inventaire)) {
                //Lancement animation de refus
                return 3; // Pas items suffisant
            }
        }
        // Le joueur a tous les items requis
        repaired++; //On incrémente la réparation
        interaction.SetPercentage(repaired / nbRepair);
        MusicManager.Effect("Reparation");
        if (IsComplete()) {
            RunCompleteAnimation();
            if (myShip) myShip.CheckFinish(); // Prévenir le bateau lorsque la pièce est finie
            return 1; // Terminé
        }
        return 2; //Construit
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
        for (int i = 0; i < inventaire.Count; i++) {
            if (inventaire[i].id == item.id) return true;
        }
        return false;
    }

    private void RunCompleteAnimation() {
        pieceAnimator.SetBool("Repaired", true);
    }

    public void checkAllBubble(List<Item> inventaire)
    {
        for (int i = 0; i < itemsNeeded.Count; i++)
        {
            infoPiece.UpdateInfo(i, HaveItem(itemsNeeded[i], inventaire));
        }
    }

    public static void UpdateAll(List<Item> inventaire)
    {
        List<GameObject> allPieces = FindGameObjectsInLayer(LayerMask.NameToLayer("Pieces"));
        for(int i = 0; i < allPieces.Count; i++)
        {
            Piece piece = allPieces[i].GetComponent<Piece>();
            if (piece) piece.checkAllBubble(inventaire);
        }
    }

    static List<GameObject> FindGameObjectsInLayer(int layer) {
        GameObject[] goArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        List<GameObject> goList = new List<GameObject>();
        for (int i = 0; i < goArray.Length; i++)
        {
            if (goArray[i].layer == layer)
            {
                goList.Add(goArray[i]);
            }
        }
        if (goList.Count == 0)
        {
            return null;
        }
        return goList;
    }
}
