using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(BackgroundColor))]
public class BackgroundColorDecorator : DecoratorDrawer
{
    BackgroundColor attr { get { return ((BackgroundColor)attribute); } }
    public override float GetHeight() { return 0; }

    public override void OnGUI(Rect position)
    {
        GUI.backgroundColor = attr.color;
    }
}