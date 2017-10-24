﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System.Linq;
using NHibernate.Linq;
using NHibernate.DomainModel.Northwind.Entities;
using NUnit.Framework;

namespace NHibernate.Test.Linq
{
	using System.Threading.Tasks;
	[TestFixture]
	public class EagerLoadTestsAsync : LinqTestCase
	{
		[Test]
		public async Task CanSelectAndFetchAsync()
		{
			//NH-3075
			var result = await (db.Orders
			  .Select(o => o.Customer)
			  .Fetch(c => c.Orders)
			  .ToListAsync());

			session.Close();

			Assert.IsNotEmpty(result);
			Assert.IsTrue(NHibernateUtil.IsInitialized(result[0].Orders));
		}

		[Test]
		public async Task CanSelectAndFetchHqlAsync()
		{
			//NH-3075
			var result = await (this.session.CreateQuery("select c from Order o left join o.Customer c left join fetch c.Orders").ListAsync<Customer>());

			session.Close();

			Assert.IsNotEmpty(result);
			Assert.IsTrue(NHibernateUtil.IsInitialized(result[0].Orders));
		}

		[Test]
		public async Task RelationshipsAreLazyLoadedByDefaultAsync()
		{
			var x = await (db.Customers.ToListAsync());

			session.Close();

			Assert.AreEqual(91, x.Count);
			Assert.IsFalse(NHibernateUtil.IsInitialized(x[0].Orders));
		}

		[Test]
		public async Task RelationshipsCanBeEagerLoadedAsync()
		{
			var x = await (db.Customers.Fetch(c => c.Orders).ToListAsync());

			session.Close();

			Assert.AreEqual(91, x.Count);
			Assert.IsTrue(NHibernateUtil.IsInitialized(x[0].Orders));
			Assert.IsFalse(NHibernateUtil.IsInitialized(x[0].Orders.First().OrderLines));
		}

		[Test]
		public async Task MultipleRelationshipsCanBeEagerLoadedAsync()
		{
			var x = await (db.Employees.Fetch(e => e.Subordinates).Fetch(e => e.Orders).ToListAsync());

			session.Close();

			Assert.AreEqual(9, x.Count);
			Assert.IsTrue(NHibernateUtil.IsInitialized(x[0].Orders));
			Assert.IsTrue(NHibernateUtil.IsInitialized(x[0].Subordinates));
		}

		[Test]
		public async Task NestedRelationshipsCanBeEagerLoadedAsync()
		{
			var x = await (db.Customers.FetchMany(c => c.Orders).ThenFetchMany(o => o.OrderLines).ToListAsync());

			session.Close();

			Assert.AreEqual(91, x.Count);
			Assert.IsTrue(NHibernateUtil.IsInitialized(x[0].Orders));
			Assert.IsTrue(NHibernateUtil.IsInitialized(x[0].Orders.First().OrderLines));
		}

		[Test]
		public void WhenFetchSuperclassCollectionThenNotThrowsAsync()
		{
			// NH-2277
			Assert.That(() => session.Query<Lizard>().Fetch(x => x.Children).ToListAsync(), Throws.Nothing);
			session.Close();
		}

		[Test]
		public Task FetchWithWhereAsync()
		{
			try
			{
				// NH-2381 NH-2362
				return (from p in session.Query<Product>().Fetch(a => a.Supplier)
			 where p.ProductId == 1
			 select p).ToListAsync();
			}
			catch (System.Exception ex)
			{
				return Task.FromException<object>(ex);
			}
		}

		[Test]
		public Task FetchManyWithWhereAsync()
		{
			try
			{
						// NH-2381 NH-2362
				return (from s
				in session.Query<Supplier>().FetchMany(a => a.Products)
			 where s.SupplierId == 1
			 select s).ToListAsync();
			}
			catch (System.Exception ex)
			{
				return Task.FromException<object>(ex);
			}
		}

