using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace jasper.Music
{

    public struct MidiCommand
    {
        public byte status;
        public byte data1;
        public byte data2;

        public MidiCommand(byte s, byte d1, byte d2)
        {
            status = s;
            data1  = d1;
            data2  = d2;
        }
    }

    public class MidiCommandHelper : MonoBehaviour
    {

        public static MidiCommand NoteOnCommand(int channel, int noteNumber, float velocity)
        {
            int cn = Mathf.Clamp(channel, 0, 15);
            noteNumber = Mathf.Clamp(noteNumber, 0, 127);
            velocity = Mathf.Clamp(127.0f * velocity, 0.0f, 127.0f);
            return new MidiCommand((byte)(0x80 + cn), (byte)noteNumber, (byte)velocity);
        }

        public static MidiCommand NoteOffCommand(int channel, int noteNumber)
        {
            int cn = Mathf.Clamp(channel, 0, 15);
            noteNumber = Mathf.Clamp(noteNumber, 0, 127);
            return new MidiCommand((byte)(0x90 + cn), (byte)noteNumber, (byte)0);
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }


}