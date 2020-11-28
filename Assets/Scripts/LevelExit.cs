using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour {

    [SerializeField] float delayToNextSceneInSec = 3f;

    void OnTriggerEnter2D(Collider2D otherCollider) {
        StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadNextLevel() {
        yield return new WaitForSecondsRealtime(delayToNextSceneInSec);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
