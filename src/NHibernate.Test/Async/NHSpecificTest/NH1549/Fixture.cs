﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH1549
{
	using System.Threading.Tasks;
	[TestFixture]
	public class FixtureAsync : BugTestCase
	{
		/// <summary>
		/// Verifies that an entity with a base class containing the id property 
		/// can have the id accessed without loading the entity
		/// </summary>
		[Test]
		public async Task CanLoadForEntitiesWithInheritedIdsAsync()
		{
			//create some related products
			var category = new CategoryWithInheritedId {Name = "Fruit"};
			var product = new ProductWithInheritedId {CategoryWithInheritedId = category};

			using (ISession session = OpenSession())
			{
				using (ITransaction trans = session.BeginTransaction())
				{
					await (session.SaveAsync(category));
					await (session.SaveAsync(product));
					await (trans.CommitAsync());
				}
			}

			ProductWithInheritedId restoredProductWithInheritedId;

			//restore the product from the db in another session so that 
			//the association is a proxy
			using (ISession session = OpenSession())
			{
				restoredProductWithInheritedId = await (session.GetAsync<ProductWithInheritedId>(product.Id));
			}
			
			//verify that the category is a proxy
			Assert.IsFalse(NHibernateUtil.IsInitialized(restoredProductWithInheritedId.CategoryWithInheritedId));

			//we should be able to access the id of the category outside of the session
			Assert.AreEqual(category.Id, restoredProductWithInheritedId.CategoryWithInheritedId.Id);
		}

		[Test]
		public async Task CanLoadForEntitiesWithTheirOwnIdsAsync()
		{
			//create some related products
			var category = new CategoryWithId { Name = "Fruit" };
			var product = new ProductWithId { CategoryWithId = category };

			using (ISession session = OpenSession())
			{
				using (ITransaction trans = session.BeginTransaction())
				{
					await (session.SaveAsync(category));
					await (session.SaveAsync(product));
					await (trans.CommitAsync());
				}
			}

			ProductWithId restoredProductWithInheritedId;

			//restore the product from the db in another session so that 
			//the association is a proxy
			using (ISession session = OpenSession())
			{
				restoredProductWithInheritedId = await (session.GetAsync<ProductWithId>(product.Id));
			}

			//verify that the category is a proxy
			Assert.IsFalse(NHibernateUtil.IsInitialized(restoredProductWithInheritedId.CategoryWithId));

			//we should be able to access the id of the category outside of the session
			Assert.AreEqual(category.Id, restoredProductWithInheritedId.CategoryWithId.Id);
		}
		
		protected override void OnTearDown()
		{
			using (ISession session = OpenSession()) {

				using (ITransaction trans = session.BeginTransaction()) 
				{
					session.Delete("from ProductWithId");
					session.Delete("from CategoryWithId");
					session.Delete("from ProductWithInheritedId");
					session.Delete("from CategoryWithInheritedId");
					trans.Commit();
				}
			}	
		}
	}
}