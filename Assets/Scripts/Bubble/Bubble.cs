using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public bool active = false;
    public Sprite itemSprite;
    public SpriteRenderer renderItem;

    private Animator animator;
    // Start is called before the first frame update
    void Start() {
        SetItemSprite(itemSprite);
        animator = GetComponent<Animator>();
        renderItem.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + 1;
    }

    // Update is called once per frame
    void Update() {
        
    }

    //Permet de modifier le sprite
    public void SetItemSprite(Sprite sprite) {
        itemSprite = sprite;
        renderItem.sprite = itemSprite;
    }

    //Permet de mettre en actif la bulle, lorsque le player possède l'item
    public void SetActive(bool active) {
        animator.SetBool("active", active);
        if (active) {
            renderItem.color = new Color(1, 1, 1);
        } else {
            renderItem.color = new Color(0, 0, 0);
        }
    }
}
