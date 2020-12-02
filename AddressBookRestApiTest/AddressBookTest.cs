using AddressBookRestAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient.Memcached;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Net;

namespace AddressBookRestApiTest
{
    [TestClass]
    public class AddressBookTest
    {
        RestClient restClient = new RestClient("http://localhost:3000");
        
        private IRestResponse GetAddressBook()
        {
            RestRequest request = new RestRequest("/addressBook", Method.GET);

            IRestResponse response = restClient.Execute(request);
            return response;
        }
       
        [TestMethod]
        public void Given_RetrieveContactsFromDatabase_ShouldReturnTrue()
        {
            RestRequest request = new RestRequest("/addressBook", Method.GET);
            IRestResponse response = restClient.Execute(request);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            List<AddressBookModel> dataResponse = JsonConvert.DeserializeObject<List<AddressBookModel>>(response.Content);
            Assert.AreEqual(4, dataResponse.Count);
        }
    }
}
