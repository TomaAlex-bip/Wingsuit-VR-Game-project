using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChunkGenerator
{

    public NoiseSettings NoiseSettings => noiseSettings;

    [SerializeField] private AnimationCurve noiseCurve;

    [SerializeField] private AnimationCurve biomeCurve;
    [SerializeField] private int redBiomes;
    [SerializeField] private int greenBiomes;
    [SerializeField] private int blueBiomes;

    [SerializeField] private NoiseSettings noiseSettings;
    [SerializeField] private List<Biome> biomes;


    public GameObject GenerateChunk(int position)
    {
        
        int biomeIndex = GenerateBiomeIndex(position);

        int chunkIndex = GenerateChunkIndex(biomeIndex);

        //Debug.Log("biomeIndex: " + biomeIndex + " | chunkIndex: " + chunkIndex);

        var chunk = biomes[biomeIndex].chunks[chunkIndex];

        biomeCurve.AddKey(position, biomeIndex);
        switch (biomeIndex)
        {
            case 0:
                redBiomes++;
                break;
            case 1:
                greenBiomes++;
                break;
            case 2:
                blueBiomes++;
                break;
        }

        return chunk;
    }





    private int GenerateBiomeIndex(int position)
    {
        float noise =  Noise.GenerateNoiseValue(noiseSettings, position);

        noiseCurve.AddKey(position, noise);

        for (int i = 0; i < biomes.Count; i++)
        {
            if(noise >= biomes[i].minValue && noise <= biomes[i].maxValue)
            {
                return i;
            }
        }
        return 0;


    }

    private int GenerateChunkIndex(int biomeIndex)
    {
        return Random.Range(0, biomes[biomeIndex].chunks.Count);
    }


    public void ValidateValues()
    {
        noiseSettings.ValidateValues();
    }



    [System.Serializable]
    public struct Biome
    {
        public List<GameObject> chunks;
        public float minValue;
        public float maxValue;
    }

}
