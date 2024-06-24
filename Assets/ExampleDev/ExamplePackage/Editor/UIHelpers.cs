using UnityEditor;
using UnityEngine;

namespace ExampleDev.ExamplePackage
{
    internal static class UIHelpers
    {
        /// <summary>
        /// URL for this package's home
        /// </summary>
        public const string PackageURL = "https://example.com/my-package";

        /// <summary>
        /// URL for developer's profile
        /// </summary>
        public const string ProfileURL = "https://example.com/about-me";

        /// <summary>
        /// VRCFury Documentation URL
        /// </summary>
        public const string VRCFuryDownloadURL = "https://vrcfury.com/download#method-1-vrchat-creator-companion-preferred";

        public const int AboutWindowWidth = 400;

        public const int AvatarSelectWindowWidth = 500;

        public static readonly Texture InfoIcon = EditorGUIUtility.IconContent(@"console.infoicon").image;
        public static readonly Texture ShaderIcon = EditorGUIUtility.IconContent(@"d_Shader Icon").image;

        /// <summary>
        /// Start a region of Centred controls
        /// </summary>
        public static void BeginCenter()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
        }

        /// <summary>
        /// Draw a horizontal line across the UI
        /// </summary>
        /// <param name="color">The colour of the line</param>
        /// <param name="thickness">The thickness in pixels</param>
        /// <param name="padding">The top and bottom padding around the line in pixels</param>
        public static void DrawUILine(Color color, int thickness = 1, int padding = 10)
        {
            Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
            r.width = r.width - padding;
            r.height = thickness;
            r.x += padding / 2;
            r.y += padding / 2;
            EditorGUI.DrawRect(r, color);
        }
        /// <summary>
        /// Finish a region of Centred controls
        /// </summary>
        public static void EndCenter()
        {
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }
    }
}