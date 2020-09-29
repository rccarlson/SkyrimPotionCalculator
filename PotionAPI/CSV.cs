using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using System.IO;

namespace PotionAPI
{
	internal class CSV
	{
		private readonly string _filepath;
		private readonly string[] _headers;
		private readonly List<string[]> _content;

		public int Rows => _content.Count;
		public int Columns => _headers.Length;

		public string[] Headers => _headers;

		protected CSV(string filepath, string[] headers, List<string[]> content)
		{
			_filepath = filepath;
			_headers = headers;
			_content = content;
		}

		/// <summary>
		/// Attempt to read from a CSV file. If successful, the function will return <see cref="true"/> and the <see cref="CSV"/> object will be output in <paramref name="csv"/>
		/// </summary>
		/// <param name="filepath">Directory of the CSV file</param>
		/// <param name="hasHeaders">True if the CSV file has headers that need to be read</param>
		/// <param name="csv">CSV parsed from file</param>
		/// <returns>True if read was successful</returns>
		public static bool TryRead(string filepath, bool hasHeaders, out CSV csv)
		{
			//check that the file exists
			if (!File.Exists(filepath))
			{
				csv = null;
				return false;
			}

			string[] headers = null;
			List<string[]> content = new List<string[]>();

			try
			{
				//Parse the CSV file
				using (TextFieldParser parser = new TextFieldParser(filepath))
				{
					//Set parser params
					parser.TextFieldType = FieldType.Delimited;
					parser.SetDelimiters(",");

					//Read headers if necessary
					if (hasHeaders)
						headers = parser.ReadFields();

					//Read all content
					while (!parser.EndOfData)
						content.Add(parser.ReadFields());

					csv = new CSV(filepath, headers, content);
					return true;
				}
			}
			catch
			{
				csv = null;
				return false;
			}
		}
		/// <summary>
		/// Performs an unsafe read of the document at <paramref name="filepath"/>
		/// </summary>
		/// <param name="filepath">Directory of CSV file</param>
		/// <param name="hasHeaders">True if the CSV file has headers that need to be read</param>
		/// <returns>CSV parsed from file</returns>
		public static CSV Read(string filepath, bool hasHeaders)
		{
			TryRead(filepath: filepath,
				hasHeaders: hasHeaders,
				out CSV csv);
			return csv;
		}

		/// <summary>
		/// Gets the index of the matching header column
		/// </summary>
		/// <param name="header">Header to locate</param>
		/// <returns>Index of column</returns>
		protected int GetHeaderIndex(string header)
		{
			for(int i = 0; i < _headers.Length; i++)
			{
				if (_headers[i].CompareTo(header) == 0)
					return i;
			}
			return -1;
		}

		/// <summary>
		/// Gets the entry located on the specified <paramref name="row"/> under the provided <paramref name="header"/>. Returns null if invalid
		/// </summary>
		/// <param name="header">Desired header</param>
		/// <param name="row">0 indexed row number, not including header</param>
		/// <returns>Entry if found. Null otherwise</returns>
		public string GetEntry(string header, int row)
		{
			int col = GetHeaderIndex(header);
			return GetEntry(column: col,
					row: row);
		}
		/// <summary>
		/// Gets the entry located at the specified <paramref name="row"/> and <paramref name="column"/>
		/// </summary>
		/// <param name="column">0 indexed column number</param>
		/// <param name="row">0 indexed row number, not including header</param>
		/// <returns>String entry</returns>
		public string GetEntry(int column, int row)
		{
			if (column >= 0 && column < Columns && row >= 0 && row < Rows)
				return _content[row][column];
			else
				return null;
		}
	}
}
