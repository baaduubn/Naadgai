using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public bool GOD;
    public int _damage = 1;
    void OnTriggerEnter(Collider other)
    {
        if (GOD)
        {
            other.gameObject.GetComponent<HP>().damage();
        }
        else
        {
            other.gameObject.GetComponent<HP>().damage(_damage);

        }
    }


}
