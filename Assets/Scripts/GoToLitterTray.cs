using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToLitterTray : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private Rigidbody2D rb2D;

    [SerializeField] private Vector2 targetPos;
    [SerializeField] private float delta;

    [SerializeField] private LitterTray[] litterTrays;

    private void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        StartCoroutine(LookForLitterTrays());

    }

    public void GoToLitter()
    {
        if (litterTrays != null)
        {
            foreach (LitterTray litterTray in litterTrays)
            {
                if(litterTray != null)
                {
                    targetPos = litterTray.transform.position;
                    Vector2 moveDir = targetPos - rb2D.position;
                    rb2D.MovePosition(rb2D.position + moveDir.normalized * moveSpeed * Time.fixedDeltaTime);
                    if (ReachedTarget())
                    {
                        rb2D.MovePosition(rb2D.position);
                    }
                }
                    
            }
        }

    }

    private bool ReachedTarget()
    {
        if (Vector2.Distance(targetPos, rb2D.position) < delta)
        {
            return true;
        }
        return false;
    }

    private IEnumerator LookForLitterTrays()
    {
        while (true)
        {
            litterTrays = FindObjectsOfType<LitterTray>();
            yield return new WaitForSeconds(1f);
        }
    }

    public LitterTray GetLitterTray()
    {
        return litterTrays[0];
    }
}
