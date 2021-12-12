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
    public AudioClip clipPumpkin;

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
        Transformation qte0 = new Transformation(new Vector3(-20.69f, -3.1f, -1), Vector3.zero);
        qte0.Add(new Vector3(-20.38f, -1.84f, -1), Vector3.zero);
        qte0.Add(new Vector3(-19.8f, -2.35f, -1), Vector3.zero);
        qte0.Add(new Vector3(-18.16f, -0.58f, -1), Vector3.zero);
        transformations.Add(qte0);
        //qte 1
        Transformation qte1 = new Transformation(new Vector3(-18.11f, -1.58f, -1), Vector3.zero);
        qte1.Add(new Vector3(-18.39f, -3.12f, -1), new Vector3(0, 0, -90f));
        qte1.AddClipToLast(Transformation.Clips.Sweep);
        qte1.Add(new Vector3(-12.69f, -2.93f, -1), new Vector3(0, 0, -90f));
        qte1.AddClipToLast(Transformation.Clips.Sweep);
        qte1.Add(new Vector3(-12.83f, -0.39f, -1), new Vector3(0, 0, -180f));
        qte1.AddClipToLast(Transformation.Clips.Sweep);
        qte1.Add(new Vector3(-17.26f, -0.76f, -1), Vector3.zero);
        qte1.Add(new Vector3(-17.26f, -0.76f, -1), Vector3.zero, 100, true);
        qte1.AddClipToLast(Transformation.Clips.Land);
        transformations.Add(qte1);
        //qte 2
        Transformation qte2 = new Transformation(new Vector3(-15.84f, -0.6f, -1), Vector3.zero);
        qte2.Add(new Vector3(-15.84f, -0.6f, -1), new Vector3(0, 0, -23.4f), 40, true);
        qte2.AddClipToLast(Transformation.Clips.Sweep);
        qte2.Add(new Vector3(-14.89f, 3.11f, -1), Vector3.zero);
        qte2.Add(new Vector3(-14.89f, 3.11f, -1), new Vector3(0, 0, -138f), 40, true);
        qte2.AddClipToLast(Transformation.Clips.Sweep);
        qte2.Add(new Vector3(-11.3f, -1.84f, -1), Vector3.zero);
        qte2.Add(new Vector3(-11f, -1.66f, -1), Vector3.zero);
        qte2.Add(new Vector3(-11f, -1.66f, -1), Vector3.zero, 100, true);
        transformations.Add(qte2);
        //qte 3
        Transformation qte3 = new Transformation(new Vector3(-10.92f, -2.12f, -1), Vector3.zero);
        qte3.Add(new Vector3(-9.48f, -1.33f, -1), Vector3.zero);
        qte3.Add(new Vector3(-9.08f, 0.21f, -1), Vector3.zero);
        qte3.Add(new Vector3(-8.22f, -0.32f, -1), Vector3.zero);
        qte3.AddClipToLast(Transformation.Clips.Land);
        transformations.Add(qte3);
        //qte 4
        Transformation qte4 = new Transformation(new Vector3(-14.07f, -0.7f, -1), Vector3.zero);
        qte4.Add(new Vector3(-12.94f, 1.64f, -1), Vector3.zero);
        qte4.Add(new Vector3(-11f, 1f, -1), Vector3.zero);
        qte4.AddClipToLast(Transformation.Clips.Land);
        transformations.Add(qte4);
        //qte 5
        Transformation qte5 = new Transformation(new Vector3(-10.88f, 1f, -1), Vector3.zero);
        qte5.Add(new Vector3(-9.14f, 1.76f, -1), new Vector3(0, 0, 800f));
        qte5.Add(new Vector3(-8.21f, -0.26f, -1), new Vector3(0, 0, -800f));
        qte5.Add(new Vector3(-8.21f, -0.26f, -1), Vector3.zero, 200, true);
        qte5.AddClipToLast(Transformation.Clips.Land);
        transformations.Add(qte5);
        //qte 6
        Transformation qte6 = new Transformation(new Vector3(-3.22f, -0.94f, -1), Vector3.zero);
        qte6.Add(new Vector3(-3.38f, 3.67f, -1), Vector3.zero);
        qte6.AddClipToLast(Transformation.Clips.Sweep);
        qte6.Add(new Vector3(-22.38f, 3.74f, -1), new Vector3(0, 0, -800f));
        qte6.AddClipToLast(Transformation.Clips.Sweep);
        qte6.Add(new Vector3(-22.38f, 3.74f, -1), Vector3.zero, 200, true);
        transformations.Add(qte6);
        //qte 7
        Transformation qte7 = new Transformation(new Vector3(-6.05f, -2.41f, -1), Vector3.zero);
        qte7.Add(new Vector3(-3.26f, 3.21f, -1), Vector3.zero);
        qte7.Add(new Vector3(-0.93f, 3.98f, -1), Vector3.zero);
        transformations.Add(qte7);
        //qte 8
        Transformation qte8 = new Transformation(new Vector3(-3.86f, -3.16f, -1), Vector3.zero);
        qte8.Add(new Vector3(-4.8f, -3.1f, -1), Vector3.zero);
        qte8.Add(new Vector3(-4.8f, -3.1f, -1), new Vector3(0, 0, -87), 50, true);
        qte8.AddClipToLast(Transformation.Clips.Sweep);
        qte8.Add(new Vector3(1.67f, -3.1f, -1), Vector3.zero);
        qte8.Add(new Vector3(1.67f, -3.6f, -1), Vector3.zero, 50, true);
        transformations.Add(qte8);
        //qte 9
        Transformation qte9 = new Transformation(new Vector3(2.11f, -3.16f, -1), Vector3.zero);
        qte9.Add(new Vector3(2.55f, -0.74f, -1), Vector3.zero);
        qte9.Add(new Vector3(4.2f, -1.22f, -1), Vector3.zero);
        qte9.Add(new Vector3(4.64f, -3.1f, -1), Vector3.zero);
        qte9.AddClipToLast(Transformation.Clips.Land);
        transformations.Add(qte9);
        //qte 10
        Transformation qte10 = new Transformation(new Vector3(-0.88f, 3f, -1), Vector3.zero);
        qte10.Add(new Vector3(1.45f, 0.94f, -1), new Vector3(0, 0, 800f));
        qte10.AddClipToLast(Transformation.Clips.Sweep);
        qte10.Add(new Vector3(-1.28f, -0.68f, -1), new Vector3(0, 0, -800f));
        qte10.AddClipToLast(Transformation.Clips.Sweep);
        qte10.Add(new Vector3(-2.24f, 2.09f, -1), new Vector3(0, 0, 800f));
        qte10.AddClipToLast(Transformation.Clips.Sweep);
        qte10.Add(new Vector3(-2.24f, 2.09f, -1), new Vector3(0, 0, -800f));
        qte10.AddClipToLast(Transformation.Clips.Sweep);
        qte10.Add(new Vector3(-0.51f, -0.68f, -1), Vector3.zero, 200, true);
        qte10.AddClipToLast(Transformation.Clips.Land);
        transformations.Add(qte10);
        //qte 11
        Transformation qte11 = new Transformation(new Vector3(6.28f, -3.13f, -1), Vector3.zero);
        qte11.Add(new Vector3(6.73f, -2.06f, -1), Vector3.zero);
        qte11.Add(new Vector3(7.42f, -2.5f, -1), Vector3.zero);
        qte11.AddClipToLast(Transformation.Clips.Land);
        transformations.Add(qte11);
        //qte 12
        Transformation qte12 = new Transformation(new Vector3(11.6f, -2.52f, -1), Vector3.zero);
        qte12.Add(new Vector3(11.84f, -0.54f, -1), Vector3.zero, 15);
        qte12.Add(new Vector3(12.9f, -1.14f, -1), Vector3.zero, 15);
        qte12.AddClipToLast(Transformation.Clips.Land);
        transformations.Add(qte12);
        //qte 13
        Transformation qte13 = new Transformation(new Vector3(12.9f, -1.14f, -1), Vector3.zero);
        qte13.Add(new Vector3(10.84f, 1.3f, -1), Vector3.zero, 15);
        qte13.Add(new Vector3(7.42f, 1.2f, -1), Vector3.zero, 15);
        qte13.AddClipToLast(Transformation.Clips.Land);
        transformations.Add(qte13);
        //qte 14
        Transformation qte14 = new Transformation(new Vector3(7.42f, 1.2f, -1), Vector3.zero);
        qte14.Add(new Vector3(10.76f, 3.16f, -1), Vector3.zero, 15);
        qte14.Add(new Vector3(12.8f, 2.69f, -1), Vector3.zero, 15);
        qte14.AddClipToLast(Transformation.Clips.Land);
        transformations.Add(qte14);
        //qte 15
        Transformation qte15 = new Transformation(new Vector3(18.24f, -2.5f, -1), Vector3.zero);
        qte15.Add(new Vector3(18.76f, -0.24f, -1), Vector3.zero);
        qte15.Add(new Vector3(19.31f, -1.13f, -1), Vector3.zero);
        qte15.AddClipToLast(Transformation.Clips.Land);

        transformations.Add(qte15);
        //qte 16
        Transformation qte16 = new Transformation(new Vector3(19.58f, -1.15f, -1), Vector3.zero);
        qte16.Add(new Vector3(21.89f, -0.5f, -1), Vector3.zero);
        qte16.Add(new Vector3(22.18f, -2.5f, -1), Vector3.zero);
        qte16.AddClipToLast(Transformation.Clips.Land);

        transformations.Add(qte16);
        //qte 17
        Transformation qte17 = new Transformation(new Vector3(23f, -2.5f, -1), Vector3.zero);
        qte17.Add(new Vector3(22.31f, 1.88f, -1), Vector3.zero);
        qte17.Add(new Vector3(19.4f, 3f, -1), Vector3.zero);
        qte17.Add(new Vector3(17f, 0.5f, -1), Vector3.zero);
        qte17.AddClipToLast(Transformation.Clips.Land);

        transformations.Add(qte17);
        //qte 18
        Transformation qte18 = new Transformation(new Vector3(16.7f, 0.46f, -1), Vector3.zero);
        qte18.Add(new Vector3(20.53f, 3f, -1), new Vector3(0, 0, 800f));
        qte18.AddClipToLast(Transformation.Clips.Sweep);
        qte18.Add(new Vector3(22.68f, 1.26f, -1), new Vector3(0, 0, -800f));
        qte18.AddClipToLast(Transformation.Clips.Sweep);
        qte18.Add(new Vector3(20.64f, -0.64f, -1), new Vector3(0, 0, 800f));
        qte18.AddClipToLast(Transformation.Clips.Sweep);
        qte18.Add(new Vector3(18.51f, 1.11f, -1), new Vector3(0, 0, -800f));
        qte18.AddClipToLast(Transformation.Clips.Sweep);
        qte18.Add(new Vector3(20.53f, 3f, -1), new Vector3(0, 0, 800f));
        qte18.AddClipToLast(Transformation.Clips.Sweep);
        qte18.Add(new Vector3(23f, 3f, -1), Vector3.zero);
        qte18.Add(new Vector3(23f, 3f, -1), Vector3.zero, 50, true);
        qte18.Add(new Vector3(24.05f, 1.64f, -1), Vector3.zero);
        qte18.AddClipToLast(Transformation.Clips.Land);
        transformations.Add(qte18);
        //qte 19
        Transformation qte19 = new Transformation(new Vector3(20.58f, 2.29f, -1), Vector3.zero);
        qte19.Add(new Vector3(19.46f, 2.72f, -1), Vector3.zero);
        qte19.Add(new Vector3(19.05f, -1.15f, -1), Vector3.zero);
        qte19.Add(new Vector3(20.49f, -1.15f, -1), Vector3.zero);
        qte19.AddClipToLast(Transformation.Clips.Sweep);
        qte19.Add(new Vector3(21.59f, -0.5f, -1), Vector3.zero);
        qte19.Add(new Vector3(22.41f, -2.53f, -1), Vector3.zero);
        qte19.AddClipToLast(Transformation.Clips.Sweep);
        qte19.Add(new Vector3(23.14f, -0.95f, -1), Vector3.zero);
        qte19.Add(new Vector3(24.18f, -1.1f, -1), Vector3.zero);
        qte19.AddClipToLast(Transformation.Clips.Land);

        transformations.Add(qte19);
        //qte 20
        Transformation qte20 = new Transformation(new Vector3(24.06f, 1.59f, -1), Vector3.zero);
        qte20.Add(new Vector3(22.06f, 3.03f, -1), Vector3.zero);
        qte20.Add(new Vector3(20.58f, 2.29f, -1), Vector3.zero);
        qte20.AddClipToLast(Transformation.Clips.Land);
        transformations.Add(qte20);
        //qte 21
        Transformation qte21 = new Transformation(new Vector3(-22.18f, 2.8f, -1), Vector3.zero);
        qte21.Add(new Vector3(3.93f, 3.94f, -1), new Vector3(0, 0, 800f));
        qte21.AddClipToLast(Transformation.Clips.Sweep);
        qte21.Add(new Vector3(3.93f, 3.94f, -1), Vector3.zero, 200, true);
        transformations.Add(qte21);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void FixedUpdate()
    {
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
            if (canMove && Input.GetKeyDown(currentQTELetter)) {
                StartCoroutine(DoAnim(currentQTEId));
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pumpkin"))
        {
            StartCoroutine(PickPumpkin(collision.gameObject));
        }
        if (collision.CompareTag("QTE"))
        {
            canMove = true;
            currentQTELetter = collision.gameObject.transform.GetChild(0).GetComponent<TextMesh>().text.ToLower();
            Regex rx = new Regex(@"^\w*\s\((\d+)\)");
            Match m = rx.Match(collision.gameObject.name);
            currentQTEId = int.Parse(m.Groups[1].Captures[0].Value);
            Debug.Log(currentQTELetter +" "+ currentQTEId);
        }
        if (collision.CompareTag("Finish"))
        {
            SceneManager.LoadScene("ChoiceScreen");
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
        transform.position = new Vector3(-23.75f, -1.59f, 0);
        transform.rotation = Quaternion.identity;
        isAnim = false;
    }

    IEnumerator PickPumpkin(GameObject pumpkin)
    {
        yield return new WaitForSeconds(1);
        GlobalScript.pumpkinOwned = true;
        Destroy(pumpkin);
        audiosrc.PlayOneShot(clipPumpkin);
    }

    IEnumerator DoAnim(int idAnim)
    {
        isAnim = true;
        rb2d.velocity = Vector2.zero;
        rb2d.gravityScale = 0;
        animControl.SetBool("isWalking", false);
        int animState = 0;
        Debug.Log("Launch anim " + idAnim + ", contains " + transformations[idAnim].positions.Count + " steps");
        while (animState < transformations[idAnim].positions.Count)
        {
            Debug.Log("Step " + animState);
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
                    transform.position = Vector3.MoveTowards(transform.position, transformations[idAnim].positions[animState], transformations[idAnim].speeds[animState] * Time.deltaTime);
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
