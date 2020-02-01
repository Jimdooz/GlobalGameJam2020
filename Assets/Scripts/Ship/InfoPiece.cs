using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPiece : MonoBehaviour
{
    private List<Item> itemsNeeded = new List<Item>();
    private List<Item> items = new List<Item>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateInfos(List<Item> itemsNeeded, List<Item> items) {
        this.itemsNeeded = itemsNeeded;
        this.items = items;
        UpdateVisual();
    }

    private void UpdateVisual() {
        //TODO
    }
}
