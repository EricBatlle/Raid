using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TestCalendar : MonoBehaviour
{

    [SerializeField] public List<Raider> raiders = new List<Raider>();

    public Raider auxRaider = null;
 

    public void CustomLoadRaiders()
    {
        Raider raider = new Raider(0, "Bronto", Raider.Spec.DPS);
        raiders.Add(raider);
        auxRaider = raider;
        raider = new Raider(1, "Burbu", Raider.Spec.Heal);
        raiders.Add(raider);

    }

}
