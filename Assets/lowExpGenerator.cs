using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lowExpGenerator : Experience
{

    // Start is called before the first frame update
    void Start()
    {
        amount = Random.Range(1, 2);
    }

}
