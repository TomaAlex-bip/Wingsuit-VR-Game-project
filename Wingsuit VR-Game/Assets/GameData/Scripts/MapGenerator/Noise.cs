using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    public static float GenerateNoiseValue(NoiseSettings noiseSettings, int sampleCentre)
    {

        System.Random prng = new System.Random(noiseSettings.seed);
        float offset = prng.Next(-100000, 100000);

        float sampleX = sampleCentre / noiseSettings.scale + offset;

        float noiseValue = (Mathf.PerlinNoise(sampleX, 0.0f) * 2 - 1) * noiseSettings.amplitude;

        noiseValue = Mathf.InverseLerp(-1, 1, noiseValue);

        Debug.Log("sampleCentre: " + sampleCentre + " | sampleX: " + sampleX + " | noiseValue: " + noiseValue);
        return noiseValue;
    }
}


[System.Serializable]
public class NoiseSettings
{
    public float scale;
    public float amplitude;

    public int seed;

    public void ValidataValues()
    {
        if(scale <= 0.01f)
        {
            scale = 0.0f;
        }
    }
}
