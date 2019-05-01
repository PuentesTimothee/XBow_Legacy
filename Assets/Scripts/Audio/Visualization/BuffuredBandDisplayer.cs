using UnityEngine;

public class BuffuredBandDisplayer : MonoBehaviour
{
    public GameObject PrefabOfSegment;
    public AudioPeer LinkedPeer;

    private Transform[] _segments;
    private Material[] _segmentsMat;
    public Vector3 BaseSize = Vector3.one;

    public float CircleRadius = 100f;
    public float CircleRepartition = 180f;
    public float MaxScale = 10f;
    public Vector3 Direction = Vector3.up;
    private readonly int _hashValue = Shader.PropertyToID("_Value");

    private void Awake()
    {
        if (_segments != null)
            return;
        
        _segments = new Transform[LinkedPeer.NumberOfBands];
        _segmentsMat = new Material[LinkedPeer.NumberOfBands];
        
        for (int x = 0; x < LinkedPeer.NumberOfBands; x++)
        {
            var go = Instantiate(PrefabOfSegment, transform);
            go.name = "Cube " + x;
            _segments[x] = go.transform;
            _segments[x].position = transform.position;
            _segments[x].rotation = transform.rotation;
            _segments[x].Rotate(transform.up, (CircleRepartition/LinkedPeer.NumberOfBands)*x);
            _segments[x].position += _segments[x].forward * CircleRadius;
            _segmentsMat[x] = _segments[x].GetComponentInChildren<Renderer>().material;
        }
    }

    private void Update()
    {
        for (int x = 0; x < LinkedPeer.NumberOfBands; x++)
        {
            _segments[x].localScale = BaseSize + Direction * LinkedPeer.BufferedBands[x] * MaxScale;
            _segmentsMat[x].SetFloat(_hashValue, LinkedPeer.BufferedBands[x]);
        }
    }
}
