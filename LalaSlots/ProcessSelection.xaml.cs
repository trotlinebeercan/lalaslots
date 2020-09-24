namespace LalaSlots
{
    using System.Diagnostics;
    using System.Collections.Generic;
    using System.Windows;

    public partial class ProcessSelection : Window
    {
        List<ProcessModel> ProcessList { get; set; }
        public LalaSlot SelectedLalaSlot { get; set; }

        public ProcessSelection()
        {
            InitializeComponent();
            ProcessList = new List<ProcessModel>();
            foreach (var lala in FFXIVMemory.AllAvailableLalas)
            {
                if (lala.Hooked)
                    continue;

                ProcessList.Add(new ProcessModel
                {
                    Name = string.Format("{0} - {1}", lala.Name, lala.Id),
                    Process = lala.Process
                });
            }
            ListBox_Processes.DataContext = ProcessList;
        }

        private void Button_OpenThisProcess_Click(object sender, RoutedEventArgs e)
        {
            if (ListBox_Processes.SelectedItems.Count == 1)
            {
                var p = ListBox_Processes.SelectedItems[0] as ProcessModel;
                this.SelectedLalaSlot = FFXIVMemory.GetLalaSlotFromProcess(p.Process);
                this.SelectedLalaSlot.Hooked = true;
                DialogResult = true;
            }
        }
    }

    class ProcessModel
    {
        public string Name { get; set; }
        public Process Process { get; set; }
    }
}
