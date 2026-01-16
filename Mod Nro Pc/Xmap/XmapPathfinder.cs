using System.Collections.Generic;

namespace Xmap
{
    public class XmapPathfinder
    {
        private static XmapPathfinder _instance;

        #region Constants - Power Requirements
        private const long POWER_40B = 40000000000L;
        private const long POWER_60B = 60000000000L;
        #endregion

        #region Constants - Map Ranges
        private const int SPECIAL_MAP_MIN = 156;
        private const int SPECIAL_MAP_MAX = 159;
        private const int COLD_MAP_MIN = 105;
        private const int COLD_MAP_MAX = 110;
        private const int CLAN_MAP_MIN = 53;
        private const int CLAN_MAP_MAX = 62;
        #endregion

        #region Singleton
        public static XmapPathfinder GetInstance()
        {
            return _instance ?? (_instance = new XmapPathfinder());
        }

        private XmapPathfinder() { }
        #endregion

        #region Pathfinding - BFS Algorithm
        public int[] FindPath(int targetMapID, int currentMapID, long cPower, bool hasCompletedTask30)
        {
            if (currentMapID == targetMapID)
                return new int[] { currentMapID };

            var context = new PathfindingContext
            {
                CurrentPower = cPower,
                HasCompletedTask30 = hasCompletedTask30,
                Queue = new Queue<int>(),
                Visited = new HashSet<int>(),
                Parent = new Dictionary<int, int>()
            };

            context.Queue.Enqueue(currentMapID);
            context.Visited.Add(currentMapID);

            while (context.Queue.Count > 0)
            {
                int current = context.Queue.Dequeue();

                if (!DataXmap.linkMaps.TryGetValue(current, out List<NextMap> nextMaps))
                    continue;

                foreach (NextMap next in nextMaps)
                {
                    if (!TryProcessNextMap(next, current, targetMapID, context))
                        continue;

                    if (next.MapID == targetMapID)
                        return BuildPath(context.Parent, currentMapID, targetMapID);
                }
            }

            return null;
        }

        private bool TryProcessNextMap(
            NextMap next,
            int current,
            int target,
            PathfindingContext context)
        {
            int nextMapID = next.MapID;

            if (context.Visited.Contains(nextMapID))
                return false;

            if (!CanMoveToMap(current, nextMapID, context.HasCompletedTask30))
                return false;

            if (!CheckMapPowerRequirement(nextMapID, context.CurrentPower))
                return false;

            if (DataXmap.futureMapSet.Contains(nextMapID) &&
                Char.myCharz().taskMaint.taskId <= 24)
                return false;

            context.Visited.Add(nextMapID);
            context.Parent[nextMapID] = current;
            context.Queue.Enqueue(nextMapID);

            return true;
        }

        private class PathfindingContext
        {
            public long CurrentPower { get; set; }
            public bool HasCompletedTask30 { get; set; }
            public Queue<int> Queue { get; set; }
            public HashSet<int> Visited { get; set; }
            public Dictionary<int, int> Parent { get; set; }
        }
        #endregion

        #region Path Building
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
        #endregion

        #region Movement Validation
        private bool CanMoveToMap(int currentMap, int nextMapID, bool hasCompletedTask30)
        {
            if (currentMap == 19 && nextMapID == 109)
                return hasCompletedTask30;

            if (nextMapID >= COLD_MAP_MIN && nextMapID <= COLD_MAP_MAX)
                return hasCompletedTask30;

            return true;
        }

        private bool CheckMapPowerRequirement(int mapID, long cPower)
        {
            if (IsHighPowerMap(mapID))
                return cPower >= POWER_60B;

            if (IsMediumPowerMap(mapID))
                return cPower >= POWER_40B;

            return true;
        }

        private bool IsHighPowerMap(int mapID)
        {
            return mapID == 155 || mapID == 166;
        }

        private bool IsMediumPowerMap(int mapID)
        {
            return mapID >= SPECIAL_MAP_MIN &&
                   mapID <= SPECIAL_MAP_MAX;
        }
        #endregion

