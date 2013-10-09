/*
    Model View for UI
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
using System.ComponentModel;

namespace DcrTestWorkBench.ModelViews
{
    public class DataFieldView : INotifyPropertyChanged
    {
        public string ClassName { get; set; }
        public string FieldTitle { get; set; }
        public string FieldName { get; set; }
        public string FieldAlias { get; set; }
        public string ModifiedFlag { get; set; }
        
        private string fieldValue { get; set; }

        public string FieldValue
        {
            get { return this.fieldValue; }
            set
            {
                if (this.fieldValue != value)
                {
                    this.fieldValue = value;
                    OnPropertyChanged("FieldValue");
                }
            }
        }

        private string expectValue { get; set; }

        public string ExpectValue
        {
            get { return this.expectValue; }
            set
            {
                if (this.expectValue != value)
                {
                    this.expectValue = value;
                    OnPropertyChanged("ExpectedValue");
                }
            }
        }

        private string resultValue { get; set; }

        public string ResultValue
        {
            get { return this.resultValue; }
            set
            {
                if (this.resultValue != value)
                {
                    this.resultValue = value;
                    OnPropertyChanged("ResultValue");

                    if (!string.IsNullOrEmpty(this.resultValue))
                        this.Status = this.resultValue.Equals(this.expectValue) ? "PASSED" : "FAILED";
                }
            }
        }

        private string status { get; set; }

        public string Status
        {
            get { return this.status; }
            private set
            {
                if (this.status != value)
                {
                    this.status = value;
                    OnPropertyChanged("Status");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
