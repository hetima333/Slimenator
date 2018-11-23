using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor (typeof (ScriptableObject), true)]
public class ScriptableObjectEditor : Editor { }
#endif