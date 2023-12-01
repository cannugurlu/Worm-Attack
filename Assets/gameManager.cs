using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class gameManager : MonoBehaviour
{
    //[SerializeField] public GameObject[] apples;
    [SerializeField] public Queue<GameObject> apples = new Queue<GameObject>();
    [SerializeField] public List<GameObject> outSideApples = new List<GameObject>();
    [SerializeField] GameObject[] appleObjects;
    [SerializeField] GameObject[] appleSpawnPoints;
    Vector3 emptyPos;
    private void Awake()
    {


        #region Adding Apples to Queue

        appleObjects = GameObject.FindGameObjectsWithTag("Apple");

        foreach (GameObject appleObject in appleObjects)
        {
            apples.Enqueue(appleObject);
        }

        print(apples.Count);
        #endregion

        #region Detect Spawnpoints
        appleSpawnPoints = GameObject.FindGameObjectsWithTag("AppleSP");
        #endregion
    }

    private void Start()
    {
        InvokeRepeating(nameof(AppleSpawnpointControl), 0.5f , 1.0f);
    }

    public void AppleEmplacement()
    {
        foreach (GameObject sp in appleSpawnPoints)
        {
            if(sp.GetComponent<appleSPController>().isAvailable)
            {
                sp.GetComponent<appleSPController>().isAvailable = false;
                emptyPos = sp.transform.position;
                break;
            }
        }

        GameObject lastApple = apples.Dequeue();
        outSideApples.Add(lastApple);

        print(lastApple.name + "  :  " + emptyPos);

        lastApple.transform.DOMoveY(lastApple.transform.position.y + 1f, 0.3f).OnComplete(()=>
        lastApple.transform.DOMoveY(lastApple.transform.localScale.y/2,0.2f));

        lastApple.transform.DOMoveX(emptyPos.x, 0.5f);
        lastApple.transform.DOMoveZ(emptyPos.z, 0.5f);
        

    }
    public void AppleSpawnpointControl()
    {
        int currentAvailablesNum = 0;

        foreach (GameObject apple in outSideApples)
        {
            if(!apple.activeSelf)
            {
                outSideApples.Remove(apple);
                break;
            }
        }

        foreach (var sp in appleSpawnPoints)
        {
            if (sp.GetComponent<appleSPController>().isAvailable)
            {
                currentAvailablesNum++;
            }
        }
        
        if(currentAvailablesNum > 0)
        {
            for (int k = 0; k < currentAvailablesNum; k++)
            {
                AppleEmplacement();
            }
        }
    }
}
