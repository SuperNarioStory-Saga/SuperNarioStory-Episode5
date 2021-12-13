using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(JustVibing());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator JustVibing()
    {
        float upLimit = transform.position.y + 0.25f;
        float downLimit = transform.position.y;
        bool up = true;
        while (true)
        {
            if (up)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.01f, transform.position.z);
                if (transform.position.y > upLimit)
                {
                    up = !up;
                }
            } else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - 0.01f, transform.position.z);
                if (transform.position.y < downLimit)
                {
                    up = !up;
                }
            }
            yield return new WaitForSeconds(0.05f);
        }
    }
}
