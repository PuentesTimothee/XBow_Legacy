using UnityEngine;

namespace Commons
{
    public class PassOnCollisionEnter : MonoBehaviour, IOnCollisionEnter
    {
        public MonoBehaviour Behaviour;
        public IOnCollisionEnter ToPass;

        private void Awake()
        {
            ToPass = Behaviour as IOnCollisionEnter;
        }
        
        public void OnCollisionEnter(Collision other)
        {
            ToPass.OnCollisionEnter(other);
        }
    }
}
