using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;

namespace utils {
	public class ObjectPool<T> : MonoBehaviour where T : Behaviour {
		public Transform parent;
		public T prefab;
		public int amount;

		[ReadOnly] public List<T> objects;
		protected void Awake(){
			objects = new List<T> ();

			for (int i = 0; i < amount; i++) {
				var obj = Spawn();
				Disable(obj);
			}

			print("Terminou de spawn " + name);
		}

		virtual protected T Spawn(){
			T obj = null;

			if (prefab == null) {
				var go = new GameObject();
				obj = go.AddComponent<T>();
			} else {
				obj = Instantiate(prefab);
			}

			obj.gameObject.transform.SetParent(parent);
			obj.gameObject.name = typeof(T).ToString();

			objects.Add(obj);

			return obj;
		}

		virtual public T GetObject(){
			foreach (var obj in objects) {
				if (obj != null && !obj.gameObject.activeSelf) {
					obj.gameObject.SetActive(true);
					return obj;
				}
			}

			return Spawn();
		}

		public void Disable(T obj){
			if (objects.Contains (obj)) {
				obj.gameObject.SetActive(false);
			}
		}

		public List<T> GetAllActive(){
			return objects.Where(x => x != null && x.gameObject.activeSelf).ToList();
		}
	}
}
