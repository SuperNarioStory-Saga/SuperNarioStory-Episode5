using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Video;

public class StartScript : MonoBehaviour
{
    public AudioClip start;
    public SpriteRenderer fade;
    public GameObject fireball;

    private AudioSource aud;
    private VideoPlayer vid;
    private bool gameStarted;
    // Start is called before the first frame update
    void Start()
    {
        aud = transform.GetComponent<AudioSource>();
        vid = transform.GetComponent<VideoPlayer>();
        gameStarted = false;
        StartCoroutine(FireballLaunch());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!gameStarted)
            {
                gameStarted = true;
                aud.PlayOneShot(start);
                StartCoroutine(GameStart());
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    IEnumerator FireballLaunch()
    {
        while(true)
        {
            float cooldown = Random.Range(0.5f, 2f);
            Instantiate(fireball, new Vector3(Random.Range(-13f, 3f), 6.38f, 0), Quaternion.AngleAxis(-57f, Vector3.forward));
            yield return new WaitForSeconds(cooldown);
        }
    }

    IEnumerator GameStart()
    {
        float a = 0;
        while (aud.volume > 0.1f)
        {
            a += 0.05f;
            fade.color = new Vector4(0, 0, 0, a); 
            aud.volume -= 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
        fade.color = new Vector4(0, 0, 0, 1f);
        aud.Stop();
        yield return new WaitForSeconds(0.4f);
        aud.volume = 1;
        vid.Play();
        yield return new WaitForSeconds(10);
        while (vid.isPlaying)
        {
            yield return new WaitForSeconds(1);
        }
        SceneManager.LoadScene("MainLevel");
    }
}
