/////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

/////////////////////////////////////////////////////////////////////////////

namespace Font
{
	/////////////////////////////////////////////////////////////////////////////
	
	public class TypeFace
	{
		/////////////////////////////////////////////////////////////////////////////
		
		public int height;
		public float baseline;
		public int layerCount;
		public int glyphCount;
		public string name;
		private Texture2D texture;

		/////////////////////////////////////////////////////////////////////////////
		
		public struct Layer
		{
			public Vector2 offset;

			public Layer(float x, float y)
			{
				offset = new Vector2(x, y);
			}
		}

		/////////////////////////////////////////////////////////////////////////////
		
		public class GlyphDescriptor
		{
			public char c;
			public int imageCount;
			public float advance;
			public Sprite[] images;
			public Vector2[] offsets;

			public GlyphDescriptor(int imgCount, float advnce, Sprite[] imgs, Vector2[] offs)
			{
				imageCount = imgCount;
				advance = advnce;
				images = imgs;
				offsets = offs;
			}
		}

		/////////////////////////////////////////////////////////////////////////////
		
		public Layer[] layers;
		public Dictionary<char, GlyphDescriptor> glyphs;

		/////////////////////////////////////////////////////////////////////////////
		
		public TypeFace (string name)
		{
			texture = (Texture2D)Resources.Load(name + "0");
			texture.filterMode = FilterMode.Trilinear;

			TextAsset t = (TextAsset)Resources.Load(name);				// load the json dat
			SimpleJSON.JSONNode d = SimpleJSON.JSON.Parse(t.text);		// parse it

			name = d["name"].Value;										// get the name
			height = d["height"].AsInt;									// and the height
			int layerCount = d["layerCount"].AsInt;						// and the layercount
			glyphCount = d["glyphCount"].AsInt;						    // and the glyphcount

			SimpleJSON.JSONNode layersNodes = d["Layers"];				// get the node with the layers in it
			layers = new Layer[layersNodes.Count];						// create array of layers

			for(int i = 0; i < layerCount; ++i)							// read in the layers
			{
				float xoffset= d["Layers"][i]["offsetX"].AsInt;
				float yoffset= d["Layers"][i]["offsetY"].AsInt;
				layers[i] = new Layer(xoffset, yoffset);				// add a layer
			}

			glyphs = new Dictionary<char, GlyphDescriptor>();						// create the Glyphs dctionary

			int glyphNodeCount = d["glyphs"].Count;						// get count of glyph nodes

			for(int i = 0; i < glyphNodeCount; ++i)
			{
				SimpleJSON.JSONNode glyphNode = d["glyphs"][i];
				int c = glyphNode["char"].AsInt;
				int imageCount = glyphNode["imageCount"].AsInt;
				float advance = glyphNode["advance"].AsFloat;
				Sprite[] images = new Sprite[imageCount];
				Vector2[] offs = new Vector2[imageCount];
				float th = (float)texture.height;
				for(int j = 0; j < imageCount; ++j)
				{
					int k = imageCount - 1 - j;
					SimpleJSON.JSONNode inode = glyphNode["images"][j];
					float ox = inode["offsetX"].AsFloat;
					float oy = inode["offsetY"].AsFloat;
					float x = inode["x"].AsInt;
					float y = inode["y"].AsInt;
					float w = inode["w"].AsInt;
					float h = inode["h"].AsInt;
					images[k] = Sprite.Create(texture, new Rect(x, th-y, w, -h), new Vector2(0, 0), 1);
					offs[k] = new Vector2(ox, -oy);
				}
				glyphs.Add((char)c, new GlyphDescriptor(imageCount, advance, images, offs));
			}
		}
	}
}

