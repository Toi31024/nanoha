using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageEffectScript : MonoBehaviour
{
    [SerializeField] Image DamageEfect;
    void Start()
    {
        DamageEfect.color = Color.clear;
    }

    void Update()
    {
        DamageEfect.color = Color.Lerp(DamageEfect.color, Color.clear, Time.deltaTime * 3);
        Damaged();
    }

    void Damaged()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            DamageEfect.color = new Color(0.7f, 0, 0, 0.9f);
        }
    }
}
