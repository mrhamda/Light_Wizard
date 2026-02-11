using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Diffculity
{
    static float secondsToMaxDiffculity = 60 * 3;
    public static float GetDiffculityPercent()
    {
        return Mathf.Clamp01( Time.timeSinceLevelLoad / secondsToMaxDiffculity);
    }
}
