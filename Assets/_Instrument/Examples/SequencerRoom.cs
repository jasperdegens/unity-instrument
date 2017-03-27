using UnityEngine;
using System.Collections;
using jasper.Music.Sequencer;

public class SequencerRoom : MonoBehaviour {

	int numSeqs = 8;
	public SequencerMultiTrack[] seqs;

	// Use this for initialization
	void Start () {
		seqs = new SequencerMultiTrack[numSeqs];
		for (int i = 0; i < numSeqs; i++) {
			GameObject go = new GameObject ();
			SequencerMultiTrack seq = go.AddComponent<SequencerMultiTrack> ();
			go.transform.position = Vector3.right * i;
			//go.transform.parent = gameObject.transform;
			seq.tracks = 1;
			seq.steps = 16;

			seqs [i] = seq;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
