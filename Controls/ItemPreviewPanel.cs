/***************************************************************************
	Copyright (C) 2014-2015 by Ari Vuollet <ari.vuollet@kapsi.fi>

	This program is free software; you can redistribute it and/or
	modify it under the terms of the GNU General Public License
	as published by the Free Software Foundation; either version 2
	of the License, or (at your option) any later version.

	This program is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with this program; if not, see <http://www.gnu.org/licenses/>.
***************************************************************************/

using System;
using System.Drawing;
using System.Windows.Forms;
using OBS;
using OBS.Graphics;
using obs_cli.Objects.Obs;
using obs_cli.Data;

namespace obs_cli.Controls
{
	class ItemPreviewPanel : DisplayPanel
	{
		private Item _item;
		private Label _loadingMessage;
		private ObsSource _source;

		public ItemPreviewPanel(Item item, ObsSource source)
		{
			this._item = item;
			this._source = source;
		}

		protected override void DisplayCreated()
		{
			base.DisplayCreated();

			if (_source != null)
				Display.AddDrawCallback(RenderItem);
		}

		protected override void DisplayDestroyed()
		{
			base.DisplayDestroyed();

			if (_source != null)
				Display.RemoveDrawCallback(RenderItem);
		}

		private void RenderItem(IntPtr data, uint cx, uint cy)
		{
			bool sourceIsInitialized = _source.Width != 0 || _source.Height != 0;
			if (!sourceIsInitialized)
			{
				if (_loadingMessage == null)
				{
					Store.Data.Webcam.Window.Dispatcher.Invoke(new Action(AddLoadingMessage));
				}
			}
			else
			{
				//if (_loadingMessage != null)
				//{
				//	Store.Data.Webcam.Window.Dispatcher.Invoke(new Action(RemoveLoadingMessage));
				//}
			}

			int newW = (int)cx;
			int newH = (int)cy;
			int itemWidth = (int)_item.Width;
			int itemHeight = (int)_item.Height;
			int itemSourceWidth = (int)_source.Width;
			int itemSourceHeight = (int)_source.Height;
			float previewAspect = (float)cx / cy;
			float itemAspect = (float)itemWidth / itemHeight;
			float sourceAspect = (float)itemSourceWidth / itemSourceHeight;

			//calculate new width and height for item to make it fit inside the preview area
			if (previewAspect > itemAspect)
				newW = (int)(cy * itemAspect);
			else
				newH = (int)(cx / itemAspect);

			int centerX = ((int)cx - newW) / 2;
			int centerY = ((int)cy - newH) / 2;

			GS.ViewportPush();
			GS.ProjectionPush();

			float difWidth = itemAspect < sourceAspect ? (itemSourceWidth - (itemSourceHeight * itemAspect)) / 2.0f : 0.0f;
			float difHeight = itemAspect < sourceAspect ? 0.0f : (itemSourceHeight - (itemSourceWidth / itemAspect)) / 2.0f;

			//setup orthographic projection of the item
			GS.Ortho(difWidth, itemSourceWidth - difWidth, difHeight, itemSourceHeight - difHeight, -100.0f, 100.0f);
			GS.SetViewport(centerX, centerY, newW, newH);

			//render item content
			_source.Render();

			GS.ProjectionPop();
			GS.ViewportPop();

			GS.LoadVertexBuffer(null);
		}

		private void AddLoadingMessage()
		{
			_loadingMessage = new Label
			{
				Text = "Loading webcam...",
				AutoSize = false,
				TextAlign = ContentAlignment.MiddleCenter,
				Dock = DockStyle.Bottom,
				ForeColor = Color.White,
				Font = new Font("Arial", 20),
				BackColor = Color.DimGray
			};

			Controls.Add(_loadingMessage);
		}

		private void RemoveLoadingMessage()
		{
			this.Controls.Remove(_loadingMessage);
			_loadingMessage = null;
		}
	}
}