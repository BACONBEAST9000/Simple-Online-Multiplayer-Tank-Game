using System.Collections.Generic;
using UnityEngine;

public static class Utils {
    public const string EMISSION_COLOR_PROPERTY_NAME = "_EmissionColor";

    public static Transform GetFarthestTransformFromPlayers(List<Transform> transforms) {
        if (transforms == null || transforms[0] == null) {
            return null;
        }
        
        if (PlayerManager.GetPlayerCount < 2) {
            return transforms[0];
        }
        
        List<Vector3> playerPositions = PlayerManager.GetAllPlayerTankPositions;

        Transform farthestTransform = null;
        float farthestDistance = float.MinValue;

        foreach (Transform transformItem in transforms) {

            float totalDistance = 0f;

            foreach (Vector3 playerPosition in playerPositions) {
                totalDistance += Vector3.Distance(playerPosition, transformItem.position);
            }

            if (totalDistance > farthestDistance) {
                farthestDistance = totalDistance;
                farthestTransform = transformItem;
            }
        }

        return farthestTransform;
    }
}