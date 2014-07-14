using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml.Linq;

namespace Recipe.Web.Services
{
    public class MeasureConverterController : ApiController
    {
        public XElement GetMeasures()
        {
            var items = XElement.Load(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/RecipeMeasures.xml"));
            return items;
        }
    }
}
