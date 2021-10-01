using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableCardBoardStartup : MonoBehaviour
{
#if UNITY_EDITOR
    private void Awake()
    {
        Destroy(gameObject);
    }
#endif
}
