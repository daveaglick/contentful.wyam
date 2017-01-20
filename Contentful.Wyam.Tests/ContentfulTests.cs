using System;
using Wyam.Common.Documents;
using System.Collections.Generic;
using System.Linq;
using Wyam.Common.Execution;
using System.Collections;
using System.IO;
using Wyam.Common.Caching;
using Wyam.Common.Configuration;
using Wyam.Common.IO;
using Wyam.Common.Meta;
using Wyam.Common.Modules;
using Xunit;
using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Contentful.Core.Models.Management;
using Newtonsoft.Json.Linq;

namespace Contentful.Wyam.Tests
{
    public class ContentfulTests
    {
        [Fact]
        public void CallingContentfulShouldReturnCorrectNumberOfDocuments()
        {
            //Arrange
            var mockClient = new Mock<IContentfulClient>();
            mockClient.Setup(c => c.GetSpaceAsync(default(CancellationToken)))
                .ReturnsAsync(
                new Space()
                {
                    SystemProperties = new SystemProperties
                    {
                        Id = "467"
                    },
                    Locales = new List<Locale>()
                    {
                        new Locale()
                        {
                            Code = "en-US",
                            Default = true
                        }
                    }
                });

            var collection = new ContentfulCollection<Entry<dynamic>>();
            collection.Items = new List<Entry<dynamic>>()
            {
                new Entry<dynamic>() { Fields = new JObject(), SystemProperties = new SystemProperties() { Id="123" } },
                new Entry<dynamic>() { Fields = new JObject(), SystemProperties = new SystemProperties() { Id="3456" } },
                new Entry<dynamic>() { Fields = new JObject(), SystemProperties = new SystemProperties() { Id="62365" } },
                new Entry<dynamic>() { Fields = new JObject(), SystemProperties = new SystemProperties() { Id="tw635" } },
                new Entry<dynamic>() { Fields = new JObject(), SystemProperties = new SystemProperties() { Id="uer46" } },
                new Entry<dynamic>() { Fields = new JObject(), SystemProperties = new SystemProperties() { Id="jy456" } },
            };

            collection.IncludedAssets = new List<Asset>();
            collection.IncludedEntries = new List<Entry<dynamic>>();

            mockClient.Setup(c => c.GetEntriesCollectionAsync(It.IsAny<QueryBuilder<Entry<dynamic>>>(), default(CancellationToken)))
            .ReturnsAsync(collection);

            var mockContext = new Mock<IExecutionContext>();


            var contentful = new Contentful(mockClient.Object).WithContentField("body");

            //Act
            var res = contentful.Execute(new List<IDocument>(), mockContext.Object);

            //Assert
            Assert.Equal(6, res.Count());
        }
    }    
}
