using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : MonoBehaviour
{
    public class Transformation
    {
        public enum Clips { Jump, Hit, Sweep, Land, None };

        public List<Vector3> positions;
        public List<Quaternion> rotations;
        public List<float> speeds;
        public List<bool> rotationOnly;
        public List<Clips> clips;

        public Transformation(Vector3 pos, Vector3 rot)
        {
            positions = new List<Vector3>();
            positions.Add(pos);
            rotations = new List<Quaternion>();
            rotations.Add(Quaternion.Euler(rot.x, rot.y, rot.z));
            speeds = new List<float>();
            speeds.Add(9f);
            rotationOnly = new List<bool>();
            rotationOnly.Add(false);
            clips = new List<Clips>();
            clips.Add(Clips.Jump);
        }

        public void AddClipToLast(Clips clip)
        {
            clips[clips.Count - 1] = clip;
        }
        public void Add(Vector3 pos, Vector3 rot)
        {
            positions.Add(pos);
            rotations.Add(Quaternion.Euler(rot.x, rot.y, rot.z));
            speeds.Add(9f);
            rotationOnly.Add(false);
            clips.Add(Clips.None);
        }

        public void Add(Vector3 pos, Vector3 rot, float speed)
        {
            positions.Add(pos);
            rotations.Add(Quaternion.Euler(rot.x, rot.y, rot.z));
            speeds.Add(speed);
            rotationOnly.Add(false);
            clips.Add(Clips.None);
        }

        public void Add(Vector3 pos, Vector3 rot, bool roton)
        {
            positions.Add(pos);
            rotations.Add(Quaternion.Euler(rot.x, rot.y, rot.z));
            speeds.Add(9f);
            rotationOnly.Add(roton);
            clips.Add(Clips.None);
        }

        public void Add(Vector3 pos, Vector3 rot, float speed, bool roton)
        {
            positions.Add(pos);
            rotations.Add(Quaternion.Euler(rot.x, rot.y, rot.z));
            speeds.Add(speed);
            rotationOnly.Add(roton);
            clips.Add(Clips.None);
        }
    }

    public float speed;
    public AudioClip clipDeath;
    public AudioClip clipHit;
    public AudioClip clipLand;
    public AudioClip clipSweep;
    public AudioClip clipJump;
    public AudioClip clipKey;

    private List<Transformation> transformations;
    public bool isAnim;

    private Rigidbody2D rb2d;
    private BoxCollider2D bcollid;
    private SpriteRenderer sprend;
    private Animator animControl;
    private AudioSource audiosrc;
    private AudioSource audioThemesrc;
    private bool canMove;
    private string currentQTELetter;
    private int currentQTEId;

    void Start()
    {   
        rb2d = transform.GetComponent<Rigidbody2D>();
        bcollid = transform.GetComponent<BoxCollider2D>();
        sprend = transform.GetComponent<SpriteRenderer>();
        animControl = transform.GetComponent<Animator>();
        audiosrc = transform.GetComponents<AudioSource>()[0];
        audioThemesrc = transform.GetComponents<AudioSource>()[1];

        transformations = new List<Transformation>();
        canMove = false;
        currentQTELetter = null;
        currentQTEId = -1;

        /*QTEs*/
        //qte 0
        Transformation qte0 = new Transformation(new Vector3(-19.64f, -2.82f, -1), Vector3.zero);
        qte0.Add(new Vector3(-19.41f, 1.34f, -1), Vector3.zero);
        qte0.Add(new Vector3(-18.02f, 0.31f, -1), Vector3.zero);
        qte0.AddClipToLast(Transformation.Clips.Land);
        transformations.Add(qte0);
        //qte 1
        Transformation qte1 = new Transformation(new Vector3(-18.02f, 0.31f, -1), Vector3.zero);
        qte1.Add(new Vector3(-17.45f, 3.8f, -1), Vector3.zero);
        qte1.Add(new Vector3(-16.53f, 0.88f, -1), new Vector3(0, 0, -180f));
        qte1.AddClipToLast(Transformation.Clips.Sweep);
        qte1.Add(new Vector3(-15.34f, 0.24f, -1), new Vector3(0, 0, 180f));
        qte1.AddClipToLast(Transformation.Clips.Sweep);
        qte1.Add(new Vector3(-15.34f, 0.24f, -1), Vector3.zero, 100, true);
        qte1.AddClipToLast(Transformation.Clips.Land);
        transformations.Add(qte1);
        //qte 2
        Transformation qte2 = new Transformation(new Vector3(-14.12f, -2.8f, -1), Vector3.zero);
        qte2.Add(new Vector3(-12.54f, -0.57f, -1), Vector3.zero);
        qte2.Add(new Vector3(-11.45f, -2.1f, -1), Vector3.zero);
        qte2.AddClipToLast(Transformation.Clips.Land);
        transformations.Add(qte2);
        //qte 3
        Transformation qte3 = new Transformation(new Vector3(-11.45f, -2.1f, -1), Vector3.zero);
        qte3.Add(new Vector3(-9.77f, -0.18f, -1), Vector3.zero);
        qte3.AddClipToLast(Transformation.Clips.Land);
        transformations.Add(qte3);
        //qte 4
        Transformation qte4 = new Transformation(new Vector3(-9.2f, -2.32f, -1), Vector3.zero);
        qte4.Add(new Vector3(-9.2f, -2.32f, -1), new Vector3(0, 0, 50f), 200, true);
        qte4.AddClipToLast(Transformation.Clips.Land);
        qte4.Add(new Vector3(-14.61f, -0.51f, -1), Vector3.zero);
        qte4.Add(new Vector3(-14.61f, -0.51f, -1), new Vector3(0, 0, -42f), 200, true);
        qte4.AddClipToLast(Transformation.Clips.Land);
        qte4.Add(new Vector3(-10.32f, 2.78f, -1), Vector3.zero);
        qte4.Add(new Vector3(-10.32f, 2.78f, -1), Vector3.zero, 200, true);
        qte4.AddClipToLast(Transformation.Clips.Land);
        transformations.Add(qte4);
        //qte 5
        Transformation qte5 = new Transformation(new Vector3(-6.12f, -0.82f, -1), Vector3.zero);
        qte5.Add(new Vector3(-4.88f, 0.41f, -1), Vector3.zero);
        qte5.Add(new Vector3(-3.58f, -0.85f, -1), Vector3.zero);
        qte5.AddClipToLast(Transformation.Clips.Land);
        transformations.Add(qte5);
        //qte 6
        Transformation qte6 = new Transformation(new Vector3(-3.58f, -0.85f, -1), Vector3.zero);
        qte6.Add(new Vector3(-3.61f, 3.46f, -1), Vector3.zero);
        qte6.Add(new Vector3(1.05f, 3.46f, -1), new Vector3(0, 0, -800f));
        qte6.AddClipToLast(Transformation.Clips.Sweep);
        qte6.Add(new Vector3(1.05f, 3.46f, -1), new Vector3(0, 0, -180f), 200, true);
        qte6.Add(new Vector3(1.12f, -0.87f, -1), Vector3.zero);
        qte6.Add(new Vector3(1.12f, -0.87f, -1), Vector3.zero, 200, true);
        qte6.AddClipToLast(Transformation.Clips.Land);
        transformations.Add(qte6);
        //qte 7
        Transformation qte7 = new Transformation(new Vector3(-0.8f, -0.02f, -1), Vector3.zero);
        qte7.Add(new Vector3(-0.78f, 2.42f, -1), Vector3.zero);
        qte7.Add(new Vector3(1.06f, 1.85f, -1), Vector3.zero);
        qte7.Add(new Vector3(1.12f, -0.87f, -1), Vector3.zero);
        qte7.AddClipToLast(Transformation.Clips.Land);
        transformations.Add(qte7);
        //qte 8
        Transformation qte8 = new Transformation(new Vector3(1.12f, -0.88f, -1), Vector3.zero);
        qte8.Add(new Vector3(2.03f, 1.61f, -1), Vector3.zero);
        qte8.Add(new Vector3(3.59f, 0.96f, -1), Vector3.zero);
        qte8.AddClipToLast(Transformation.Clips.Land);
        transformations.Add(qte8);
        //qte 9
        Transformation qte9 = new Transformation(new Vector3(3.59f, 0.96f, -1), Vector3.zero);
        qte9.Add(new Vector3(4.56f, 3.82f, -1), Vector3.zero);
        qte9.Add(new Vector3(5.56f, 1.83f, -1), Vector3.zero);
        qte9.AddClipToLast(Transformation.Clips.Land);
        transformations.Add(qte9);
        //qte 10
        Transformation qte10 = new Transformation(new Vector3(5.56f, 1.83f, -1), Vector3.zero);
        qte10.Add(new Vector3(6.53f, 3.4f, -1), Vector3.zero);
        qte10.Add(new Vector3(7.4f, 2.9f, -1), Vector3.zero);
        qte10.AddClipToLast(Transformation.Clips.Land);
        transformations.Add(qte10);
        //qte 11
        Transformation qte11 = new Transformation(new Vector3(4.64f, -2.28f, -1), Vector3.zero);
        qte11.Add(new Vector3(4.64f, 3.52f, -1), Vector3.zero);
        qte11.Add(new Vector3(4.64f, 3.52f, -1), new Vector3(0, 0, 90f), 9, true);
        qte11.Add(new Vector3(-0.79f, 3.52f, -1), Vector3.zero);
        qte11.Add(new Vector3(-0.79f, 3.52f, -1), Vector3.zero, 9, true);
        qte11.Add(new Vector3(-0.79f, 0f, -1), Vector3.zero);
        qte11.AddClipToLast(Transformation.Clips.Land);
        transformations.Add(qte11);
        //qte 12
        Transformation qte12 = new Transformation(new Vector3(10.06f, -0.3f, -1), Vector3.zero);
        qte12.Add(new Vector3(8.17f, 2.19f, -1), Vector3.zero);
        qte12.Add(new Vector3(8.17f, 2.19f, -1), new Vector3(0, 0, -35f), 20, true);
        qte12.AddClipToLast(Transformation.Clips.Jump);
        qte12.Add(new Vector3(10.69f, 4.05f, -1), Vector3.zero);
        qte12.Add(new Vector3(10.69f, 4.05f, -1), new Vector3(0, 0, -154f), 20, true);
        qte12.AddClipToLast(Transformation.Clips.Sweep);
        qte12.Add(new Vector3(12.92f, -1.92f, -1), Vector3.zero, 15);
        qte12.Add(new Vector3(12.92f, -1.92f, -1), Vector3.zero, 200, true);
        qte12.Add(new Vector3(12.92f, -2.78f, -1), Vector3.zero);
        qte12.AddClipToLast(Transformation.Clips.Land);
        transformations.Add(qte12);
        //qte 13
        Transformation qte13 = new Transformation(new Vector3(12.92f, -2.77f, -1), Vector3.zero);
        qte13.Add(new Vector3(13.74f, 2.73f, -1), Vector3.zero);
        qte13.Add(new Vector3(13.74f, 2.73f, -1), new Vector3(0, 0, 800f), 40, true);
        qte13.AddClipToLast(Transformation.Clips.Sweep);
        qte13.Add(new Vector3(13.74f, 2.73f, -1), Vector3.zero, 40, true);
        qte13.AddClipToLast(Transformation.Clips.Sweep);
        qte13.Add(new Vector3(14.15f, -1f, -1), Vector3.zero);
        qte13.AddClipToLast(Transformation.Clips.Land);
        transformations.Add(qte13);
        //qte 14
        Transformation qte14 = new Transformation(new Vector3(15.59f, -2.27f, -1), Vector3.zero);
        qte14.Add(new Vector3(15.59f, -2.27f, -1), new Vector3(0, 0, -90f), 50, true);
        qte14.Add(new Vector3(19.03f, -2.97f, -1), Vector3.zero, 15);
        qte14.AddClipToLast(Transformation.Clips.Jump);
        qte14.Add(new Vector3(21.58f, -1.5f, -1), Vector3.zero, 15);
        qte14.AddClipToLast(Transformation.Clips.Land);
        qte14.Add(new Vector3(21.58f, -1.5f, -1), Vector3.zero, 200, true);
        qte14.Add(new Vector3(21.87f, -1.91f, -1), Vector3.zero);
        transformations.Add(qte14);
        //qte 15
        Transformation qte15 = new Transformation(new Vector3(21.93f, -2.07f, -1), Vector3.zero);
        qte15.Add(new Vector3(22.29f, 0.5f, -1), Vector3.zero);
        qte15.Add(new Vector3(22.79f, -0.98f, -1), Vector3.zero);
        qte15.Add(new Vector3(21.71f, 3.93f, -1), Vector3.zero);
        qte15.AddClipToLast(Transformation.Clips.Jump);
        qte15.Add(new Vector3(20.29f, 3.82f, -1), Vector3.zero);
        qte15.Add(new Vector3(18.46f, 3.85f, -1), Vector3.zero);
        qte15.AddClipToLast(Transformation.Clips.Jump);
        qte15.Add(new Vector3(17.99f, 2.02f, -1), Vector3.zero);
        qte15.Add(new Vector3(16.69f, 2.94f, -1), Vector3.zero);
        qte15.AddClipToLast(Transformation.Clips.Jump);
        qte15.Add(new Vector3(15.97f, 0.36f, -1), Vector3.zero);
        qte15.AddClipToLast(Transformation.Clips.Land);
        transformations.Add(qte15);

        //qte 16
        Transformation qte16 = new Transformation(new Vector3(18.94f, -2.84f, -1), Vector3.zero);
        qte16.Add(new Vector3(18.9f, -0.74f, -1), Vector3.zero);
        qte16.Add(new Vector3(19.99f, -1.52f, -1), Vector3.zero);
        qte16.AddClipToLast(Transformation.Clips.Jump);
        qte16.Add(new Vector3(20.37f, 1.07f, -1), Vector3.zero);
        qte16.Add(new Vector3(22.34f, 0.16f, -1), Vector3.zero);
        qte16.AddClipToLast(Transformation.Clips.Jump);
        qte16.Add(new Vector3(22.3f, 3.97f, -1), Vector3.zero);
        qte16.Add(new Vector3(20.44f, 3.75f, -1), Vector3.zero);
        qte16.AddClipToLast(Transformation.Clips.Land);
        transformations.Add(qte16);

        //qte 17
        Transformation qte17 = new Transformation(new Vector3(16.94f, 0.36f, -1), Vector3.zero);
        qte17.Add(new Vector3(17.16f, 1.53f, -1), Vector3.zero);
        qte17.Add(new Vector3(14.94f, 2.47f, -1), Vector3.zero);
        qte17.AddClipToLast(Transformation.Clips.Jump);
        qte17.Add(new Vector3(14.11f, -1f, -1), Vector3.zero);
        qte17.AddClipToLast(Transformation.Clips.Land);
        transformations.Add(qte17);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        //Controle par le joueur
        if (!isAnim)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            Vector2 movement = new Vector2(moveHorizontal, 0);
            rb2d.velocity = movement * speed;
            animControl.SetBool("isWalking", movement.x != 0);
            if (movement.x != 0)
            {
                sprend.flipX = movement.x >= 0;
            }
            //Debug.Log(canMove);
            if (canMove && Input.GetKeyDown(currentQTELetter)) {
                StartCoroutine(DoAnim(currentQTEId));
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Key"))
        {
            StartCoroutine(PickKey(collision.gameObject));
        }
        if (collision.CompareTag("QTE"))
        {
            canMove = true;
            currentQTELetter = collision.gameObject.transform.GetChild(0).GetComponent<TextMesh>().text.ToLower();
            Regex rx = new Regex(@"^\w*\s\((\d+)\)");
            Match m = rx.Match(collision.gameObject.name);
            currentQTEId = int.Parse(m.Groups[1].Captures[0].Value);
            //Debug.Log(currentQTELetter +" "+ currentQTEId);
        }
        if (collision.CompareTag("Finish"))
        {
            SceneManager.LoadScene("BossScene");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("QTE"))
        {
            canMove = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        /*if(!isAnim && collision.CompareTag("QTE"))
        {
            Debug.Log("callable" + collision.gameObject.transform.GetChild(0).GetComponent<TextMesh>().text.ToLower());
            if (Input.GetKeyDown(collision.gameObject.transform.GetChild(0).GetComponent<TextMesh>().text.ToLower()))
            {
                Regex rx = new Regex(@"^\w*\s\((\d+)\)");
                Match m = rx.Match(collision.gameObject.name);
                int qteID = int.Parse(m.Groups[1].Captures[0].Value);
                StartCoroutine(DoAnim(qteID));
            }
        }*/
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (!isAnim)
            {
                StartCoroutine(Death());
            }
            else
            {
                Destroy(collision.gameObject);
                audiosrc.PlayOneShot(clipHit);
            }
        }
    }

    IEnumerator Death()
    {
        isAnim = true;
        sprend.enabled = false;
        bcollid.enabled = false;
        rb2d.gravityScale = 0;
        rb2d.velocity = Vector2.zero;
        while(audioThemesrc.volume > 0.3)
        {
            audioThemesrc.volume -= 0.25f;
            yield return new WaitForSeconds(0.02f);
        }
        audiosrc.PlayOneShot(clipDeath);
        while (audioThemesrc.volume < 1)
        {
            audioThemesrc.volume += 0.05f;
            yield return new WaitForSeconds(0.04f);
        }
        audioThemesrc.volume = 1;
        yield return new WaitForSeconds(0.04f);

        sprend.enabled = true;
        bcollid.enabled = true;
        rb2d.gravityScale = 32;
        transform.position = new Vector3(-24f, -1f, 0);
        transform.rotation = Quaternion.identity;
        isAnim = false;
    }

    IEnumerator PickKey(GameObject key)
    {
        yield return new WaitForSeconds(1);
        GlobalScript.keyOwned = true;
        Destroy(key);
        audiosrc.PlayOneShot(clipKey);
    }

    IEnumerator DoAnim(int idAnim)
    {
        isAnim = true;
        rb2d.velocity = Vector2.zero;
        rb2d.gravityScale = 0;
        animControl.SetBool("isWalking", false);
        int animState = 0;
        //Debug.Log("Launch anim " + idAnim + ", contains " + transformations[idAnim].positions.Count + " steps");
        while (animState < transformations[idAnim].positions.Count)
        {
            //Debug.Log("Step " + animState);
            //Sound effect
            switch(transformations[idAnim].clips[animState])
            {
                case Transformation.Clips.Jump:
                    audiosrc.PlayOneShot(clipJump);
                    break;
                case Transformation.Clips.Hit:
                    audiosrc.PlayOneShot(clipHit);
                    break;
                case Transformation.Clips.Land:
                    audiosrc.PlayOneShot(clipLand);
                    break;
                case Transformation.Clips.Sweep:
                    audiosrc.PlayOneShot(clipSweep);
                    break;
            }
            //Physics transformations
            if (transformations[idAnim].rotationOnly[animState])
            {
                do
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, transformations[idAnim].rotations[animState], transformations[idAnim].speeds[animState] * 30 * Time.deltaTime);
                    yield return new WaitForSeconds(0.01f);
                } while (Quaternion.Angle(transformations[idAnim].rotations[animState], transform.rotation) > 0.1f);
            }
            else
            {
                do
                {
                    //Debug.Log("Anim iter pos");
                    transform.position = Vector3.MoveTowards(transform.position, transformations[idAnim].positions[animState], transformations[idAnim].speeds[animState] * 2f * Time.deltaTime);
                    if(transformations[idAnim].rotations[animState].z != 0)
                    {
                        transform.Rotate(new Vector3(0, 0, transform.rotation.eulerAngles.z + transformations[idAnim].rotations[animState].z));
                    }    
                    yield return new WaitForSeconds(0.01f);
                } while (Vector3.Distance(transformations[idAnim].positions[animState], transform.position) > 0.1f);
            }
            animState++;
        }
        isAnim = false;
        rb2d.gravityScale = 32;
    }
}
