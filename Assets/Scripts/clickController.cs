using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickController : MonoBehaviour
{
    public ParticleSystem clickParticle;
    public List<GameObject> selections = new List<GameObject>();
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
                if (hit.collider != null && hit.collider.transform.parent != null && hit.collider.transform.parent.tag == "Worm")
                {
                    selections.Add(hit.collider.gameObject);
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            foreach (GameObject obj in selections)
            {
                Destroy(obj);
            }

            clickParticle.Stop();
        }
    }
}
