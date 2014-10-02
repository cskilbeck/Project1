using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

namespace Text
{
    public class Parameter
    {
    }

    public class Node
    {
        [XmlArrayItem("Parameter")]
        Parameter[] parameters;
    }

    public class NodeList
    {
        [XmlElement("Nodes")]
        Node[] nodes;
    }

    public class LayerNode : Node
    {
    }

    public class FontNode : Node
    {
    }

    public class FunctionNode : Node
    {
    }

    public class Connection
    {
    }

    public class ConnectionList
    {
        [XmlArray("Connections")]
        Connection[] connections;
    }

    public class Graph
    {
    }

    public class Layer
    {
    }

    public class LayerList
    {
    }

    public class Graphic
    {
    }

    public class Glyph
    {
    }

    public class GlyphList
    {
    }

    [XmlRoot("BitmapFont")]
    public class BitmapFont
    {
        [XmlElement("Graph")]
        Graph graph;

        [XmlElement("Layers")]
        LayerList layerList;

        [XmlElement("Glyphs")]
        GlyphList glyphs;
    }
}
