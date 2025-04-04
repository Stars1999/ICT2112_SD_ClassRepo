using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Utilities;
using WP = DocumentFormat.OpenXml.Drawing.Wordprocessing;

namespace Utilities
{
	public static partial class ExtractContent
	{
		// Call TableStructureManager and pass table to extractTableStructure, then collect table detail dictionary
		// (Jonathan - COMPLETED)
		public static Dictionary<string, object> ExtractTable(Table table)
		{
			return null;
			// return new TableStructureManager().extractTableStructure(table);
		}
	}
}
