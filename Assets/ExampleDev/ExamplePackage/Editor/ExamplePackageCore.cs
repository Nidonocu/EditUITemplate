#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System;
using System.IO;

namespace ExampleDev.ExamplePackage
{
    [InitializeOnLoad]
    public class ExamplePackageCore : MonoBehaviour
    {
        public static Version Version = new Version(0, 1, 0);

        /// <summary>
        /// Static Constructor to register events and perform OOBE
        /// </summary>
        static ExamplePackageCore()
        {
            LoadAutoState();
            if (AlwaysShow && !EditorApplication.isPlaying)
            {
                EditorApplication.update -= ShowAboutWindow;
                EditorApplication.update += ShowAboutWindow;
            }
        }

        [MenuItem("ExampleDev/Example Package/Perform Action", false, 100)]
        public static void DoAction()
        {
            EditorUtility.DisplayDialog(
                "Example Package", 
                "You just clicked a menu item. This could be used to perform some action.",
                "OK"
                );
        }

        public static void OpenSupportFile(string targetGUID)
        {
            if (PackageHunter.IsFileGuidPresent(targetGUID))
            {
                var assetPath = Application.dataPath + AssetDatabase.GUIDToAssetPath(targetGUID).Substring(6);
                Debug.Log("Opening " + assetPath + "...");

                Application.OpenURL(assetPath);
            }
            else
            {
                EditorUtility.DisplayDialog(
                    "Example Package",
                    "Sorry but Unity was unable to find the document! You may need to reinstall the package.",
                    "OK"
                );
            }
        }

        /// <summary>
        /// Displays About box
        /// </summary>
        [MenuItem("ExampleDev/Example Package/About Example", false, 112)]
        static void ShowAboutWindow()
        {
            EditorApplication.update -= ShowAboutWindow;

            var window = (ExampleWindow)EditorWindow.GetWindow(typeof(ExampleWindow), true, "Example Package - About");

            var width = UIHelpers.AboutWindowWidth + 20;
            var height = 850;
            int x = 300;
            int y = 100;
            window.position = new Rect(x, y, width, height);

            window.VRCFuryInstalled = PackageHunter.IsPackageInstalled(PackageHunter.VRCFuryPackageName);

            window.ReadMeAssetReady = PackageHunter.IsFileGuidPresent(PackageHunter.ReadmeGUID);

            window.ShaderReady = PackageHunter.IsShaderPresent(PackageHunter.PoiyomiShaderName);

            window.Show();
            window.Focus();
        }

        private static bool m_AlwaysShow = true;

        public static bool AlwaysShow
        {
            get { return m_AlwaysShow; }
            set
            {
                m_AlwaysShow = value;
                SaveAutoState();
            }
        }

        private static string GetSettingsPath()
        {
            return Path.Combine(new DirectoryInfo(Application.dataPath).Parent?.FullName, "ProjectSettings", "ExampleDev-ExamplePackage");
        }

        private static void EnsurePathExists()
        {
            if (!Directory.Exists(GetSettingsPath()))
            {
                Directory.CreateDirectory(GetSettingsPath());
            }
        }

        const string settingsFilename = "settings.json";

        private static void SaveAutoState()
        {
            EnsurePathExists();
            var settings = new ExamplePackageSettings() { AlwaysShow = m_AlwaysShow };
            var json = JsonUtility.ToJson(settings);
            File.WriteAllText(Path.Combine(GetSettingsPath(), settingsFilename), json);
        }

        private static void LoadAutoState()
        {
            EnsurePathExists();
            var path = Path.Combine(GetSettingsPath(), settingsFilename);
            if (File.Exists(path))
            {
                var settings = new ExamplePackageSettings();
                EditorJsonUtility.FromJsonOverwrite(File.ReadAllText(path), settings);
                m_AlwaysShow = settings.AlwaysShow;
            }
        }
    }

    public class ExamplePackageSettings
    {
        public bool AlwaysShow = true;
    }
}
#endif