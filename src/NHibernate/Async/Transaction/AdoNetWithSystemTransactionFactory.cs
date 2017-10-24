﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Transactions;
using NHibernate.AdoNet;
using NHibernate.Engine;
using NHibernate.Engine.Transaction;
using NHibernate.Impl;
using NHibernate.Util;

namespace NHibernate.Transaction
{
	using System.Threading.Tasks;
	public partial class AdoNetWithSystemTransactionFactory : AdoNetTransactionFactory
	{

		/// <inheritdoc />
		public override async Task ExecuteWorkInIsolationAsync(ISessionImplementor session, IIsolatedWork work, bool transacted, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			using (var tx = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled))
			{
				await (base.ExecuteWorkInIsolationAsync(session, work, transacted, cancellationToken)).ConfigureAwait(false);
				tx.Complete();
			}
		}
	}
}
