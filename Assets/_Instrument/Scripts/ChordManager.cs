using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jasper.Music
{
    // TODO: Have chords not rely on Scale!!! (what about chords outside of scale??)


    public class ChordManager : MusicDataStore
    {

        void Start()
        {

        }

        public int[] GetChord(string name)
        {
            if(dataStore.ContainsKey(name))
                return dataStore[name].notes;
            return null;
        }

        public void AddChord(string name, int[] notes)
        {
            AddMusicData(name, notes);
        }

        public bool RemoveChord(string name)
        {
            return RemoveMusicData(name);
        }

        // all scales are 0 indexed
        void CreateDefaultChords()
        {
            int[] one = new int[] { 0, 2, 4 };
            int[] three = new int[] { 2, 4, 6 };
            int[] five = new int[] { 6, 8, 1 }; // ????

            AddChord("1", one);
            AddChord("3", three);
            AddChord("5", five);
        }

    }
}