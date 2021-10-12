using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessMapGenerator : MonoBehaviour
{
    public static EndlessMapGenerator Instance { get; private set; }

    public Material wallsMaterial;
 
    [SerializeField] private Transform player;

    [SerializeField] private GameObject scorePoint;
    [Range(0f, 1f)]
    [SerializeField] private float spawnChance;

    [SerializeField] private int chunksVisibileInFront;
    [SerializeField] private int chunksVisibleBack;

    [SerializeField] private float minMoveDistanceThreshold;

    [SerializeField] private int chunkStartOffset;
    [SerializeField] private float chunkSize;
    [SerializeField] private ChunkGenerator chunkGenerator;


    public readonly Dictionary<int, GameObject> VisibleChunks = new Dictionary<int, GameObject>();

    float playerPositionZ;
    float oldPlayerPositionZ;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        oldPlayerPositionZ = player.localPosition.z;
        UpdateChunks();

        if(chunkGenerator.NoiseSettings.randomSeed)
        {
            chunkGenerator.NoiseSettings.seed = Random.Range(int.MinValue, int.MaxValue);
        }
        
        wallsMaterial.color = new Color(1f, 1f, 1f, 0f);

    }

    private void Update()
    {
        playerPositionZ = player.localPosition.z;

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

            if(!VisibleChunks.ContainsKey(inRangeChunkCoord) && inRangeChunkCoord >= chunkStartOffset)
            {
                var chunkToSpawn = chunkGenerator.GenerateChunk(inRangeChunkCoord);
                var instChunk = Instantiate(chunkToSpawn, transform);
                instChunk.transform.localPosition = new Vector3(0f, 0f, inRangeChunkCoord * chunkSize);
                VisibleChunks[inRangeChunkCoord] = instChunk;

                var spawnPointsTransform = instChunk.transform.Find("SpawnPointsForScorePoints");
                if (spawnPointsTransform != null)
                {
                    var spawnPoints = spawnPointsTransform.GetComponentsInChildren<Transform>();

                    for (int i = 1; i < spawnPoints.Length; i++)
                    {
                        var rng = Random.Range(0f, 1f);
                        if (rng <= spawnChance)
                        {
                            Instantiate(scorePoint, spawnPoints[i].transform);
                        }
                    }
                }
                else
                {
                    //Debug.LogError("Spawn Point for points not found!");
                }


            }

        }


        // checks if we have some chunks left behind and destroy them

        List<int> leftBehindChunksKeys = new List<int>(); 
        foreach (var chunk in VisibleChunks)
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
            VisibleChunks.Remove(key);
        }

        leftBehindChunksKeys.Clear();

    }


    private void OnValidate()
    {
        chunkGenerator.ValidateValues();
    }


}
