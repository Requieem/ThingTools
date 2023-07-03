using UnityEngine;
using UnityEngine.UIElements;

public static class ShireColors
{
    public static Color ShiftHue(Color color, float hueShift)
    {
        Color.RGBToHSV(color, out float h, out float s, out float v);
        var newColor = Color.HSVToRGB((h + hueShift) % 360, s, v);
        return newColor;
    }
}
