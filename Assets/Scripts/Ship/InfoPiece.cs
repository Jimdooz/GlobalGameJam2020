using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPiece : MonoBehaviour
{
    public GameObject bubblePrefab; //Représente une bulle modèle qu'on modifiera
    List<Bubble> bubbles = new List<Bubble>();
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void CreateBubble(Item item) {
        GameObject bubble = Instantiate(bubblePrefab);
        bubble.transform.SetParent(transform);
        Bubble currBubble = bubble.GetComponent<Bubble>();
        currBubble.SetItemSprite(item.graphic);
        bubbles.Add(currBubble);
    }

    public void UpdateInfos(List<Item> itemsNeeded, List<Item> inventory) {
        UpdateVisual();
    }

    private void UpdateVisual() {
        //TODO
    }
}
