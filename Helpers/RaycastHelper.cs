using SDG.Framework.Landscapes;
using SDG.Unturned;
using System.Linq;
using UnityEngine;

namespace RestoreMonarchy.Pickpocket.Helpers
{
    public class RaycastHelper
    {
        public static RaycastHit[] RaycastAll(Ray ray, float maxDistance, int layerMask, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
        {
            if ((layerMask & RayMasks.GROUND) == RayMasks.GROUND)
            {
                LandscapeHoleUtility.raycastIgnoreLandscapeIfNecessary(ray, maxDistance, ref layerMask);
            }
            return Physics.RaycastAll(ray, maxDistance, layerMask, queryTriggerInteraction);
        }

        public static Player GetPlayerFromHits(Player caller, RaycastHit[] hits)
        {
            Player player = null;
            int hitsCount = hits.Count();
            if (hitsCount > 0)
            {                
                for (int i = 0; i < hitsCount; i++)
                {
                    Player suspect = hits[i].transform.GetComponentInParent<Player>();
                    if (suspect != caller)
                    {
                        player = suspect;
                        break;
                    }
                }
            }
            return player;
        }
    }
}
