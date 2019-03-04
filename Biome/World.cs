using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Biome
{
    internal class World
    {
        public enum Terrain
        {
            Bush,
            Grass,
            Tree ,
            Water
        }
        private static readonly Dictionary<Terrain, char> TerrainMap = new Dictionary<Terrain, char>();
        public int Height { get; }
        public int Width { get; }
        public Terrain[,] Map { get; }

        public World(int h, int w)
        {
            Height = h;
            Width = w;
            TerrainMap.Add(Terrain.Bush, '#');
            TerrainMap.Add(Terrain.Grass, '.');
            TerrainMap.Add(Terrain.Tree, '^');
            TerrainMap.Add(Terrain.Water, '~');
            Map = BuildMap(Height, Width);
            NaturalizeMap(Map);

        }

        private Terrain[,] BuildMap(int h, int w)
        {
            Terrain[,] mapGrid = new Terrain[h, w];

            for (int i = 0; i < h; i++)
            {
                for (int e = 0; e < w; e++)
                {
                    mapGrid[i, e] = SetTileRandomTerrain();
                }
            }

            return mapGrid;
        }

        private Terrain SetTileRandomTerrain()
        {
            var rand = new Random();
            var terrainCount = Enum.GetNames(typeof(Terrain)).Length;
            return (Terrain)rand.Next(0, terrainCount);
        }

        private void NaturalizeMap(Terrain[,] mapGrid)
        {
            Random rand = new Random();
            for (int h = 0; h < mapGrid.GetLength(0); h++)
            {
                for (int w = 0; w < mapGrid.GetLength(1); w++)
                {
                    if (CheckSurroundingTerrainForMatch(h, w))
                    {
                        var mostCommonNeighbor = GetMajorityTerrainMatch(h, w);

                        if (rand.Next(0, 10) <= mostCommonNeighbor.Value)
                        {
                            mapGrid[h, w] = mostCommonNeighbor.Key;
                        }
                    }
                }
            }
        }

        public bool CheckSurroundingTerrainForMatch(int x, int y)
        {
            bool hasMatchingNeighbor = false;

            var rowLimit = Map.GetLength(0);
            if (rowLimit > 0)
            {
                var columnLimit = Map.GetLength(1);
                for (int i = Math.Max(0, x - 1); i < Math.Min(x + 1, rowLimit); i++)
                {
                    for (int j = Math.Max(0, y - 1); j < Math.Min(y + 1, columnLimit); j++)
                    {
                        if (i == x || j == y)
                        {
                            continue;
                        }

                        if (Map[i, j] == Map[x, y])
                        {
                            hasMatchingNeighbor = true;
                        }
                    }
                }
            }

            return hasMatchingNeighbor;
        }

        public KeyValuePair<Terrain, int> GetMajorityTerrainMatch(int x, int y)
        {
            Dictionary<Terrain, int> neighbors = new Dictionary<Terrain, int>();
            foreach (var type in Enum.GetValues(typeof(Terrain)))
            {
                neighbors.Add((Terrain)type, 0);
            }

            var rowLimit = Map.GetLength(0);
            if (rowLimit > 0)
            {
                var columnLimit = Map.GetLength(1);
                for (int i = Math.Max(0, x - 1); i < Math.Min(x + 1, rowLimit); i++)
                {
                    for (int j = Math.Max(0, y - 1); j < Math.Min(y + 1, columnLimit); j++)
                    {
                        if (i == x && j == y)
                        {
                            continue;
                        }

                        if (Map[i, j] == Map[x, y])
                        {
                            int currentCount;
                            neighbors.TryGetValue(Map[i, j], out currentCount);
                            neighbors[Map[i, j]] += 1;
                        }
                    }
                }
            }

            return neighbors.FirstOrDefault(k => k.Value == neighbors.Values.Max());
        }

        public void PrintMap()
        {
            var defaultForeground = Console.ForegroundColor;

            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int e = 0; e < Map.GetLength(1); e++)
                {
                    switch (Map[i, e])
                    {
                        case Terrain.Bush:
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write(TerrainMap[Map[i, e]]);
                            break;
                        case Terrain.Grass:
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(TerrainMap[Map[i, e]]);
                            break;
                        case Terrain.Tree:
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.Write(TerrainMap[Map[i, e]]);
                            break;
                        case Terrain.Water:
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write(TerrainMap[Map[i, e]]);
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(TerrainMap[Map[i, e]]);
                            break;
                    }
                }

                Console.ForegroundColor = defaultForeground;
                Console.WriteLine();
            }
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();

            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int e = 0; e < Map.GetLength(1); e++)
                {
                    str.Append(TerrainMap[Map[i, e]]);
                }

                str.Append(Environment.NewLine);
            }

            return str.ToString();
        }
    }
}
