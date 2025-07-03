using System.Linq.Expressions;

namespace SnapToTable.Domain.Common;

public record SortDescriptor<T>(
    Expression<Func<T, object>> KeySelector, 
    SortDirection Direction
);