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
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH1920
{
	using System.Threading.Tasks;
	[TestFixture]
	public class FixtureAsync : BugTestCase
	{

		[Test] 
		public async Task Can_Query_Without_Collection_Size_ConditionAsync() 
		{ 
			using (ISession sess = OpenSession()) 
			using (ITransaction tx = sess.BeginTransaction()) 
			{ 
				await (sess.SaveOrUpdateAsync(new Customer() { IsDeleted = false })); 
				await (tx.CommitAsync()); 
			} 
			using (ISession sess = OpenSession()) 
			using (ITransaction tx = sess.BeginTransaction()) 
			{ 
				sess.EnableFilter("state").SetParameter("deleted", false); 
				var result = await (sess 
					.CreateQuery("from Customer c join c.Orders o where c.id > :cid") 
					.SetParameter("cid", 0) 
					.ListAsync()); 
				Assert.That(result.Count == 0); 
				await (tx.CommitAsync()); 
			} 
			using (ISession sess = OpenSession()) 
			using (ITransaction tx = sess.BeginTransaction()) 
			{ 
				await (sess.DeleteAsync("from System.Object")); 
				await (tx.CommitAsync()); 
			} 
		} 

		[Test] 
		public async Task Can_Query_With_Collection_Size_ConditionAsync()
		{
			if (!Dialect.SupportsScalarSubSelects)
				Assert.Ignore("Dialect does not support scalar sub-select");

			using (ISession sess = OpenSession()) 
			using (ITransaction tx = sess.BeginTransaction()) 
			{ 
				await (sess.SaveOrUpdateAsync(new Customer() { IsDeleted = false })); 
				await (tx.CommitAsync()); 
			} 
			using (ISession sess = OpenSession()) 
			using (ITransaction tx = sess.BeginTransaction()) 
			{ 
				sess.EnableFilter("state").SetParameter("deleted", false); 
				var result = await (sess 
					.CreateQuery("from Customer c join c.Orders o where c.id > :cid and c.Orders.size > 0") 
					.SetParameter("cid", 0) 
					.ListAsync()); 
				Assert.That(result.Count == 0); 
				await (tx.CommitAsync()); 
			} 
			using (ISession sess = OpenSession()) 
			using (ITransaction tx = sess.BeginTransaction()) 
			{ 
				await (sess.DeleteAsync("from System.Object")); 
				await (tx.CommitAsync()); 
			} 
		} 

	}
}
