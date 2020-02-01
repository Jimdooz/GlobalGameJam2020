using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer)), RequireComponent(typeof(CircleCollider2D))]
public class Item : MonoBehaviour
{
    public string id = "";
    public Sprite graphic; //Sans contour (UI)
    public Sprite graphicGround; //Avec contour
    public bool grabbable = false;

    private bool taken = false;
    private SpriteRenderer renderSprite;
    private CircleCollider2D itemCollider;

    void Start()
    {
        renderSprite = GetComponent<SpriteRenderer>();
        itemCollider = GetComponent<CircleCollider2D>();

        if (renderSprite.sprite) graphic = renderSprite.sprite;
        else renderSprite.sprite = graphic;
        if (grabbable) setFree();
        else setTaken();
    }

    //Permet de récupérer l'item
    //Renvois vrai si l'item peut être pris, faux sinon
    public bool Take() {
        if (taken || !grabbable) return false;
        setTaken();
        return true;
    }

    //Permet de libérer l'item
    //Renvois vrai si l'item peut être libéré, faux sinon
    public bool Free() {
        if (!taken || !grabbable) return false;
        setFree();
        return true;
    }

    public bool IsTaken() {
        return taken;
    }

    void setTaken() {
        renderSprite.enabled = false;
        itemCollider.enabled = false;
        taken = true;
    }

    void setFree() {
        renderSprite.enabled = true;
        itemCollider.enabled = true;
        taken = false;
    }
}
