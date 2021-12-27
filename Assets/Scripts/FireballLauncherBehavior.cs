using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballLauncherBehavior : MonoBehaviour
{
    public GameObject fireball;
    public AudioClip fireballClip;
    public float gap = 0f;

    private AudioSource audsrc;
    // Start is called before the first frame update
    void Start()
    {
        audsrc = transform.GetComponent<AudioSource>();
        StartCoroutine(Shoot());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(gap);
        while (true)
        {
            var fireb = Instantiate(fireball, transform.position, Quaternion.AngleAxis(90f, Vector3.forward));
            fireb.GetComponent<FireballBehavior>().lowerLimit = null;
            fireb.GetComponent<FireballBehavior>().xlimit = null;
            fireb.GetComponent<FireballBehavior>().upperLimit = 10f;
            if (fireballClip != null)
            {
                audsrc.PlayOneShot(fireballClip);
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
