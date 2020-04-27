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

using OBS;
using OBS.Graphics;
using obs_cli.Objects.Obs;
using System;
using System.Windows.Forms;

namespace obs_cli.Controls
{
	class ItemPreviewPanel : DisplayPanel
	{
		private ObsSource source;
		private Item item;

		private Label loadingMessage;
		private LinkLabel loadingMessageLink;

		public ItemPreviewPanel(Item item, ObsSource source)
		{
			this.item = item;
			this.source = source;
		}

		protected override void DisplayCreated()
		{
			base.DisplayCreated();

			if (source != null)
				Display.AddDrawCallback(RenderItem);
		}

		protected override void DisplayDestroyed()
		{
			base.DisplayDestroyed();

			if (source != null)
				Display.RemoveDrawCallback(RenderItem);
		}

		private void RenderItem(IntPtr data, uint cx, uint cy)
		{
			int newW = (int)cx;
			int newH = (int)cy;
			int itemWidth = (int)item.Width;
			int itemHeight = (int)item.Height;
			int itemSourceWidth = (int)source.Width;
			int itemSourceHeight = (int)source.Height;
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
			source.Render();

			GS.ProjectionPop();
			GS.ViewportPop();

			GS.LoadVertexBuffer(null);
		}
	}
}
