using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyEditorWindow : EditorWindow {


    private GameObject _main;
    private GameObject _patrol;

    // Enemy Data
    public int id;
    public float health = 100;

    // AI Data
    public bool enableAISystem = true;
    public float idleTime = 1.0f;
    public GameObject[] wayPoints;
    private GameObject wayPoint;

    // GUI SETTINGS
    public Vector2 scrollPosition = Vector2.zero;
    public Vector2 mainScrollPosition = Vector2.zero;
    private bool newWayPoint = false;

    [MenuItem("Configurations/Enemy Editor", false, 1)]
    public static void ShowWindow() {
        var window = EditorWindow.GetWindow(typeof(EnemyEditorWindow), true, "Enemy Editor");
        window.position = new Rect(Screen.width / 2 + 0.5f, Screen.height / 2 + 0.5f, 1024, 500);
    }

    private void Awake() {

        SetOrUpdateInitialData();

    }

    private void OnGUI() {

        GUILayout.Box("Enemy Editor Configurations", GUILayout.Width(1024));

        EditorGUILayout.BeginHorizontal();

        #region Display Left Options
        EditorGUILayout.BeginVertical(GUILayout.Width(10));

        DisplayLeftControlButtons(); // Display Left Buttons
        DisplayCurrentEnemiesInScene();

        EditorGUILayout.EndVertical();
        #endregion
        EditorGUILayout.BeginVertical();

        DisplayData();

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

    }

    #region GUI & GUI BUTTONS

    private void DisplayLeftControlButtons() {
        GUILayout.Box("Options", GUILayout.Width(200));

        if (GUILayout.Button("Create New Enemy", GUILayout.Width(200))) {
            //TODO:
        }
        GUI.enabled = false;
        if (GUILayout.Button("Save", GUILayout.Width(200))) {
            //TODO:
        }
        if (GUILayout.Button("Load", GUILayout.Width(200))) {
            //TODO:
        }
        GUI.enabled = true;
    }

    private void DisplayCurrentEnemiesInScene() {
        EditorGUILayout.BeginVertical();

        GUILayout.Box("Current Enemies In Scene", GUILayout.Width(200));

        EditorGUILayout.EndVertical();
    }

    private void DisplayData() {
        GUILayout.BeginVertical();

        //EditorGUILayout.HelpBox("System Mode Set to Linear, use this method for linear story, one quest at a time.", MessageType.Info);
        GUILayout.Box("Enemy Settings", GUILayout.ExpandWidth(true));

        EditorGUI.BeginDisabledGroup(true);
        id = EditorGUILayout.IntField("ID (auto)", id);
        EditorGUI.EndDisabledGroup();
        health = EditorGUILayout.FloatField("Enemy Health", health);
        //health = EditorGUILayout.TextField("Internal Title", internalTitle);

        GUILayout.Box("AI System", GUILayout.ExpandWidth(true));
        GUILayout.Label("SETTINGS", EditorStyles.boldLabel);
        enableAISystem = EditorGUILayout.Toggle(new GUIContent("Enable AI System", "Define if this enemy will be affected with AI System"), enableAISystem);
        if (!enableAISystem) {
            EditorGUILayout.HelpBox("AI System Disable.", MessageType.Info);
            GUI.enabled = false;
        }

        idleTime = EditorGUILayout.FloatField(new GUIContent("Idle Time", "Set here the amount of time the enemy will stay in idle Mode"), idleTime);
        GUILayout.Label("PATROL BEHAVIOUR SETTINGS", EditorStyles.boldLabel);
        DisplayWayPoints();
        GUILayout.EndVertical();

    }

    private void DisplayWayPoints() {
        GUILayout.Box("Patrol Points", GUILayout.ExpandWidth(true));
        GUI.enabled = !newWayPoint;
        if (GUILayout.Button("Create New WayPoint")) {
            newWayPoint = true;
            wayPoint = EditorCeator.CreateGameObject("WayPoint");
        }
        GUI.enabled = true;
        if (newWayPoint) {
            GUILayout.Label("Create new Way Point", EditorStyles.boldLabel);
        }
        mainScrollPosition = EditorGUILayout.BeginScrollView(mainScrollPosition, false, false, GUILayout.Height(0));

        EditorGUILayout.EndScrollView();
        /*
        ScriptableObject target = this;
        SerializedObject so = new SerializedObject(target);
        SerializedProperty actions = so.FindProperty("wayPoints");

        EditorGUILayout.PropertyField(actions, true, GUILayout.ExpandWidth(true));
        so.ApplyModifiedProperties();
        */
    }

    #endregion

    #region LOGIC

    private void SetOrUpdateInitialData() {
        if (!GameObject.Find(EditorConstants.enemyHolderName)) {
            _main = EditorCeator.CreateGameObject(EditorConstants.enemyHolderName);
            Debug.Log("Added Enemy Holder to Hierarchy.");
        } else {
            _main = GameObject.Find(EditorConstants.enemyHolderName);
            Debug.Log("Enemy Holder Updated.");
        }

        if (!GameObject.Find(EditorConstants.enemyPatrolName)) {
            _patrol = EditorCeator.CreateGameObject(EditorConstants.enemyPatrolName);
            Debug.Log("Added Enemy Patrol Points to Hierarchy.");
        } else {
            _patrol = GameObject.Find(EditorConstants.enemyPatrolName);
            Debug.Log("Enemy Patrol Points Updated.");
        }
    }

    #endregion

}
