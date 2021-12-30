using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossScript : MonoBehaviour
{
    public Sprite crossharmSprite;
    public Sprite attackSprite;
    public GameObject fireball;
    public AudioClip fireballClip;
    public AudioClip victoryClip;
    public GameObject qte1;
    public GameObject qte2;
    public GameObject qte3;

    private AudioSource audsrc;
    private SpriteRenderer spr;
    private Vector3 startPos;
    private float speed = 12f;
    private int stage = 1;
    private List<Coroutine> attacks;
    private bool invu;

    private readonly string st = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private void randomizeQTE()
    {
        qte1.transform.GetChild(0).GetComponent<TextMesh>().text = st[Random.Range(0, st.Length)].ToString();
        qte2.transform.GetChild(0).GetComponent<TextMesh>().text = st[Random.Range(0, st.Length)].ToString();
        qte3.transform.GetChild(0).GetComponent<TextMesh>().text = st[Random.Range(0, st.Length)].ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        audsrc = GetComponent<AudioSource>();
        spr = GetComponent<SpriteRenderer>();
        startPos = transform.position;
        attacks = new List<Coroutine>();
        attacks.Add(StartCoroutine(Part1()));
        invu = false;
        randomizeQTE();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void switchStage()
    {
        transform.position = new Vector3(6.9f, -2.09f, 0);
        spr.sprite = crossharmSprite;
        foreach (Coroutine attack in attacks)
        {
            StopCoroutine(attack);
        }
        attacks.Clear();
        if (stage == 2)
        {
            attacks.Add(StartCoroutine(Part2()));
        } else if (stage == 3)
        {
            attacks.Add(StartCoroutine(Part2()));
            attacks.Add(StartCoroutine(FireballLaunch()));
        } else if (stage == 4)
        {
            StartCoroutine(Win());
        }
        randomizeQTE();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<PlayerBossBehaviour>().isAnim && !invu)
            {
                stage++;
                invu = true;
                StartCoroutine(Invu());
                switchStage();
            }
        }
    }

    IEnumerator Part1()
    {
        yield return new WaitForSeconds(2.5f);
        var intimidate = true;
        var intimidateStep = 0;
        var attack = false;
        var attackStep = 0;
        var attackCooldown = 0;
        var target = new Vector3(startPos.x - 3f, startPos.y, startPos.z);
        while(true) {
            if (intimidate)
            {
                while (Mathf.Abs(transform.position.x - target.x) > 0.1) {
                    transform.position = Vector3.MoveTowards(transform.position, target, speed*1.7f*Time.deltaTime);
                    yield return new WaitForSeconds(0.05f);
                }
                intimidateStep++;
                if (intimidateStep == 1 || intimidateStep == 3)
                {
                    target = startPos;
                } else if (intimidateStep == 2)
                {
                    target = new Vector3(startPos.x - 3f, startPos.y, startPos.z);
                } else if (intimidateStep == 4)
                {
                    intimidate = false;
                    intimidateStep = 0;
                    attack = true;
                    target = new Vector3(startPos.x, startPos.y + 5f, startPos.z);
                    yield return new WaitForSeconds(1.5f);
                }
                yield return new WaitForSeconds(0.2f);
            }
            if (attack)
            {
                spr.sprite = attackSprite;
                while (Mathf.Abs(transform.position.y - target.y) > 0.1)
                {
                    if (attackCooldown <= 0 && transform.position.y > -1.4f)
                    {
                        Instantiate(fireball, transform.position, Quaternion.AngleAxis(180f, Vector3.back));
                        attackCooldown = 10;
                        audsrc.PlayOneShot(fireballClip);
                    } else
                    {
                        attackCooldown--;
                    }
                    transform.position = Vector3.MoveTowards(transform.position, target, speed * 4 * Time.deltaTime);
                    yield return new WaitForSeconds(0.05f);
                }
                attackStep++;
                if (attackStep == 1)
                {
                    target = startPos;
                }
                else if (attackStep == 2)
                {
                    spr.sprite = crossharmSprite;
                    attack = false;
                    attackStep = 0;
                    intimidate = true;
                    target = new Vector3(startPos.x - 3f, startPos.y, startPos.z);
                    yield return new WaitForSeconds(5f);
                }
                yield return new WaitForSeconds(0.02f);
            }
        }
    }

    IEnumerator Part2()
    {
        yield return new WaitForSeconds(1.5f);
        speed *= 1.2f;
        var intimidate = true;
        var intimidateStep = 0;
        var attack = false;
        var attackStep = 0;
        var attackCooldown = 0;
        var target = new Vector3(startPos.x - 3f, startPos.y, startPos.z);
        while (true)
        {
            if (intimidate)
            {
                while (Mathf.Abs(transform.position.x - target.x) > 0.1)
                {
                    transform.position = Vector3.MoveTowards(transform.position, target, speed * 3 * Time.deltaTime);
                    yield return new WaitForSeconds(0.05f);
                }
                intimidateStep++;
                if (intimidateStep == 1 || intimidateStep == 3)
                {
                    target = startPos;
                }
                else if (intimidateStep == 2)
                {
                    target = new Vector3(startPos.x - 3f, startPos.y, startPos.z);
                }
                else if (intimidateStep == 4)
                {
                    intimidate = false;
                    intimidateStep = 0;
                    attack = true;
                    target = new Vector3(startPos.x, startPos.y + 5f, startPos.z);
                    yield return new WaitForSeconds(1f);
                }
                yield return new WaitForSeconds(0.2f);
            }
            if (attack)
            {
                spr.sprite = attackSprite;
                while (Mathf.Abs(transform.position.y - target.y) > 0.1)
                {
                    if (attackCooldown <= 0)
                    {
                        if (transform.position.y > -1.4f)
                        {
                            Instantiate(fireball, transform.position, Quaternion.AngleAxis(180f, Vector3.back));
                            attackCooldown = 3;
                            audsrc.PlayOneShot(fireballClip);
                        }
                    }
                    else
                    {
                        attackCooldown--;
                    }
                    transform.position = Vector3.MoveTowards(transform.position, target, speed * 4 * Time.deltaTime);
                    yield return new WaitForSeconds(0.05f);
                }
                attackStep++;
                if (attackStep == 1)
                {
                    target = startPos;
                }
                else if (attackStep == 2)
                {
                    spr.sprite = crossharmSprite;
                    attack = false;
                    attackStep = 0;
                    intimidate = true;
                    target = new Vector3(startPos.x - 3f, startPos.y, startPos.z);
                    yield return new WaitForSeconds(2.5f);
                }
                yield return new WaitForSeconds(0.02f);
            }
        }
    }

    IEnumerator FireballLaunch()
    {
        while (true)
        {
            float cooldown = Random.Range(0.2f, 1.3f);
            Instantiate(fireball, new Vector3(Random.Range(-13f, 3f), 6.38f, 0), Quaternion.AngleAxis(-57f, Vector3.forward));
            yield return new WaitForSeconds(cooldown);
        }
    }

    IEnumerator Invu()
    {
        var i = 0;
        while (i < 10)
        {
            i++;
            spr.color = new Vector4(1, 1, 1, i % 2 == 0 ? 1 : 0);
            yield return new WaitForSeconds(0.1f);
        }
        spr.color = new Vector4(1, 1, 1, 1);
        invu = false;
    }

    IEnumerator Win()
    {
        var i = 0;
        var baseColor = spr.color;
        while (i < 10)
        {
            i++;
            spr.color = new Vector4(1, 1, 1, i % 2 == 0 ? 1 : 0);
            yield return new WaitForSeconds(0.2f);
        }
        spr.color = new Vector4(1, 1, 1, 0);
        audsrc.PlayOneShot(victoryClip);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("ChoiceScene");
    }
}
