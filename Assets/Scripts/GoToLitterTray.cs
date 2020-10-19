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

    [SerializeField] private bool goToLitter;

    private void Awake()
    {
        goToLitter = false;
    }

    private void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        targetPos = GetRandomLitterTrayPosition();
    }

    private void Update()
    {
        if (ReachedTarget())
        {
            targetPos = GetRandomLitterTrayPosition();
        }
    }

    private void FixedUpdate()
    {
        if (goToLitter)
        {
            GoToLitter(targetPos);
        }
    }
    private Vector2 GetRandomLitterTrayPosition()
    {
        LookForLitterTrays();
        int randomIndex = Random.Range(0, litterTrays.Length);
        if (litterTrays[randomIndex] != null)
        {
            return litterTrays[randomIndex].transform.position;
        }
        return Vector2.zero;
    }
    public void SetGoToLitter()
    {
        goToLitter = true;
    }
    
    private void GoToLitter(Vector2 targetPos)
    {    
        Vector2 moveDir = targetPos - rb2D.position;
        rb2D.MovePosition(rb2D.position + moveDir.normalized * moveSpeed * Time.fixedDeltaTime);
        if (ReachedTarget())
        {
            rb2D.MovePosition(rb2D.position);
            goToLitter = false;
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

    private void LookForLitterTrays()
    {
        litterTrays = FindObjectsOfType<LitterTray>();
   
    }
    
    public LitterTray GetLitterTray()
    {
        return FindObjectOfType<LitterTray>();
    }
}
