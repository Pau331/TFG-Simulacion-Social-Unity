using UnityEngine;
using UnityEngine.EventSystems;

public class ImagenClick : MonoBehaviour, IPointerClickHandler
{
    public Transform character;
    public CamaraController camara;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Vector3 pos = character.position;
            camara.target.position = new Vector3(pos.x, camara.target.position.y, pos.z);
        }
    }
}