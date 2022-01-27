using UnityEditor;

namespace pdxpartyparrot.Core.Editor
{
    public static class AssetHandler
    {
        [UnityEditor.Callbacks.OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            string assetPath = AssetDatabase.GetAssetPath(instanceID);

            // TODO:

            return false;
        }
    }
}
