// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityCollective.Extensions;
using RealityCollective.ServiceFramework.Services;
using UnityEngine;

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

        private NearInteractable nearInteractable;
        private FarInteractable farInteractable;
        private IInteractionService interactionService;

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
        public InteractionState State { get; private set; }

        /// <summary>
        /// Executed when the <see cref="Interactable"/> is loaded the first time.
        /// </summary>
        private void Awake()
        {
            if (!ServiceManager.Instance.TryGetService(out interactionService))
            {
                Debug.LogError($"{nameof(Interactable)} requires the {nameof(IInteractionService)} to work.");
                this.Destroy();
                return;
            }

            interactionService.Add(this);
        }

        /// <summary>
        /// Exeuted whenever the <see cref="Interactable"/> is enabled.
        /// </summary>
        private void OnEnable()
        {
            nearInteractable = GetComponent<NearInteractable>();
            farInteractable = GetComponent<FarInteractable>();
            State = InteractionState.Normal;
        }

        /// <summary>
        /// Executed when the <see cref="Interactable"/> is about to be destroyed.
        /// </summary>
        private void OnDestroy()
        {
            if (!ServiceManager.Instance.TryGetService<IInteractionService>(out var interactionService))
            {
                return;
            }

            interactionService.Remove(this);
        }
    }
}
