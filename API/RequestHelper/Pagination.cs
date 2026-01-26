namespace API.RequestHelper
{
	public class pagination<T>(int pageIndex, int PageSize, int Count, IReadOnlyList<T>data)
	{
		public int PageIndex { get; set; } = pageIndex;
		public int PageSize { get; set; } = PageSize;
		public int Count { get; set; } = Count;
		public IReadOnlyList<T> Data { get; set; } = data;
	}

}

