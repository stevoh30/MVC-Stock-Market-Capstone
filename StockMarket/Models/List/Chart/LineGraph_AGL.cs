using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockMarket.Models.List.Chart
{
    // Label for the chart
    public class Col
    {
        public string id { get; set; }
        public string label { get; set; }
        public string pattern { get; set; }
        public string type { get; set; }
    }

    // Data objects
    public class C
    {
        public object v { get; set; }
    }

    // Row list data of C objects
    public class Row
    {
        public List<C> c { get; set; }
    }

    // Class that formats the chart data
    public class LineGraph_AGL
    {
        public List<Col> cols { get; set; }
        public List<Row> rows { get; set; }
    }
}