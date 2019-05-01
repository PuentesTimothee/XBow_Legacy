using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPeer : MonoBehaviour
{
    [Range(8, 12)]
    public int NumberOfBands = 8;

    private int _numberOfSamples = -1;
    public int NumberOfSamples
    {
        get
        {
            if (_numberOfSamples == -1)
                _numberOfSamples = (int)Mathf.Pow(2, NumberOfBands + 1);
            return (_numberOfSamples);
        }
    }

    public float BufferFalloutSpeed = 1f;

    private AudioSource    _source;

    private float[]        _frequencyBands;
    public float[]         Bands
    {
        get { return (_frequencyBands); }
    }

    private float[]        _bufferedBands;
    private float[]        _bufferedBandsDecrease;
    public float[]         BufferedBands
    {
        get { return (_bufferedBands); }
    }

    private float[]        _samples;
    public float[]         Samples
    {
        get { return (_samples); }
    }

    private void Awake()
    {
        _frequencyBands = new float[NumberOfBands];
        _bufferedBands = new float[NumberOfBands];
        _bufferedBandsDecrease = new float[NumberOfBands];
        _samples = new float[NumberOfSamples];
        _source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        GetSamples();
        GetFrequencyBands();
        GetBuffuredBands();
    }

    private void GetSamples()
    {
        _source.GetSpectrumData(Samples, 0, FFTWindow.Blackman);
    }

    private void GetFrequencyBands()
    {
        int count = 0;
        for (int x = 0; x < NumberOfBands; x++)
        {
            float average = 0;
            int sampleCount = (int) Mathf.Pow(2, x) * 2;
            for (int y = 0; y < sampleCount; y++)
            {
                average += Samples[count];
                count++;
            }

            average /= count;
            Bands[x] = average;
        }
    }

    private void GetBuffuredBands()
    {
        for (int x = 0; x < NumberOfBands; x++)
        {
            if (BufferedBands[x] < Bands[x])
            {
                BufferedBands[x] = Bands[x];
                _bufferedBandsDecrease[x] = BufferFalloutSpeed;
            }
            else
            {
                BufferedBands[x] = Mathf.MoveTowards(BufferedBands[x], Bands[x], _bufferedBandsDecrease[x] * Time.deltaTime);
                _bufferedBandsDecrease[x] += BufferFalloutSpeed * Time.deltaTime;
            }
        }
    }
}
