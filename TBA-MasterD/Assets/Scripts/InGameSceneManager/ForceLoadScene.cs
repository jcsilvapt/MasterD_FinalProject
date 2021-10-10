using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ForceLoadScene : MonoBehaviour {

    [Header("Scene Controller Load Next Scene")]
    [SerializeField] GoToScene goToScene;

    private void Start() {
        switch (goToScene) {
            case GoToScene.LABORATORY:
                GameManager.ForceAsyncLoad(2);
                break;
            case GoToScene.GARAGE:
                GameManager.ForceAsyncLoad(3);
                break;
        }

    }

}

public enum GoToScene {
    MENU,
    TUTORIAL,
    LABORATORY,
    GARAGE
}
