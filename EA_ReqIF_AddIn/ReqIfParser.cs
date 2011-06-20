﻿using System;
using System.IO;
using System.Xml;

namespace EA_ReqIF_AddIn
{
	/// <summary>
	/// This class represents a validating Requirements Interchange Format (ReqIF) Parser.
	/// </summary>
	public sealed class ReqIfParser
	{
		private XmlReader xmlReader;
		
		/// <summary>
		/// An initialization constructor.
		/// </summary>
		/// <param name="filename">A string containing the path and the name of the ReqIF file.</param>
		/// <param name="doValidate">Set this to <c>true</c> to validate the ReqIF file.</param>
		public ReqIfParser(String filename, bool doValidate)
		{
			CheckFilenameArgument(filename);
			if (doValidate)
			{
				ValidateXmlDocument(filename);
			}
			xmlReader = XmlTextReader.Create(filename);
		}
		
		public void Parse(IReqIfParserCallbackReceiver callbackReceiver)
		{
			if (xmlReader != null)
			{
				while ( xmlReader.Read() )
				{
					if (xmlReader.NodeType == XmlNodeType.Element)
					{
						callbackReceiver.ProcessElementStartNode(xmlReader.Name);
					} else if (xmlReader.NodeType == XmlNodeType.Text)
					{
						callbackReceiver.ProcessTextNode(xmlReader.Name);
					} else if (xmlReader.NodeType == XmlNodeType.EndElement)
					{
						callbackReceiver.ProcessElementEndNode(xmlReader.Name);
					}
				}
			}
		}

		private void CheckFilenameArgument(String filename)
		{
			if (filename == null) {
				throw new System.ArgumentNullException("The filename must not be null!");
			}
			if (filename == String.Empty) {
				throw new System.ArgumentException("The filename must not be empty!");
			}
		}

		private void ValidateXmlDocument(String filename)
		{
			ReqIfFileValidator validator = new ReqIfFileValidator();
			if (! validator.Validate(filename, "reqif.xsd"))
			{
				// TODO: Show a dialog containing a list view with errors and warnings here!
			}
		}
	}
}
