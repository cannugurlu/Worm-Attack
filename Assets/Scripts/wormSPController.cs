using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wormSPController : MonoBehaviour
{
    [SerializeField] int whenShouldStart;
    [SerializeField] int period;
    [SerializeField] int probability;

    public GameObject wormPrefab;

    private void Start()
    {
        InvokeRepeating(nameof(spawnFunc), 1.0f,period);
    }


    void spawnFunc()
    {
        if(whenShouldStart <= GameObject.Find("gameManager").GetComponent<gameManager>().defeatedWorms)
        {
            int rand = Random.Range(0, 100);

            if(rand <= probability)
            {
                Instantiate(wormPrefab, transform.position, Quaternion.identity);
            }
        }

    }
}
