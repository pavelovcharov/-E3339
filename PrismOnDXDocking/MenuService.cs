// Developer Express Code Central Example:
// Prism - How to define Prism regions for various DXDocking elements
// 
// Since Prism RegionManager supports standard controls only, it is necessary to
// write custom RegionAdapters (a descendant of the
// Microsoft.Practices.Prism.Regions.RegionAdapterBase class) in order to instruct
// Prism RegionManager how to deal with DXDocking elements.
// 
// This example covers
// the following scenarios:
// 
// Using a LayoutPanel as a Prism region. The
// LayoutPanelAdapter class creates a new ContentControl containing a View and then
// places it into a target LayoutPanel.
// Using a LayoutGroup as a Prism region. The
// LayoutGroupAdapter creates a new LayoutPanel containing a View, and then adds it
// to a target LayoutGroup’s Items collection,
// Using a DocumentGroup as a Prism
// region. The DocumentGroupAdapter behaves similarly to the LayoutGroupAdapter.
// The only difference is that it manipulates DocumentPanels.
// 
// You can find sample updates and versions for different programming languages here:
// http://www.devexpress.com/example=E3339

using Microsoft.VisualBasic;
using System.ComponentModel.Composition;
using System.Text.RegularExpressions;
using DevExpress.Xpf.Bars;

namespace PrismOnDXDocking.Infrastructure {
    [Export(typeof(IMenuService))]
    public class MenuService : IMenuService {
        private readonly BarManager manager;
        private readonly Bar bar;
        [ImportingConstructor]
        public MenuService(Shell shell) {
            this.manager = shell.BarManager;
            this.bar = shell.MainMenu;
        }
        public void Add(MenuItem item) {
            BarSubItem parent = GetParent(item.Parent);
            BarButtonItem button = new BarButtonItem { Content = item.Title, Command = item.Command, Name = "bbi" + Regex.Replace(item.Title, "[^a-zA-Z0-9]", "") };
            manager.Items.Add(button);
            parent.ItemLinks.Add(new BarButtonItemLink { BarItemName = button.Name });
        }

        BarSubItem GetParent(string parentName) {
            foreach(BarItem item in manager.Items) {
                BarSubItem button = item as BarSubItem;
                if(button != null && button.Content.ToString() == parentName)
                    return button;
            }
            BarSubItem newParent = new BarSubItem { Content = parentName, Name = "bsi" + Regex.Replace(parentName, "[^a-zA-Z0-9]", "") };
            manager.Items.Add(newParent);
            bar.ItemLinks.Add(new BarSubItemLink { BarItemName = newParent.Name });
            return newParent;
        }
    }
}