using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MusicObject))]
[CanEditMultipleObjects]
public class MusicObjectEditor : Editor
{
    private AudioSource audioSource;

    // Properties
    SerializedProperty audioClip;
    SerializedProperty pitch;
    SerializedProperty volume;
    SerializedProperty transitionTime;
    // Foldouts
    bool showAudioSourceProperties = false;

    private void OnEnable()
    {
        audioSource = EditorUtility.CreateGameObjectWithHideFlags("GameObject", HideFlags.HideAndDontSave, typeof(AudioSource)).GetComponent<AudioSource>();
        audioClip = serializedObject.FindProperty("audioClip");
        // Serialized Properties
        pitch = serializedObject.FindProperty("pitch");
        volume = serializedObject.FindProperty("volume");
        transitionTime = serializedObject.FindProperty("transitionTime");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        MusicObject audioController = (MusicObject)target;

        EditorGUILayout.PropertyField(audioClip);

        showAudioSourceProperties = EditorGUILayout.Foldout(showAudioSourceProperties, "Audio Source Properties");
        if (showAudioSourceProperties)
        {
            EditorGUILayout.Slider(pitch, 0, 10, new GUIContent("Pitch"));
            audioSource.pitch = audioController.pitch;
            EditorGUILayout.Slider(volume, 0.0f, 1.0f, new GUIContent("Volume"));
            audioSource.volume = audioController.volume;
            EditorGUILayout.Slider(transitionTime, 0, 10, new GUIContent("Transition Time", "The transition time between audio clips"));
            
            
        }

        if (GUILayout.Button("Play"))
        {
            audioController.Play(audioSource);
        }

        if (GUILayout.Button("Pause"))
        {
            audioController.Pause(audioSource);
        }

        if (GUILayout.Button("Stop"))
        {
            audioController.Stop(audioSource);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
