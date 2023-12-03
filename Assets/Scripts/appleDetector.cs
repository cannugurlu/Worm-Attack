using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class appleDetector : MonoBehaviour
{
    private GameObject detectedApple, parentWorm;
    private Vector3 initialRot;
    public float initialSpeed;
    private int i = 1;
    bool isTouching = false;
    public bool isHurt = false;
    AudioSource audioSource;

    private void Start()
    {
        parentWorm = gameObject.transform.parent.gameObject;
        initialSpeed = parentWorm.GetComponent<wormMovement>().speed;
        initialRot = parentWorm.transform.eulerAngles;
        audioSource = GameObject.Find("audioSource").GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Apple")
        {
            isTouching = true;

            detectedApple = other.gameObject;

            StartCoroutine(animExit());
            parentWorm.GetComponent<wormMovement>().speed = 0;
            StartCoroutine(Attack());
        }
        if(other.gameObject.tag == "BasketCollider")
        {
            print("sepetttt");
            BasketDetected();
        }

        if(other.gameObject.tag == "Finish")
        {
            print("destroyed" + gameObject.transform.parent.gameObject.name);
            Destroy(gameObject.transform.parent.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isTouching=false;
    }

    IEnumerator animExit()
    {
        parentWorm.transform.GetChild(1).GetComponent<Animator>().SetBool("shouldExit", true);
        parentWorm.transform.GetChild(1).GetComponent<Animator>().SetBool("shouldRepeat", false);
        yield return new WaitForSeconds(.5f);
    }
    IEnumerator animEnter()
    {
        parentWorm.transform.GetChild(1).GetComponent<Animator>().SetBool("shouldRepeat", true);
        parentWorm.transform.GetChild(1).GetComponent<Animator>().SetBool("shouldExit", false);
        yield return new WaitForSeconds(.5f);
    }
    IEnumerator Attack()
    {
        print("attacking");
        while(!isHurt && detectedApple.GetComponent<appleStats>().health > 0 && isTouching && parentWorm.transform.eulerAngles.y == initialRot.y)
        {
            //boyutunu büyült küçült
            parentWorm.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f).OnComplete(() =>
            parentWorm.transform.DOScale(new Vector3(1, 1, 1), 0.2f));

            //elmanýn boyunu küçült
            detectedApple.transform.GetChild(0).transform.DOScale(detectedApple.GetComponent<appleStats>().health*0.3f, 0.2f);
            audioSource.Play();


            //eðer elmanýn caný 0 veya küçükse geri dön

            //can azalt
            detectedApple.GetComponent<appleStats>().health -= 1;
            yield return new WaitForSeconds(1);
        }

        if(detectedApple.GetComponent<appleStats>().health<=0)
        {
            detectedApple.SetActive(false);
            GoBack();
        }

        void GoBack()
        {
            parentWorm.transform.GetChild(1).DOLocalRotate(new Vector3(0, 270, 0), 0.5f);
            gameObject.GetComponent<SphereCollider>().enabled = false;
            parentWorm.GetComponent<wormMovement>().speed = -2 * initialSpeed;
        }

        //float appleSize = 1;
        //while(detectedApple.GetComponent<appleStats>().health>0 && isTouching) 
        //{
        //    parentWorm.transform.DOScale(1.2f, 0.2f).OnComplete(() =>
        //    {
        //        parentWorm.transform.DOScale(1, 0.2f);
        //    });

        //    appleSize -= 0.3f;
        //    detectedApple.transform.DOScale(appleSize, 0.2f);

        //    detectedApple.GetComponent<appleStats>().health--;

        //    yield return new WaitForSeconds(1);
        //}

        //if (detectedApple.GetComponent<appleStats>().health <= 0)
        //{
        //    detectedApple.SetActive(false);
        //    //parentWorm.transform.GetChild(1).localEulerAngles = new Vector3(0, 270, 0); 
        //    parentWorm.GetComponent<wormMovement>().speed = -2*initialSpeed;

        //    parentWorm.transform.GetChild(1).GetComponent<Animator>().SetBool("shouldRepeat", true);
        //    parentWorm.transform.GetChild(1).GetComponent<Animator>().SetBool("shouldExit", false);
        //}

    }

    void BasketDetected()
    {
        parentWorm.transform.GetChild(1).DOLocalRotate(new Vector3(0, 270, 0), 0.5f);
        gameObject.GetComponent<SphereCollider>().enabled = false;
        parentWorm.GetComponent<wormMovement>().speed = -2 * initialSpeed;
    }
}
