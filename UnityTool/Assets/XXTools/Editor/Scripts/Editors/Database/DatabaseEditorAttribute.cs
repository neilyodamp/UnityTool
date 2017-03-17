using UnityEngine;
using System.Collections;

namespace XXToolsEditor
{

    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class DatabaseEditorAttribute : System.Attribute
    {
        public string Name { get { return this.name; } }
        public int Priority = 999; 

        private readonly string name;

        // ================================================================================================================

        public DatabaseEditorAttribute(string name)
        {
            this.name = name;
        }

        // ================================================================================================================
    }
}