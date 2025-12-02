using System.Collections;

public class MyHashTable
{
	public Hashtable h = new Hashtable();

	public object get(object k)
	{
		return h[k];
	}

	public void clear()
	{
		h.Clear();
	}

	public IDictionaryEnumerator GetEnumerator()
	{
		return h.GetEnumerator();
	}

	public int size()
	{
		return h.Count;
	}

	public void put(object k, object v)
	{
		if (h.ContainsKey(k))
		{
			h.Remove(k);
		}
		h.Add(k, v);
	}

	public void remove(object k)
	{
		h.Remove(k);
	}

	public void Remove(string key)
	{
		h.Remove(key);
	}

	public bool containsKey(object key)
	{
		return h.ContainsKey(key);
	}
}
