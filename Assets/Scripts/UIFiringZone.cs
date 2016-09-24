using UnityEngine;
using System.Collections;
//using UnityEngine.UI;
using UnityEngine.EventSystems;
public class UIFiringZone : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public GameObject player;
    public bool fireMagnet = false;

    private bool isHovered = false;    

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;       
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }

    void Update()
    {
        if (isHovered)
            if (fireMagnet)
                player.GetComponent<PlayerController>().fireMagnet();
            else
                player.GetComponent<PlayerController>().fireDestroy();
    }
    }
