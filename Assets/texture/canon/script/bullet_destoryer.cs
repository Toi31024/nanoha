using UnityEngine;

public class bullet_destoryer : MonoBehaviour
{
    void Update()
    {
        var pos = this.gameObject.transform.position;
        if (pos.x < 0)
        {
            pos.x *= -1;
        }

        if (pos.x > 36)
        {
            Destroy(this.gameObject);
        }
    }
}
