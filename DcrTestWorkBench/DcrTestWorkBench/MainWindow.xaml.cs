/*
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

using DcrTestWorkBench.Windows;
using System.Windows;
using System.Collections.ObjectModel;
using DcrTestWorkBench.Models;
using System.Threading;
using System.Threading.Tasks;
using DcrTestWorkBench.ModelViews;
using Activant.P21.Extensions.BusinessRule;
using System.Collections.Generic;
using Microsoft.Win32;
using System.IO;

namespace DcrTestWorkBench
{
    public partial class MainWindow : Window
    {
        ObservableCollection<DataFieldView> _fields;
        CancellationTokenSource _source;
        
        public MainWindow()
        {
            InitializeComponent();
            btnStop.IsEnabled = false;
            _fields = new ObservableCollection<DataFieldView>();
            _source = new CancellationTokenSource();
            gridDataFields.ItemsSource = _fields;
            IniData();
        }

        private void IniData()
        {
            _fields.Add(new DataFieldView
            {
                ClassName = "global",
                FieldTitle = "Logged in User",
                FieldName = "global_user_id",
                FieldAlias = "",
                FieldValue = "my_id",
                ModifiedFlag = "N"
            });

            _fields.Add(new DataFieldView
            {
                ClassName = "global",
                FieldTitle = "Logged in Version",
                FieldName = "global_version",
                FieldAlias = "",
                FieldValue = "12.11",
                ModifiedFlag = "N"
            });

            _fields.Add(new DataFieldView
            {
                ClassName = "global",
                FieldTitle = "Logged in Server",
                FieldName = "global_server",
                FieldAlias = "",
                FieldValue = "sql",
                ModifiedFlag = "N"
            });

            _fields.Add(new DataFieldView
            {
                ClassName = "global",
                FieldTitle = "Logged in Database",
                FieldName = "global_database",
                FieldAlias = "",
                FieldValue = "brenner_play",
                ModifiedFlag = "N"
            });

            gridDataFields.Columns[8].Visibility = System.Windows.Visibility.Hidden;
        }

        private void btnLoadXml_Click(object sender, RoutedEventArgs e)
        {
            LoadXmlWindow dialog = new LoadXmlWindow();
            dialog.Owner = this;
            dialog.ShowDialog();

            if (dialog.IsSubmit)
            {
                _fields.Clear();

                foreach (var field in dialog.DataFields)
                {
                    _fields.Add(new DataFieldView
                    {
                        ClassName = field.ClassName,
                        FieldTitle = field.FieldTitle,
                        FieldName = field.FieldName,
                        FieldAlias = field.FieldAlias,
                        ModifiedFlag = field.ModifiedFlag,
                        FieldValue = field.FieldValue
                    });
                }
            }
            dialog.Close();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            _fields.Clear();
            this.IniData();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            btnStop.IsEnabled = true;
            btnLoadXml.IsEnabled = false;
            btnStart.IsEnabled = false;
            btnClear.IsEnabled = false;

            string xml = Helper.ToXml(ref _fields); // Generate xml format

            if (!string.IsNullOrEmpty(xml))
            {
                // Clear Result Values
                foreach (var field in _fields)
                {
                    field.ResultValue = "";
                }

                bool success = false;   // For P21 Result.Success
                
                // Reset Cancellation Source and Token
                _source.Dispose();
                _source = new CancellationTokenSource();
                var token = _source.Token;

                Task<List<P21DataField>> task = Task<List<P21DataField>>.Run(() =>
                    {
                        List<P21DataField> results = new List<P21DataField>();
 
                        //Rule r = new PickTicketNoConverter();
                        //r.Initialize(xml);                      // Load xml
                        //RuleResult result = r.Execute();
                        //success = result.Success;

                        if (!token.IsCancellationRequested)
                        {
                            //foreach (DataField data in r.Data.Fields)
                            //{
                            //    P21DataField field = new P21DataField
                            //    {
                            //        ClassName = data.ClassName,
                            //        FieldTitle = data.FieldTitle,
                            //        FieldName = data.FieldName,
                            //        FieldAlias = data.FieldAlias,
                            //        ModifiedFlag = data.Modified ? "Y" : "N",
                            //        FieldValue = data.FieldValue
                            //    };
                            //    results.Add(field);
                            //}
                        }

                        return results;
                    }, token);

                var last = task.ContinueWith((antecedent) =>
                {
                    if (!token.IsCancellationRequested)
                    {
                        foreach (var result in task.Result)
                        {
                            foreach (var field in _fields)
                            {
                                if (result.FieldName.Equals(field.FieldName))
                                {
                                    field.ResultValue = result.FieldValue;      // Update Result Value
                                    break;
                                }
                            }
                        }
                        lbResult.Content = success ? "SUCCESS" : "FAILED";
                        gridDataFields.Columns[8].Visibility = System.Windows.Visibility.Visible;
                    }

                    btnStart.IsEnabled = true;
                    btnLoadXml.IsEnabled = true;
                    btnClear.IsEnabled = true;
                    btnStop.IsEnabled = false;
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            _source.Cancel();

            btnStart.IsEnabled = true;
            btnLoadXml.IsEnabled = true;
            btnClear.IsEnabled = true;
            btnStop.IsEnabled = false;
            gridDataFields.Columns[8].Visibility = System.Windows.Visibility.Hidden;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (_fields.Count > 0)
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.FileName = "dcr_xml";
                dialog.DefaultExt = ".xml";
                dialog.Filter = "XML (.xml)|*.xml";

                if (dialog.ShowDialog() == true)
                {
                    string xml = Helper.ToXml(ref _fields);
                    File.WriteAllText(dialog.FileName, xml);
                }
                

            }
        }
    }
}
