using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class highExpGenerator : Experience
{

    // Start is called before the first frame update
    void Start()
    {
        amount = Random.Range(10,25);
    }

}
