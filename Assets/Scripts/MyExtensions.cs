using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;

public static partial class MyExtensions {
    public static T PickRandomNotNull<T>(this T[] arr) {
        List<T> res = new List<T>();

        foreach (T t in arr) {
            if (t != null) res.Add(t);
        }

        return res.PickRandom();
    }

  
    public static T PickRandom<T>(this List<T> list) {
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    public static int GetCount<T>(this Dictionary<T, int> dict, T t) {
        if (dict.ContainsKey(t)) {
            return dict[t];
        } else {
            return 0;
        }
    }

    public static void AddI<T>(this Dictionary<T, int> dict, T t, int amt) {
        if (dict.ContainsKey(t)) {
            dict[t] += amt;
        } else {
            dict[t] = amt;
        }
    }

 

    public static bool ContainsAny<T>(this List<T> list, List<T> other) {

        foreach (T t in list) {
            if (other.Contains(t)) return true;
        }

        return false;
    }


    public static void AddValue<T>(this Dictionary<T, int> dictionary, T key, int amount) {
        int count;
        dictionary.TryGetValue(key, out count);
        dictionary[key] = count + amount;
    }


    public static string FirstCharToUpper(this string input) =>
      input switch
      {
          null => throw new ArgumentNullException(nameof(input)),
          "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
          _ => input.First().ToString().ToUpper() + input.Substring(1)
      };


    public static string ExceptChars(this string str, IEnumerable<char> toExclude) {
        StringBuilder sb = new StringBuilder(str.Length);
        for (int i = 0; i < str.Length; i++) {
            char c = str[i];
            if (!toExclude.Contains(c))
                sb.Append(c);
        }
        return sb.ToString();
    }

}


