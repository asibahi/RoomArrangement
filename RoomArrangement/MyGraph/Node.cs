namespace RoomArrangement.MyGraph
{
	class Node<T>
	{
		public T Value { get; set; }
		public NodeList<T> Neighbors { get; set; }

		public Node(T data) : this(data, new NodeList<T>()) { }
		public Node(T data, NodeList<T> nodeList)
		{
			Value = data;
			Neighbors = nodeList;
		}
	}
}