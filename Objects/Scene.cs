using OBS;
using obs_cli.Helpers.Extensions;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace obs_cli.Objects
{
    public class Scene : ObsScene
    {
        public BindingList<Item> Items { get; set; }

        public Scene(string name)
            : base(name)
        {
            Items = new BindingList<Item>();
        }

        public Scene(IntPtr pointer)
            : base(pointer)
        {
            Items = new BindingList<Item>();
        }

        /// <summary>
        /// Returns the name of the scene from obs-lib
        /// </summary>
        public string Name
        {
            get { return GetName(); }
        }

        /// <summary>
        /// Adds an Item to Items
        /// </summary>
        /// <param name="source">Source to use to create item</param>
        /// <param name="name">Name of the item (UI only)</param>
        /// <returns></returns>
        public Item Add(Source source, string name)
        {
            var sceneitem = base.Add(source);
            var item = new Item(sceneitem.GetPointer()) { Name = name };
            sceneitem.Dispose();
            return item;
        }

        /// <summary>
        /// Removes, Disposes and Clears all items from Items
        /// </summary>
        public void ClearItems()
        {
            foreach (var item in Items)
            {
                item.Remove();
                item.Dispose();
            }

            Items.Clear();
        }

        /// <summary>
        ///	Moves Item in both the local list and in the obs viewport
        /// </summary>
        /// <param name="item">Item to move</param>
        /// <param name="direction">Where to move the item to</param>
        /// <returns>New index of "item"</returns>
        public int MoveItem(Item item, obs_order_movement direction)
        {
            var oldindex = Items.IndexOf(item);
            int newindex = -1;
            switch (direction)
            {
                case obs_order_movement.OBS_ORDER_MOVE_UP:
                    newindex = oldindex - 1;
                    break;
                case obs_order_movement.OBS_ORDER_MOVE_DOWN:
                    newindex = oldindex + 1;
                    break;
                case obs_order_movement.OBS_ORDER_MOVE_TOP:
                    newindex = 0;
                    break;
                case obs_order_movement.OBS_ORDER_MOVE_BOTTOM:
                    newindex = Items.Count - 1;
                    break;
            }

            item.SetOrder(direction);

            Items.Move(oldindex, newindex);

            Debug.WriteLine(String.Format("{0} new index is {1}", item.Name, newindex));
            return newindex;
        }
    }
}
