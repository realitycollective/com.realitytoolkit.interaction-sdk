// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityCollective.Extensions;
using RealityCollective.ServiceFramework.Services;
using RealityToolkit.EventDatum.Input;
using RealityToolkit.InputSystem.Definitions;
using RealityToolkit.InputSystem.Interfaces.Handlers;
using RealityToolkit.InteractionSDK.Actions;
using RealityToolkit.InteractionSDK.Interactors;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RealityToolkit.InteractionSDK.Interactables
{
    /// <summary>
    /// An <see cref="Interactable"/> marks an object that can be interacted with.
    /// </summary>
    public class Interactable : MonoBehaviour, IInteractable, IMixedRealityInputHandler
    {
        [SerializeField]
        [Tooltip("Optional label that may be used to identify the interactable or categorize it.")]
        private string label = null;

        [SerializeField]
        [Tooltip("The input action used to interact with this interactable.")]
        private MixedRealityInputAction inputAction = MixedRealityInputAction.None;

        [SerializeField]
        [Tooltip("Should near interaction be enabled at startup?")]
        private bool nearInteraction = true;

        [SerializeField]
        [Tooltip("Should far interaction be enabled at startup?")]
        private bool farInteraction = true;

        [Space]
        [SerializeField]
        [Tooltip("Event raised whenever the interactable's state has changed.")]
        private UnityEvent<InteractionState> stateChanged = null;

        private InteractionState currentState;
        private IInteractionService interactionService;
        private readonly Dictionary<uint, IInteractor> focusingInteractors = new Dictionary<uint, IInteractor>();
        private readonly Dictionary<uint, IInteractor> selectingInteractors = new Dictionary<uint, IInteractor>();
        private readonly List<IAction> actions = new List<IAction>();

        /// <inheritdoc/>
        public string Label
        {
            get => label;
            set => label = value;
        }

        /// <inheritdoc/>
        public bool IsValid => isActiveAndEnabled && (NearInteractionEnabled || FarInteractionEnabled);

        /// <inheritdoc/>
        public bool NearInteractionEnabled => interactionService.NearInteractionEnabled && nearInteraction;

        /// <inheritdoc/>
        public bool FarInteractionEnabled => interactionService.FarInteractionEnabled && farInteraction;

        /// <inheritdoc/>
        public InteractionState State
        {
            get => currentState;
            private set
            {
                currentState = value;
                stateChanged?.Invoke(currentState);
                UpdateActions();
            }
        }

        /// <inheritdoc/>
        public IInteractor PrimaryInteractor => selectingInteractors.Values.FirstOrDefault();

        public IReadOnlyList<IInteractor> Interactors => selectingInteractors.Values.ToList();

        /// <summary>
        /// Executed when the <see cref="Interactable"/> is loaded the first time.
        /// </summary>
        private async void Awake()
        {
            try
            {
                interactionService = await ServiceManager.Instance.GetServiceAsync<IInteractionService>();
            }
            catch (System.Exception)
            {
                Debug.LogError($"{nameof(Interactable)} requires the {nameof(IInteractionService)} to work.");
                this.Destroy();
                return;
            }

            // We've been destroyed during the await.
            if (this == null) { return; }

            interactionService.Add(this);
            actions.AddRange(GetComponentsInChildren<IAction>(true));
        }

        /// <summary>
        /// Exeuted whenever the <see cref="Interactable"/> is enabled.
        /// </summary>
        private void OnEnable()
        {
            OnReset();
        }

        /// <summary>
        /// Executed when the <see cref="Interactable"/> is about to be destroyed.
        /// </summary>
        private void OnDestroy()
        {
            if (interactionService == null)
            {
                return;
            }

            interactionService.Remove(this);
        }

        /// <summary>
        /// The <see cref="Interactable"/> interaction state was reset.
        /// </summary>
        private void OnReset()
        {
            focusingInteractors.Clear();
            selectingInteractors.Clear();
            State = InteractionState.Normal;
        }

        /// <summary>
        /// Updates all <see cref="IAction"/>s on the <see cref="IInteractable"/>.
        /// </summary>
        private void UpdateActions()
        {
            for (var i = 0; i < actions.Count; i++)
            {
                actions[i].OnStateChanged(currentState);
            }
        }

        /// <summary>
        /// The <see cref="Interactable"/> is focused by <paramref name="interactor"/>.
        /// </summary>
        /// <param name="interactor">The <see cref="IInteractor"/> focusing the object.</param>
        public void OnFocused(IInteractor interactor)
        {
            focusingInteractors.EnsureDictionaryItem(interactor.InputSource.SourceId, interactor, true);
            if (State != InteractionState.Selected)
            {
                State = InteractionState.Focused;
            }
        }

        /// <summary>
        /// The <see cref="Interactable"/> was unfocused by <paramref name="interactor"/>.
        /// </summary>
        /// <param name="interactor">The <see cref="IInteractor"/> that unfocused the object.</param>
        public void OnUnfocused(IInteractor interactor)
        {
            if (focusingInteractors.TrySafeRemoveDictionaryItem(interactor.InputSource.SourceId) &&
                focusingInteractors.Count == 0 &&
                State == InteractionState.Focused)
            {
                State = InteractionState.Normal;
            }

            if (selectingInteractors.TryGetValue(interactor.InputSource.SourceId, out _))
            {
                // If an interactor that was interacting with the object has lost focus to it,
                // then that is the same as ending the interaction.
                OnDeselected(interactor);
            }
        }

        /// <summary>
        /// The <see cref="Interactable"/> is now selected by <paramref name="interactor"/>.
        /// </summary>
        protected void OnSelected(IInteractor interactor)
        {
            selectingInteractors.EnsureDictionaryItem(interactor.InputSource.SourceId, interactor, true);
            State = InteractionState.Selected;
        }

        /// <summary>
        /// The <see cref="Interactable"/> is no longer selected by <paramref name="interactor"/>.
        /// </summary>
        protected void OnDeselected(IInteractor interactor)
        {
            selectingInteractors.TrySafeRemoveDictionaryItem(interactor.InputSource.SourceId);
            if (selectingInteractors.Count > 0)
            {
                return;
            }

            State = focusingInteractors.Count == 0 ? InteractionState.Normal : InteractionState.Focused;
        }

        /// <inheritdoc/>
        public void OnInputDown(InputEventData eventData)
        {
            if (eventData.used ||
                eventData.MixedRealityInputAction != inputAction ||
                !interactionService.TryGetInteractor(eventData.InputSource, out var interactor))
            {
                return;
            }

            eventData.Use();
            OnSelected(interactor);
        }

        /// <inheritdoc/>
        public void OnInputUp(InputEventData eventData)
        {
            if (eventData.MixedRealityInputAction != inputAction)
            {
                return;
            }

            interactionService.TryGetInteractor(eventData.InputSource, out var interactor);
            OnDeselected(interactor);
        }
    }
}
