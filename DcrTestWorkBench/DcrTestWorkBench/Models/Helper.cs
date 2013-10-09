/*
    Helper Class
    Copyright (C) 2013 Dai Nguyen

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using DcrTestWorkBench.ModelViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace DcrTestWorkBench.Models
{
    public static class Helper
    {
        public static string ToXml(ref ObservableCollection<DataFieldView> list)
        {
            try
            {
                XDocument doc = new XDocument(
                    new XDeclaration("1.0", "utf-16le", "no")
                );

                XElement elements = new XElement("business_rule_extensions_xml");
                foreach (var field in list)
                {
                    elements.Add(new XElement("fieldList",
                        new XElement("className", field.ClassName),
                        new XElement("fieldTitle", field.FieldTitle),
                        new XElement("fieldName", field.FieldName),
                        new XElement("fieldAlias", field.FieldAlias),
                        new XElement("fieldValue", field.FieldValue),
                        new XElement("modifiedFlag", field.ModifiedFlag)
                        ));
                }
                doc.Add(elements);
                return doc.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<P21DataField> ParseXML(string xml)
        {
            try
            {
                List<P21DataField> results = new List<P21DataField>();
                XDocument doc = XDocument.Parse(xml);

                foreach (var fieldList in doc.Descendants("business_rule_extensions_xml").Elements("fieldList"))
                {
                    P21DataField dataField = new P21DataField
                    {
                        ClassName = fieldList.Element("className").Value,
                        FieldTitle = fieldList.Element("fieldTitle").Value,
                        FieldName = fieldList.Element("fieldName").Value,
                        FieldAlias = fieldList.Element("fieldAlias").Value,
                        FieldValue = fieldList.Element("fieldValue").Value,
                        ModifiedFlag = fieldList.Element("modifiedFlag").Value
                    };
                    results.Add(dataField);
                }
                return results;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
