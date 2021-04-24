﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using utils;

namespace Audio {
	public class AudioSourcePool : ObjectPool<AudioSource> {
		override protected AudioSource Spawn() {
			var source = base.Spawn();

			source.playOnAwake = false;
			source.enabled = false;

			return source;
		}

		override public AudioSource GetObject(){
			var obj = base.GetObject();
			obj.enabled = true;

			return obj;
		}

		public void StopAll(){
			foreach (var source in objects) {
				source.Stop ();
				Disable(source);
			}
		}

		public void Disable(AudioSource source, AudioClip clip){
			if (objects.Contains (source) && source.clip == clip) {
				source.enabled = false;
			}
		}
	}
}
