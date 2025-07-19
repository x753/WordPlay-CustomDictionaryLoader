using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CustomDictionaryLoader
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class CustomDictionaryLoaderMod : BaseUnityPlugin
    {
        private const string modGUID = "WordPlay.CustomDictionaryLoader";
        private const string modName = "CustomDictionaryLoader";
        private const string modVersion = "1.0.1";

        private readonly Harmony harmony = new Harmony(modGUID);
        internal static ManualLogSource ModLogger;

        public static CustomDictionaryLoaderMod Instance;

        public static bool OverwriteVanilla = false;
        public static bool LoadAppDataDictionary = true;

        public static List<string> CustomDictionaryPaths;
        public static List<string> CustomValidWords;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            harmony.PatchAll();
            ModLogger = BepInEx.Logging.Logger.CreateLogSource(modName);
            ModLogger.LogInfo($"Plugin {modName} is loaded!");

            OverwriteVanilla = Config.Bind("Dictionary", "Overwrite Vanilla Dictionary", false, "If true, custom dictionaries will overwrite the vanilla dictionary instead of adding words to it.").Value;
            LoadAppDataDictionary = Config.Bind("Dictionary", "Load Dictionary from AppData folder", true, "If true, a customdictionary.txt file in Word Play's AppData folder will be loaded if it exists.").Value;
        }

        // ==============================================================================
        // After the game loads the word list normally, overwrite or add to it
        // ==============================================================================
        [HarmonyPatch(typeof(WordChecker), "LoadData")]
        class WordChecker_LoadData_Patch
        {
            [HarmonyPostfix]
            public static void WordChecker_LoadData_Postfix(WordChecker __instance)
            {
                ModLogger.LogInfo("Loading custom dictionaries...");

                List<string> customDictionaryFolders = Directory.GetDirectories(Paths.PluginPath, "*", SearchOption.AllDirectories)
                    .Where(dir => string.Equals(Path.GetFileName(dir), "CustomDictionary", StringComparison.OrdinalIgnoreCase))
                    .ToList();

                CustomDictionaryPaths = customDictionaryFolders.SelectMany(d => Directory.EnumerateFiles(d, "*.txt")).ToList();

                string appDataDictionaryPath = Path.Combine(UnityEngine.Application.persistentDataPath, "customdictionary.txt");
                if (LoadAppDataDictionary && File.Exists(appDataDictionaryPath))
                {
                    CustomDictionaryPaths.Add(appDataDictionaryPath);
                    ModLogger.LogInfo($"A dictionary was loaded from: {appDataDictionaryPath}");
                }
                else
                {
                    ModLogger.LogInfo($"This dictionary will not be loaded based on config settings: {appDataDictionaryPath}");
                }

                CustomValidWords = CustomDictionaryPaths.SelectMany(file => File.ReadAllLines(file))
                    .Where(line => !string.IsNullOrWhiteSpace(line))
                    .Select(line => line.ToUpperInvariant())
                    .ToList();

                if (OverwriteVanilla)
                {
                    ModLogger.LogInfo($"Config is set to overwrite the vanilla dictionary. Default valid words have been removed.");
                    __instance.validWords = CustomValidWords.ToArray();
                }
                else
                {
                    __instance.validWords = __instance.validWords.Concat(CustomValidWords).ToArray();
                }
                __instance.stringSet = new HashSet<string>(__instance.validWords);
                __instance.validWords = __instance.stringSet.ToArray(); // deduplicate

                int customDictionaryCount = CustomDictionaryPaths.Count;
                int customWordCount = CustomValidWords.Count;
                int totalWordCount = __instance.validWords.Length;

                ModLogger.LogInfo($"Successfully loaded {customDictionaryCount} custom dictionaries containing {customWordCount} custom words!");
                ModLogger.LogInfo($"New total: {totalWordCount} words");
            }
        }
    }
}