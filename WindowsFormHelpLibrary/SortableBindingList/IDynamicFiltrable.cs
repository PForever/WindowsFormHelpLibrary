using WindowsFormHelpLibrary.FilterHelp;

namespace WindowsFormHelpLibrary.SortableBindingList
{
    public interface IDynamicFiltrable
    {
        void ApplyFilter(PropertiesFilter filter);
        void RemoveFilter();
    }
}