using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    //[SerializeField] public GameObject[] apples;
    [SerializeField] public Queue<GameObject> apples = new Queue<GameObject>();
    [SerializeField] public List<GameObject> outSideApples = new List<GameObject>();
    [SerializeField] GameObject[] appleObjects;
    [SerializeField] GameObject[] appleSpawnPoints;
    public GameObject endPanel,startPanel,pausePanel;
    public TextMeshProUGUI endText,highScoreText;
    public int defeatedWorms=0;
    public int appleNumber = 0;
    Vector3 emptyPos;

    public int highScore;

    public Image appleImage;
    public TextMeshProUGUI wormText;
    private void Awake()
    {
        Time.timeScale = 0.0f;

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

    private void Update()
    {
        appleNumber = GameObject.FindGameObjectsWithTag("Apple").Length;
        appleImage.fillAmount = (float)appleNumber / 10;

        wormText.text = defeatedWorms.ToString();

        if(appleNumber == 0)
        {
            highScore = PlayerPrefs.GetInt("Highscore", 0);
            CheckAndUpdateHighscore();
            highScoreText.text = "High Score : " + highScore;

            Time.timeScale = 0;
            endPanel.SetActive(true);
            endText.text = "You Have Defeated " + defeatedWorms.ToString() + " Worms!";
        }

        if(startPanel.active && Input.GetKeyDown(KeyCode.Space))
        {
            startGame();
        }
        else if(!startPanel.active && Input.GetKeyDown(KeyCode.Space) && Time.timeScale>0)
        {
            pauseGame();
        }
        else if(pausePanel.active && Input.GetKeyDown(KeyCode.Space))
        {
            startGame();
        }

        else if(endPanel.active && Input.GetKeyDown(KeyCode.Space))
        {
            restartGame();
        }
    }

    public void CheckAndUpdateHighscore()
    {
        if (defeatedWorms > highScore)
        {
            highScore = defeatedWorms;
            PlayerPrefs.SetInt("Highscore", highScore);
            PlayerPrefs.Save();
        }
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

        if(apples.Count > 0)
        {
            GameObject lastApple = apples.Dequeue();
            outSideApples.Add(lastApple);

            lastApple.transform.DOMoveY(lastApple.transform.position.y + 1f, 0.3f).OnComplete(() =>
            lastApple.transform.DOMoveY(lastApple.transform.localScale.y / 2, 0.2f));

            lastApple.transform.DOMoveX(emptyPos.x, 0.5f);
            lastApple.transform.DOMoveZ(emptyPos.z, 0.5f);
        }

        //GameObject lastApple = apples.Dequeue();
        //outSideApples.Add(lastApple);

        //print(lastApple.name + "  :  " + emptyPos);

        //lastApple.transform.DOMoveY(lastApple.transform.position.y + 1f, 0.3f).OnComplete(()=>
        //lastApple.transform.DOMoveY(lastApple.transform.localScale.y/2,0.2f));

        //lastApple.transform.DOMoveX(emptyPos.x, 0.5f);
        //lastApple.transform.DOMoveZ(emptyPos.z, 0.5f);
        

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

    public void pauseGame()
    {
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
    }
    public void startGame()
    {
        Time.timeScale = 1;
        startPanel.SetActive(false);
        pausePanel.SetActive(false);
    }
    public void restartGame()
    {
        SceneManager.LoadScene(0);
    }
}
