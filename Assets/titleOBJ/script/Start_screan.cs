using UnityEngine;
using UnityEngine.SceneManagement;

public class Start_screan : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("Game");
        }
    }
}
