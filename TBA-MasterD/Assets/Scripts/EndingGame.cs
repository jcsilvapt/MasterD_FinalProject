using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingGame : MonoBehaviour {

    public void GoToMainMenu() {
        GameManager.ChangeScene(0, false);
    }


}
