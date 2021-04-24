using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace utils {
	public class ScriptableVariable<T> : ScriptableObject {
		public delegate void ValueEvent(T newValue);

		public event ValueEvent onValueChange;

		public bool ResetEveryTime = true;

		public T initValue;
		public T _value;
		public T Value {
			get {
				return _value;
			} set {
				OnValueChange(value);
				_value = value;
			}
		}

		virtual protected void OnEnable(){ // This ensures that the value does not saves beetween playthorughs, but it may cause problems while changing scenes
			Value = initValue;
		}

		protected void OnValueChange(T newValue){
			if (onValueChange != null) {
				onValueChange(newValue);
			}
		}

		public void Reset(){
			if (ResetEveryTime) {
				OnEnable();
			}
		}
	}
}
