using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionLoader : InteractionButton
{
    public Image loader;
    public Color colorLoader;
    [Range(0f,1f)]
    public float percentage = 0f;

    private float currPercentage = 0f;

    // Start is called before the first frame update
    void Start() {
        if (loader) {
            loader.color = colorLoader;
        }
        animator = GetComponent<Animator>();
        button.rectTransform.sizeDelta = new Vector2(button.sprite.rect.width * InteractionButton.unit - InteractionButton.diminue, button.sprite.rect.height * InteractionButton.unit - InteractionButton.diminue);
        loader.rectTransform.sizeDelta = new Vector2(loader.sprite.rect.width * InteractionButton.unit - InteractionButton.diminue, loader.sprite.rect.height * InteractionButton.unit - InteractionButton.diminue);
    }

    // Update is called once per frame
    void Update() {
        currPercentage = Mathf.Lerp(currPercentage, percentage, Time.deltaTime * 5f);
        this.loader.fillAmount = currPercentage;
    }

    public void setPercentage(float percentage) {
        this.percentage = percentage;
    }

}
