using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SfxObject))]
[CanEditMultipleObjects]
public class SfxObjectEditor : Editor
{
    private AudioSource audioSource;

    // Properties
    SerializedProperty audioClip;
    SerializedProperty pitch;
    SerializedProperty volume;
    // Foldouts
    bool showAudioSourceProperties = false;

    private void OnEnable()
    {
        audioSource = EditorUtility.CreateGameObjectWithHideFlags("GameObject", HideFlags.HideAndDontSave, typeof(AudioSource)).GetComponent<AudioSource>();
        audioClip = serializedObject.FindProperty("audioClip");
        // Serialized Properties
        pitch = serializedObject.FindProperty("pitch");
        volume = serializedObject.FindProperty("volume");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        SfxObject audioController = (SfxObject)target;

        EditorGUILayout.PropertyField(audioClip);

        showAudioSourceProperties = EditorGUILayout.Foldout(showAudioSourceProperties, "Audio Source Properties");
        if (showAudioSourceProperties)
        {
            EditorGUILayout.Slider(pitch, 0, 10, new GUIContent("Pitch"));
            audioSource.pitch = audioController.pitch;
            EditorGUILayout.Slider(volume, 0.0f, 1.0f, new GUIContent("Volume"));
            audioSource.volume = audioController.volume;
        }

        if (GUILayout.Button("Play"))
        {
            audioController.Play(audioSource);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
