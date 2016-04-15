using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RoomArrangement.MyGraph
{
	class Graph<T> : IEnumerable<T>
	{
		public NodeList<T> NodeSet { get; private set; }

		public Graph(NodeList<T> nodeSet)
		{
			NodeSet = nodeSet;
		}

		public void AddNode(Node<T> node) => NodeSet.Add(node);

		public void AddAdjacency(Node<T> fst, Node<T> snd)
		{
			fst.Neighbors.Add(snd);
			snd.Neighbors.Add(fst);
		}

		public bool Contains(T value) => NodeSet.FindByValue(value) != null;

		public bool Remove(T value)
		{
			var nodeToRemove = NodeSet.FindByValue(value);

			if(nodeToRemove == null)
				return false;

			NodeSet.Remove(nodeToRemove);

			foreach(Node<T> node in NodeSet)
			{
				int i = node.Neighbors.IndexOf(nodeToRemove);
				if(i != -1)
					node.Neighbors.RemoveAt(i);
			}

			return true;
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		public IEnumerator<T> GetEnumerator()
		{
			var elements = new Collection<T>();
			foreach(Node<T> node in NodeSet)
				elements.Add(node.Value);

			return elements.GetEnumerator();
		}
	}
}
