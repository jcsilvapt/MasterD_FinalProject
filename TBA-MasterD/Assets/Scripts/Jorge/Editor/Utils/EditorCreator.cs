using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorCeator {

    public static GameObject CreateGameObject(string name) {
        GameObject _object = new GameObject(name);

        return _object;
    }

}
