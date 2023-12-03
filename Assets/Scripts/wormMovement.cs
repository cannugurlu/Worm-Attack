using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wormMovement : MonoBehaviour
{
    [SerializeField] GameObject[] apples;
    Transform closestTarget = null;
    float closestDistance = Mathf.Infinity;
    Vector3 currentPosition;
    public float speed;

    void Start()
    {
        apples = GameObject.FindGameObjectsWithTag("Apple");

        FindClosestApple();

        gameObject.transform.LookAt(closestTarget.transform.position);

        print(closestTarget.transform.position);

    }

    private void Update()
    {
        move();
    }
    private void FindClosestApple()
    {
        currentPosition = transform.position;

        foreach (GameObject targetObject in apples)
        {
            float distanceToTarget = Vector3.Distance(currentPosition, targetObject.transform.position);

            if (distanceToTarget < closestDistance)
            {
                closestDistance = distanceToTarget;
                closestTarget = targetObject.transform;
            }
        }
    }


    public void move()
    {
        gameObject.transform.position += transform.forward * speed*Time.deltaTime;
    }
}
