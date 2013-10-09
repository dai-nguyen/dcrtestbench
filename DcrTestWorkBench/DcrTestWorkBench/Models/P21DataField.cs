/*
    Model Class for P21 Rule
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

namespace DcrTestWorkBench.Models
{
    public class P21DataField
    {
        public string ClassName     { get; set; }
        public string FieldTitle    { get; set; }
        public string FieldName     { get; set; }
        public string FieldAlias    { get; set; }
        public string ModifiedFlag  { get; set; }
        public string FieldValue    { get; set; }
    }
}
