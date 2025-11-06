using System.Runtime.Remoting.Messaging;
using UnityEngine;

namespace WalkOfLife
{

    // This script/class is used for the edgecase where the player gets stuck on a very steep slop due to clipping inside the box model.
    public static class CharacterControllerUtils
    {

        public static Vector3 GetNormalWithSphereCast(CharacterController characterController, LayerMask layerMask = default)
        {
            Vector3 normal = Vector3.up;
            Vector3 center = characterController.transform.position + characterController.center;
            float distance = characterController.height / 2f + characterController.stepOffset + 0.01f;

            RaycastHit hit;
            if (Physics.SphereCast(center, characterController.radius, Vector3.down, out hit, distance, layerMask))
            {
                normal = hit.normal;
            }
            return normal;
        }      
        

    }
}
