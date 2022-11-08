using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniDini;

namespace MiniDini.Nodes
{
	/// <summary>
	/// Abstract base class for nodes.. scriptableobject so we can save/load via serializefield
	/// </summary>
	public abstract class Node : ScriptableObject
	{
		/// <value>
		/// The Children that this <see cref="Node"/> contains.
		/// </value>
		[SerializeField, HideInInspector] protected List<Node> children = new List<Node>();
		[SerializeField, HideInInspector] protected List<Node> parents = new List<Node>();


		/// <value>
		/// The GUID of the Node View, used to get the Node View that this Node is associated with.
		/// </value>
		[HideInInspector] public string guid;


		/// <value>
		/// The Position in the Graph Editor View that this Node is at.
		/// </value>
		[HideInInspector] public Vector2 nodeGraphPosition;


		/// <value>
		/// Does this node have more then one parent.
		/// </value>
		[HideInInspector] public bool hasMultipleParents;

		/// <value>
		/// Geometry for this node
		/// </value>
		[HideInInspector] public Geometry m_geometry = new Geometry();


		#region Abstract Methods

		#endregion

		#region Virtual Methods

		public virtual string GetDescription() { return "Null"; }
		/// <summary>
		/// Add the child node to this node.
		/// </summary>
		/// <param name="childNode">The Node to add as a Child.</param>
		public virtual void AddChild(Node childNode) 
		{
			children.Add(childNode);
		}

		/// <summary>
		/// Remove a Child from the Node.
		/// </summary>
		/// <param name="childNode">The Child to remove.</param>
		public virtual void RemoveChild(Node childNode) 
		{
			children.Remove(childNode);
		}

		/// <summary>
		/// Get a list of children the node contains.
		/// </summary>
		/// <returns>A list of children Nodes.</returns>
		public virtual List<Node> GetChildren()
		{
			return children;
		}

		/// <summary>
		/// Get a list of parents of this node.
		/// </summary>
		/// <returns>A list of parent Nodes.</returns>
		public virtual List<Node> GetParents()
		{
			return parents;
		}

		/// <summary>
		/// Add a parent to this node
		/// </summary>
		/// <param name="parent"></param>
		public virtual void AddParent(Node parent) 
		{
			parents.Add(parent);
		}

		/// <summary>
		/// Remove a parent from the Node.
		/// </summary>
		/// <param name="parent">The parent to remove.</param>
		public virtual void RemoveParent(Node parent) 
		{
			parents.Remove(parent);
		}

		/// <summary>
		/// Remove all parent(s) from the Node.
		/// </summary>
		public virtual void RemoveAllParents()
		{
			parents.Clear();
		}


		/// <summary>
		/// Get the geometry for this Node.
		/// </summary>
		/// <returns>A geometry object</returns>
		public virtual Geometry GetGeometry() 
		{
			if (m_geometry == null)
				m_geometry = new Geometry();
			return m_geometry; 
		}


		#endregion

		/// <summary>
		/// Clone the Node.
		/// </summary>
		/// <returns>A Clone of the Node.</returns>
		public Node Clone()
		{
			return Instantiate(this);
		}

	}

}
