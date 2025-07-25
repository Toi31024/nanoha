using UnityEngine;

public class player_controller : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            transform.Translate(0, 3, 0);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.Translate(3, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            transform.Translate(0, -3, 0);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.Translate(-3, 0, 0);
        }
    }
}
