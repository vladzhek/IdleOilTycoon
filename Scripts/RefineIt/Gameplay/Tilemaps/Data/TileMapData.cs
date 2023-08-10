using System.Collections.Generic;
using System.Threading.Tasks;
using Gameplay.Tilemaps.Services;
#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Gameplay.Tilemaps.Data
{
    [CreateAssetMenu(fileName = "TilemapData", menuName = "Data/Tiles/TilemapData")]
    public class TileMapData : ScriptableObject
    {
        public List<BuildingData> BuildingsData;

#if UNITY_EDITOR
        [Button]
        public async void Collect()
        {
            BuildingsData.Clear();
            var tilemapMarker = FindObjectOfType<TilemapMarker>();
            //TODO Collect Region Data
            var tilesContainer =
                Resources.Load<TilesContainer>("Configs/Region/Desert/Tiles/TilesContainer");
            await InitializeTiles(tilemapMarker.InteractableTilemap, tilesContainer);
            EditorUtility.SetDirty(this);
            Debug.Log($"Collect Success, collected {BuildingsData.Count} Tiles");
        }
#endif

        private async Task InitializeTiles(Tilemap tilemap, TilesContainer tilesContainer)
        {
            var loadedTiles =
                new Dictionary<BuildingTileData, IList<TileBase>>();
            if (BuildingsData == null)
                BuildingsData = new List<BuildingData>();

            for (var i = tilemap.cellBounds.xMin; i < tilemap.cellBounds.xMax; i++)
            {
                for (var j = tilemap.cellBounds.yMin; j < tilemap.cellBounds.yMax; j++)
                {
                    var localPlace = new Vector3Int(i, j, (int)tilemap.transform.position.y);
                    if (tilemap.HasTile(localPlace))
                    {
                        var tileBase = tilemap.GetTile(localPlace);
                        await AddBuildingData(localPlace, tileBase, tilesContainer, loadedTiles);
                    }
                }
            }
        }

        private async Task AddBuildingData(Vector3Int position, TileBase tileBase, TilesContainer tilesContainer,
            Dictionary<BuildingTileData, IList<TileBase>> loadedBuildingsTiles)
        {
            foreach (var tilesContainerBuilding in tilesContainer.Buildings)
            {
                if (loadedBuildingsTiles.TryGetValue(tilesContainerBuilding, out var loadedTiles))
                {
                }
                else
                {
                    loadedTiles = new List<TileBase>();
                    foreach (var buildingLevelTileData in tilesContainerBuilding.Levels)
                    {
                        var animationTileData = buildingLevelTileData.AnimationTileData;
                        loadedTiles.Add(animationTileData.DefaultTile);
                        loadedTiles.Add(animationTileData.AnimatedTile1);
                        loadedTiles.Add(animationTileData.AnimatedTile2);
                    }

                    loadedBuildingsTiles[tilesContainerBuilding] = loadedTiles;
                }

                foreach (var loadedTile in loadedTiles)
                {
                    if (loadedTile == tileBase)
                    {
                        AddTileData(position, tilesContainerBuilding);
                        break;
                    }
                }
            }
        }

        private void AddTileData(Vector3Int position, BuildingTileData tilesContainerBuilding) => CreateTileData(position, tilesContainerBuilding);

        private static void UpdateTileData(Vector3Int position, BuildingTileData tilesContainerBuilding,
            BuildingData buildingData)
        {
            buildingData.Id = tilesContainerBuilding.Id;
            buildingData.Position = position;
            buildingData.Type = tilesContainerBuilding.Type;
        }

        private void CreateTileData(Vector3Int position, BuildingTileData tilesContainerBuilding)
        {
            BuildingsData.Add(new BuildingData(tilesContainerBuilding.Id, position, tilesContainerBuilding.Type));
        }

        private static AdditionalBuilding AddedAdditionalBuilding(List<AdditionalBuilding> additionalBuildings,
            int previousIndex, int i, Vector3Int value)
        {
            AdditionalBuilding additionalBuilding = new()
            {
                Position = additionalBuildings[previousIndex].Position + value,
                BuildingNumber = i + 1
            };
            return additionalBuilding;
        }
    }
}