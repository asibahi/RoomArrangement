using System.Collections.ObjectModel;

namespace RoomArrangement.MyGraph
{
	class NodeList<T> : Collection<Node<T>>
	{
		public Node<T> FindByValue(T value)
		{
			foreach(Node<T> node in Items)
				if(node.Value.Equals(value))
					return node;
			return null;
		}
	}
}
