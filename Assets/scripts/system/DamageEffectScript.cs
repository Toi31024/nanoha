using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageEffectScript : MonoBehaviour
{
    [SerializeField] Image DamageEfect;
    [SerializeField] GameObject beam;
    AttackData Beam;
    void Start()
    {
        DamageEfect.color = Color.clear;
        Beam = beam.GetComponent<AttackData>();
    }

    void Update()
    {
        DamageEfect.color = Color.Lerp(DamageEfect.color, Color.clear, Time.deltaTime * 3);
        if (Beam.causesStun == false)
        {
            Damaged();
        }
    }

    public void Damaged()
    {
        DamageEfect.color = new Color(0.7f, 0, 0, 0.9f);
    }
}
