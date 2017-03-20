using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jasper.Music { 

    public class KeyBoardInstument : BaseInstrument {

        private void Start()
        {
            base.Start();
        }

        public void OnReceive(Osc.OscPort.Capsule c)
        {
            MidiCommand com = (MidiCommand)JsonUtility.FromJson((string)c.message.data[0], typeof(MidiCommand));
            print(com.status);
        }


		void Update(){
			
            CheckKeyboardEvents();
			base.Update ();
            
        }

        void CheckKeyboardEvents()
        {
            // as much as it sucks need to check each key
            if (Input.GetKeyDown(KeyCode.A))
            {

                FlowerManager.Instance.SetFlower(FLOWER_TYPE.AJISAI, GrowingFlower.MODE.PONG, Vector3.zero, Quaternion.LookRotation(Vector3.one, Vector3.up), Vector3.one, Vector3.zero, 0.9f, Random.value * 0.1f);


                int interval = noteMode == NoteMode.RELATIVE_MODE ? -4 : 0;
                PlayNote(interval);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                int interval = noteMode == NoteMode.RELATIVE_MODE ? -3 : 1;
                PlayNote(interval);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                int interval = noteMode == NoteMode.RELATIVE_MODE ? -2 : 2;
                PlayNote(interval); ;
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                int interval = noteMode == NoteMode.RELATIVE_MODE ? -1 : 3;
                PlayNote(interval);
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                int interval = noteMode == NoteMode.RELATIVE_MODE ? -0 : 4;
                PlayNote(interval);
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                int interval = noteMode == NoteMode.RELATIVE_MODE ? 1 : 5;
                PlayNote(interval);
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                int interval = noteMode == NoteMode.RELATIVE_MODE ? 2 : 6;
                PlayNote(interval);
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                int interval = noteMode == NoteMode.RELATIVE_MODE ? 3 : 7;
                PlayNote(interval);
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                int interval = noteMode == NoteMode.RELATIVE_MODE ? 4 : 8;
                PlayNote(interval);
            }
        }
    }
}
