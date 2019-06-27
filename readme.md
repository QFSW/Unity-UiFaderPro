# UI Fader Pro V1.0.0

UiFaderPro is designed to make fading your UI as simple as possible. By attaching a single component to your Canvas or root GameObject, all elements of the tree will automatically be faded in when activated, and automatically faded out when using CanvasControllerInstance.FadeOut(). It is highly versatile and easy to use, with little to no effort required to add to an existing scene.

![](https://github.com/QFSW/Unity-UiFaderPro/blob/master/Preview.gif)

# How to Use It
To use it, simply add the script to each Canvas or root GameObject of the GUI you would like to add fading too (an object with a CanvasController may still have CanvasControllers in the children object for individual control, but it is not necessary if you only want to fade in or out the entire thing). If AutoFade is ticked, then it will automatically fade in when activated. To use the cross fading, replace your SetActive calls with FadeIn and FadeOut.
