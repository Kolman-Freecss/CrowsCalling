// -------------------------------------------------------------------
// @author Kolman-Freecss https://github.com/Kolman-Freecss
// MIT License
// -------------------------------------------------------------------

using System;

namespace _999__Working_Space.Kolman_Freecss.Modules.Utils
{
    /// <summary>
    /// Util class to serialize a dictionary inside a ScriptableObject
    /// <snippet>
    ///     List<SerializableDictionaryEntry<CameraMode, SkinView>> _skinViews;
    /// </snippet>
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    [Serializable]
    public class SerializableDictionaryEntry<TKey, TValue>
    {
        public TKey Key;
        public TValue Value;

        public SerializableDictionaryEntry(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
}