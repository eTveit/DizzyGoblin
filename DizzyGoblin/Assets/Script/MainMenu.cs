using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    private void OnMouseDown() {
        if(name == "PlayText") {
            SceneManager.LoadScene(1);
        }
        else if(name == "ExitText") {
            Application.Quit();
        }
    }
}
