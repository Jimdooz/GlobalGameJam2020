using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionLoader : MonoBehaviour
{
    public Sprite buttonSprite;
    public Image loader;
    public Color colorLoader;
    [Range(0f,100f)]
    public float percentage = 0f;

    private Image buttonImage;
    private float currPercentage = 0f;
    private float percentageMax = 100f;
    // Start is called before the first frame update
    void Start() {
        buttonImage = GetComponent<Image>();
        if (loader) {
            loader.color = colorLoader;
        }
        float unit = 1 / 32f;
        float diminue = 0.00005f;
        buttonImage.rectTransform.sizeDelta = new Vector2(buttonSprite.rect.width * unit - diminue, buttonSprite.rect.height * unit - diminue);
        loader.rectTransform.sizeDelta = new Vector2(loader.sprite.rect.width * unit - diminue, loader.sprite.rect.height * unit - diminue);

    }

    // Update is called once per frame
    void Update() {
        currPercentage = Mathf.Lerp(currPercentage, percentage, Time.deltaTime * 5f);
        this.loader.fillAmount = currPercentage / percentageMax;
    }

    public void setPercentage(float percentage) {
        this.percentage = percentage;
    }
}
