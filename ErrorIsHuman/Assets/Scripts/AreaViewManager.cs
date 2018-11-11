using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaViewManager : MonoBehaviour {
    #region Fields
    [SerializeField]
    private Sprite currentSprite;

    private SpriteRenderer areaRenderer;
    #endregion

    #region Methods
    /// <summary>
    /// Disables AreaView renderer to return to RoomView
    /// </summary>
    public void backToRoomView()
    {
        if (areaRenderer != null)
        { 
            areaRenderer.enabled = false;
        }
    }

    /// <summary>
    /// Set Area View
    /// </summary>
    public void goToAreaView(Sprite areaView)
    {
        this.currentSprite = areaView;
        if(areaRenderer != null)
        {
            areaRenderer.sprite = currentSprite;
            areaRenderer.enabled = true;
        }
    }
    #endregion

    #region Functions
    private void Start()
    {
        areaRenderer = this.GetComponent<SpriteRenderer>();
    }
    #endregion

}
