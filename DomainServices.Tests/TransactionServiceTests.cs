using AutoMapper;
using DataLibrary.Interfaces;
using DomainServices.Exceptions;
using DomainServices.Services;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainServices.Tests
{
    public class TransactionServiceTests
    {
        [Test]
        public void Get_NoException()
        {
            IMapper mapper = Substitute.For<IMapper>();
            ITransactionRepository repository = Substitute.For<ITransactionRepository>();
            repository.Get(1).Returns(Task.FromResult(new DataLibrary.Models.Transaction()));
            TransactionService service = new(mapper, repository);
            Assert.DoesNotThrowAsync(() => service.Get(1));
        }

        [Test]
        public void Get_Exception()
        {
            IMapper mapper = Substitute.For<IMapper>();
            ITransactionRepository repository = Substitute.For<ITransactionRepository>();
            TransactionService service = new(mapper, repository);
            Assert.ThrowsAsync<NotFoundException>(() => service.Get(1));
        }
    }
}
