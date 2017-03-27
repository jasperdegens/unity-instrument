Unity Interactive Instrument
============================
Easily add interactive music to your unity projects.

Requirements
------------
For midi output, please install [kejiro's midi bridge](https://github.com/keijiro/unity-midi-bridge).

Dependencies
------------
This package also containt [nobnak's unity-osc](https://github.com/nobnak/unity-osc).

Examples
--------
Check out examples scenes in the Assets/_Instrument/Scenes folder.

Basic Usage
===========

BaseInstrument:
---------------
####Playing Notes
- public virtual void PlayNote(int interval, NoteMode mode = STATIC_MODE, float duration = 1.5f)
This will send a midi note on command, and schedule an off command after duration.
- public virtual void NoteOn(int noteNum, int velocity = 127)
This will only play note, but will not schedule note off.
- public virtual void PlayChord(string chordName, float duration = 1.5f)
Note off event is scheduled for each note.

####Stoping Notes Manually
- public virtual void NoteOff(int noteNum)
Instrument will output noteOff midi command.

####Properties
- key (0 = c, 1 = c#, 2 = d, etc)
- minOctave, maxOctave (clamps octave ranges -- useful for RELATIVE_MODE)
- SetScale(string scale)
- enum OuputMode outPutMode
  * MIDI -> send midi commands to midi bridge
  * AUDIOSOURCE -> will try and play audio files (AudioSource.playOneShot(noteNum + ".wav"))
  * OSC -> sends midi message via osc under path = "/midi"

ScaleManager:
-------------
####Adding/Removing/Setting Scale:
- public void AddScale(string name, int[] notes)
- public bool RemoveScale(string name)
- public void SetScale(string name)
It is recommended to set scales in an instrument

ChordManager:
-------------
####Adding/Removing Chords:
- public void AddChord(string name, int[] notes)
- public bool RemoveChord(string name)





