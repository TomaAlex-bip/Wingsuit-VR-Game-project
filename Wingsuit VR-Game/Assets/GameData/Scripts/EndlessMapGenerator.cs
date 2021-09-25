using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessMapGenerator : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private int chunksVisibileInFront;
    [SerializeField] private int chunksVisibleBack;

    [SerializeField] private float minMoveDistanceThreshold;

    [SerializeField] private int chunkStartOffset;
    [SerializeField] private float chunkSize;
    [SerializeField] private GameObject[] chunks;


    //List<GameObject> visibleChunks = new List<GameObject>();

    Dictionary<int, GameObject> visibleChunks = new Dictionary<int, GameObject>();

    float playerPositionZ;
    float oldPlayerPositionZ;


    private void Start()
    {
        oldPlayerPositionZ = player.position.z;
        UpdateChunks();
        for (int i = 0; i < chunkStartOffset; i++)
        {
            visibleChunks[i] = null;
        }
    }

    private void Update()
    {
        playerPositionZ = player.position.z;

        if (playerPositionZ - oldPlayerPositionZ > minMoveDistanceThreshold)
        {
            oldPlayerPositionZ = playerPositionZ;
            UpdateChunks();
            //print("UpdatedChunks");
        }
        
    }

    private void UpdateChunks()
    {

        int currentChunkCoord = Mathf.RoundToInt(playerPositionZ / chunkSize);
        //print(currentChunkCoord);

        // iterate through all the positions where there are supposed to be chunks and create them if there are none
        for (int offset = -chunksVisibleBack; offset <= chunksVisibileInFront ; offset++)
        {
            int inRangeChunkCoord = currentChunkCoord + offset;

            if(!visibleChunks.ContainsKey(inRangeChunkCoord) && inRangeChunkCoord >= chunkStartOffset)
            {
                var instChunk = Instantiate(chunks[0], new Vector3(0f, 0f, inRangeChunkCoord * chunkSize), Quaternion.identity);
                visibleChunks[inRangeChunkCoord] = instChunk;
            }

        }


        // checks if we have some chunks left behind and destroy them

        List<int> leftBehindChunksKeys = new List<int>(); 
        foreach (var chunk in visibleChunks)
        {
            if (chunk.Key < currentChunkCoord - chunksVisibleBack)
            {
                var chunkToDestroy = chunk.Value;
                leftBehindChunksKeys.Add(chunk.Key);
                Destroy(chunkToDestroy);
            }
        }

        foreach(int key in leftBehindChunksKeys)
        {
            visibleChunks.Remove(key);
        }

        leftBehindChunksKeys.Clear();

    }
}
