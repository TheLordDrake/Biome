using System;

namespace Biome
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            var world = new World(20, 40);
            world.PrintMap();
            world.CheckSurroundingTerrainForMatch(2, 2);
        }
    }
}
