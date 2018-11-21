using System.Collections.Generic;
using UnityEngine;

public static class TextureLoader {
	private static readonly Dictionary<string, Sprite> _dictionary = new Dictionary<string, Sprite>();

	public static Sprite Load(string path) {
		if (!_dictionary.ContainsKey(path)) {
			_dictionary[path] = Resources.Load<Sprite>(path);
		}
		return _dictionary[path];
	}

	public static void Clear() {
		_dictionary.Clear();
	}
}