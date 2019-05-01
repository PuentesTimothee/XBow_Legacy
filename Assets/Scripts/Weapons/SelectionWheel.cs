using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

namespace Weapons
{
    [RequireComponent(typeof(CanvasGroup))]
    public class SelectionWheel : MonoBehaviour
    {
        [Serializable]
        public struct SelectionOption
        {
            public string Text;
            public float Angle;
            public Vector2 PositionInWheel;
        }
        [SerializeField] private RectTransform _selector;
        [SerializeField] private Image _previewed;

        [SerializeField] private GameObject _prefabs;
        [SerializeField] private Transform _prefabRoot;

        [SerializeField] private float _deadZone = 0.1f;

        private CanvasGroup _group;
        private int _selectedOption;
        private SelectionOption[] _options;
        private Vector2 _previewDir;

        public float Radius;

        private int _numberOfOption;

        public SteamVR_Input_Sources InputHand;
        public SteamVR_Action_Vector2 SelectOnWheel = SteamVR_Input.GetAction<SteamVR_Action_Vector2>("SelectOnWheel");
        public SteamVR_Action_Boolean IsSelectingOnWheel = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("IsSelectingOnWheel");
        public SteamVR_Action_Boolean ValidateOnWheel = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("ValidateOnWheel");

        public event Action<int> OnValidateOption;

        private void Awake()
        {
            _group = GetComponent<CanvasGroup>();
        }

        public void RemoveAllSlot()
        {
            _numberOfOption = 0;
            _options = null;
            foreach (Transform child in _prefabRoot.transform)
                Destroy(child.gameObject);
        }
        
        public void UpdateNumberOfSlot(List<string> values)
        {
            _numberOfOption = values.Count;
            
            float angleDiff = 360f / values.Count;
            _options = new SelectionOption[values.Count];
            for (int x = 0; x < values.Count; x++)
            {
                _options[x].Text = values[x];
                _options[x].Angle = x * angleDiff;
                _options[x].PositionInWheel = Quaternion.Euler(0, 0, x * angleDiff) * Vector2.up;				
            }
            
            if (_numberOfOption != 0)
                _previewed.fillAmount = 1f / _numberOfOption;
            
            foreach (Transform child in _prefabRoot.transform)
                Destroy(child.gameObject);

            foreach (var value in _options)
            {
                var obj = Instantiate(_prefabs, _prefabRoot);
                obj.transform.localPosition = Quaternion.Euler(0, 0, value.Angle) * (Radius * 0.75f * Vector3.up);
                var text = obj.GetComponent<Text>();
                text.text = value.Text;
            }
        }

        private void Update()
        {
            GetInputPreviewedWeapon();

            if (IsSelectingOnWheel.GetState(InputHand) && _numberOfOption > 0)
            {
                _group.alpha = Mathf.MoveTowards(_group.alpha, 1, Time.deltaTime);
                SelectPreviewedOption();

                if (ValidateOnWheel.GetStateDown(InputHand) && _selectedOption != -1 && OnValidateOption != null)
                    OnValidateOption(_selectedOption);
            }
            else
            {
                _group.alpha = Mathf.MoveTowards(_group.alpha, 0, Time.deltaTime * 5f);
                _previewDir = Vector2.MoveTowards(_previewDir, Vector2.zero, Time.deltaTime);
            }
        }

        private void GetInputPreviewedWeapon()
        {
            if (IsSelectingOnWheel.GetState(InputHand))
                _previewDir = SelectOnWheel.GetAxis(InputHand);
            else
                _previewDir = Vector2.MoveTowards(_previewDir, Vector2.zero, Time.deltaTime);

            _selector.localPosition = _previewDir * Radius;
        }
	
        private void SelectPreviewedOption()
        {
            int closest = -1;

            if (_previewDir.magnitude > _deadZone)
            {
                float angle = Vector2.SignedAngle(Vector2.up, _previewDir);

                float closestDelta = 1f;
                for (int x = 0; x <_options.Length; x++)
                {
                    float delta = Vector2.Distance(_options[x].PositionInWheel, _previewDir);
                    if (delta < closestDelta)
                    {
                        closestDelta = delta;
                        closest = x;
                    }
                }
            }

            _selectedOption = closest;
            _previewed.gameObject.SetActive(_selectedOption != -1);

            if (_selectedOption != -1)
            {
                var zAngle = 180f / _numberOfOption + _options[_selectedOption].Angle;
                _previewed.transform.localEulerAngles = new Vector3(0, 0, zAngle);
            }
        }
    }
}
