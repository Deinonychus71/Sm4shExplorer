using Sm4shFileExplorer.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Sm4shFileExplorer.Globals
{
    public class PluginActionResult
    {
        private static PluginActionResult _DefaultPass = new PluginActionResult(false, string.Empty);
        private static PluginActionResult _DefaultCancel = new PluginActionResult(true, "Exception");
        public bool Cancel { get; private set; }
        public string Reason { get; private set; }

        public static PluginActionResult DefaultPass { get { return _DefaultPass; } }
        public static PluginActionResult DefaultCancel { get { return _DefaultCancel; } }

        public PluginActionResult(bool cancel, string reason)
        {
            Cancel = cancel;
            Reason = reason;
        }
    }

    public abstract class Sm4shBasePlugin
    {
        private Sm4shProject _Project;

        /// <summary>
        /// Name of the plugin
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Description of the plugin
        /// </summary>
        public abstract string Description { get; }
        
        /// <summary>
        /// Author of the GUI
        /// </summary>
        public abstract string GUI { get; }

        /// <summary>
        /// Researcher
        /// </summary>
        public abstract string Research { get; }

        /// <summary>
        /// Plugin version
        /// </summary>
        public abstract string Version { get; }

        /// <summary>
        /// URL of the project
        /// </summary>
        public abstract string URL { get; }

        [System.ComponentModel.Browsable(false)]
        internal string Filename { get; set; }

        /// <summary>
        /// Boolean that show or hide the plugin in the plugins menu
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        public virtual bool ShowInPluginList { get { return true; } }

        /// <summary>
        /// List of icons associated with this plugin. Used to override the default file icon to show that a resource is compatible with the plugin
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        public virtual Bitmap[] Icons { get { return null; } }

        /// <summary>
        /// Instance of Sm4shProject, access to a bunch of useless methods to extract resources, insert resources in workspace and more
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        public Sm4shProject Sm4shProject { get { return _Project; } }

        /// <summary>
        /// Main constructor of the abstract class
        /// </summary>
        /// <param name="project"></param>
        public Sm4shBasePlugin(Sm4shProject project)
        {
            _Project = project;
        }

        internal void InternalOpenPluginMenu()
        {
            try
            {
                OpenPluginMenu();
            }
            catch(Exception e)
            {
                LogHelper.Error(string.Format("OpenPluginMenu with {0}: {1}", Name, e.Message));
                LogHelper.Debug(string.Format("Stacktrace: {0}", e.StackTrace));
            }
        }

        internal bool InternalCanBeLoaded()
        {
            try
            {
                return CanBeLoaded();
            }
            catch (Exception e)
            {
                LogHelper.Error(string.Format("CanBeLoaded with {0}: {1}", Name, e.Message));
                LogHelper.Debug(string.Format("Stacktrace: {0}", e.StackTrace));
                return false;
            }
        }

        internal void InternalOnLoad()
        {
            try
            {
                OnLoad();
            }
            catch (Exception e)
            {
                LogHelper.Error(string.Format("OnLoad with {0}: {1}", Name, e.Message));
                LogHelper.Debug(string.Format("Stacktrace: {0}", e.StackTrace));
            }
        }

        internal PluginActionResult InternalNewModBuilding(string exportFolder)
        {
            try
            {
                return NewModBuilding(exportFolder);
            }
            catch (Exception e)
            {
                LogHelper.Error(string.Format("NewModBuilding with {0}: {1}", Name, e.Message));
                LogHelper.Debug(string.Format("Stacktrace: {0}", e.StackTrace));
                return PluginActionResult.DefaultCancel;
            }
        }

        internal void InternalNewModBuilt(string exportFolder)
        {
            try
            {
                NewModBuilt(exportFolder);
            }
            catch (Exception e)
            {
                LogHelper.Error(string.Format("NewModBuilt with {0}: {1}", Name, e.Message));
                LogHelper.Debug(string.Format("Stacktrace: {0}", e.StackTrace));
            }
        }

        internal int InternalCanResourceBeLoaded(ResourceCollection resCol, string relativePath)
        {
            try
            {
                return CanResourceBeLoaded(resCol, relativePath);
            }
            catch (Exception e)
            {
                LogHelper.Error(string.Format("CanResourceBeLoaded with {0}: {1}", Name, e.Message));
                LogHelper.Debug(string.Format("Stacktrace: {0}", e.StackTrace));
                return -1;
            }
        }

        internal bool InternalResourceSelected(ResourceCollection resCol, string relativePath, string extractedFile)
        {
            try
            {
                return ResourceSelected(resCol, relativePath, extractedFile);
            }
            catch (Exception e)
            {
                LogHelper.Error(string.Format("ResourceSelected with {0}: {1}", Name, e.Message));
                LogHelper.Debug(string.Format("Stacktrace: {0}", e.StackTrace));
                return false;
            }
        }

        internal PluginActionResult InternalResourcesAddingToWorkspace(ResourceCollection resCol, string relativePath, string fileToBeAdded)
        {
            try
            {
                return ResourcesAddingToWorkspace(resCol, relativePath, fileToBeAdded);
            }
            catch (Exception e)
            {
                LogHelper.Error(string.Format("ResourcesAddingToWorkspace with {0}: {1}", Name, e.Message));
                LogHelper.Debug(string.Format("Stacktrace: {0}", e.StackTrace));
                return PluginActionResult.DefaultCancel;
            }
        }

        internal void InternalResourcesAddedToWorkspace(ResourceCollection resCol, string relativePath, string fileAdded)
        {
            try
            {
                ResourcesAddedToWorkspace(resCol, relativePath, fileAdded);
            }
            catch (Exception e)
            {
                LogHelper.Error(string.Format("ResourcesAddedToWorkspace with {0}: {1}", Name, e.Message));
                LogHelper.Debug(string.Format("Stacktrace: {0}", e.StackTrace));
            }
        }

        internal PluginActionResult InternalResourcesRemovingFromWorkspace(ResourceCollection resCol, string relativePath, string fileToBeAdded)
        {
            try
            {
                return ResourcesRemovingFromWorkspace(resCol, relativePath, fileToBeAdded);
            }
            catch (Exception e)
            {
                LogHelper.Error(string.Format("ResourcesRemovingFromWorkspace with {0}: {1}", Name, e.Message));
                LogHelper.Debug(string.Format("Stacktrace: {0}", e.StackTrace));
                return PluginActionResult.DefaultCancel;
            }
        }

        internal void InternalResourcesRemovedFromWorkspace(ResourceCollection resCol, string relativePath, string fileAdded)
        {
            try
            {
                ResourcesRemovedFromWorkspace(resCol, relativePath, fileAdded);
            }
            catch (Exception e)
            {
                LogHelper.Error(string.Format("ResourcesRemovedFromWorkspace with {0}: {1}", Name, e.Message));
                LogHelper.Debug(string.Format("Stacktrace: {0}", e.StackTrace));
            }
        }

        internal Dictionary<string, string> InternalGridViewPopulated(ResourceCollection resCol, string relativePath, string extractedFile)
        {
            try
            {
                return GridViewPopulated(resCol, relativePath, extractedFile);
            }
            catch (Exception e)
            {
                LogHelper.Error(string.Format("GridViewPopulated with {0}: {1}", Name, e.Message));
                LogHelper.Debug(string.Format("Stacktrace: {0}", e.StackTrace));
                return null;
            }
        }

        #region Hooks
        /// <summary>
        /// Is triggered whenever a user click in the plugin in the plugins menu
        /// </summary>
        public virtual void OpenPluginMenu()
        {
            return;
        }

        /// <summary>
        /// Is triggered while loading the list of plugins to make sure the plugin can be used with this configuration (check for version, files...) (default: true)
        /// </summary>
        /// <returns></returns>
        public virtual bool CanBeLoaded()
        {
            return true;
        }

        /// <summary>
        /// Is triggered after CanBeLoaded to initialize any kind of global object or one time operation
        /// </summary>
        public virtual void OnLoad()
        {

        }

        /// <summary>
        /// Is triggered before building a new mod, the action can be cancelled (default: pass)
        /// </summary>
        /// <param name="exportFolder">Folder where the mod will be built</param>
        /// <returns>PluginActionResult object</returns>
        public virtual PluginActionResult NewModBuilding(string exportFolder)
        {
            return PluginActionResult.DefaultPass;
        }

        /// <summary>
        /// Is triggered after buidling a new mod
        /// </summary>
        /// <param name="exportFolder">Folder where the mod has been built</param>
        public virtual void NewModBuilt(string exportFolder)
        {

        }

        /// <summary>
        /// Is triggered while browsing the list of nodes in the treeview (default: -1)
        /// </summary>
        /// <param name="resCol">ResourceCollection instance</param>
        /// <param name="relativePath">path/to/resource</param>
        /// <returns>Signed int that represent the index of the Icon that should override the default Icon. -1 is interpreted as not compatible with the plugin</returns>
        public virtual int CanResourceBeLoaded(ResourceCollection resCol, string relativePath)
        {
            return -1;
        }

        /// <summary>
        /// Is triggered while double clicking on a resource (default: false)
        /// </summary>
        /// <param name="resCol">ResourceCollection instance</param>
        /// <param name="relativePath">path/to/resource</param>
        /// <param name="extractedFile">Extracted file in the filesystem (extract folder)</param>
        /// <returns>True means that the plugin did an action, false means that the next plugin in the order of priorities will be checked. If no plugin is compatible, the hexa editor will be used</returns>
        public virtual bool ResourceSelected(ResourceCollection resCol, string relativePath, string extractedFile)
        {
            return false;
        }

        /// <summary>
        /// Is triggered before adding a new resource in the workspace, the action can be cancelled (default: pass)
        /// </summary>
        /// <param name="resCol">ResourceCollection instance</param>
        /// <param name="relativePath">path/to/resource</param>
        /// <param name="fileToBeAdded">File or folder that is being added</param>
        /// <returns>PluginActionResult object</returns>
        public virtual PluginActionResult ResourcesAddingToWorkspace(ResourceCollection resCol, string relativePath, string fileToBeAdded)
        {
            return PluginActionResult.DefaultPass;
        }

        /// <summary>
        /// Is triggered after adding a new resource in the workspace
        /// </summary>
        /// <param name="resCol">ResourceCollection instance</param>
        /// <param name="relativePath">path/to/resource</param>
        /// <param name="fileAdded">File or folder that was added</param>
        public virtual void ResourcesAddedToWorkspace(ResourceCollection resCol, string relativePath, string fileAdded)
        {
        }

        /// <summary>
        /// Is triggered before removing a resource from the workspace, the action can be cancelled (default: pass)
        /// </summary>
        /// <param name="resCol">ResourceCollection instance</param>
        /// <param name="relativePath">path/to/resource</param>
        /// <param name="fileToBeAdded">File or folder that is being removed</param>
        /// <returns>PluginActionResult object</returns>
        public virtual PluginActionResult ResourcesRemovingFromWorkspace(ResourceCollection resCol, string relativePath, string fileToBeAdded)
        {
            return PluginActionResult.DefaultPass;
        }

        /// <summary>
        /// Is triggered after removing a resource from the workspace
        /// </summary>
        /// <param name="resCol">ResourceCollection instance</param>
        /// <param name="relativePath">path/to/resource</param>
        /// <param name="fileAdded">File or folder that has been removed</param>
        public virtual void ResourcesRemovedFromWorkspace(ResourceCollection resCol, string relativePath, string fileAdded)
        {
        }

        /// <summary>
        /// Is triggered while populating the gridview in order to add new information about a specific resource (default: null)
        /// </summary>
        /// <param name="resCol">ResourceCollection instance</param>
        /// <param name="relativePath">path/to/resource</param>
        /// <param name="extractedFile">Physical path of a file, if the resource is mod</param>
        /// <returns>Dictionary of key/value to add to the gridview. Can be null</returns>
        public virtual Dictionary<string,string> GridViewPopulated(ResourceCollection resCol, string relativePath, string extractedFile)
        {
            return null;
        }
        #endregion
    }
}
