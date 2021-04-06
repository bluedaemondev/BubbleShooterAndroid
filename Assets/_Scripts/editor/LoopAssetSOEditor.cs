using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BackgroundAndMusic))]
public class LoopAssetSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        BackgroundAndMusic backgroundAndMusicSO = (BackgroundAndMusic)target;

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("OUTPUT", new GUIStyle { fontStyle = FontStyle.Bold });
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        EditorGUILayout.BeginVertical();

        Texture texture = null;
        if (backgroundAndMusicSO.SpriteBackground != null)
        {
            texture = backgroundAndMusicSO.SpriteBackground.texture;
        }
        GUILayout.Box(texture, GUILayout.Width(150), GUILayout.Height(150));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("SpriteBackground"), GUIContent.none, true, GUILayout.Width(150));
        EditorGUILayout.EndVertical();

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();



        EditorGUILayout.Space();

        serializedObject.ApplyModifiedProperties();
    }
}
