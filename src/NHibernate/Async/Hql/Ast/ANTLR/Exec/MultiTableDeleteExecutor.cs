﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using NHibernate.Engine;
using NHibernate.Exceptions;
using NHibernate.Hql.Ast.ANTLR.Tree;
using NHibernate.Param;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Util;
using IQueryable = NHibernate.Persister.Entity.IQueryable;

namespace NHibernate.Hql.Ast.ANTLR.Exec
{
	using System.Threading.Tasks;
	using System.Threading;
	public partial class MultiTableDeleteExecutor : AbstractStatementExecutor
	{

		public override async Task<int> ExecuteAsync(QueryParameters parameters, ISessionImplementor session, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			await (CoordinateSharedCacheCleanupAsync(session, cancellationToken)).ConfigureAwait(false);

			await (CreateTemporaryTableIfNecessaryAsync(persister, session, cancellationToken)).ConfigureAwait(false);

			try
			{
				// First, save off the pertinent ids, saving the number of pertinent ids for return
				DbCommand ps = null;
				int resultCount;
				try
				{
					try
					{
						var paramsSpec = Walker.Parameters;
						var sqlQueryParametersList = idInsertSelect.GetParameters().ToList();
						SqlType[] parameterTypes = paramsSpec.GetQueryParameterTypes(sqlQueryParametersList, session.Factory);

						ps = await (session.Batcher.PrepareCommandAsync(CommandType.Text, idInsertSelect, parameterTypes, cancellationToken)).ConfigureAwait(false);
						foreach (var parameterSpecification in paramsSpec)
						{
							await (parameterSpecification.BindAsync(ps, sqlQueryParametersList, parameters, session, cancellationToken)).ConfigureAwait(false);
						}

						resultCount = await (session.Batcher.ExecuteNonQueryAsync(ps, cancellationToken)).ConfigureAwait(false);
					}
					finally
					{
						if (ps != null)
						{
							session.Batcher.CloseCommand(ps, null);
						}
					}
				}
				catch (DbException e)
				{
					throw ADOExceptionHelper.Convert(Factory.SQLExceptionConverter, e, "could not insert/select ids for bulk delete", idInsertSelect);
				}

				// Start performing the deletes
				for (int i = 0; i < deletes.Length; i++)
				{
					try
					{
						try
						{
							ps = await (session.Batcher.PrepareCommandAsync(CommandType.Text, deletes[i], new SqlType[0], cancellationToken)).ConfigureAwait(false);
							await (session.Batcher.ExecuteNonQueryAsync(ps, cancellationToken)).ConfigureAwait(false);
						}
						finally
						{
							if (ps != null)
							{
								session.Batcher.CloseCommand(ps, null);
							}
						}
					}
					catch (DbException e)
					{
						throw ADOExceptionHelper.Convert(Factory.SQLExceptionConverter, e, "error performing bulk delete", deletes[i]);
					}
				}

				return resultCount;
			}
			finally
			{
				await (DropTemporaryTableIfNecessaryAsync(persister, session, cancellationToken)).ConfigureAwait(false);
			}
		}
	}
}