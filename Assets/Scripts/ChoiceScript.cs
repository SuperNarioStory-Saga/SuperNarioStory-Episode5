using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class ChoiceScript : MonoBehaviour
{
    public VideoClip choice1;
    public VideoClip choice2;
    public VideoClip choice3;
    public GameObject special;

    private VideoPlayer vidp;
    private AudioSource audiosrc;
    private SpriteRenderer sprend;
    private bool cinematicStarted;

    // Start is called before the first frame update
    void Start()
    {
        cinematicStarted = false;
        audiosrc = transform.GetComponent<AudioSource>();
        vidp = transform.GetComponent<VideoPlayer>();
        sprend = transform.GetComponent<SpriteRenderer>();

        if (!GlobalScript.keyOwned)
        {
            special.SetActive(false);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        else if (!cinematicStarted)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                StartCoroutine(startVideo(choice1));
                GlobalScript.choice = 1;
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                StartCoroutine(startVideo(choice2));
                GlobalScript.choice = 2;
            }
            else if (Input.GetKeyDown(KeyCode.G) && GlobalScript.keyOwned)
            {
                StartCoroutine(startVideo(choice3));
                GlobalScript.choice = 3;
            }
        }
    }

    IEnumerator startVideo(VideoClip v)
    {
        cinematicStarted = true;
        while (sprend.color.a < 0.95f)
        {
            sprend.color = new Vector4(0, 0, 0, sprend.color.a + 0.05f);
            yield return new WaitForSeconds(0.05f);
        }
        audiosrc.Stop();
        sprend.color = new Vector4(0, 0, 0, 1);
        yield return new WaitForSeconds(0.4f);
        vidp.clip = v;
        vidp.Play();
        yield return new WaitForSeconds(10);
        while (vidp.isPlaying)
        {
            yield return new WaitForSeconds(1);
        }
        SceneManager.LoadScene("EndScene");
    }
}
