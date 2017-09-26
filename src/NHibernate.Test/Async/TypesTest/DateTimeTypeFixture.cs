﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using NHibernate.Type;
using NUnit.Framework;

namespace NHibernate.Test.TypesTest
{
	using System.Threading.Tasks;
	using System.Threading;
	/// <summary>
	/// TestFixtures for the <see cref="DateTimeType"/>.
	/// </summary>
	[TestFixture]
	public class DateTimeTypeFixtureAsync
	{
		[Test]
		public async Task NextAsync()
		{
			DateTimeType type = (DateTimeType) NHibernateUtil.DateTime;
			object current = DateTime.Parse("2004-01-01");
			object next = await (type.NextAsync(current, null, CancellationToken.None));

			Assert.IsTrue(next is DateTime, "Next should be DateTime");
			Assert.IsTrue((DateTime) next > (DateTime) current,
						  "next should be greater than current (could be equal depending on how quickly this occurs)");
		}

		[Test]
		public async Task SeedAsync()
		{
			DateTimeType type = (DateTimeType) NHibernateUtil.DateTime;
			Assert.IsTrue(await (type.SeedAsync(null, CancellationToken.None)) is DateTime, "seed should be DateTime");
		}
	}
}