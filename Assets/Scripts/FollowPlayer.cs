using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Transform playerTransform;
    [SerializeField] private Vector3 offset;

    private void Awake()
    {
        playerTransform = FindObjectOfType<PlayerController>().gameObject.transform;
    }

    private void LateUpdate()
    {
        transform.position = playerTransform.position + offset;
    }
}
