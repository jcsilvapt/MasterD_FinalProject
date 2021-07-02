using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DoorController))]
public class DoorControllerEditor : Editor {

    public override void OnInspectorGUI() {

        base.OnInspectorGUI();

        var window = (DoorController)target;

        window.interactable = EditorGUILayout.Toggle("Interactable", window.interactable);

        if (window.interactable) {
            EditorGUILayout.HelpBox("Please Define what Key needs to be pressed.", MessageType.Info);
            window.inputKey = (KeyCode)EditorGUILayout.EnumPopup("Interaction Key", window.inputKey);
        }

        window.isDoubleDoor = EditorGUILayout.Toggle(new GUIContent("Is Double Door", "Use this for double Doors"), window.isDoubleDoor);

        if (window.isDoubleDoor) {
            window.openSideWays = EditorGUILayout.Toggle(new GUIContent("Open Side Ways", "When Enable the door will open to the sides"), window.openSideWays);
        }


        if(GUI.changed) {
            EditorUtility.SetDirty(window);
        }
    }

}
