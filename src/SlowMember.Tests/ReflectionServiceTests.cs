using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SlowMember.Tests
{
    [TestClass]
    public class ReflectionServiceTests
    {

        private static ComplexClass _complexClass;
        private static IReflectionService _reflectionService;

        [ClassInitialize]
        public static void FixtureSetup(TestContext testContext)
        {
            _reflectionService = new ReflectionService();
            _complexClass = new ComplexClass
            {
                Text = "Test ComplexClass",
                Value = 1000,
                SubClasses = new List<SubClass>(5)
                {
                    new SubClass
                    {
                        Caption = "Test SubClass 0",
                        Description = 100,
                        List = new List<string>(6) {"0", "1", "2", "3", "4", "5"}
                    },
                    new SubClass
                    {
                        Caption = "Test SubClass 1",
                        Description = 100,
                        List = new List<string>(6) {"0", "1", "2", "3", "4", "5"}
                    },
                    new SubClass
                    {
                        Caption = "Test SubClass 2",
                        Description = 100,
                        List = new List<string>(6) {"0", "1", "2", "3", "4", "5"}
                    },
                    new SubClass
                    {
                        Caption = "Test SubClass 3",
                        Description = 100,
                        List = new List<string>(6) {"0", "1", "2", "3", "4", "5"}
                    },
                    new SubClass
                    {
                        Caption = "Test SubClass 4",
                        Description = 100,
                        List = new List<string>(6) {"0", "1", "2", "3", "4", "5"}
                    }
                }
            };
        }

        [TestMethod]
        public void Member_type_should_be_same_with_member_property_or_field_type()
        {
            _reflectionService.CacheDisabled = true;
            var description = _reflectionService.GetObjectDescription(_complexClass);
            var memberDescription = description.MemberDescriptions.FirstOrDefault(f => f.Name == "Text");
            Assert.IsNotNull(memberDescription);
            Assert.AreEqual(typeof (string), memberDescription.MemberType);
            memberDescription = description.MemberDescriptions.FirstOrDefault(f => f.Name == "Modified");
            Assert.IsNotNull(memberDescription);
            Assert.AreEqual(typeof (DateTime), memberDescription.MemberType);
            _reflectionService.CacheDisabled = false;
        }

        [TestMethod]
        public void Reflection_cache_performans_mesurement()
        {
            var reflectionService = new ReflectionService();
            const int maxParallelLoop = 1000;
            // Call one time to avoid first initialization for .net object cache
            reflectionService.CacheDisabled = true;
            // Warmup .Net
            reflectionService.GetObjectDescription(_complexClass);
            // Get ready...
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            Parallel.For(0, maxParallelLoop, i => { reflectionService.GetObjectDescription(_complexClass); });
            stopwatch.Stop();
            var elapsedNotCache = stopwatch.ElapsedMilliseconds;
            reflectionService.CacheDisabled = false;
            stopwatch.Reset();
            stopwatch.Start();
            Parallel.For(0, maxParallelLoop, i => { reflectionService.GetObjectDescription(_complexClass); });
            stopwatch.Stop();
            var elapsedCache = stopwatch.ElapsedMilliseconds;
            Debug.WriteLine(string.Format("Cached: {0}, Not Cached: {1}", elapsedCache, elapsedNotCache));
            //Assert.Less(elapsedCache, elapsedNotCache);
            Assert.IsTrue((elapsedCache < elapsedNotCache));
        }

        [TestMethod]
        public void Test_Member_description_attribute_count()
        {
            _reflectionService.CacheDisabled = true;
            var description = _reflectionService.GetObjectDescription(_complexClass);
            var memberDescription = description.MemberDescriptions.FirstOrDefault(f => f.Name == "Text");
            Assert.IsNotNull(memberDescription);
            Assert.AreEqual(2, memberDescription.AttributeDescriptions.Count);
            _reflectionService.CacheDisabled = false;
        }

        [TestMethod]
        public void Test_Object_description_attribute_count()
        {
            _reflectionService.CacheDisabled = true;
            var description = _reflectionService.GetObjectDescription(_complexClass);
            Assert.AreEqual(2, description.AttributeDescriptions.Count);
            _reflectionService.CacheDisabled = false;
        }

        [TestMethod]
        public void Test_Object_description_member_count()
        {
            _reflectionService.CacheDisabled = true;
            var description = _reflectionService.GetObjectDescription(_complexClass);
            Assert.AreEqual(5, description.MemberDescriptions.Count);
            _reflectionService.CacheDisabled = false;
        }
    }

    [TranslationContract]
    [Table("Complex")]
    public class ComplexClass
    {
        public ComplexClass()
        {
            SubClasses = new List<SubClass>(10);
        }

        [Key]
        public long PrimaryKeyId { get; set; }

        [TranslationMember(Name = "Title")]
        [Required(AllowEmptyStrings = true)]
        public string Text { get; set; }

        [System.ComponentModel.DataAnnotations.Range(0, 100)]
        public int Value { get; set; }

        public List<SubClass> SubClasses { get; set; }

        public DateTime Modified { get; set; }
    }

    [TranslationContract]
    public class SubClass
    {
        public SubClass()
        {
            List = new List<string>(10);
        }

        [TranslationMember(Name = "Title")]
        [Required(AllowEmptyStrings = true)]
        public string Caption { get; set; }

        [TranslationMember]
        [Required(AllowEmptyStrings = true)]
        public int Description { get; set; }

        public List<string> List { get; set; }
    }

    /// <summary>
    ///     Attribute to automate translation of the data objects
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TranslationContractAttribute : Attribute
    {
        /// <summary>
        ///     Name of the Entity/Table/Container
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Primary Key Field Name
        /// </summary>
        public string KeyFieldName { get; set; }

        public bool IsValid
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(KeyFieldName)) return true;
                return false;
            }
        }
    }

    /// <summary>
    ///     Attribute to automate translation of the data object's fields
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class TranslationMemberAttribute : Attribute
    {
        /// <summary>
        ///     Name of the member (Field/Property)
        /// </summary>
        public string Name { get; set; }

        public bool IsValid
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(Name)) return true;
                return false;
            }
        }
    }
}
