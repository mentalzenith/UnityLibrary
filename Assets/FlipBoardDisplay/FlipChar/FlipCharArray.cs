using UnityEngine;
using System.Collections;

public class FlipCharArray : MonoBehaviour
{
    public FlipChar[] flipChars;
    public bool flipped;

    public void SetString(string s)
    {
        var charArray = s.ToCharArray();
        for (int i = 0; i < flipChars.Length; i++)
            flipChars[i].SetChar(charArray[flipped ? charArray.Length - i - 1 : i]);
    }
}
