using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using BlogSharp.Core.DataAccess;
using BlogSharp.Core.Impl.DataAccess;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;
using Rhino.Mocks;
using Xunit;
using Model = BlogSharp.Model;
using MPost=BlogSharp.Model.Impl.Post;
namespace BlogSharp.Core.Impl.Tests.DataAccess
{
	public class Db4oRepositoryTests:BaseTest
	{
		public Db4oRepositoryTests()
		{

			this.objectContainer = MockRepository.GenerateMock<IObjectContainer>();
			this.postRepository = new Db4oRepository<Model.IPost>(this.objectContainer);
			
		}

		public override void  OnTearDown()
		{
			this.objectContainer.Close();
		}
		private readonly IObjectContainer objectContainer;
		private readonly IRepository<Model.IPost> postRepository;
		[Fact]
		public void CanStore()
		{
			Model.IPost post = this.GetEntityFactory<Model.IPost>().Create();
			postRepository.Add(post);
			this.objectContainer.AssertWasCalled(x=>x.Store(post));
			
		}
		[Fact]
		public void CanDelete()
		{
			Model.IPost post = this.GetEntityFactory<Model.IPost>().Create();
			postRepository.Remove(post);
			this.objectContainer.AssertWasCalled(x=>x.Delete(post));
		}

		//[Fact]
		//public void CanQuery()
		//{
		//    var result = postRepository.GetByExpression(x => x.FriendlyTitle.StartsWith("osman"));
		//    this.objectContainer.AssertWasCalled(x => x.Cast<Model.IPost>());
		//}
	}
}