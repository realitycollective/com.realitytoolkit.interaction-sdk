// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityCollective.ServiceFramework.Definitions;
using UnityEngine;

namespace RealityToolkit.InteractionSDK
{
    /// <summary>
    /// Configuration profile for the <see cref="InteractionService"/>.
    /// </summary>
    public class InteractionServiceProfile : BaseServiceProfile<IInteractionServiceDataProvider>
    {
        [Header("General Settings")]
        [SerializeField]
        [Tooltip("Should near interaction be enabled at startup?")]
        private bool nearInteraction = true;

        /// <summary>
        /// Should near interaction be enabled at startup?
        /// </summary>
        public bool NearInteraction => nearInteraction;

        [SerializeField]
        [Tooltip("Should far interaction be enabled at startup?")]
        private bool farInteraction = true;

        /// <summary>
        /// Should far interaction be enabled at startup?
        /// </summary>
        public bool FarInteraction => farInteraction;
    }
}
