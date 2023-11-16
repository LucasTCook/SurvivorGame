using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class midExpGenerator : Experience
{

    // Start is called before the first frame update
    void Start()
    {
        amount = Random.Range(3, 9);
    }

}
