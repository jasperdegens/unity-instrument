/* ************************************
 * ChordManager.cs
 * By Jasper Degens
 * 26-03-2017
 * 
 * ChordManager contains several core chords.
 * 
 * ************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jasper.Music
{
    // TODO: Have chords not rely on Scale!!! (what about chords outside of scale??)


    public class ChordManager : MusicDataStore
    {

		public ChordManager(){
			CreateDefaultChords ();
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
			int[] two = new int[] { 1, 3, 5 };
			int[] four = new int[] { 3, 5, 7 };
			int[] six = new int[] { 5, 7, 9 };
			int[] seventh = new int[] { 0, 2, 4, 6 };

            AddChord("1", one);
            AddChord("3", three);
			AddChord("5", five);
			AddChord("2", two);
			AddChord("4", four);
			AddChord("6", six);
			AddChord("7", seventh);
        }

    }
}