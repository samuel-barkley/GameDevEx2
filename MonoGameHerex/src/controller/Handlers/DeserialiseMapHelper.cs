using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using MonoGameHerex.src.model;

namespace MonoGameHerex.Handlers
{
    public static class DeserialiseMapHelper
    {
        public static Map DeserialiseMap(List<string> mapData, Map resultMap)
        {
            for (int i = 0; i < mapData.Count; i++)
            {
                for (int j = 0; j < mapData[i].Length; j++)
                {
                    switch (mapData[i][j])
                    {
                        case '.':
                            resultMap.mapLayout[i, j] = TileType.Air;
                            break;
                        case 'o':
                            resultMap.mapLayout[i, j] = TileType.Ground; 
                            break;
                        case 'c':
                            resultMap.mapLayout[i, j] = TileType.Coin;
                            break;
                        case 'e':
                            resultMap.mapLayout[i, j] = TileType.Enemy;
                            break;
                        case 's':
                            resultMap.mapLayout[i, j] = TileType.Start;
                            break;
                        case 'x':
                            resultMap.mapLayout[i, j] = TileType.End;
                            break;
                    }
                }
            }

            return resultMap;
        }
    }
}