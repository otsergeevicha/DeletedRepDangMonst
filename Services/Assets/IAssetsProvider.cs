using UnityEngine;

namespace Services.Assets
{
    public interface IAssetsProvider
    {
        GameObject InstantiateEntity(string path);
    }
}