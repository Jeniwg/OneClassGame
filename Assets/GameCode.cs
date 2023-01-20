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
        Vector3 fwd = shootCast.transform.TransformDirection(Vector3.up);
        RaycastHit objectHit;
        Debug.DrawRay(shootCast.transform.position, fwd * 50, Color.green);

        if (Input.GetKey(KeyCode.Mouse0) && canShoot)
        {


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
        yield return new WaitForSeconds(4.5f);
        // close eyes 
        startTimer = true;

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

        yield return new WaitForSeconds(.1f);

        canShoot = true;

        yield return new WaitForSeconds(4.15f);

        eyesColor = new Color(eyesColor.r, eyesColor.g, eyesColor.b, 0);
        timerColor = new Color(timerColor.r, timerColor.g, timerColor.b, 0);
        eyes.color = eyesColor;
        timer.color = timerColor;

        bang.SetActive(false);
        canShoot = false;

        startTimer = false;

        yield return new WaitForSeconds(2f);

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