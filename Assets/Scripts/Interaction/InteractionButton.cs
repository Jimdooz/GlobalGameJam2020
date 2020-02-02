﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionButton : MonoBehaviour
{
    public Image button;
    public Animator animator;

    static protected float unit = 1 / 32f;
    static protected float diminue = 0.00005f;
    // Start is called before the first frame update
    void Start()
    {
        button.rectTransform.sizeDelta = new Vector2(button.sprite.rect.width * InteractionButton.unit - InteractionButton.diminue, button.sprite.rect.height * InteractionButton.unit - InteractionButton.diminue);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void show()
    {
        animator.SetBool("show", true);
    }

    public void hide()
    {
        animator.SetBool("show", false);
    }
}
