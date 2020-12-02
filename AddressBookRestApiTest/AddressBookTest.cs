using AddressBookRestAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient.Memcached;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        [TestMethod]
        public void GivenMultipleContactEntries_UsingPostOperation_ShouldReturnAddedContacts()
        {
            List<AddressBookModel> contactList = new List<AddressBookModel>();
            contactList.Add(new AddressBookModel { firstName = "RAJU",secondName="SHAH", address = "RN nagar", city="tiruvallur",state="tamilnadu",zip=90876,phoneNumber = 999888999888, emailid = "raju@gmail.com", contactType = "Professional" , addressBookName="local"});
            contactList.Add(new AddressBookModel { firstName = "Somesh", secondName = "SHAH", address = "hope farm", city = "Bangalore", state = "Karnataka", zip = 90876, phoneNumber = 999888999888, emailid = "somesh@gmail.com", contactType = "Friends", addressBookName = "local" });
            foreach (AddressBookModel contact in contactList)
            {
                RestRequest request = new RestRequest("/addressBook", Method.POST);
                JObject jObject = new JObject();
                jObject.Add("FirstName", contact.firstName);
                jObject.Add("secodName", contact.secondName);
                jObject.Add("address", contact.address);
                jObject.Add("city", contact.city);
                jObject.Add("state", contact.state);
                jObject.Add("zip", contact.zip);
                jObject.Add("phonenumber", contact.phoneNumber);
                jObject.Add("emailid", contact.emailid);
                jObject.Add("contactType", contact.contactType);
                jObject.Add("addressBookName", contact.addressBookName);
                request.AddParameter("application/json", jObject, ParameterType.RequestBody);
                IRestResponse response = restClient.Execute(request);
                //Assert
                Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
                //derserializing object for assert and checking test case
                AddressBookModel dataResponse = JsonConvert.DeserializeObject<AddressBookModel>(response.Content);
                Assert.AreEqual(contact.firstName, dataResponse.firstName);
            }
        }
        [TestMethod]
        public void UpdateDataUsingPutOperation()
        {
            RestRequest request = new RestRequest("addressBook/5", Method.PUT);
            JObject jobject = new JObject();
            jobject.Add("firstname", "Shewag");
            jobject.Add("contactType", "Batsmen");
            request.AddParameter("application/json", jobject, ParameterType.RequestBody);
            IRestResponse response = restClient.Execute(request);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            AddressBookModel dataResponse = JsonConvert.DeserializeObject<AddressBookModel>(response.Content);
            Assert.AreEqual(dataResponse.firstName, "Shewag");
            Assert.AreEqual(dataResponse.contactType, "Batsmen");
        }
    }
}
