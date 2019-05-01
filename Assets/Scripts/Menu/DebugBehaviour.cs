using UnityEngine;
using UnityEngine.EventSystems;

namespace Menu
{
    public class DebugBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public void PrintDebug(string text)
        {
            Debug.Log(name + ": " + text);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log(name + ": Pointer enter : " + eventData.currentInputModule);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log(name + ": Pointer exit : " + eventData.currentInputModule);
        }
    }
}
