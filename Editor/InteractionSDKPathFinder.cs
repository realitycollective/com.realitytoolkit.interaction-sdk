// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityToolkit.Editor.Utilities;
using UnityEngine;

namespace RealityToolkit.InteractionSDK.Editor
{
    /// <summary>
    /// Dummy <see cref="ScriptableObject"/> used to find the relative path of the platform package.
    /// </summary>
    /// <inheritdoc cref="IPathFinder" />
    public class InteractionSDKPathFinder : ScriptableObject, IPathFinder
    {
        /// <inheritdoc />
        public string Location => $"/Editor/{nameof(InteractionSDKPathFinder)}.cs";
    }
}
