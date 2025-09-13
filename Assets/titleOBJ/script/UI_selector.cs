using UnityEngine;

public class UI_selector : MonoBehaviour
{
    public bool selector_start = true;
    void Start()
    {
        
    }

    void Update()
    {
        if (selector_start == true)
        {
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                selector_start = false;
            }

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("Start!");
            }
        }

        if (selector_start == false)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                selector_start = true;
            }

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("Option");
            }
        }
    }
}
