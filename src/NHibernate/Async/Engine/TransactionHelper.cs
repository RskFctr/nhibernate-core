﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System.Data.Common;
using NHibernate.Engine.Transaction;
using NHibernate.Exceptions;

namespace NHibernate.Engine
{
	using System.Threading.Tasks;
	using System.Threading;
	public abstract partial class TransactionHelper
	{
		public partial class Work : IIsolatedWork
		{

			#region Implementation of IIsolatedWork

			public async Task DoWorkAsync(DbConnection connection, DbTransaction transaction, CancellationToken cancellationToken)
			{
				cancellationToken.ThrowIfCancellationRequested();
				try
				{
					generatedValue = await (owner.DoWorkInCurrentTransactionAsync(session, connection, transaction, cancellationToken)).ConfigureAwait(false);
				}
				catch (DbException sqle)
				{
					throw ADOExceptionHelper.Convert(session.Factory.SQLExceptionConverter, sqle, "could not get or update next value", null);
				}
			}

			#endregion
		}

		/// <summary> The work to be done</summary>
		public abstract Task<object> DoWorkInCurrentTransactionAsync(ISessionImplementor session, DbConnection conn, DbTransaction transaction, CancellationToken cancellationToken);

		/// <summary> Suspend the current transaction and perform work in a new transaction</summary>
		public virtual async Task<object> DoWorkInNewTransactionAsync(ISessionImplementor session, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			Work work = new Work(session, this);
			await (Isolater.DoIsolatedWorkAsync(work, session, cancellationToken)).ConfigureAwait(false);
			return work.generatedValue;
		}
	}
}