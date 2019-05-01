using UnityEngine;

namespace AudioVisualization
{
    public class ParticleSystemBand : MonoBehaviour
    {
        public AudioPeer Peer;
        public ParticleSystem ParticleSystem;
        
        public int BandListened = 1;
        
        public Vector2 BandFrenquecyRange = new Vector2(0.01f, 0.25f);
        public Vector2 Speed = new Vector2(0.1f, 10f);

        void Update()
        {
            var val = Peer.Bands[BandListened];

            val = Mathf.Clamp01((val - BandFrenquecyRange.x) / BandFrenquecyRange.y);

            if (val > 0)
            {
                var emi = ParticleSystem.emission;
                emi.rateOverTime = (Mathf.Lerp(Speed.x, Speed.y, val));
            }
        }
    }
}
