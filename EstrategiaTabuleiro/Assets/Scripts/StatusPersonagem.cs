using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatusPersonagem : MonoBehaviour {
    public Text VidaText, DanoText;
    public Canvas CanvasStatus;
    public UnitManager unitManager;

    void OnMouseEnter()
    {
        CanvasStatus.enabled = true;
    }

    void OnMouseExit()
    {
        CanvasStatus.enabled = false;
    }

    void Update()
    {
        if (VidaText != null && DanoText != null)
        {
            VidaText.text = "Vida : " + unitManager.curHealth;
            DanoText.text = "Dano : " + unitManager.Damage;
        }
        CanvasStatus.transform.rotation = Camera.main.transform.rotation;
    }

    void Start()
    {
        unitManager = GetComponent<UnitManager>();
        CanvasStatus.enabled = false;
    }
}
