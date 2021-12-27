using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    public Sprite crossharmSprite;
    public Sprite attackSprite;
    public GameObject fireball;
    public AudioClip fireballClip;

    private AudioSource audsrc;
    private SpriteRenderer spr;
    private Vector3 startPos;
    private float speed = 12f;

    // Start is called before the first frame update
    void Start()
    {
        audsrc = GetComponent<AudioSource>();
        spr = GetComponent<SpriteRenderer>();
        startPos = transform.position;
        StartCoroutine(Part2());
        StartCoroutine(FireballLaunch());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Part1()
    {
        yield return new WaitForSeconds(5f);
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
                    transform.position = Vector3.MoveTowards(transform.position, target, speed*Time.deltaTime);
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
                    yield return new WaitForSeconds(3f);
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
        yield return new WaitForSeconds(5f);
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
                    transform.position = Vector3.MoveTowards(transform.position, target, speed * 2 * Time.deltaTime);
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
                    yield return new WaitForSeconds(1.5f);
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
                            attackCooldown = 6;
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
            float cooldown = Random.Range(0.5f, 2f);
            Instantiate(fireball, new Vector3(Random.Range(-13f, 3f), 6.38f, 0), Quaternion.AngleAxis(-57f, Vector3.forward));
            yield return new WaitForSeconds(cooldown);
        }
    }
}
