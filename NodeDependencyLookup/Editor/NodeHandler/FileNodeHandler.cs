﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;

namespace Com.Innogames.Core.Frontend.NodeDependencyLookup
{
    /**
	 * NodeHandler for files
	 */
    public class FileNodeHandler : INodeHandler
    {
        private string[] HandledTypes = {"File"};
		
        public string GetId()
        {
            return "FileNodeHandler";
        }

        public string[] GetHandledNodeTypes()
        {
            return HandledTypes;
        }
		
        public int GetOwnFileSize(string type, string id, string key, HashSet<string> traversedNodes,
            NodeDependencyLookupContext stateContext)
        {
            return NodeDependencyLookupUtility.GetPackedAssetSize(id);
        }

        public bool IsNodePackedToApp(string id, string type, bool alwaysExcluded)
        {
            if (alwaysExcluded)
            {
                return !IsNodeEditorOnly(id, type);
            }
			
            string path = AssetDatabase.GUIDToAssetPath(id);
            return IsSceneAndPacked(path) || IsInResources(path) || id.StartsWith("0000000");
        }

        public bool IsNodeEditorOnly(string id, string type)
        {
            string path = AssetDatabase.GUIDToAssetPath(id);
            return path.Contains("/Editor/");
        }

        public bool ContributesToTreeSize()
        {
            return true;
        }

        public void InitContext(NodeDependencyLookupContext nodeDependencyLookupContext)
        {
            // nothing to do
        }
		
        private bool IsSceneAndPacked(string path)
        {
            if (Path.GetExtension(path).Equals(".unity"))
            {
                return EditorBuildSettings.scenes.Any(scene => scene.enabled && scene.path.Equals(path));
            }

            return false;
        }

        private bool IsInResources(string path)
        {
            return path.Contains("/Resources/");
        }
    }
}