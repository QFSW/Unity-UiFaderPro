using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class CanvasController : MonoBehaviour
{
    //Private Variables
    /// <summary>Default speed for fade in and fade out.</summary>
    [Tooltip("Default speed for fade in and fade out.")]
    public float DefaultSpeed = 0.4f;
    /// <summary>Automatically fade on activating the GameObject.</summary>
    [Tooltip("Automatically fade on activating the GameObject.")]
    public bool AutoFade = true;
    /// <summary>Take excluse control of the components and stop parent objects from using them.</summary>
    [Tooltip("Take excluse control of the components and stop parent objects from using them.")]
    public bool ExclusiveController;

    //Private Variables
    /// <summary>List of the FadableComponents this CanvasController has access to.</summary>
    private List<Fadable> FadableComponents = new List<Fadable>();
    /// <summary>Whether or not the GameObject contains a RayCaster component.</summary>
    private bool IsRaycaster;
    /// <summary>Whether or not the CanvasController is initialised.</summary>
    private bool Initialised = false;
    /// <summary>The GraphicsRaycaster on the canvas, if it exists.</summary>
    private GraphicRaycaster CanvasRaycaster;

    //Gets all components on Canvas
    void Awake() { Initialise(); }
    /// <summary>Initialises the CanvasController.</summary>
    public void Initialise() { Initialise(false, false); }
    /// <summary>Initialises the CanvasController.</summary>
    /// <param name="Override">Clears cache and re-initialises.</param>
    public void Initialise(bool Override) { Initialise(Override, false); }
    /// <summary>Initialises the CanvasController.</summary>
    /// <param name="Override">Clears the cache and re-initialises.</param>
    /// <param name="TreeOverload">Clears the cache of all children CanvasController and re-initialises them.</param>
    public void Initialise(bool Override, bool TreeOverload)
    {
        //If it will re-initialise
        if (Override)
        {
            Initialised = false;
            FadableComponents.Clear();
        }

        if (!Initialised)
        {
            //Gets all CanvasControllers in children and initialises them
            CanvasController[] Controllers = GetComponentsInChildren<CanvasController>(true);
            foreach (CanvasController Controller in Controllers) { if (Controller != this) { Controller.Initialise(TreeOverload, TreeOverload); } }

            //Adds components to the FadableComponent list
            //Does not add components to its list if a child CanvasController has taken Exclusive control of it
            foreach (MaskableGraphic MaskableComponent in GetComponentsInChildren<MaskableGraphic>(true))
            {
                bool Locked = false;
                foreach (CanvasController Controller in Controllers) { if (Controller.ExclusiveController && Controller.FadableComponents.Exists(x => x == MaskableComponent)){ Locked = true; break; } }
                if (!Locked) { FadableComponents.Add(new Fadable(MaskableComponent)); }
            }
            
            //Raycasters
            IsRaycaster = GetComponent<GraphicRaycaster>() != null;
            if (IsRaycaster) { CanvasRaycaster = GetComponent<GraphicRaycaster>(); }
            Initialised = true;
        }
    }

    /// <summary>Makes all components fully faded out.</summary>
    public void NoAlpha() { for (int i = 0; i < FadableComponents.Count; i++) { FadableComponents[i].NoAlpha(); } }
    /// <summary>Makes all components fully faded in.</summary>
    public void FullAlpha() { for (int i = 0; i < FadableComponents.Count; i++) { FadableComponents[i].FullAlpha(); } }
    /// <summary>Caches which objects are and aren't active.</summary>
    public void SetActivity() { for (int i = 0; i < FadableComponents.Count; i++) { FadableComponents[i].SetActivity(); } }

    /// <summary>Fades out.</summary>
    public void FadeOut() { FadeOut(DefaultSpeed); }
    /// <summary>Fades out.</summary>
    /// <param name="Duration">Duration of the fade.</param>
    public void FadeOut(float Duration)
    {
        if (gameObject.activeInHierarchy)
        {
            //Stops any other fading
            StopAllCoroutines();

            //Disables raycaster
            if (IsRaycaster) { CanvasRaycaster.enabled = false; }

            //Begins Fade
            SetActivity();
            StartCoroutine(FadeOutComponents(Duration));
        }
    }
    /// <summary>Fades out.</summary>
    /// <param name="Duration">Duration of the fade.</param>
    IEnumerator FadeOutComponents(float Duration)
    {
        float TimeRemaining = Duration;

        //Fades each component
        FullAlpha();
        while (TimeRemaining > 0)
        {
            for (int i = 0; i < FadableComponents.Count; i++) { if (FadableComponents[i].Active) { FadableComponents[i].Fade(-Duration); } }
            TimeRemaining -= Time.unscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        }
        FullAlpha();

        //Finishes fade
        if (IsRaycaster && !AutoFade) { CanvasRaycaster.enabled = true; }
        gameObject.SetActive(false);
    }

    /// <summary>Fades in.</summary>
    public void FadeIn() { FadeIn(DefaultSpeed); }
    /// <summary>Fades in.</summary>
    /// <param name="Duration">Duration of the fade.</param>
    public void FadeIn(float Length)
    {
        if (gameObject.activeInHierarchy)
        {
            //Stops any other fading
            if (!Initialised) { Initialise(); }
            StopAllCoroutines();


            //Disables raycaster
            if (IsRaycaster) { CanvasRaycaster.enabled = false; }

            //Begins Fade
            SetActivity();
            StartCoroutine(FadeInComponents(Length));
        }
    }
    /// <summary>Fades in.</summary>
    /// <param name="Duration">Duration of the fade.</param>
    IEnumerator FadeInComponents(float Length)
    {
        float TimeRemaining = Length;

        //Fades each component
        NoAlpha();
        while (TimeRemaining > 0)
        {
            for (int i = 0; i < FadableComponents.Count; i++) { if (FadableComponents[i].Active) { FadableComponents[i].Fade(Length); } }
            TimeRemaining -= Time.unscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        }
        FullAlpha();

        //Finishes fade
        if (IsRaycaster) { CanvasRaycaster.enabled = true; }
    }

    //Auto fade in
    void OnEnable() { if (AutoFade) { FadeIn(); } }
}
