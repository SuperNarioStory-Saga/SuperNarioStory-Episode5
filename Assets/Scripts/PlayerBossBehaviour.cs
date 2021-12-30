using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBossBehaviour : MonoBehaviour
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
        Transformation qte0 = new Transformation(new Vector3(-3.22f, -3.32f, -1), Vector3.zero);
        qte0.Add(new Vector3(-2.05f, 0.9f, -1f), Vector3.zero);
        qte0.Add(new Vector3(0f, 3f, -1f), Vector3.zero);
        transformations.Add(qte0);
        //qte 1
        Transformation qte1 = new Transformation(new Vector3(0f, 0.32f, -1), Vector3.zero);
        qte1.Add(new Vector3(1.87f, 3.23f, -1f), Vector3.zero);
        qte1.Add(new Vector3(4.64f, 4.32f, -1f), Vector3.zero);
        transformations.Add(qte1);
        //qte 2
        Transformation qte2 = new Transformation(new Vector3(4.6f, 2.86f, -1), Vector3.zero);
        qte2.Add(new Vector3(5.63f, 0.25f, - 1f), new Vector3(0, 0, 500f));
        qte2.Add(new Vector3(5.63f, 0.25f, -1f), Vector3.zero, 200, true);
        qte2.Add(new Vector3(-8.05f, -3.3f, -1f), Vector3.zero, 20);
        transformations.Add(qte2);
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
            if (canMove && Input.GetKeyDown(currentQTELetter))
            {
                StartCoroutine(DoAnim(currentQTEId));
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("QTE"))
        {
            canMove = true;
            currentQTELetter = collision.gameObject.transform.GetChild(0).GetComponent<TextMesh>().text.ToLower();
            Regex rx = new Regex(@"^\w*\s\((\d+)\)");
            Match m = rx.Match(collision.gameObject.name);
            currentQTEId = int.Parse(m.Groups[1].Captures[0].Value);
            //Debug.Log(currentQTELetter + " " + currentQTEId);
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
                if (collision.gameObject.name == "Bowsy")
                {
                    audiosrc.PlayOneShot(clipHit);
                } else
                {
                    Destroy(collision.gameObject);
                    audiosrc.PlayOneShot(clipHit);
                }
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
        while (audioThemesrc.volume > 0.3)
        {
            audioThemesrc.volume -= 0.25f;
            yield return new WaitForSeconds(0.02f);
        }
        audiosrc.PlayOneShot(clipDeath);
        while (audioThemesrc.volume < 0.5f)
        {
            audioThemesrc.volume += 0.05f;
            yield return new WaitForSeconds(0.04f);
        }
        audioThemesrc.volume = 0.5f;
        yield return new WaitForSeconds(0.04f);

        sprend.enabled = true;
        bcollid.enabled = true;
        rb2d.gravityScale = 32;
        transform.position = new Vector3(-7.35f, -3.2f, 0);
        transform.rotation = Quaternion.identity;
        isAnim = false;
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
            switch (transformations[idAnim].clips[animState])
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
                    if (transformations[idAnim].rotations[animState].z != 0)
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