		[Test]
		public Task FetchAndThenFetchWithWhereAsync()
		{
			try
			{
				// NH-2362
				return (from p
				in session.Query<User>().Fetch(a => a.Role).ThenFetch(a => a.Entity)
			 where p.Id == 1
			 select p).ToListAsync();
			}
			catch (System.Exception ex)
			{
				return Task.FromException<object>(ex);
			}
		}

		[Test]
		public Task FetchAndThenFetchManyWithWhereAsync()
		{
			try
			{
				// NH-2362
				return (from p
				in session.Query<Employee>().Fetch(a => a.Superior).ThenFetchMany(a => a.Orders)
			 where p.EmployeeId == 1
			 select p).ToListAsync();
			}
			catch (System.Exception ex)
			{
				return Task.FromException<object>(ex);
			}
		}

		[Test]
		public Task FetchManyAndThenFetchWithWhereAsync()
		{
			try
			{
				// NH-2362
				return (from s
				in session.Query<Supplier>().FetchMany(a => a.Products).ThenFetch(a => a.Category)
			 where s.SupplierId == 1
			 select s).ToListAsync();
			}
			catch (System.Exception ex)
			{
				return Task.FromException<object>(ex);
			}
		}

		[Test]
		public Task FetchManyAndThenFetchManyWithWhereAsync()
		{
			try
			{
				// NH-2362
				return (from s
				in session.Query<Supplier>().FetchMany(a => a.Products).ThenFetchMany(a => a.OrderLines)
			 where s.SupplierId == 1
			 select s).ToListAsync();
			}
			catch (System.Exception ex)
			{
				return Task.FromException<object>(ex);
			}
		}

		[Test]
		public async Task WhereBeforeFetchAndOrderByAsync()
		{
			//NH-2915
			var firstOrderId = await (db.Orders.OrderBy(x => x.OrderId)
				.Select(x => x.OrderId)
				.FirstAsync());

			var orders = await (db.Orders
				.Where(x => x.OrderId != firstOrderId)
				.Fetch(x => x.Customer)
				.OrderBy(x => x.OrderId)
				.ToListAsync());

			Assert.AreEqual(829, orders.Count);
			Assert.IsTrue(NHibernateUtil.IsInitialized(orders[0].Customer));
		}
		
		[Test]
		public async Task WhereBeforeFetchManyAndOrderByAsync()
		{
			//NH-2915
			var firstOrderId = await (db.Orders.OrderBy(x => x.OrderId)
				.Select(x => x.OrderId)
				.FirstAsync());

			var orders = await (db.Orders
				.Where(x => x.OrderId != firstOrderId)
				.FetchMany(x => x.OrderLines)
				.OrderBy(x => x.OrderId)
				.ToListAsync());

			Assert.AreEqual(829, orders.Count);
			Assert.IsTrue(NHibernateUtil.IsInitialized(orders[0].OrderLines));
		}
		
		[Test]
		public async Task WhereBeforeFetchManyThenFetchAndOrderByAsync()
		{
			//NH-2915
			var firstOrderId = await (db.Orders.OrderBy(x => x.OrderId)
				.Select(x => x.OrderId)
				.FirstAsync());

			var orders = await (db.Orders
				.Where(x => x.OrderId != firstOrderId)
				.FetchMany(x => x.OrderLines)
				.ThenFetch(x => x.Product)
				.OrderBy(x => x.OrderId)
				.ToListAsync());

			Assert.AreEqual(829, orders.Count);
			Assert.IsTrue(NHibernateUtil.IsInitialized(orders[0].OrderLines));
			Assert.IsTrue(NHibernateUtil.IsInitialized(orders[0].OrderLines.First().Product));
		}

		[Test]
		public async Task WhereBeforeFetchAndSelectAsync()
		{
			//NH-3056
			var firstOrderId = await (db.Orders.OrderBy(x => x.OrderId)
				.Select(x => x.OrderId)
				.FirstAsync());

			var orders = await (db.Orders
				.Where(x => x.OrderId != firstOrderId)
				.Fetch(x => x.Customer)
				.Select(x => x)
				.ToListAsync());

			Assert.AreEqual(829, orders.Count);
			Assert.IsTrue(NHibernateUtil.IsInitialized(orders[0].Customer));
		}
		
