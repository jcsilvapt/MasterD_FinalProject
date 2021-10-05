using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSceneController : MonoBehaviour {

    [Header("DEVELOPER SETTINGS")]
    [SerializeField] GameObject player;

    [SerializeField] List<GameObject> objectsInScene;

    // Start is called before the first frame update
    void Start() {
        if (!GameManager.IsContinuous()) {
            ActivateAllObjectsInScene();
        }
    }

    private void ActivateAllObjectsInScene() {
        foreach (GameObject b in objectsInScene) {
            b.SetActive(true);
        }
    }

    public void SetPlayer(GameObject player) {
        this.player = player;
    }

    public void COMECA() {
        ActivateAllObjectsInScene();
    }
}
