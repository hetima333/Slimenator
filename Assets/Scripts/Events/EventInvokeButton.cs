﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameEvent))]
public class EventInvokeButton : Editor
{
    GameEvent GameEvent;
    Object source;
    ScriptableObject so;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GameEvent = (GameEvent)target;
        if (GameEvent != null)
        {
            source = EditorGUILayout.ObjectField(source, typeof(Object), true);
            if (GUILayout.Button("Raise Event"))
            {
                GameEvent.InvokeAllListeners();
            }

            if (GUILayout.Button("Raise Specific Event"))
            {
                GameEvent.InvokeSpecificListner(source.GetInstanceID());
            }
        }
    }
}
