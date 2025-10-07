using UnityEngine;
using UnityEngine.SceneManagement;

public class result_scene_transition : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("TitleScean");
        }
    }
}