        #region Error Messages
        public string GetPathErrorMessage(
            int targetMapID,
            int currentMapID,
            long currentPower,
            bool hasCompletedTask30)
        {
            if (CheckMapPowerError(targetMapID, currentPower, out string error))
                return error;

            if (CheckTaskError(targetMapID, out error))
                return error;

            if (CheckClanError(targetMapID, out error))
                return error;

            if (CheckItemError(targetMapID, out error))
                return error;

            return $"Không thể tìm thấy đường đi từ map {currentMapID} đến map {targetMapID}.";
        }

        private bool CheckMapPowerError(int mapID, long power, out string error)
        {
            error = null;

            if (IsHighPowerMap(mapID) && power < POWER_60B)
            {
                error = $"Yêu cầu sức mạnh tối thiểu cho map {mapID}: {POWER_60B:N0}.";
                return true;
            }

            if (IsMediumPowerMap(mapID) && power < POWER_40B)
            {
                error = $"Yêu cầu sức mạnh tối thiểu cho map {mapID}: {POWER_40B:N0}.";
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

            if (DataXmap.RequiresClan(mapID))
            {
                error = $"Cần có pt để vào map {mapID}.";
                return true;
            }

            return false;
        }

        private bool CheckItemError(int mapID, out string error)
        {
            error = null;

            if (mapID == 160 && !ModProCL.ExistItemBag(992))
            {
                error = "Không có Nhẫn thời không!";
                return true;
            }

            return false;
        }
        #endregion

        #region Next Map Selection
        public NextMap FindNextMapToGo(int currentMapID, int nextMapID)
        {
            if (!DataXmap.linkMaps.TryGetValue(currentMapID, out List<NextMap> maps))
                return null;

            NextMap preferredOption = null;
            NextMap fallbackOption = null;

            foreach (NextMap map in maps)
            {
                if (map.MapID != nextMapID)
                    continue;

                if (map.NpcID != -1 && map.Options.HasMenuOptions)
                    return map;

                if (map.walk)
                    preferredOption = map;

                if (map.NpcID != -1 && preferredOption == null)
                    preferredOption = map;

                if (map.NpcID == -1 && !map.walk && !map.Options.HasMenuOptions)
                    fallbackOption = map;
            }

            return preferredOption ?? fallbackOption;
        }
        #endregion

        #region Path Validation
        public bool ValidatePathPowerRequirement(int[] path, long power, out string errorMessage)
        {
            errorMessage = null;

            foreach (int mapID in path)
            {
                if (!CheckMapPowerRequirement(mapID, power))
                {
                    long requiredPower = IsHighPowerMap(mapID) ? POWER_60B : POWER_40B;
                    errorMessage = $"Không thể đi qua map {mapID} vì sức mạnh {power:N0} < {requiredPower:N0}.";
                    return false;
                }
            }

            return true;
        }

        public bool ValidatePathRequirements(
int[] path,
long power,
bool hasTask30,
bool hasClan,
out string errorMessage)
        {
            errorMessage = null;

            foreach (int mapID in path)
            {
                if (!CheckMapPowerRequirement(mapID, power))
                {
                    errorMessage = $"Sức mạnh không đủ cho map {mapID}";
                    return false;
                }

                if (DataXmap.IsColdMap(mapID) && !hasTask30)
                {
                    errorMessage = $"Cần hoàn thành nhiệm vụ 30 để vào map {mapID}";
                    return false;
                }

                if (DataXmap.RequiresClan(mapID) && !hasClan)
                {
                    errorMessage = $"Cần có bang hội để vào map {mapID}";
                    return false;
                }
            }

            return true;
        }
        #endregion

        #region Utility Methods
        public int GetEstimatedDistance(int fromMapID, int toMapID)
        {
            int[] path = FindPath(toMapID, fromMapID, long.MaxValue, true);
            return path?.Length ?? -1;
        }

        public bool HasDirectConnection(int fromMapID, int toMapID)
        {
            if (!DataXmap.linkMaps.TryGetValue(fromMapID, out List<NextMap> maps))
                return false;

            foreach (NextMap map in maps)
            {
                if (map.MapID == toMapID)
                    return true;
            }

            return false;
        }

        public List<int> GetConnectedMaps(int mapID)
        {
            List<int> connected = new List<int>();

            if (DataXmap.linkMaps.TryGetValue(mapID, out List<NextMap> maps))
            {
                foreach (NextMap map in maps)
                {
                    if (!connected.Contains(map.MapID))
                        connected.Add(map.MapID);
                }
            }

            return connected;
        }
        #endregion
    }
}