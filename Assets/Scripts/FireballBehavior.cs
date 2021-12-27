using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballBehavior : MonoBehaviour
{
    public float? lowerLimit = -3.65f;
    public float? upperLimit = null;
    public float? xlimit = -10f;

    private Rigidbody2D rb2d;
    private bool hitGround = false;


    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (!hitGround)
        {
            transform.position += transform.right * Time.deltaTime * 8.5f;
            if ((upperLimit != null && transform.position.y > upperLimit) || (lowerLimit != null && transform.position.y < lowerLimit) || (xlimit != null && transform.position.x < xlimit))
            {
                hitGround = true;
                StartCoroutine(Shrink());
            }
        }
    }

    IEnumerator Shrink()
    {
        Vector3 initTransform = transform.localScale;
        float fullSize = 1f;
        while (fullSize > 0.2f)
        {
            fullSize -= 0.13f;
            transform.localScale = initTransform * fullSize;
            yield return new WaitForSeconds(0.05f);
        }
        Destroy(gameObject);
    }
}
