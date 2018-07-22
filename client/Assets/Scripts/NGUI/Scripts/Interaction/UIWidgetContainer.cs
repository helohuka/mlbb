//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System.Collections;

/// <summary>
/// Widget container is a generic type class that acts like a non-resizeable widget when selecting things in the scene view.
/// </summary>

[AddComponentMenu("NGUI/Interaction/Widget Container")]
public class UIWidgetContainer : MonoBehaviour {
    public virtual bool Visible
    {
        get { return gameObject.active; }
        set { gameObject.SetActive(value); }
    }

    public virtual void Show()
    {
        Visible = true;
    }

    public virtual void Hide()
    {
        Visible = false;
    }

    public virtual void SwapVisible()
    {
        Visible = !Visible;
    }
}
