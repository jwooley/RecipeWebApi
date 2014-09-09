using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecipeDal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data;
using System.Data.SqlClient;

namespace Repair.Tests
{
    [TestClass]
    public class RecipeContextTests
    {
        [TestMethod]
        public void Context_CanCreateDatabase()
        {
            var targetPath = @"C:\projects\temp\testdb.mdf";
            try
            {
                if (File.Exists(targetPath))
                    File.Delete(targetPath);
            }
            catch
            {
            }

            using (var context = new RecipeContext(@"Server =.; AttachDbFilename =" + targetPath + "; Database = RecipeMdf; Trusted_Connection = Yes;"))
            {
                Assert.IsFalse(context.Recipes.Any());
            }
            Assert.IsTrue(File.Exists(targetPath));
        }
    }
}
