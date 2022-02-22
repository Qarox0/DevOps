using UnityEngine;

namespace Code.Utils
{
    public class GlobalConsts
    {
        public const           string PathToItems       = "Prefabs/Items/";
        public const           string PathToRecpies     = "ScriptableObjects/CookingRecipes/";
        public const           string PathToHexObjects  = "Prefabs/Objects/";
        public static readonly string PathToSaves       = Application.persistentDataPath + "/Saves/";
        public const           string SaveFileExtension = ".Kep";
        public const           string PathToEvents      = "ScriptableObjects/Events";
        public const           string PathToAnswers      = "ScriptableObjects/Answers";
    }
}