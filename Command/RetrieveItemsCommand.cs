using Client.Facade;
using System.Windows.Controls;


namespace Client.Command
{
    public class RetrieveItemsCommand
    {
        private readonly StoreFacade _storeFacade = new StoreFacade();

        public void Execute(DataGrid dataGrid)
        {
            var dataTable = _storeFacade.GetAllItems();
            dataGrid.ItemsSource = dataTable.DefaultView;
        }
    }
}