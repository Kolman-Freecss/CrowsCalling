// -------------------------------------------------------------------
// @author Kolman-Freecss https://github.com/Kolman-Freecss
// MIT License
// -------------------------------------------------------------------

using _999__Working_Space.Kolman_Freecss.Modules.AudioModule;
using UnityEngine;

namespace _999__Working_Space.Kolman_Freecss.Modules.Common.Config
{
    public static class ClientPrefs
    {
        const string k_MasterVolumeKey = "MasterVolume";
        const string k_MusicVolumeKey = "MusicVolume";
        const string k_SFXVolumeKey = "SFXVolume";
        
        const string k_MasterMuteKey = "MasterMute";
        const string k_MusicMuteKey = "MusicMute";
        const string k_SFXMuteKey = "SFXMute";
        
        public static float GetMasterVolume()
        {
            return PlayerPrefs.GetFloat(k_MasterVolumeKey, SoundManager.MasterAudioVolume);
        }
        
        public static void SetMasterVolume(float volume)
        {
            PlayerPrefs.SetFloat(k_MasterVolumeKey, volume);
        }
        
        public static float GetMusicVolume()
        {
            return PlayerPrefs.GetFloat(k_MusicVolumeKey, SoundManager.MusicAudioVolume);
        }
        
        public static void SetMusicVolume(float volume)
        {
            PlayerPrefs.SetFloat(k_MusicVolumeKey, volume);
        }
        
        public static float GetSFXVolume()
        {
            return PlayerPrefs.GetFloat(k_SFXVolumeKey, SoundManager.SFXAudioVolume);
        }
        
        public static void SetSFXVolume(float volume)
        {
            PlayerPrefs.SetFloat(k_SFXVolumeKey, volume);
        }
        
        public static bool GetMasterMute()
        {
            return PlayerPrefs.GetInt(k_MasterMuteKey, 0) == 1;
        }
        
        public static void SetMasterMute(bool mute)
        {
            PlayerPrefs.SetInt(k_MasterMuteKey, mute ? 1 : 0);
        }
        
        public static bool GetMusicMute()
        {
            return PlayerPrefs.GetInt(k_MusicMuteKey, 0) == 1;
        }
        
        public static void SetMusicMute(bool mute)
        {
            PlayerPrefs.SetInt(k_MusicMuteKey, mute ? 1 : 0);
        }
        
        public static bool GetSFXMute()
        {
            return PlayerPrefs.GetInt(k_SFXMuteKey, 0) == 1;
        }
        
        public static void SetSFXMute(bool mute)
        {
            PlayerPrefs.SetInt(k_SFXMuteKey, mute ? 1 : 0);
        }
        
        public static void ResetAll()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
    }
}