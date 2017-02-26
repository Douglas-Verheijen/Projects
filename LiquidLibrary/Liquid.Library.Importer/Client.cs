using Liquid.Library.Importer.Events;
using Liquid.Library.Importer.Services;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Liquid.Library.Importer
{
    public partial class Client : Form
    {
        private readonly CSVReadService _readService;
        private readonly MovieBuilderService _builderService;
        private readonly MovieApiService _apiService;

        public Client()
        {
            InitializeComponent();

            _readService = new CSVReadService();
            _readService.DataRead += _readService_DataRead;
            _readService.ReadComplete += _readService_ReadComplete;

            _builderService = new MovieBuilderService();
            _builderService.EntityBuilt += _builderService_EntityBuilt;
            _builderService.BuildComplete += _builderService_BuildComplete;


            _apiService = new MovieApiService();
            _apiService.EntityCreated += _apiService_EntityCreated;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _readService.FilePath = openFileDialog.FileName;
                _readService.BeginRead();

                Text = "In Progress...";
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void _readService_DataRead(object sender, ReadEventArgs e)
        {
            var header = e.Data.FirstOrDefault();
            var sets = e.Data.Where(x => x != header).ToArray();

            _builderService.Data = sets;
            _builderService.BeginBuild();
        }

        private void _readService_ReadComplete(object sender, EventArgs e)
        {
            Text = "Read Complete.";
        }

        private void _builderService_EntityBuilt(object sender, BuilderEventArgs e)
        {

            _apiService.BeginSend(e.Movie);
        }

        private void _builderService_BuildComplete(object sender, EventArgs e)
        {
            Text = "Build Complete.";
        }

        private void _apiService_EntityCreated(object sender, EventArgs e)
        {
            results.Items.Add(new ListViewItem("Entity created."));
        }
    }
}
