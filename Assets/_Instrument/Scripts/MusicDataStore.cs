using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jasper.Music
{

    public struct BaseMusicData
    {
        public string name;
        public int[] notes;
        public int length;

        public BaseMusicData(string n, int[] noteArr)
        {
            name = n;
            notes = noteArr;
            length = noteArr.Length;
        }
    }

    public class MusicDataStore : MonoBehaviour
    {

        protected Dictionary<string, BaseMusicData> dataStore = new Dictionary<string, BaseMusicData>();

        protected void AddMusicData(string name, int[] notes)
        {
            // base behaviour is to add to dataStore, overwrite if already exists
            // ensure notes have at least one value
            if (notes.Length > 0)
            {
                BaseMusicData data = new BaseMusicData(name, notes);
                dataStore.Add(name, data);
            }
        }

        protected bool RemoveMusicData(string name)
        {
            return dataStore.Remove(name);
        }


    }

}