using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameEvent))]
public class EventInvokeButton : Editor
{
    GameEvent GameEvent;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GameEvent = (GameEvent)target;
        if (GameEvent != null)
        {
            if (GUILayout.Button("Raise Event"))
            {
                GameEvent.InvokeAllListeners();
            }
        }
    }
}