		[Test]
		public async Task WhereBeforeFetchManyAndSelectAsync()
		{
			//NH-3056
			var firstOrderId = await (db.Orders.OrderBy(x => x.OrderId)
				.Select(x => x.OrderId)
				.FirstAsync());

			var orders = await (db.Orders
				.Where(x => x.OrderId != firstOrderId)
				.FetchMany(x => x.OrderLines)
				.Select(x => x)
				.ToListAsync());

			Assert.AreEqual(829, orders.Count);
			Assert.IsTrue(NHibernateUtil.IsInitialized(orders[0].OrderLines));
		}
		
		[Test]
		public async Task WhereBeforeFetchManyThenFetchAndSelectAsync()
		{
			//NH-3056
			var firstOrderId = await (db.Orders.OrderBy(x => x.OrderId)
				.Select(x => x.OrderId)
				.FirstAsync());

			var orders = await (db.Orders
				.Where(x => x.OrderId != firstOrderId)
				.FetchMany(x => x.OrderLines)
				.ThenFetch(x => x.Product)
				.Select(x => x)
				.ToListAsync());

			Assert.AreEqual(829, orders.Count);
			Assert.IsTrue(NHibernateUtil.IsInitialized(orders[0].OrderLines));
			Assert.IsTrue(NHibernateUtil.IsInitialized(orders[0].OrderLines.First().Product));
		}

		[Test]
		public async Task WhereBeforeFetchAndWhereAsync()
		{
			var firstOrderId = await (db.Orders.OrderBy(x => x.OrderId)
				.Select(x => x.OrderId)
				.FirstAsync());

			var orders = await (db.Orders
				.Where(x => x.OrderId != firstOrderId)
				.Fetch(x => x.Customer)
				.Where(x => true)
				.ToListAsync());

			Assert.AreEqual(829, orders.Count);
			Assert.IsTrue(NHibernateUtil.IsInitialized(orders[0].Customer));
		}
		
		[Test]
		public async Task WhereBeforeFetchManyAndWhereAsync()
		{
			var firstOrderId = await (db.Orders.OrderBy(x => x.OrderId)
				.Select(x => x.OrderId)
				.FirstAsync());

			var orders = await (db.Orders
				.Where(x => x.OrderId != firstOrderId)
				.FetchMany(x => x.OrderLines)
				.Where(x => true)
				.ToListAsync());

			Assert.AreEqual(829, orders.Count);
			Assert.IsTrue(NHibernateUtil.IsInitialized(orders[0].OrderLines));
		}
		
		[Test]
		public async Task WhereBeforeFetchManyThenFetchAndWhereAsync()
		{
			var firstOrderId = await (db.Orders.OrderBy(x => x.OrderId)
				.Select(x => x.OrderId)
				.FirstAsync());

			var orders = await (db.Orders
				.Where(x => x.OrderId != firstOrderId)
				.FetchMany(x => x.OrderLines)
				.ThenFetch(x => x.Product)
				.Where(x => true)
				.ToListAsync());

			Assert.AreEqual(829, orders.Count);
			Assert.IsTrue(NHibernateUtil.IsInitialized(orders[0].OrderLines));
			Assert.IsTrue(NHibernateUtil.IsInitialized(orders[0].OrderLines.First().Product));
		}

		[Test]
		public async Task WhereAfterFetchAndSingleOrDefaultAsync()
		{
			//NH-3186
			var firstOrderId = await (db.Orders.OrderBy(x => x.OrderId)
				.Select(x => x.OrderId)
				.FirstAsync());

			var order = await (db.Orders
				.Fetch(x => x.Shipper)
				.SingleOrDefaultAsync(x => x.OrderId == firstOrderId));

			Assert.IsTrue(NHibernateUtil.IsInitialized(order.Shipper));
		}
	}
}
