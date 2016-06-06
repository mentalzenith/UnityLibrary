using UnityEngine;
using System.Collections;

public class RandomContinousColor
{
    public static float cycleLength = 1;
    static int cycle;
    static Color thisColor, lastColor;


    public static Color GetRandomHSVColor()
    {
        return GetRandomHSVColor(0, 1, 0.5f, 1, 0.5f, 1, 0.3f, 1);
    }

    public static Color GetRandomHSVColor(float hueMin, float hueMax, float saturationMin, float saturationMax, float valueMin, float valueMax, float alphaMin, float alphaMax)
    {
        int currentCycle = (int)(Time.time / cycleLength);
        if (cycle != currentCycle)
        {
            cycle = currentCycle;
            lastColor = thisColor;
            thisColor = Random.ColorHSV(hueMin, hueMax, saturationMin, saturationMax, valueMin, valueMax, alphaMin, alphaMax);
        }
        return Color.Lerp(lastColor, thisColor, (Time.time % cycleLength) / cycleLength);
    }
}
