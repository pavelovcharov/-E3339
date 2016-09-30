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
using Microsoft.Practices.Prism.Regions;
using System.Windows.Controls;
using DevExpress.Xpf.Docking;
using System;

namespace PrismOnDXDocking.Infrastructure.Adapters {
	[Export(typeof(DockManagerAdapter)), PartCreationPolicy(CreationPolicy.NonShared)]
	public class DockManagerAdapter : RegionAdapterBase<DockLayoutManager> {
 [ImportingConstructor]
		public DockManagerAdapter(IRegionBehaviorFactory behaviorFactory) : 
            base(behaviorFactory) {
		}
		protected override IRegion CreateRegion() {
			return new SingleActiveRegion();
		}
		protected override void Adapt(IRegion region, DockLayoutManager regionTarget) {
			BaseLayoutItem[] items = regionTarget.GetItems();
            foreach(BaseLayoutItem item in items) {
                string regionName = RegionManager.GetRegionName(item);
                if(!String.IsNullOrEmpty(regionName)) {
                    LayoutPanel panel = item as LayoutPanel;
                    if(panel != null && panel.Content == null) {
                        ContentControl control = new ContentControl();
                        RegionManager.SetRegionName(control, regionName);
                        panel.Content = control;
                    }
                }
            }
		}
	}
}