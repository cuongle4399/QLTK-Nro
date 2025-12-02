using System;
using System.Collections.Generic;

namespace Client244.Xmap;

public class XmapPathfinder
{
	private static XmapPathfinder _instance;

	public static XmapPathfinder GetInstance()
	{
		return _instance ?? (_instance = new XmapPathfinder());
	}

	public int[] FindPath(int targetMapID, int currentMapID, long cPower, bool hasCompletedTask30)
	{
		List<int[]> list = new List<int[]>();
		Queue<int[]> queue = new Queue<int[]>();
		HashSet<int> hashSet = new HashSet<int>();
		queue.Enqueue(new int[1] { currentMapID });
		hashSet.Add(currentMapID);
		while (queue.Count > 0)
		{
			int[] array = queue.Dequeue();
			int num = array[array.Length - 1];
			if (num == targetMapID)
			{
				if (IsValidPath(array, cPower, hasCompletedTask30))
				{
					list.Add(array);
				}
			}
			else
			{
				if (!DataXmap.linkMaps.ContainsKey(num))
				{
					continue;
				}
				foreach (NextMap item in DataXmap.linkMaps[num])
				{
					int mapID = item.MapID;
					if (!hashSet.Contains(mapID) && CanMoveToMap(num, mapID, hasCompletedTask30))
					{
						AddPathToQueue(array, mapID, queue, hashSet);
					}
				}
			}
		}
		return GetShortestPath(list);
	}

	private bool CanMoveToMap(int currentMap, int nextMapID, bool hasCompletedTask30)
	{
		if (currentMap == 19 && nextMapID == 109 && !hasCompletedTask30)
		{
			return true;
		}
		if (hasCompletedTask30 || nextMapID < 105 || nextMapID > 110)
		{
			return true;
		}
		return false;
	}

	private void AddPathToQueue(int[] currentPath, int nextMapID, Queue<int[]> queue, HashSet<int> visited)
	{
		visited.Add(nextMapID);
		int[] array = new int[currentPath.Length + 1];
		Array.Copy(currentPath, array, currentPath.Length);
		array[currentPath.Length] = nextMapID;
		queue.Enqueue(array);
	}

	private bool IsValidPath(int[] path, long cPower, bool hasCompletedTask30)
	{
		if (HasFutureMapLoop(path))
		{
			return false;
		}
		if (!hasCompletedTask30 && HasColdMapInPath(path))
		{
			return false;
		}
		foreach (int num in path)
		{
			if (!CheckMapPowerRequirement(num, cPower))
			{
				return false;
			}
			if (DataXmap.futureMapSet.Contains(num) && Char.myCharz().taskMaint.taskId <= 24)
			{
				return false;
			}
		}
		return true;
	}

	private bool CheckMapPowerRequirement(int mapID, long cPower)
	{
		if (mapID != 155 && mapID >= 153 && mapID <= 159 && cPower < 40000000000L)
		{
			return false;
		}
		if ((mapID == 155 || mapID == 166) && cPower < 60000000000L)
		{
			return false;
		}
		return true;
	}

	private bool HasFutureMapLoop(int[] path)
	{
		for (int i = 1; i < path.Length - 1; i++)
		{
			if (path[i] == 102 && path[i + 1] == 24 && DataXmap.futureMapSet.Contains(path[i - 1]))
			{
				return true;
			}
		}
		return false;
	}

	private bool HasColdMapInPath(int[] path)
	{
		foreach (int num in path)
		{
			if (num >= 105 && num <= 110)
			{
				return true;
			}
		}
		return false;
	}

	private int[] GetShortestPath(List<int[]> paths)
	{
		if (paths.Count == 0)
		{
			return null;
		}
		int num = int.MaxValue;
		int[] result = null;
		foreach (int[] path in paths)
		{
			if (path.Length < num)
			{
				num = path.Length;
				result = path;
			}
		}
		return result;
	}

	public string GetPathErrorMessage(int targetMapID, int currentMapID, long currentPower, bool hasCompletedTask30)
	{
		if (CheckMapPowerError(targetMapID, currentPower, out var error))
		{
			return error;
		}
		if (CheckTaskError(targetMapID, out error))
		{
			return error;
		}
		if (CheckClanError(targetMapID, out error))
		{
			return error;
		}
		if (targetMapID == 160 && !ModProCL.ExistItemBag(992))
		{
			return "Không có Nhẫn thời không!";
		}
		return $"Không thể tìm thấy đường đi từ map {currentMapID} đến map {targetMapID}.";
	}

	private bool CheckMapPowerError(int mapID, long power, out string error)
	{
		error = null;
		if (mapID == 154 && power < 40000000000L)
		{
			error = $"Yêu cầu sức mạnh tối thiểu cho map 154: {40000000000L:N0}.";
			return true;
		}
		if ((mapID == 155 || mapID == 166) && power < 60000000000L)
		{
			error = $"Yêu cầu sức mạnh tối thiểu cho map {mapID}: {60000000000L:N0}.";
			return true;
		}
		if (mapID >= 153 && mapID <= 159 && mapID != 155 && power < 40000000000L)
		{
			error = $"Yêu cầu sức mạnh tối thiểu cho map {mapID}: {40000000000L:N0}.";
			return true;
		}
		return false;
	}

	private bool CheckTaskError(int mapID, out string error)
	{
		error = null;
		if (DataXmap.IsFutureMap(mapID) && Char.myCharz().taskMaint.taskId <= 24)
		{
			error = $"Hãy hoàn thành nhiệm vụ để vào map {mapID}.";
			return true;
		}
		return false;
	}

	private bool CheckClanError(int mapID, out string error)
	{
		error = null;
		if (Char.myCharz().clan != null)
		{
			return false;
		}
		if (DataXmap.khiGasMapSet.Contains(mapID) || DataXmap.manhVoBTMapSet.Contains(mapID) || (mapID >= 53 && mapID <= 62))
		{
			error = $"Cần có pt để vào map {mapID}.";
			return true;
		}
		return false;
	}

	public NextMap FindNextMapToGo(int currentMapID, int nextMapID)
	{
		if (!DataXmap.linkMaps.ContainsKey(currentMapID))
		{
			return null;
		}
		NextMap nextMap = null;
		NextMap nextMap2 = null;
		foreach (NextMap item in DataXmap.linkMaps[currentMapID])
		{
			if (item.MapID == nextMapID)
			{
				if (item.NpcID != -1 || item.walk)
				{
					nextMap = item;
					break;
				}
				if (item.NpcID == -1 && string.IsNullOrEmpty(item.NameIndex1) && !item.walk)
				{
					nextMap2 = item;
				}
			}
		}
		return nextMap ?? nextMap2;
	}

	public bool ValidatePathPowerRequirement(int[] path, long power, out string errorMessage)
	{
		errorMessage = null;
		foreach (int num in path)
		{
			if (num == 154 && power < 40000000000L)
			{
				errorMessage = $"Không thể đi qua map 154 vì sức mạnh {power:N0} < {40000000000L:N0}.";
				return false;
			}
			if ((num == 155 || num == 166) && power < 60000000000L)
			{
				errorMessage = $"Không thể đi qua map {num} vì sức mạnh {power:N0} < {60000000000L:N0}.";
				return false;
			}
			if (num >= 153 && num <= 159 && num != 155 && power < 40000000000L)
			{
				errorMessage = $"Không thể đi qua map {num} vì sức mạnh {power:N0} < {40000000000L:N0}.";
				return false;
			}
		}
		return true;
	}
}
