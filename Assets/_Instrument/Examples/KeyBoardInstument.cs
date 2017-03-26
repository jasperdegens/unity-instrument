/* ************************************
 * KeyBoardInstrument.cs
 * By Jasper Degens
 * 26-03-2017
 * 
 * This is an example of how to create a
 * playable instrument. Uses keybindings
 * to the keyboard. A-L are mapped to notes,
 * Z-M are mapped to chords. 
 * ************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jasper.Music { 

    public class KeyBoardInstument : BaseInstrument {

		public override void Start ()
		{
			base.Start ();
		}


		override public void Update(){
			
            CheckKeyboardEvents();
			base.Update ();
            
        }

        void CheckKeyboardEvents()
        {
			// reset
			if (Input.GetKeyDown (KeyCode.R)) 
			{
				base.currInterval = 0;
			}


            // as much as it sucks need to check each key
            if (Input.GetKeyDown(KeyCode.A))
            { 
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

			// play chords
			if (Input.GetKeyDown (KeyCode.Z)) 
			{
				PlayChord ("1");
			}
			if (Input.GetKeyDown (KeyCode.X)) 
			{
				PlayChord ("2");
			}
			if (Input.GetKeyDown (KeyCode.C)) 
			{
				PlayChord ("3");
			}
			if (Input.GetKeyDown (KeyCode.V)) 
			{
				PlayChord ("4");
			}
			if (Input.GetKeyDown (KeyCode.B)) 
			{
				PlayChord ("5");
			}
			if (Input.GetKeyDown (KeyCode.N)) 
			{
				PlayChord ("6");
			}
			if (Input.GetKeyDown (KeyCode.M)) 
			{
				PlayChord ("7");
			}
		
		}

    }
}
