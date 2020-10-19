using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    private Vector2 moveDir;
    [SerializeField] private float moveSpeed;
    private Rigidbody2D rb2D;
    private Animator animator;

    [SerializeField] private int maxNumberOfFoodBowls;
    [SerializeField] private int currentNumberOfFoodBowls;

    [SerializeField] private int maxNumberOfWaterBowls;
    [SerializeField] private int currentNumberOfWaterBowls;

    [SerializeField] private int maxNumberOfLitterTrays;
    [SerializeField] private int currentNumberOfLitterTrays;

    [SerializeField] private GameObject foodBowl;
    
    [SerializeField] private GameObject waterBowl;
    
    [SerializeField] private GameObject litterTray;
    
    [SerializeField] private bool isNearPoop;
    [SerializeField] private bool isNearLitterTray;
    private GameObject poopToClean;
    private LitterTray litterTrayToClean;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        MovePlayer(moveDir);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Poop")
        {
            isNearPoop = true;
            poopToClean = collision.gameObject;
        }

        if(collision.gameObject.tag == "Litter")
        {
            isNearLitterTray = true;
            litterTrayToClean = collision.gameObject.GetComponent<LitterTray>();   
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Poop")
        {
            isNearPoop = false;
            poopToClean = null;
        }

        if (collision.gameObject.tag == "Litter")
        {
            isNearLitterTray = false;
            litterTrayToClean = null;
        }
    }

    private void MovePlayer(Vector2 moveDir)
    {
        rb2D.MovePosition(rb2D.position + moveDir * moveSpeed * Time.fixedDeltaTime);
    }

    private void HandleInput()
    {
        moveDir.x = Input.GetAxisRaw("Horizontal");
        moveDir.y = Input.GetAxisRaw("Vertical");
        animator.SetFloat("horizontal_input", moveDir.x);
        animator.SetFloat("vertical_input", moveDir.y);

        // Place food bowl when J key is pressed
        if (Input.GetKeyUp(KeyCode.J))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsTag("idleFront"))
            {
                Vector2 position = rb2D.position + new Vector2(0, -1);
                PlaceFoodBowl(position);
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsTag("idleBack"))
            {
                Vector2 position = rb2D.position + new Vector2(0, 1);
                PlaceFoodBowl(position);
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsTag("idleRight"))
            {
                Vector2 position = rb2D.position + new Vector2(1, 0);
                PlaceFoodBowl(position);
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsTag("idleLeft"))
            {
                Vector2 position = rb2D.position + new Vector2(-1, 0);
                PlaceFoodBowl(position);
            }
        }

        // Place water bowl when K key is pressed
        if (Input.GetKeyUp(KeyCode.K))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsTag("idleFront"))
            {
                Vector2 position = rb2D.position + new Vector2(0, -1);
                PlaceWaterBowl(position);
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsTag("idleBack"))
            {
                Vector2 position = rb2D.position + new Vector2(0, 1);
                PlaceWaterBowl(position);
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsTag("idleRight"))
            {
                Vector2 position = rb2D.position + new Vector2(1, 0);
                PlaceWaterBowl(position);
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsTag("idleLeft"))
            {
                Vector2 position = rb2D.position + new Vector2(-1, 0);
                PlaceWaterBowl(position);
            }
        }

        // Place litter tray when L key is pressed
        if (Input.GetKeyUp(KeyCode.L))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsTag("idleFront"))
            {
                Vector2 position = rb2D.position + new Vector2(0, -1);
                PlaceLitterTray(position);
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsTag("idleBack"))
            {
                Vector2 position = rb2D.position + new Vector2(0, 1);
                PlaceLitterTray(position);
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsTag("idleRight"))
            {
                Vector2 position = rb2D.position + new Vector2(1, 0);
                PlaceLitterTray(position);
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsTag("idleLeft"))
            {
                Vector2 position = rb2D.position + new Vector2(-1, 0);
                PlaceLitterTray(position);
            }
        }

        // Clean poop when U key is pressed
        if (Input.GetKeyUp(KeyCode.U))
        {
            CleanPoop();
        }

        //Clean litter tray when O key is pressed
        if (Input.GetKeyUp(KeyCode.O))
        {
            CleanLitterTray();
        }

        // Destroy litter tray when I key is pressed
        if (Input.GetKeyUp(KeyCode.I))
        {
            DestroyLitterTray();
        }
    }

    private void PlaceFoodBowl(Vector2 pos)
    {
        if(currentNumberOfFoodBowls < maxNumberOfFoodBowls)
        {
            Instantiate(foodBowl, pos, Quaternion.identity);
            currentNumberOfFoodBowls++;
        }
        
    }

    private void PlaceWaterBowl(Vector2 pos)
    {
        if(currentNumberOfWaterBowls < maxNumberOfWaterBowls)
        {
            Instantiate(waterBowl, pos, Quaternion.identity);
            currentNumberOfWaterBowls++;
        }
        
    }

    private void PlaceLitterTray(Vector2 pos)
    {
        if(currentNumberOfLitterTrays < maxNumberOfLitterTrays)
        {
            Instantiate(litterTray, pos, Quaternion.identity);
            currentNumberOfLitterTrays++;
        }
        
    }

    private void CleanPoop()
    {
        if(poopToClean != null && isNearPoop)
        {
            Destroy(poopToClean);
        }
    }

    private void CleanLitterTray()
    {
        if(litterTrayToClean != null && !litterTrayToClean.IsClean())
        {
            litterTrayToClean.CleanTray();
        }
    }

    private void DestroyLitterTray()
    {
        if (isNearLitterTray)
        {
            currentNumberOfLitterTrays--;
            Destroy(litterTrayToClean.gameObject);
        }
        
    }

    public void MinusOneFoodBowl()
    {
        if(currentNumberOfFoodBowls > 0)
        {
            currentNumberOfFoodBowls--;
        }
    }

    public void MinusOneWaterBowl()
    {
        if (currentNumberOfWaterBowls > 0)
        {
            currentNumberOfWaterBowls--;
        }
    }

}
