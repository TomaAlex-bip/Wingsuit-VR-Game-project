using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour
{
    [SerializeField] private float timeAlive = 1f;
    
    private void Awake()
    {
        StartCoroutine(DestroyAfterSeconds());
    }


    private IEnumerator DestroyAfterSeconds()
    {
        yield return new WaitForSeconds(timeAlive);
        Destroy(gameObject);
    }
}
