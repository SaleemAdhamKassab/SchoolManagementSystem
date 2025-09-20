using Microsoft.EntityFrameworkCore;

namespace SchoolManagementSystem.Application.Contracts.Common
{
	public class PaginatedList<T>(List<T> items, int count, int pageIndex, int pageSize)
	{
		public int PageIndex { get; private set; } = pageIndex;
		public int PageSize { get; private set; } = pageSize;
		public int TotalCount { get; private set; } = count;
		public int TotalPages { get; private set; } = (int)Math.Ceiling(count / (double)pageSize);
		public List<T> Items { get; private set; } = items;

		public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize, CancellationToken cancellationToken)
		{

			if (source == null)
				return new PaginatedList<T>([], 0, pageIndex, pageSize);

			if (pageIndex < 0)
				pageIndex = 0;

			if (pageSize < 1)
				pageSize = 10;

			var count = await source.CountAsync(cancellationToken);

			var items = await source
				.Skip((pageIndex - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync(cancellationToken);

			return new PaginatedList<T>(items, count, pageIndex, pageSize);
		}
	}
}