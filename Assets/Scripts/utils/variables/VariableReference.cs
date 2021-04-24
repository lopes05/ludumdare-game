using System;

namespace utils {
	[Serializable]
	public class VariableReference<T, U> where U : ScriptableVariable<T> {
		public bool UseConstant = true;
		public T ConstantValue;
		public U Variable;

		public VariableReference() { }

		public VariableReference(T value) {
			UseConstant = true;
			ConstantValue = value;
		}

		public T Value {
			get { return UseConstant ? ConstantValue : Variable.Value; }
			set { 
				if (UseConstant) {
					ConstantValue = value;
				} else {
					Variable.Value = value;
				}
			}
		}

		public static implicit operator T(VariableReference<T, U> reference) {
			return reference.Value;
		}
	}
}
