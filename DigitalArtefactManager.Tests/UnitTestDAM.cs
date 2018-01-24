using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using DigitalArtefactManager.Models;

namespace DigitalArtefactManager.Tests
{
    [TestClass]
    public class UnitTestDAM
    {
        [TestMethod]
        public void Validate_Model_Given_UserName_Exceeds_50_Characters()
        {
            var model = new User()
            {
                UserName = new string('*', 51)
            };
            var results = TestModelHelper.Validate(model);
            Assert.AreNotEqual(1, results.Count);
        }


        [TestMethod]
        public void Validate_Model_Given_ArticleTitle_Exceeds_100_Characters()
        {
            var model = new Article()
            {
                Title = new string('*', 101)
            };
            var results = TestModelHelper.Validate(model);
            Assert.AreNotEqual(1, results.Count);
        }


        [TestMethod]
        public void Validate_Model_Given_Title_Is_Null_ExpectOneValidationError()
        {
            var model = new Article();

            var results = TestModelHelper.Validate(model);

            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("The Article Title field is required.", results[0].ErrorMessage);
        }
    }   
}
