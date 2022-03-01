﻿using UnityEngine;

namespace Code.Utils
{
    public class GlobalConsts
    {
        public const           string PathToItems           = "Prefabs/Items/";
        public const           string PathToRecpies         = "ScriptableObjects/CookingRecipes/";
        public const           string PathToHexObjects      = "Prefabs/Objects/";
        public static readonly string PathToSaves           = Application.persistentDataPath + "/Saves/";
        public static readonly string PathToSettings        = Application.persistentDataPath + "/settings.xml";
        public const           string SaveFileExtension     = ".Kep";
        public const           string PathToEvents          = "ScriptableObjects/Events";
        public const           string PathToAnswers         = "ScriptableObjects/Answers";
        public const           string PathToTranslations    = "Translations/";
        public const           string DebugLanguage         = "PL_pl";
        public const           string DiscordLink           = "https://discord.gg/xKBvaAKU";
        public const           string TwitterLink           = "https://discord.gg/xKBvaAKU";
        public const           string MixerSFXProperyName   = "SFXVolume";
        public const           string MixerMusicProperyName = "MusicVolume";
        public const           string EmailHost             = "smtp.gmail.com";
        public const           string EmailUser             = "KeplerBugs@gmail.com";
        public const           string EmailDest             = "orchestrabussiness@orchestra-kings.eu";
        public const           string EmailPassword         = "AllYourBaseBelongToUs";
        public const           string EmailName             = "Bug Reporter";
        public const           int    EmailPort             = 587;

    }
}