using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experience : MonoBehaviour
{
    public int amount;

    void Start()
    {
        amount = Random.Range(1, 2);
    }
}