using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickController : MonoBehaviour
{
    public ParticleSystem clickParticle;
    public List<GameObject> selections = new List<GameObject>();
    HashSet<GameObject> applesSet = new HashSet<GameObject>();

    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane + 3));
        clickParticle.transform.localPosition = mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            selections.Clear();

            clickParticle.Play();
        }
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null && hit.collider.transform.parent != null && hit.collider.tag == "WormCollider")
                {
                    selections.Add(hit.collider.gameObject);
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            foreach (GameObject obj in selections)
            {
                obj.transform.parent.GetChild(2).GetComponent<appleDetector>().isHurt = true;
                obj.transform.parent.GetChild(2).GetComponent<SphereCollider>().enabled = false;
                obj.transform.parent.GetChild(1).DOLocalRotate(new Vector3(0, 270, 0), 0.5f);
                obj.transform.parent.GetComponent<wormMovement>().speed = -2*obj.transform.parent.GetChild(2).GetComponent<appleDetector>().initialSpeed;
                
                applesSet.Add(obj);
            //parentWorm.transform.GetChild(1).DOLocalRotate(new Vector3(0, 270, 0), 0.5f);
            //gameObject.GetComponent<SphereCollider>().enabled = false;
            //parentWorm.GetComponent<wormMovement>().speed = -2 * initialSpeed;
            }
            GameObject.Find("gameManager").GetComponent<gameManager>().defeatedWorms += applesSet.Count;
            applesSet.Clear();
            clickParticle.Stop();
        }
    }
}
