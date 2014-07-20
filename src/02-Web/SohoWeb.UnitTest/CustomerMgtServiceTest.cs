using SohoWeb.Service.Customer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SohoWeb.Entity.Customer;
using SohoWeb.Entity.Enums;
using System.Collections.Generic;

namespace SohoWeb.UnitTest
{
    
    
    /// <summary>
    ///This is a test class for CustomerMgtServiceTest and is intended
    ///to contain all CustomerMgtServiceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CustomerMgtServiceTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for InsertCustomer
        ///</summary>
        [TestMethod()]
        public void InsertCustomerTest()
        {
            CustomerMgtService target = new CustomerMgtService(); // TODO: Initialize to an appropriate value
            CustomerInfo entity = new CustomerInfo()
            {
                CustomerID = "Test1@126.com",
                Password = "123",
                Status = CustomerStatus.Valid,
                RegIP = "192.168.1.104"
            };
            var customerSysNo = CustomerMgtService.Instance.InsertCustomer(entity);
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            actual = target.InsertCustomer(entity);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for UpdateCustomerBaseBySysNo
        ///</summary>
        [TestMethod()]
        public void UpdateCustomerBaseBySysNoTest()
        {
            CustomerMgtService target = new CustomerMgtService(); // TODO: Initialize to an appropriate value
            CustomerInfo entity = new CustomerInfo()
            {
                SysNo = 100002,
                CustomerID = "Test@126.com",
                CustomerName = "Test",
                Status = CustomerStatus.InValid
            };
            target.UpdateCustomerBaseBySysNo(entity);
        }

        /// <summary>
        ///A test for UpdateCustomerStatusBySysNo
        ///</summary>
        [TestMethod()]
        public void UpdateCustomerStatusBySysNoTest()
        {
            CustomerMgtService target = new CustomerMgtService(); // TODO: Initialize to an appropriate value
            CustomerInfo entity = new CustomerInfo()
            {
                SysNo = 100002,
                Status = CustomerStatus.Deleted
            };
            target.UpdateCustomerStatusBySysNo(entity);
        }

        /// <summary>
        ///A test for UpdateCustomerPasswordByUserID
        ///</summary>
        [TestMethod()]
        public void UpdateCustomerPasswordByUserIDTest()
        {
            CustomerMgtService target = new CustomerMgtService(); // TODO: Initialize to an appropriate value
            CustomerInfo entity = new CustomerInfo()
            {
                CustomerID = "Test1@126.com",
                Password = "123321"
            };
            string oldPassword = "123";
            target.UpdateCustomerPasswordByUserID(entity, oldPassword);
        }

        /// <summary>
        ///A test for GetValidCustomerByCustomerID
        ///</summary>
        [TestMethod()]
        public void GetValidCustomerByCustomerIDTest()
        {
            CustomerMgtService target = new CustomerMgtService(); // TODO: Initialize to an appropriate value
            string customerID = "Test1@126.com"; // TODO: Initialize to an appropriate value
            CustomerInfo actual;
            actual = target.GetValidCustomerByCustomerID(customerID);
        }

        /// <summary>
        ///A test for GetValidCustomerListByCustomerID
        ///</summary>
        [TestMethod()]
        public void GetValidCustomerListByCustomerIDTest()
        {
            CustomerMgtService target = new CustomerMgtService(); // TODO: Initialize to an appropriate value
            string customerID = "Test1@126.com"; // TODO: Initialize to an appropriate value
            List<CustomerInfo> actual;
            actual = target.GetValidCustomerListByCustomerID(customerID);
        }

        /// <summary>
        ///A test for GetValidCustomerByCustomerSysNo
        ///</summary>
        [TestMethod()]
        public void GetValidCustomerByCustomerSysNoTest()
        {
            CustomerMgtService target = new CustomerMgtService(); // TODO: Initialize to an appropriate value
            int customerSysNo = 100003; // TODO: Initialize to an appropriate value
            CustomerInfo actual;
            actual = target.GetValidCustomerByCustomerSysNo(customerSysNo);
        }
    }
}
