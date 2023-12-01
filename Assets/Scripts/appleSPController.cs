using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class appleSPController : MonoBehaviour
{
    public bool isAvailable;

    private void Awake() { isAvailable = true; }

    private void Start()
    {
        InvokeRepeating("isThereApple", 1f,0.5f);
    }

    void isThereApple()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position , 1f);
        Collider[] targetColliders = System.Array.FindAll(colliders, c => c.CompareTag("Apple"));

        if (targetColliders.Length > 0) isAvailable = false;
        else isAvailable = true;
    }

}
