// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityCollective.Extensions;
using RealityCollective.ServiceFramework.Services;
using RealityToolkit.InteractionSDK.Interactors;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RealityToolkit.InteractionSDK.Interactables
{
    /// <summary>
    /// An <see cref="Interactable"/> marks an object that can be interacted with.
    /// </summary>
    public class Interactable : MonoBehaviour, IInteractable
    {
        [SerializeField]
        [Tooltip("Optional label that may be used to identify the interactable or categorize it.")]
        private string label = null;

        [Space]
        [SerializeField]
        [Tooltip("Event raised whenever the interactable's state has changed.")]
        private UnityEvent<InteractionState> stateChanged = null;

        private NearInteractable nearInteractable;
        private FarInteractable farInteractable;
        private IInteractionService interactionService;
        private InteractionState currentState;
        private readonly Dictionary<uint, IInteractor> focusingInteractors = new Dictionary<uint, IInteractor>();
        private readonly Dictionary<uint, IInteractor> selectingInteractors = new Dictionary<uint, IInteractor>();

        /// <inheritdoc/>
        public string Label
        {
            get => label;
            set => label = value;
        }

        /// <inheritdoc/>
        public bool IsValid => isActiveAndEnabled && (NearInteractionEnabled || FarInteractionEnabled);

        /// <inheritdoc/>
        public bool NearInteractionEnabled => interactionService.NearInteractionEnabled && nearInteractable.IsNotNull();

        /// <inheritdoc/>
        public bool FarInteractionEnabled => interactionService.FarInteractionEnabled && farInteractable.IsNotNull();

        /// <inheritdoc/>
        public InteractionState State
        {
            get => currentState;
            private set
            {
                currentState = value;
                stateChanged?.Invoke(currentState);
            }
        }

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
        }

        /// <summary>
        /// Exeuted whenever the <see cref="Interactable"/> is enabled.
        /// </summary>
        private void OnEnable()
        {
            nearInteractable = GetComponent<NearInteractable>();
            farInteractable = GetComponent<FarInteractable>();

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
        public void OnReset()
        {
            focusingInteractors.Clear();
            selectingInteractors.Clear();
            State = InteractionState.Normal;
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
        }

        /// <summary>
        /// The <see cref="Interactable"/> is now selected by <paramref name="interactor"/>.
        /// </summary>
        public void OnSelected(IInteractor interactor)
        {
            selectingInteractors.EnsureDictionaryItem(interactor.InputSource.SourceId, interactor, true);
            State = InteractionState.Selected;
        }

        /// <summary>
        /// The <see cref="Interactable"/> is no longer selected by <paramref name="interactor"/>.
        /// </summary>
        public void OnDeselected(IInteractor interactor)
        {
            selectingInteractors.TrySafeRemoveDictionaryItem(interactor.InputSource.SourceId);
            if (selectingInteractors.Count > 0)
            {
                return;
            }

            State = focusingInteractors.Count == 0 ? InteractionState.Normal : InteractionState.Focused;
        }
    }
}
