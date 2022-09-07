using RealityToolkit.EventDatum.Input;
using RealityToolkit.InputSystem.Interfaces.Handlers;
using RealityToolkit.InputSystem.Listeners;

namespace RealityToolkit.InteractionSDK.Interactors
{
    public class InteractorRegistrar : InputSystemGlobalListener, IMixedRealitySourceStateHandler
    {
        /// <inheritdoc/>
        public void OnSourceDetected(SourceStateEventData eventData)
        {

        }

        /// <inheritdoc/>
        public void OnSourceLost(SourceStateEventData eventData)
        {

        }
    }
}
