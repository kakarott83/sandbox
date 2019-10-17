using System.Windows;
using System.Windows.Input;
using XmlConfiguratorBase.DAO;
using XmlConfiguratorBase.DTO;

namespace XmlConfiguratorBase
{
    public interface ICoreControl
    {
        object SelectedObject { get; set; }
        IStatusBar Status { set; }
        IListControl ListBoxes { set; }
        ISettings Settings { set; }

        void Register(IDependentControl child);

        void SetDataManager(IDataManager access);
        void SetElementFromXml(string xml);
        void DoSave();
        void DoOpenDatabase();
        void DoOpenCustom();
        void DoSaveFile();
        void DoSaveDatabase();
        void DoCreateNewWfvEntry();
        void DoCreateNewElement();
        void DoDeleteElement();
        void DoOpenFile();
        void DoShowDependencies();
    }
}
