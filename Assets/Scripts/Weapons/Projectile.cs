using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Commons;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace Weapons
{
    [RequireComponent(typeof(AppearScript))]
    public class Projectile : MonoBehaviour, IOnCollisionEnter
    {
        [Header("Common projectiles variables")]
        [SerializeField] protected Rigidbody MainRigidbody;
        [SerializeField] protected List<Rigidbody> AllRigidBodies;
        [SerializeField] protected List<Collider> Colliders;
        [SerializeField] protected Animator Animator;
        [SerializeField] protected AppearScript AppearScript;
        
        [Flags, Serializable]
        public enum State
        {
            Waiting   = 0x0001,
            InFlight  = 0x0010,
            Stuck     = 0x0100,
            Destroyed = 0x1000
        }
        //[HideInInspector]
        public State Status = State.Waiting;
        
        public float StickMinimalSpeed = 0.2f;
        public float MaxPenetrationRequiredSpeed = 10f;
        public float MaxPenetration = 0.1f;

        public CollisionDetectionMode FiredMode = CollisionDetectionMode.ContinuousDynamic;
        public CollisionDetectionMode StopedMode = CollisionDetectionMode.Discrete;
        
        private float _timeBeforeDestruction = -1;

        public float MaxFlyTime = 10f;
        public float MaxStuckTime = 5f;
        
        private Vector3 _prevPosition;
        private Quaternion _prevRotation;
        private Vector3 _prevVelocity;

        #if UNITY_EDITOR
        private void Reset()
        {
            MainRigidbody = GetComponent<Rigidbody>();
            AllRigidBodies = GetComponentsInChildren<Rigidbody>().ToList();
            Colliders = GetComponentsInChildren<Collider>().ToList();
            Animator = GetComponent<Animator>();
            AppearScript = GetComponent<AppearScript>();
            
            if (MainRigidbody == null)
                Debug.LogError("Reset of " + name + "'s " + GetType() + ": No Rigidbody found");
            if (Animator == null)
                Debug.LogError("Reset of " + name + "'s " + GetType() + ": No Animator found");
            if (Colliders == null || Colliders.Count == 0)
                Debug.LogError("Reset of " + name + "'s " + GetType() + ": No Colliders found");
        }
        #endif

        private void Awake()
        {
            _timeBeforeDestruction = MaxFlyTime;
        }
        
        #region Stop
        [Serializable]
        private struct HoldStop
        {
            public Vector3 Velocity;
            public Vector3 AngularVelocity;
        }
        private HoldStop[] _valueKeeped;
        private float _stopTimer = -1f;
        public void Stop(float length = 1, bool keepSpeed = false)
        {
            if (_stopTimer == -1)
            {
                _stopTimer = length;
                StartCoroutine(StopCoroutine(keepSpeed));
            }
            else
                _stopTimer += length;
        }

        protected virtual void StopStart(bool keepSpeed)
        {
            if (_valueKeeped == null || _valueKeeped.Length != AllRigidBodies.Count)
                _valueKeeped = new HoldStop[AllRigidBodies.Count];

            for (int x = 0; x < AllRigidBodies.Count; x ++)
            {
                if (keepSpeed)
                {
                    _valueKeeped[x].Velocity = AllRigidBodies[x].velocity;
                    _valueKeeped[x].AngularVelocity = AllRigidBodies[x].angularVelocity;
                }

                AllRigidBodies[x].collisionDetectionMode = StopedMode;
                AllRigidBodies[x].isKinematic = true;
                AllRigidBodies[x].useGravity = false;
            }
        }

        protected virtual void StopEnd(bool keepSpeed)
        {
            for (int x = 0; x < AllRigidBodies.Count; x ++)
            {
                AllRigidBodies[x].isKinematic = false;
                AllRigidBodies[x].collisionDetectionMode = FiredMode;
                AllRigidBodies[x].useGravity = true;
                
                if (keepSpeed)
                {
                    AllRigidBodies[x].velocity = _valueKeeped[x].Velocity;
                    AllRigidBodies[x].angularVelocity = _valueKeeped[x].AngularVelocity;
                }
            }
        }
        private IEnumerator StopCoroutine(bool keepSpeed)
        {
            StopStart(keepSpeed);
            while (_stopTimer > 0)
            {
                yield return new WaitForFixedUpdate();
                _stopTimer -= Time.deltaTime;
                if (_stopTimer < 0)
                    _stopTimer = 0;
            }
            StopEnd(keepSpeed);
            _stopTimer = -1f;
        }
        #endregion
        
        #region Fire

        void FixedUpdate()
        {
            if (Status == State.InFlight)
            {
                _prevPosition = transform.position;
                _prevRotation = transform.rotation;
                _prevVelocity = MainRigidbody.velocity;
            }

            if ((Status & (State.Waiting | State.Destroyed)) == 0 && _stopTimer == -1)
            {
                _timeBeforeDestruction -= Time.deltaTime;
                if (_timeBeforeDestruction < 0)
                {
                    Status = State.Destroyed;
                    StartCoroutine(DestroySelf());
                }
            }
        }

        protected IEnumerator DestroySelf()
        {

            Colliders.ForEach(col => { col.enabled = false; });
            AllRigidBodies.ForEach(rb =>
            {
                rb.collisionDetectionMode = StopedMode;
                rb.isKinematic = true;
                rb.useGravity = false;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            });
            
            yield return AppearScript.Disapear();
            Destroy(gameObject);
        }
        
        public virtual void Fire(Vector3 speedAndDir)
        {
            Status = State.InFlight;
            _timeBeforeDestruction = MaxFlyTime;
            
            _prevPosition = transform.position;
            _prevRotation = transform.rotation;
            _prevVelocity = MainRigidbody.velocity;            
            
            Colliders.ForEach(col => { col.enabled = true; });
            
            AllRigidBodies.ForEach(rb =>
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                rb.collisionDetectionMode = FiredMode;
            });

            Fire_SetVelocity(speedAndDir);
        }
        protected virtual void Fire_SetVelocity(Vector3 speedAndDir)
        {
            MainRigidbody.AddForce(transform.forward * speedAndDir.sqrMagnitude, ForceMode.VelocityChange);
        }
        #endregion
        
        #region Collision
        protected virtual bool CanStick(float actualSpeed)
        {
           return (actualSpeed > StickMinimalSpeed);
        }
        
        public virtual void OnCollisionEnter(Collision collision)
        {
            if (Status == State.InFlight)
            {
                var rbSpeed = MainRigidbody.velocity;
                /*  Code to use is we want to pass throught some object
				if ( true )
				{
					// Revert my physics properties cause I don't want balloons to influence my travel
					transform.position = _prevPosition;
					transform.rotation = _prevRotation;
					arrowHeadRB.velocity = _prevVelocity;
					Physics.IgnoreCollision( arrowHeadRB.GetComponent<Collider>(), collision.collider );
					Physics.IgnoreCollision( shaftRB.GetComponent<Collider>(), collision.collider );
				}
                 */
                
                if (CanStick(rbSpeed.sqrMagnitude))
                    StickInTarget(collision, rbSpeed);
            }
        }

        protected virtual void StickInTarget(Collision collision, Vector3 actualVelocity)
        {
            Status = State.Stuck;
            _timeBeforeDestruction = MaxStuckTime;
            
            Colliders.ForEach(col => { col.enabled = false; });
            AllRigidBodies.ForEach(rb =>
            {
                rb.collisionDetectionMode = StopedMode;
                rb.isKinematic = true;
                rb.useGravity = false;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            });
            
            transform.rotation = _prevRotation;
            Vector3 penetration = Vector3.zero;
            if (MaxPenetration > 0)
                penetration = _prevVelocity.normalized * Util.RemapNumberClamped(_prevVelocity.magnitude, 0f, MaxPenetrationRequiredSpeed, 0.0f, MaxPenetration);
            transform.position = transform.position + penetration;
            transform.parent =  collision.collider.transform;
        }
        #endregion
    }
}
