using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPiece : MonoBehaviour
{
    public float displamentY = 1.5f;
    [Range(0f,64f)]
    public float padding = 40;

    public GameObject bubblePrefab; //Représente une bulle modèle qu'on modifiera
    public List<Bubble> bubbles = new List<Bubble>();
    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        RefreshPositionBubble();
    }

    public void CreateBubble(Item item) {
        GameObject bubble = Instantiate(bubblePrefab);
        bubble.transform.SetParent(transform);
        Bubble currBubble = bubble.GetComponent<Bubble>();
        currBubble.SetItemSprite(item.graphic);
        bubbles.Add(currBubble);

        RefreshPositionBubble();
    }

    public void UpdateInfo(int index, bool active) {
        if(index >= 0 && index < bubbles.Count)
        {
            bubbles[index].SetActive(active);
        }
    }

    private void UpdateVisual() {
        
    }

    private void RefreshPositionBubble() {
        float widthTotal = (bubbles.Count - 1) * (padding / 32f);
        float start = - (widthTotal / 2);

        for (int i = 0; i < bubbles.Count; i++) {
            bubbles[i].transform.localPosition = new Vector3(start, displamentY, 0f);
            start += (padding / 32f);
        }
    }

    public void ByeAll()
    {
        for (int i = 0; i < bubbles.Count; i++) {
            bubbles[i].Bye();
        }
    }
}
