using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // private Vector2 originalScale;
    // private RectTransform rectTransform;
    // private Vector2 originalSize;
    // Start is called before the first frame update
    public Text text;
    public int originalFontSize = 60;
    public int hoverFontSize = 80;

    void Start()
    {
        // originalScale = transform.localScale;
        // rectTransform = transform.GetComponent<RectTransform>();
        // originalSize = rectTransform.sizeDelta;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Scale the button by the expand factor
        // Vector2 newScale = new Vector2(transform.localScale.x, transform.localScale.y); 
        // transform.localScale = newScale;
        // print("dshfjsdhf");
        // rectTransform.sizeDelta = originalSize * 1.2f;
        text.fontSize = hoverFontSize;
    }

    // This is called when the pointer exits the button
    public void OnPointerExit(PointerEventData eventData)
    {
        // Revert to the original size
        // transform.localScale = originalScale;
        text.fontSize = originalFontSize;
    }
}
