using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;


namespace BringMyFood
{
    public partial class Form1 : Form
    {
        private DataProcessor _dataProcessor;

        public Form1()
        {
            InitializeComponent();
            _dataProcessor = new DataProcessor();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML files (*.xml)|*.xml|Zip files (*.zip)|*.zip";
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                XmlFileReader xmlFileReader = new XmlFileReader();

                foreach (string fileName in openFileDialog.FileNames)
                {
                    string[] fileNamesToProcess;

                    if (Path.GetExtension(fileName).ToLower() == ".zip")
                    {
                        string tempFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                        Directory.CreateDirectory(tempFolder);

                        try
                        {
                            ZipFile.ExtractToDirectory(fileName, tempFolder);
                            fileNamesToProcess = Directory.GetFiles(tempFolder, "*.xml");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"An error occurred while extracting the zip file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        fileNamesToProcess = new string[] { fileName };
                    }

                    foreach (string filePath in fileNamesToProcess)
                    {
                        try
                        {
                            Driver driver = await Task.Run(() => xmlFileReader.ReadDriverFromFile(filePath));
                            _dataProcessor.EnqueueDriver(driver);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error reading file {Path.GetFileName(filePath)}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

                // Update the UI with the new data
                UpdateDriverJourneys();
                UpdateTotalDistancesByDriver();
                UpdateTotalDistancesByDate();
                UpdateLongestJourney();
            }
        }

        private void UpdateDriverJourneys()
        {
            listViewDriverJourneys.Items.Clear();
            foreach (Driver driver in _dataProcessor.GetDrivers())
            {
                foreach (Journey journey in driver.Journeys)
                {
                    ListViewItem item = new ListViewItem(new[] {
                        driver.Name,
                        journey.Date.ToString("dd/MM/yyyy"),
                        journey.Collection,
                        journey.Delivery,
                        journey.Distance.ToString()
                    });
                    listViewDriverJourneys.Items.Add(item);
                }
            }
        }

        private void UpdateTotalDistancesByDriver()
        {
            listViewTotalDistancesByDriver.Items.Clear();
            foreach (KeyValuePair<string, int> pair in _dataProcessor.GetTotalDistancesByDriver())
            {
                ListViewItem item = new ListViewItem(new[] {
                    pair.Key,
                    pair.Value.ToString()
                });
                listViewTotalDistancesByDriver.Items.Add(item);
            }
        }

        private void UpdateTotalDistancesByDate()
        {
            listViewTotalDistancesByDate.Items.Clear();
            foreach (KeyValuePair<DateTime, int> pair in _dataProcessor.GetTotalDistancesByDate())
            {
                ListViewItem item = new ListViewItem(new[] {
                    pair.Key.ToString("dd/MM/yyyy"),
                    pair.Value.ToString()
                });
                listViewTotalDistancesByDate.Items.Add(item);
            }
        }

        private void UpdateLongestJourney()
        {
            listViewLongestJourney.Items.Clear();
            KeyValuePair<Driver, Journey> longestJourneyPair = _dataProcessor.GetLongestJourney();
            if (longestJourneyPair.Value != null)
            {
                ListViewItem item = new ListViewItem(new[] {
            longestJourneyPair.Key.Name,
            longestJourneyPair.Value.Date.ToString("dd/MM/yyyy"),
            longestJourneyPair.Value.Collection,
            longestJourneyPair.Value.Delivery,
            longestJourneyPair.Value.Distance.ToString()
        });
                listViewLongestJourney.Items.Add(item);
            }
        }
    }
}
