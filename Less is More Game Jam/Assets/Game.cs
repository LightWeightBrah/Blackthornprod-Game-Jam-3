using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public Image fadeScreen;
    public float fadeSpeed;
    bool fadeToBlack, fadeOutBlack;

    [SerializeField] GameObject[] armorToTakeOff;
    [SerializeField] GameObject[] armorToSpawn;

    [SerializeField] GameObject pauseMenu;

    public bool isPaused;

    [SerializeField] float waitToLoad = 1f;

    int counter;

    public GameObject[] cutscenes;

    public bool areCutScenesEnabled;

    [SerializeField] GameObject GameOverScreen;

    void Start()
    {
        fadeOutBlack = true;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }

        if (fadeOutBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
            if (fadeScreen.color.a == 0f)
            {
                fadeOutBlack = false;
            }
        }

        if (fadeToBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
            if (fadeScreen.color.a == 1f)
            {
                fadeToBlack = false;
            }
        }

        if (areCutScenesEnabled && Input.anyKeyDown)
        {
            StartCoroutine(ShowNextCutscene());
        }
    }

    public IEnumerator ShowNextCutscene()
    {
        StartFadeToBlack();

        yield return new WaitForSeconds(waitToLoad);

        StartFadeOutBlack();

        if (counter + 1 == cutscenes.Length)
        {
            SceneManager.LoadScene("Menu");
        }
        else
        {
            cutscenes[counter].gameObject.SetActive(false);
            counter++;
            cutscenes[counter].gameObject.SetActive(true);
        }
    }

    public void GameOver()
    {
        GameOverScreen.gameObject.SetActive(true);
    }

    public void WinGame()
    {
        StartCoroutine(EndGame());
    }

    IEnumerator EndGame()
    {
        StartFadeToBlack();

        yield return new WaitForSeconds(waitToLoad);

        StartFadeOutBlack();

        cutscenes[0].gameObject.SetActive(true);

        areCutScenesEnabled = true;
    }

    public void TakeOffArmor(int index,bool isDouble)
    {
        if(isDouble)
        {
            for (int i = index; i <= index + 1; i++)
            {
                Instantiate(armorToSpawn[i], armorToTakeOff[i].transform.position, armorToTakeOff[i].transform.rotation);
                armorToTakeOff[i].gameObject.SetActive(false);
            }
        }
        else
        {
            Instantiate(armorToSpawn[index], armorToTakeOff[index].transform.position, armorToTakeOff[index].transform.rotation);
            armorToTakeOff[index].gameObject.SetActive(false);
        }
        
    }

    public void PauseUnpause()
    {
        if (!isPaused)
        {
            pauseMenu.SetActive(true);

            isPaused = true;

            Time.timeScale = 0f;
        }
        else
        {
            pauseMenu.SetActive(false);

            isPaused = false;

            Time.timeScale = 1f;
        }
    }
    public void StartFadeToBlack()
    {
        fadeToBlack = true;
        fadeOutBlack = false;
    }

    public void StartFadeOutBlack()
    {
        fadeToBlack = false;
        fadeOutBlack = true;
    }

    public void GoToMenu()
    {
        StartCoroutine(StartGoToMenu());
    }

    public void Restart()
    {
        SceneManager.LoadScene("SCENA");
    }

    IEnumerator StartGoToMenu()
    {
        Time.timeScale = 1f;
        StartFadeToBlack();

        yield return new WaitForSeconds(waitToLoad);

        SceneManager.LoadScene("Menu");
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            WinGame();
        }
    }
}
