#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE. 
// The License is based on the Mozilla Public License Version 1.1, but Sections 14 
// and 15 have been added to cover use of software over a computer network and 
// provide for limited attribution for the Original Developer. In addition, Exhibit A has 
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is MiNET.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System.Drawing;
using System.Drawing.Imaging;
using MiNET.Net;
using MiNET.Utils;

namespace MiNET.Entities.ImageProviders
{
	public class TextMapImageProvider : IMapImageProvider
	{
		private static Font _font = null;
		
		static TextMapImageProvider()
		{
			_font = new Font("Arial", 9);
		}
		
		public string Text { get; set; }

		public TextMapImageProvider(string text = "")
		{
			Text = text;
		}

		private byte[] _mapData = null;

		public virtual byte[] GetData(MapInfo mapInfo, bool forced)
		{
			byte[] data = null;

			if (_mapData == null)
			{
				_mapData = DrawText(mapInfo, Text);
				data = _mapData;
			}
			else if (_mapData.Length != mapInfo.Col * mapInfo.Row)
			{
				_mapData = DrawText(mapInfo, Text);
				data = _mapData;
			}

			return data;
		}

		public virtual McpeClientboundMapItemData GetClientboundMapItemData(MapInfo mapInfo)
		{
			return null;
		}

		public virtual McpeWrapper GetBatch(MapInfo mapInfo, bool forced)
		{
			return null;
		}

		private static byte[] DrawText(MapInfo map, string text)
		{
			using (var bitmap = new Bitmap(map.Col, map.Row))
			{
				using (Graphics graphics = Graphics.FromImage(bitmap))
				{
					graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
					graphics.DrawString(text, _font, Brushes.AntiqueWhite, new PointF(0, 0));
				}

				Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
				BitmapData bmpData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, bitmap.PixelFormat);

				int bytesPerPixel = Bitmap.GetPixelFormatSize(bitmap.PixelFormat) / 8;
				int byteCount = bmpData.Stride * bitmap.Height;
				byte[] bytes = new byte[byteCount];

				System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, bytes, 0, byteCount);

				bitmap.UnlockBits(bmpData);

				for (int i = 0; i < bytes.Length; i += bytesPerPixel)
				{
					byte b = bytes[i];
					byte g = bytes[i + 1];
					byte r = bytes[i + 2];
					byte a = bytes[i + 3];
					bytes[i] = r;
					bytes[i + 1] = g;
					bytes[i + 2] = b;
					bytes[i + 3] = a;
				}

				return bytes;
			}
		}
	}
}