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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Regions;
using DevExpress.Xpf.Docking;
using System.Collections.Specialized;
using System.ComponentModel.Composition;

namespace PrismOnDXDocking.Infrastructure.Adapters {
    [Export(typeof(DocumentGroupAdapter)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class DocumentGroupAdapter : RegionAdapterBase<DocumentGroup> {
        [ImportingConstructor]
        public DocumentGroupAdapter(IRegionBehaviorFactory behaviorFactory) :
            base(behaviorFactory) {
        }
        protected override IRegion CreateRegion() {
            return new AllActiveRegion();
        }
        protected override void Adapt(IRegion region, DocumentGroup regionTarget) {
            region.Views.CollectionChanged += (s, e) => {
                OnViewsCollectionChanged(region, regionTarget, s, e);
            };
        }
        void OnViewsCollectionChanged(IRegion region, DocumentGroup regionTarget, object sender, NotifyCollectionChangedEventArgs e) {
            if(e.Action == NotifyCollectionChangedAction.Add) {
                foreach(object view in e.NewItems) {
                    DockLayoutManager manager = regionTarget.GetDockLayoutManager();
                    DocumentPanel panel = manager.DockController.AddDocumentPanel(regionTarget);
                    panel.Content = view;
                    if(view is IPanelInfo)
                        panel.Caption = ((IPanelInfo)view).GetPanelCaption();
                    else panel.Caption = "new Page";
                    manager.DockController.Activate(panel);
                }
            }
        }
    }
}
