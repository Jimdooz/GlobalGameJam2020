using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPiece : MonoBehaviour
{
    public float displamentY = 32f;
    public float padding = 32f;

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

        RefreshPositionBubble();
    }

    public void UpdateInfos(List<Item> itemsNeeded, List<Item> inventory) {
        UpdateVisual();
    }

    private void UpdateVisual() {
        //TODO
    }

    private void RefreshPositionBubble() {
        float itemsSize = 32f;

        for(int i = 0; i < bubbles.Count; i++) {

        }
    }
}
