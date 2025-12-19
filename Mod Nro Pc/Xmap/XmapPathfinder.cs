using System;
using System.Collections.Generic;

namespace Xmap
{
    public class XmapPathfinder
    {
        private static XmapPathfinder _instance;

        public static XmapPathfinder GetInstance()
        {
            return _instance ?? (_instance = new XmapPathfinder());
        }

        // =========================
        // FIND PATH (OPTIMIZED BFS)
        // =========================
        public int[] FindPath(int targetMapID, int currentMapID, long cPower, bool hasCompletedTask30)
        {
            if (currentMapID == targetMapID)
                return new int[] { currentMapID };

            Queue<int> queue = new Queue<int>();
            HashSet<int> visited = new HashSet<int>();
            Dictionary<int, int> parent = new Dictionary<int, int>();

            queue.Enqueue(currentMapID);
            visited.Add(currentMapID);

            while (queue.Count > 0)
            {
                int current = queue.Dequeue();

                if (!DataXmap.linkMaps.TryGetValue(current, out List<NextMap> nextMaps))
                    continue;

                foreach (NextMap next in nextMaps)
                {
                    int nextMapID = next.MapID;

                    if (visited.Contains(nextMapID))
                        continue;

                    if (!CanMoveToMap(current, nextMapID, hasCompletedTask30))
                        continue;

                    if (!CheckMapPowerRequirement(nextMapID, cPower))
                        continue;

                    if (DataXmap.futureMapSet.Contains(nextMapID) &&
                        Char.myCharz().taskMaint.taskId <= 24)
                        continue;

                    visited.Add(nextMapID);
                    parent[nextMapID] = current;

                    // ===== Found target -> build path and return immediately =====
                    if (nextMapID == targetMapID)
                        return BuildPath(parent, currentMapID, targetMapID);

                    queue.Enqueue(nextMapID);
                }
            }

            return null;
        }

        // =========================
        // BUILD PATH FROM PARENT
        // =========================
        private int[] BuildPath(Dictionary<int, int> parent, int start, int end)
        {
            List<int> path = new List<int>();
            int current = end;

            while (current != start)
            {
                path.Add(current);
                current = parent[current];
            }

            path.Add(start);
            path.Reverse();

            return path.ToArray();
        }

        // =========================
        // MOVE CONDITIONS
        // =========================
        private bool CanMoveToMap(int currentMap, int nextMapID, bool hasCompletedTask30)
        {
            // Logic cũ giữ nguyên
            if (currentMap == 19 && nextMapID == 109 && !hasCompletedTask30)
                return true;

            if (hasCompletedTask30 || nextMapID < 105 || nextMapID > 110)
                return true;

            return false;
        }

        // =========================
        // POWER CHECK
        // =========================
        private bool CheckMapPowerRequirement(int mapID, long cPower)
        {
            if (mapID != 155 && mapID >= 153 && mapID <= 159 && cPower < 40000000000L)
                return false;

            if ((mapID == 155 || mapID == 166) && cPower < 60000000000L)
                return false;

            return true;
        }

        // =========================
        // ERROR MESSAGE
        // =========================
        public string GetPathErrorMessage(int targetMapID, int currentMapID, long currentPower, bool hasCompletedTask30)
        {
            if (CheckMapPowerError(targetMapID, currentPower, out string error))
                return error;

            if (CheckTaskError(targetMapID, out error))
                return error;

            if (CheckClanError(targetMapID, out error))
                return error;

            if (targetMapID == 160 && !ModProCL.ExistItemBag(992))
                return "Không có Nhẫn thời không!";

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

            if (DataXmap.IsFutureMap(mapID) &&
                Char.myCharz().taskMaint.taskId <= 24)
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
                return false;

            if (DataXmap.khiGasMapSet.Contains(mapID) ||
                DataXmap.manhVoBTMapSet.Contains(mapID) ||
                (mapID >= 53 && mapID <= 62))
            {
                error = $"Cần có pt để vào map {mapID}.";
                return true;
            }

            return false;
        }

        // =========================
        // NEXT MAP SELECTOR
        // =========================
        public NextMap FindNextMapToGo(int currentMapID, int nextMapID)
        {
            if (!DataXmap.linkMaps.TryGetValue(currentMapID, out List<NextMap> maps))
                return null;

            NextMap fallback = null;

            foreach (NextMap map in maps)
            {
                if (map.MapID != nextMapID)
                    continue;

                if (map.NpcID != -1 || map.walk)
                    return map;

                if (map.NpcID == -1 && string.IsNullOrEmpty(map.NameIndex1) && !map.walk)
                    fallback = map;
            }

            return fallback;
        }

        // =========================
        // VALIDATE PATH POWER
        // =========================
        public bool ValidatePathPowerRequirement(int[] path, long power, out string errorMessage)
        {
            errorMessage = null;

            foreach (int mapID in path)
            {
                if (mapID == 154 && power < 40000000000L)
                {
                    errorMessage = $"Không thể đi qua map 154 vì sức mạnh {power:N0} < {40000000000L:N0}.";
                    return false;
                }

                if ((mapID == 155 || mapID == 166) && power < 60000000000L)
                {
                    errorMessage = $"Không thể đi qua map {mapID} vì sức mạnh {power:N0} < {60000000000L:N0}.";
                    return false;
                }

                if (mapID >= 153 && mapID <= 159 && mapID != 155 && power < 40000000000L)
                {
                    errorMessage = $"Không thể đi qua map {mapID} vì sức mạnh {power:N0} < {40000000000L:N0}.";
                    return false;
                }
            }

            return true;
        }
    }
}
