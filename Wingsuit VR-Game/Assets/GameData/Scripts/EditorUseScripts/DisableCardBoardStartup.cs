using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableCardBoardStartup : MonoBehaviour
{
    private void Awake()
    {
        Destroy(gameObject);
    }
}
