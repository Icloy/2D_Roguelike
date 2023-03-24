using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnewayPlat : MonoBehaviour
{
    private GameObject currentOneWayPlatform;

    [SerializeField] private CapsuleCollider2D playercollier;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            if(currentOneWayPlatform != null)
            {
                StartCoroutine(DisableCollision());
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = null;
        }
    }

    private IEnumerator DisableCollision()
    {
        Collider2D platformcollider = currentOneWayPlatform.GetComponent<Collider2D>();
        CompositeCollider2D platformcollider2 = currentOneWayPlatform.GetComponent<CompositeCollider2D>();

        Physics2D.IgnoreCollision(playercollier, platformcollider);
        Physics2D.IgnoreCollision(playercollier, platformcollider2);
        yield return new WaitForSeconds(0.25f);
        Physics2D.IgnoreCollision(playercollier, platformcollider, false);
        Physics2D.IgnoreCollision(playercollier, platformcollider2, false);

    }
}
