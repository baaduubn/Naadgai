using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour
{
    public int health = 100; private int currentHp;
    void Awake()
    {
        currentHp = health;
        die();
    }


    #region damage
    /// <summary>
    /// тоглогч гэмтэл авах үед дуудна.
    /// </summary>
    /// <param name="hp">хэр хэмжээний цус алдах</param>
    public void damage(int hp)
    {
        currentHp -= hp;
        if (currentHp <= health)
        {
            die();
        }
    }

    /// <summary>
    /// тоглогч шууд үхнэ.
    /// </summary>
    public void damage()
    {
        currentHp -= health;
        if (currentHp <= health)
        {
            die();
        }
    }
    #endregion

    private void die()
    {
        Debug.Log(this.gameObject.name+" үхэв.");
    }


}
