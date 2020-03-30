using System;
using OBS;

namespace obs_cli.Objects
{
    public class Item : ObsSceneItem
    {
        public Item(IntPtr sceneItem)
            : base(sceneItem)
        {
        }

        /// <summary>
        /// Gets or Sets the Name of Item (UI only)
        /// </summary>
        public string Name { get; set; }
    }
}
