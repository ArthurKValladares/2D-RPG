using UnityEngine;
using UnityEngine.UI;

public class Helpers
{
    static public Color GetColorByHex(string hex)
    {
        Color color;
        ColorUtility.TryParseHtmlString(hex, out color);
        return color;
    }
}