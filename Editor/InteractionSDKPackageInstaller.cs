// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityCollective.Editor.Utilities;
using RealityCollective.Extensions;
using RealityToolkit.Editor;
using RealityToolkit.Editor.Utilities;
using System.IO;
using UnityEditor;

namespace RealityToolkit.InteractionSDK.Editor
{
    [InitializeOnLoad]
    internal static class InteractionSDKPackageInstaller
    {
        private static readonly string DefaultPath = $"{MixedRealityPreferences.ProfileGenerationPath}InteractionSDK";
        private static readonly string HiddenPath = Path.GetFullPath($"{PathFinderUtility.ResolvePath<IPathFinder>(typeof(InteractionSDKPathFinder)).ForwardSlashes()}{Path.DirectorySeparatorChar}{MixedRealityPreferences.HIDDEN_PACKAGE_ASSETS_PATH}");

        static InteractionSDKPackageInstaller()
        {
            EditorApplication.delayCall += CheckPackage;
        }

        [MenuItem(MixedRealityPreferences.Editor_Menu_Keyword + "/Packages/Install Interaction SDK Package Assets...", true)]
        private static bool ImportPackageAssetsValidation()
        {
            return !Directory.Exists($"{DefaultPath}{Path.DirectorySeparatorChar}");
        }

        [MenuItem(MixedRealityPreferences.Editor_Menu_Keyword + "/Packages/Install Interaction SDK Package Assets...")]
        private static void ImportPackageAssets()
        {
            EditorPreferences.Set($"{nameof(InteractionSDKPackageInstaller)}.Assets", false);
            EditorApplication.delayCall += CheckPackage;
        }

        private static void CheckPackage()
        {
            if (!EditorPreferences.Get($"{nameof(InteractionSDKPackageInstaller)}.Assets", false))
            {
                EditorPreferences.Set($"{nameof(InteractionSDKPackageInstaller)}.Assets", PackageInstaller.TryInstallAssets(HiddenPath, DefaultPath));
            }
        }
    }
}
