using FamilyCreate.Controls.GraphArea;
using FamilyCreate.Models;
using FamilyCreate.Views;
using GraphX.Common.Enums;
using GraphX.Controls;
using GraphX.Controls.Models;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FamilyCreate.ViewModels
{
    public class TreeViewerViewModel : NotifyPropertyChangedBase
    {
        private readonly TreeViewerView parent;
        private readonly int treeId;

        public ICommand UpdateCommand => new RelayCommand(Update);
        public ICommand PrintCommand => new RelayCommand(Print);
        public ICommand ExportCommand => new RelayCommand(ExportToPNG);

        #region Constructors
        public TreeViewerViewModel() { }

        public TreeViewerViewModel(TreeViewerView parent, Tree currentTree)
        {
            parent.Area.VertexSelected += tg_Area_VertexSelected;
            this.parent = parent;
            treeId = currentTree.ID;
            ZoomControl.SetViewFinderVisibility(parent.zoomctrl, Visibility.Visible);

            GraphVisualizerSetup();
            parent.Loaded += OnWindowLoaded;
        }
        #endregion

        #region Events
        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            PrintGraph();
        }

        private void tg_Area_VertexSelected(object sender, VertexSelectedEventArgs args)
        {
            if (args.MouseArgs.RightButton != MouseButtonState.Pressed) return;
            args.VertexControl.ContextMenu = new ContextMenu();
            MenuItem menuitem = new MenuItem { Header = "Редактировать персону", Tag = args.VertexControl };
            menuitem.Click += tg_updateitem_click;
            args.VertexControl.ContextMenu.Items.Add(menuitem);
            menuitem = new MenuItem { Header = "Удалить персону", Tag = args.VertexControl };
            menuitem.Click += tg_deleteitem_Click;
            args.VertexControl.ContextMenu.Items.Add(menuitem);

            args.VertexControl.ContextMenu.IsOpen = true;
        }

        private void tg_deleteitem_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem == null) return;
            var vc = menuItem.Tag as VertexControl;
            if (vc == null) return;
            var datavertex = vc.Vertex as DataVertex;
            parent.Area.RemoveVertexAndEdges(datavertex);
            App.DatabaseContext.PersonsTable.RemoveAt(datavertex.PersonID);
        }

        private void tg_updateitem_click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem == null) return;
            var vc = menuItem.Tag as VertexControl;
            if (vc == null) return;
            var datavertex = vc.Vertex as DataVertex;
            EditPersonView editview = new EditPersonView(App.DatabaseContext.PersonsTable.ElementAt(datavertex.PersonID));
            if (editview.ShowDialog() == true)
            {
                GraphVisualizerSetup();
                PrintGraph();
            }
        }
        #endregion

        #region Graph Visualisation
        private void PrintGraph()
        {
            parent.Area.SetEdgesDashStyle(EdgeDashStyle.Solid);

            parent.Area.ShowAllEdgesArrows();

            parent.Area.GenerateGraph();

            parent.zoomctrl.ZoomToFill();
        }

        private void GraphVisualizerSetup()
        {
            parent.Area.SetVerticesDrag(true);
            var logicCore = new GXLogicCoreExample() { Graph = GraphSetup() };
            logicCore.DefaultLayoutAlgorithm = LayoutAlgorithmTypeEnum.Tree;
            logicCore.DefaultLayoutAlgorithmParams = logicCore.AlgorithmFactory.
                CreateLayoutParameters(LayoutAlgorithmTypeEnum.Tree);

            logicCore.DefaultEdgeRoutingAlgorithm = EdgeRoutingAlgorithmTypeEnum.None;
            logicCore.AsyncAlgorithmCompute = true;
            //Finally assign logic core to GraphArea object
            parent.Area.LogicCore = logicCore;
        }

        private void Update(object obj)
        {
            GraphVisualizerSetup();
            PrintGraph();
        }

        private Graph GraphSetup()
        {
            Graph dataGraph = new Graph();

            var personsList = App.DatabaseContext.PersonsTable.Select($"SELECT * FROM Persons WHERE Rodid in (SELECT ID FROM RODS WHERE TREEID = {treeId});");
            //adding persons

            foreach (var item in personsList)
            {
                var dataVertex = new DataVertex(item.ID, item.FIO!, item.BornDate, item.BornPlace, item.DeathDate, item.DeathPlace, item.IsMale);
                //Add vertex to data graph
                dataGraph.AddVertex(dataVertex);
            }

            //setup s

            var vlist = dataGraph.Vertices.ToList();

            foreach (Person item in personsList)
            {
                int source = vlist.FindIndex((x) => x.PersonID == item.ID);
                if (item.FatherID != 0)
                {
                    int targetone = vlist.FindIndex((x) => x.PersonID == item.FatherID);
                    DataEdge dataEdge = new DataEdge(vlist[targetone], vlist[source]);
                    dataGraph.AddEdge(dataEdge);
                }
                if (item.MotherID != 0)
                {
                    int targettwo = vlist.FindIndex((x) => x.PersonID == item.MotherID);
                    DataEdge dataEdge = new DataEdge(vlist[targettwo], vlist[source]);
                    dataGraph.AddEdge(dataEdge);
                }
            }

            return dataGraph;
        }
        #endregion

        private void ExportToPNG(object sender) => parent.Area.ExportAsImageDialog(ImageType.PNG, true);

        private void Print(object sender) => parent.Area.PrintDialog("Печать генеалогического дерева");
    }
}
