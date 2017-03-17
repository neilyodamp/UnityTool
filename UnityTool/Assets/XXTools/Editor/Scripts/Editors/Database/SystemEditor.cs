using UnityEngine;
using System.Collections;

namespace XXToolsEditor
{
    [DatabaseEditor("Main",Priority =0)]
    public class SystemEditor : DatabaseEdBase
    {
        private Vector2[] scroll = { };

        private static readonly string[] MenuItems = {
            "Path From",
            "SwapMesh",
        };



    }
}
