using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameCode : MonoBehaviour
{
    [SerializeField]
    private Material backShot;
    [SerializeField]
    private Material targetShot;
    [SerializeField]
    private Image eyes;
    [SerializeField]
    private Text timer;
    [SerializeField]
    private GameObject bang;
    [SerializeField]
    private GameObject shootCast;

    [SerializeField]
    private GameObject winScreen;
    [SerializeField]
    private GameObject loseScreen;

    private bool canShoot = false;
    private bool startTimer = false;
    private float miliseconds = 0;
    private float seconds = 0;

    private bool win = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("CloseEyes");
    }

    // Update is called once per frame
    void Update()
    {
        //verifica se pode atirar e se carrega no leftmousebutton, só se pode atirar uma vez
        if (Input.GetKey(KeyCode.Mouse0) && canShoot)
        {
            //raycast para ver no que acerta, dummy ou background
            Vector3 fwd = shootCast.transform.TransformDirection(Vector3.up);
            RaycastHit objectHit;
            Debug.DrawRay(shootCast.transform.position, fwd * 50, Color.green);

            if (Physics.Raycast(shootCast.transform.position, fwd, out objectHit, 50))
            {
                if (objectHit.collider.tag == "Dummy")
                {
                    win = true;
                    bang.SetActive(true);
                    Debug.Log("Close to enemy");
                    objectHit.collider.GetComponent<MeshRenderer>().material = targetShot;
                    canShoot = false;
                    return;

                }
                else if (objectHit.collider.tag == "Back")
                {
                    win = false;
                    bang.SetActive(true);
                    objectHit.collider.GetComponent<MeshRenderer>().material = backShot;
                    Debug.Log("Background");
                    canShoot = false;
                    return;
                }
            }
        }

        //countdown de sec:milsec de 5sec
        if (startTimer)
        {
            if (miliseconds <= 0)
            {
                if (seconds <= 0)
                {
                    seconds = 4;
                }
                else if (seconds >= 0)
                {
                    seconds--;
                }

                miliseconds = 100;
            }

            miliseconds -= Time.deltaTime * 100;
            timer.text = string.Format("{0}:{1}", seconds, (int)miliseconds);
        }

    }

    IEnumerator CloseEyes()
    {
        //tempo para observar
        yield return new WaitForSeconds(4.5f);

        startTimer = true;

        // close eyes -> fade do ecrã
        Color eyesColor = eyes.color;
        Color timerColor = timer.color;
        float fadeAmount;
        int fadeSpeed = 1;

        while (eyes.color.a < 1)
        {
            fadeAmount = eyesColor.a + (fadeSpeed * Time.deltaTime);

            eyesColor = new Color(eyesColor.r, eyesColor.g, eyesColor.b, fadeAmount);
            timerColor = new Color(timerColor.r, timerColor.g, timerColor.b, fadeAmount);
            eyes.color = eyesColor;
            timer.color = timerColor;

            yield return null;
        }

        // mini tempo para n poder atirar logo
        yield return new WaitForSeconds(.1f);

        //pode atirar até fim dos 5sec
        canShoot = true;

        yield return new WaitForSeconds(4.15f);

        //Abre os olhos 
        eyesColor = new Color(eyesColor.r, eyesColor.g, eyesColor.b, 0);
        timerColor = new Color(timerColor.r, timerColor.g, timerColor.b, 0);
        eyes.color = eyesColor;
        timer.color = timerColor;

        bang.SetActive(false);
        canShoot = false;

        startTimer = false;

        //mini tempo depois de abrir os olhos para o jogador observar o que acertou
        yield return new WaitForSeconds(2f);

        //verifica se ganhou ou n
        if (win)
        {
            //win Screen
            winScreen.SetActive(true);
        }
        else
        {
            // lose screen
            loseScreen.SetActive(true);
        }
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        SceneManager.LoadScene(0);
    }
}