using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Fadable
{
    //Public Variables
    /// <summary>Whether or not the component is active.</summary>
    [HideInInspector]
    public bool Active;

    //Private Variables
    /// <summary>The current opacity of the encapsulated component.</summary>
    public float Alpha;
    /// <summary>The base colour of the encapsulated component.</summary>
    public Color Col;
    /// <summary>The encapsulated component.</summary>
    private MaskableGraphic EncapsulatedComponent;
    /// <summary>The object that the encapsulated component is a part of.</summary>
    private GameObject Object;

    /// <summary>Creates a FadableComponent encapsulating a component deriving from MaskableGraphic.</summary>
    /// <param name="Element">The component to encapsulate.</param>
    public Fadable(MaskableGraphic Element)
    {
        EncapsulatedComponent = Element;
        Col = EncapsulatedComponent.color;
        Alpha = Col.a;
        Object = EncapsulatedComponent.gameObject;
    }

    /// <summary>Makes it a fully filled element.</summary>
    public void FullAlpha() { EncapsulatedComponent.color = Col; }

    /// <summary>Makes it an invisible element.</summary>
    public void NoAlpha() { EncapsulatedComponent.color = new Color(Col.r, Col.g, Col.b, 0); }

    /// <summary>Increments/decrements the alpha for one frame.</summary>
    /// <param name="Length">How long the total fade will take.</param>
    public void Fade(float Length) { EncapsulatedComponent.color = new Color(Col.r, Col.g, Col.b, EncapsulatedComponent.color.a + (Alpha * Time.unscaledDeltaTime) / Length); }

    /// <summary>Sets if active.</summary>
    /// <returns>the activity of the component.</returns>
    public bool SetActivity() { return Active = Object.activeInHierarchy && EncapsulatedComponent.enabled; }

    //Comparison operator support
    public static bool operator ==(Fadable a, Fadable b) { return a.EncapsulatedComponent == b.EncapsulatedComponent; }
    public static bool operator !=(Fadable a, Fadable b) { return !(a == b); }
    public static bool operator ==(Fadable a, MaskableGraphic b) { return a.EncapsulatedComponent == b; }
    public static bool operator ==(MaskableGraphic a, Fadable b) { return a == b.EncapsulatedComponent; }
    public static bool operator !=(Fadable a, MaskableGraphic b) { return !(a == b); }
    public static bool operator !=(MaskableGraphic a, Fadable b) { return !(a == b); }
}

