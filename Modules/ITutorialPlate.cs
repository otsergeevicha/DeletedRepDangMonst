using System;
using UnityEngine;

namespace Modules
{
    public interface ITutorialPlate
    {
        event Action OnTutorialContacted;
        Vector3 GetPositionMarker();
        Transform GetRootCamera();
    }
}