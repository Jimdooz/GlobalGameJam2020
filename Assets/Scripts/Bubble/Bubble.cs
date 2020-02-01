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
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void SetItemSprite(Sprite sprite) {
        itemSprite = sprite;
        renderItem.sprite = itemSprite;
    }

    public void SetActive(bool active) {
        animator.SetBool("active", active);
    }
}
