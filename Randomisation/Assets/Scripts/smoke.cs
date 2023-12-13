using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smoke : MonoBehaviour
{
    public void end()
    {
        Destroy(transform.parent.gameObject);
    }
}
