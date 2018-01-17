using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PeopleRecords.Interfaces;
using PeopleRecords.DataAccess;
using PeopleRecords.Models;
using Moq;
using System.Collections.Generic;

namespace PeopleRecords.UnitTests
{
    [TestClass]
    public class LineReaderTests
    {
        Mock<IPeopleRepository> repository = new Mock<IPeopleRepository>();

        [TestInitialize]
        public void Initialize()
        {
            repository.Reset();
        }

        [TestMethod]
        public void TestLineSuccess()
        {
            List<Person> peeps = new List<Person>();

            // There is a discrepancy that gets introduced here, if we don't parse the stringified value.
            DateTimeOffset nowTime = DateTimeOffset.Now;
            string now = nowTime.ToString();

            nowTime = DateTimeOffset.Parse(now);
            var testPerson = new Person("last", "first", "gender", nowTime, "favoriteColor");

            string[] lines = new string[] {
                $"last,first,gender,favoriteColor,{now}",
                $"last, first, gender, favoriteColor, {now}",
                $"last|first|gender|favoriteColor|{now}",
                $"last| first| gender| favoriteColor| {now}",
                $"last first gender favoriteColor {now}",
                $"last  first  gender  favoriteColor  {now}",
            };

            LineReader.ImportFileIntoRepository(lines, repository.Object);

            repository.Verify(x => x.CreatePerson(testPerson), Times.Exactly(6));
        }


        [TestMethod]
        public void TestLineFailsForBadDateTimeOffSet()
        {
            List<Person> peeps = new List<Person>();

            string now = "abasd";

            string[] lines = new string[] {
                $"last,first,gender,favoriteColor,{now}",
                $"last, first, gender, favoriteColor, {now}",
                $"last|first|gender|favoriteColor|{now}",
                $"last| first| gender| favoriteColor| {now}",
                $"last first gender favoriteColor {now}",
                $"last  first  gender  favoriteColor  {now}",
            };

            Assert.ThrowsException<ArgumentOutOfRangeException>(()=>LineReader.ImportFileIntoRepository(lines, repository.Object));
        }

        [TestMethod]
        public void TestLineSuccessForExtraSpaces()
        {
            List<Person> peeps = new List<Person>();

            // There is a discrepancy that gets introduced here, if we don't parse the stringified value.
            DateTimeOffset nowTime = DateTimeOffset.Now;
            string now = nowTime.ToString();

            nowTime = DateTimeOffset.Parse(now);
            var testPerson = new Person("last name", "first name", "my gender", nowTime, "favorite Color");

            string[] lines = new string[] {
                $"last name,first name,my gender,favorite Color,{now}",
                $"last name , first name , my gender , favorite Color , {now}",
                $"last name|first name|my gender|favorite Color|{now}",
                $"last name | first name | my gender | favorite Color | {now}",
            };

            LineReader.ImportFileIntoRepository(lines, repository.Object);

            repository.Verify(x => x.CreatePerson(testPerson), Times.Exactly(4));
        }

        [TestMethod]
        public void TestLineFailsOnBadLine_WrongDelimiter()
        {
            // There is a discrepancy that gets introduced here, if we don't parse the stringified value.
            DateTimeOffset nowTime = DateTimeOffset.Now;
            string now = nowTime.ToString();
            nowTime = DateTimeOffset.Parse(now);

            string[] lines = new string[] {
                $"last-first-gender-favoriteColor-{now}",
            };

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                LineReader.ImportFileIntoRepository(lines, repository.Object));
        }

        [TestMethod]
        public void TestLineFailsOnBadLine_ShortLine()
        {
            // There is a discrepancy that gets introduced here, if we don't parse the stringified value.
            DateTimeOffset nowTime = DateTimeOffset.Now;
            string now = nowTime.ToString();
            nowTime = DateTimeOffset.Parse(now);

            string[] lines = new string[] {
                $"last-first-gender-favoriteColor",
            };

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                LineReader.ImportFileIntoRepository(lines, repository.Object));
        }
    }
}
