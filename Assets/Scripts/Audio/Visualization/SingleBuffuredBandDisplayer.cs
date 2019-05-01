using UnityEngine;

public class SingleBuffuredBandDisplayer : MonoBehaviour
{
    public AudioPeer LinkedPeer;
    public int BandToListen = 0;
    public Renderer Renderer;
    
    private Material _mat;
    private Vector3 BaseSize;

    public float MaxScale = 10f;
    public Vector3 Direction = Vector3.up;
    private readonly int _hashValue = Shader.PropertyToID("_Value");

    private void Awake()
    {
        BaseSize = Renderer.transform.localScale;
        _mat = Renderer.material;
    }

    private void Update()
    {
        Renderer.transform.localScale = BaseSize + Direction * LinkedPeer.BufferedBands[BandToListen] * MaxScale;
        _mat.SetFloat(_hashValue, LinkedPeer.BufferedBands[BandToListen]);
    }
}
