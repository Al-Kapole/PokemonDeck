using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomToggle : Toggle
{
    public UnityEngine.Events.UnityEvent<int> OnToggleIsOn;

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        OnToggleIsOn?.Invoke(0);
    }
}
