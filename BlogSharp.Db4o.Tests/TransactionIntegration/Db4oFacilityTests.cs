﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Remoting.Messaging;
using System.Text;
using BlogSharp.Db4o.Impl;
using Castle.Core.Resource;
using Castle.Facilities.AutomaticTransactionManagement;
using Castle.Services.Transaction;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using Db4objects.Db4o;
using Db4objects.Db4o.Ext;
using Xunit;

namespace BlogSharp.Db4o.Tests.TransactionIntegration
{
	public class Db4oFacilityTests:IDisposable
	{
		public Db4oFacilityTests()
		{
			File.Delete("mydb.yap");
			CallContext.SetData(ThreadContextObjectContainerStore.CONTEXTKEY,null);
		}

		private const string CONFIGFILE=@"BlogSharp.Db4o.Tests/TransactionIntegration/CastleConfiguration.xml";
		[Fact]
		public void Can_configure_facility_without_exception()
		{
			using (var container = new WindsorContainer(new XmlInterpreter(new AssemblyResource(CONFIGFILE))))
			{

			}
		}

		[Fact]
		public void Registers_default_components()
		{
			using (var windsorContainer = new WindsorContainer(new XmlInterpreter(new AssemblyResource(CONFIGFILE))))
			{


				var types = new[]
				            	{
				            		typeof (IObjectServerConfigurationBuilder),
				            		typeof (IObjectContainerManager),
				            		typeof (IObjectContainerStore),
				            		typeof (IObjectContainerProviderProvider)
				            	};
				foreach (var type in types)
				{
					Assert.True(windsorContainer.Kernel.HasComponent(type), string.Format("{0} was not registered", type.Name));
				}
			}
		}

		[Fact]
		public void Register_appropriate_provider()
		{
			using (var windsorContainer = new WindsorContainer(new XmlInterpreter(new AssemblyResource(CONFIGFILE))))
			{

			}
		}

		[Fact]
		public void Does_not_dispose_when_in_transaction()
		{
			using (var windsorContainer = new WindsorContainer(
				new XmlInterpreter(
					new AssemblyResource(CONFIGFILE))))
			{
				var db4oContainerManager = windsorContainer.Resolve<IObjectContainerManager>();
				var transactionManager = windsorContainer.Resolve<ITransactionManager>();
				ITransaction transaction = transactionManager.CreateTransaction(TransactionMode.RequiresNew,
																				IsolationMode.ReadCommitted);
				transaction.Begin();
				IExtObjectContainer objectContainer = db4oContainerManager.GetContainer().Ext();
				objectContainer.Store(new MailAddress("mymail@mymail.ccc", "Tuna Toksoz"));
				objectContainer.Dispose();
				Assert.False(objectContainer.IsClosed(),"Database shouldn't be closed");
				transaction.Rollback();
				transactionManager.Dispose(transaction);
			}
		}

		[Fact]
		public void Does_commit_when_not_in_transaction()
		{
			using (var windsorContainer = new WindsorContainer(
				new XmlInterpreter(
					new AssemblyResource(CONFIGFILE))))
			{
				var db4oContainerManager = windsorContainer.Resolve<IObjectContainerManager>();
				var transactionManager = windsorContainer.Resolve<ITransactionManager>();
				
				var tran = transactionManager.CurrentTransaction;
				ITransaction transaction = transactionManager.CreateTransaction(TransactionMode.RequiresNew,IsolationMode.ReadCommitted);
				
				transaction.Begin();
				IObjectServer server = windsorContainer.Resolve<IObjectServer>();
				IExtObjectContainer objectContainer = db4oContainerManager.GetContainer().Ext();
				Assert.False(objectContainer.IsClosed());
				objectContainer.Store(new MailAddress("mymail@mymail.ccc", "Tuna Toksoz"));
				//Assert.True(secondaryContainer.Query<MailAddress>().Count == 0);
				//transaction.Commit();
				//Assert.True(secondaryContainer.Query<MailAddress>().Count == 1);
			}
		}

		#region IDisposable Members

		public void Dispose()
		{
			File.Delete("mydb.yap");
		}

		#endregion
	}
}