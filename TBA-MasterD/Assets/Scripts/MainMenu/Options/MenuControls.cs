using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuControls : MonoBehaviour {

    private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

    [SerializeField] TMP_Text up, down, left, right, crouch, jump, sprint, droneAction, droneMoveUp, droneMoveDown, interaction;
    [SerializeField] Slider sensitivity;
    [SerializeField] Toggle invertLook;

    [SerializeField] GameObject currentKey;

    private void Start() {
        // Define initial keys settings

        keys.Add("M_FW", KeyMapper.inputKey.WalkFoward);
        keys.Add("M_BK", KeyMapper.inputKey.WalkBackwards);
        keys.Add("M_L", KeyMapper.inputKey.WalkLeft);
        keys.Add("M_R", KeyMapper.inputKey.WalkRight);
        keys.Add("M_CROUCH", KeyMapper.inputKey.Crouch);
        keys.Add("M_JUMP", KeyMapper.inputKey.Jump);
        keys.Add("M_SPRINT", KeyMapper.inputKey.Sprint);
        keys.Add("D_ACTIVE", KeyMapper.inputKey.DroneActivation);
        keys.Add("D_MUP", KeyMapper.inputKey.DroneMoveUp);
        keys.Add("D_MDOWN", KeyMapper.inputKey.DroneMoveDown);
        keys.Add("INTERACTION", KeyMapper.inputKey.Interaction);

        // Change text in settings
        up.text = keys["M_FW"].ToString();
        down.text = keys["M_BK"].ToString();
        left.text = keys["M_L"].ToString();
        right.text = keys["M_R"].ToString();
        crouch.text = keys["M_CROUCH"].ToString();
        jump.text = keys["M_JUMP"].ToString();
        sprint.text = keys["M_SPRINT"].ToString();
        droneAction.text = keys["D_ACTIVE"].ToString();
        droneMoveUp.text = keys["D_MUP"].ToString();
        droneMoveDown.text = keys["D_MDOWN"].ToString();
        interaction.text = keys["INTERACTION"].ToString();

        sensitivity.value = KeyMapper.inputKey.MouseSensitivity;
        invertLook.isOn = KeyMapper.inputKey.InvertMouse;

    }

    private void OnGUI() {
        if (currentKey != null) {
            Event e = Event.current;
            if (e.isKey) {
                SaveKey(currentKey.name, e.keyCode);
                keys[currentKey.name] = e.keyCode;
                currentKey.GetComponentInChildren<TMP_Text>().text = e.keyCode.ToString();
                currentKey = null;
            }
        }
    }

    public void ChangeMouseSensitivity(GameObject slider) {
        KeyMapper.inputKey.MouseSensitivity = slider.GetComponent<Slider>().value;
        SaveSystemManager.SavePlayerSettings();
    }

    public void InvertLook(GameObject toogle) {
        KeyMapper.inputKey.InvertMouse = toogle.GetComponent<Toggle>().isOn;
        SaveSystemManager.SavePlayerSettings();
    }

    public void ChangeKey(GameObject clicked) {

        currentKey = clicked;

    }

    private void SaveKey(string name, KeyCode key) {
        switch (name) {
            case "M_FW":
                KeyMapper.inputKey.WalkFoward = key;
                break;
            case "M_BK":
                KeyMapper.inputKey.WalkBackwards = key;
                break;
            case "M_L":
                KeyMapper.inputKey.WalkLeft = key;
                break;
            case "M_R":
                KeyMapper.inputKey.WalkRight = key;
                break;
            case "M_CROUCH":
                KeyMapper.inputKey.Crouch = key;
                break;
            case "M_JUMP":
                KeyMapper.inputKey.Jump = key;
                break;
            case "M_SPRINT":
                KeyMapper.inputKey.Sprint = key;
                break;
            case "D_ACTIVE":
                KeyMapper.inputKey.DroneActivation = key;
                break;
            case "D_MUP":
                KeyMapper.inputKey.DroneMoveUp = key;
                break;
            case "D_MDOWN":
                KeyMapper.inputKey.DroneMoveDown = key;
                break;
            case "INTERACTION":
                KeyMapper.inputKey.Interaction = key;
                break;

        }

        SaveSystemManager.SavePlayerSettings();

    }



}
