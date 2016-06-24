using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SafeTween;

public class FlipChar : MonoBehaviour
{
    [Header("Reference")]
    public Text top;
    public Text buttom;
    public GameObject flap;
    public GameObject flapThis, flapNext;
    public Text thisText, nextText;
    [Header("Setting")]
    public float flipDuration = 0.5f;
    public float textOffset;
    State state;
    char thisChar = ' ';
    char nextChar = ' ';
    char nextNextChar;

    Tweener flipAnimation;

    void Start()
    {
        flipAnimation = new Tweener();
        flipAnimation.Add(new TweenLocalRotation(flap.transform, Vector3.zero, new Vector3(180, 0, 0), 0, flipDuration));
        flipAnimation.OnTime(0.5f, OnHalfFlipped);
        flipAnimation.OnForwardComplete(OnFlippingCompleted);

        top.text = thisChar.ToString();
        buttom.text = thisChar.ToString();
    }

    void Update()
    {
        if (thisChar != nextChar | nextChar != nextNextChar && state == State.Ready)
            StartFlipping();
    }

    void StartFlipping()
    {
        state = State.Fliping;
        flap.SetActive(true);
        flapThis.SetActive(true);
        flapNext.SetActive(false);

        top.text = "" + nextChar;
        thisText.text = "" + thisChar;
        nextText.text = "" + nextChar;

        flipAnimation.Play();
    }

    void OnHalfFlipped()
    {
        flapThis.SetActive(false);
        flapNext.SetActive(true);
    }

    void OnFlippingCompleted()
    {
        state = State.Ready;
        thisChar = nextChar;
        buttom.text = "" + thisChar;

        if (nextChar != nextNextChar)
            nextChar = nextNextChar;
    }

    public void SetChar(char character)
    {
        if (state != State.Fliping)
            nextChar = character;
        nextNextChar = character;
    }

    public enum State
    {
        Ready,
        Fliping
    }

    void OnValidate()
    {
        
        float x;
        x = top.rectTransform.anchoredPosition.x;
        top.rectTransform.anchoredPosition = new Vector2(x, textOffset);
        x = buttom.rectTransform.anchoredPosition.x;
        buttom.rectTransform.anchoredPosition = new Vector2(x, textOffset);
        x = thisText.rectTransform.anchoredPosition.x;
        thisText.rectTransform.anchoredPosition = new Vector2(x, textOffset);
        x = nextText.rectTransform.anchoredPosition.x;
        nextText.rectTransform.anchoredPosition = new Vector2(x, textOffset);
    }
}
