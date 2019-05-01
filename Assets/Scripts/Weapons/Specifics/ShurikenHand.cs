using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Weapons;

public class ShurikenHand : MonoBehaviour
{
    public GameObject ShurikenPrefab;
    public Transform ShurikenNock;
    
    private GameObject _actualShuriken;

    private GrabTypes _throwType;
    private bool _wasGrabbing = false;

    public GameObject InstantiateShuriken()
    {
        GameObject shuriken = Instantiate(ShurikenPrefab, ShurikenNock.position, ShurikenNock.rotation);
        shuriken.name = "Shuriken";
        shuriken.transform.parent = ShurikenNock;
        shuriken.SetActive(true);
        return shuriken;
    }
    
    
    protected virtual void HandAttachedUpdate(Hand hand)
    {
        _throwType = hand.GetBestGrabbingType(GrabTypes.Pinch, true);
        
        bool isGrabbing = hand.IsGrabbingWithType(_throwType);
        
        if (_actualShuriken == null && _wasGrabbing == false && isGrabbing)
            _actualShuriken = InstantiateShuriken();
        if (_wasGrabbing && !isGrabbing && _actualShuriken != null)
            FireShuriken();
        _wasGrabbing = isGrabbing;
    }
    
    private void FireShuriken()
    {
        _actualShuriken.transform.parent = null;
    
        Shuriken shuriken = _actualShuriken.GetComponent<Shuriken>();

        _throwType = GrabTypes.None;

        shuriken.Fire(shuriken.VelocityEstimator.GetVelocityEstimate().magnitude * transform.forward);

        _actualShuriken = null;
    }
}
