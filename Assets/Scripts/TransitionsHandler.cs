using UnityEngine;
/// <summary>
/// Handles the transition between screens
/// </summary>
public class TransitionsHandler : MonoBehaviour
{
    public CanvasGroup HomeCG;
    public CanvasGroup DeckBuilderCG;
    public CanvasGroup AboutCG;

    public void GoToHome(CanvasGroup from)
    {
        FadeTransition.Transition(from, HomeCG);
    }
    public void GoToDeckBuilder(CanvasGroup from)
    {
        FadeTransition.Transition(from, DeckBuilderCG);
    }
    public void GoToAbout(CanvasGroup from)
    {
        FadeTransition.Transition(from, AboutCG);
    }

}
