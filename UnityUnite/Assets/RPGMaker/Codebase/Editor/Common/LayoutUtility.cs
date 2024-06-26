using System;
using System.IO;
using System.Reflection;

namespace RPGMaker.Codebase.Editor.Common
{
    public class LayoutUtility
    {
        private static MethodInfo GetMethod(MethodType method_type) {
            var layout = Type.GetType("UnityEditor.WindowLayout,UnityEditor");

            MethodInfo save = null;
            MethodInfo load = null;

            if (layout != null)
            {
                load = layout.GetMethod("LoadWindowLayout",
                    BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static, null,
                    new[] {typeof(string), typeof(bool)}, null);
                save = layout.GetMethod("SaveWindowLayout",
                    BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static, null,
                    new[] {typeof(string)}, null);
            }

            if (method_type == MethodType.Save)
                return save;
            return load;
        }

        public static void SaveLayout(string path) {
            path = Path.Combine(Directory.GetCurrentDirectory(), path);
            GetMethod(MethodType.Save).Invoke(null, new object[] {path});
        }

        public static void LoadLayout(string path) {
            //バージョンアップ時の隙間でのみエラーになる問題の対処
            try {
                path = Path.Combine(Directory.GetCurrentDirectory(), path);
                GetMethod(MethodType.Load).Invoke(null, new object[] {path, false});
            }
            catch (Exception) {
            }
        }

        private enum MethodType
        {
            Save,
            Load
        }
    }
}