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
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.Regions;
using DevExpress.Xpf.Docking;

namespace PrismOnDXDocking.Infrastructure.Adapters {
	[Export(typeof(LayoutGroupAdapter)), PartCreationPolicy(CreationPolicy.NonShared)]
	public class LayoutGroupAdapter : RegionAdapterBase<LayoutGroup> {
        [ImportingConstructor]
		public LayoutGroupAdapter(IRegionBehaviorFactory behaviorFactory) : 
            base(behaviorFactory) {
		}
		protected override IRegion CreateRegion() {
			 return new AllActiveRegion();
		}
       protected override void Adapt(IRegion region, LayoutGroup regionTarget) {
            region.Views.CollectionChanged += (s, e) => OnViewsCollectionChanged(region, regionTarget, s, e);
            regionTarget.Items.CollectionChanged += (s, e) => OnItemsCollectionChanged(region, regionTarget, s, e);
        }

        bool _lockItemsChanged;
        bool _lockViewsChanged;

        void OnItemsCollectionChanged(IRegion region, LayoutGroup regionTarget, object sender, NotifyCollectionChangedEventArgs e) {
            if (_lockItemsChanged)
                return;

            //if (e.Action == NotifyCollectionChangedAction.Remove)
            //{
            //    _lockViewsChanged = true;
            //    var lp = (LayoutPanel)e.OldItems[0];
            //    var view = lp.Content;
            //    lp.Content = null;
            //    region.Remove(view);
            //    _lockViewsChanged = false;
            //}
        }

        void OnViewsCollectionChanged(IRegion region, LayoutGroup regionTarget, object sender, NotifyCollectionChangedEventArgs e) {
            if (_lockViewsChanged)
                return;

            if (e.Action == NotifyCollectionChangedAction.Add) {
                foreach (var view in e.NewItems) {
                    var panel = new LayoutPanel { Content = view };
                    if (view is IPanelInfo)
                        panel.Caption = ((IPanelInfo)view).GetPanelCaption();
                    else
                        panel.Caption = "new Page";

                    _lockItemsChanged = true;
                    regionTarget.Items.Add(panel);
                    _lockItemsChanged = false;

                    regionTarget.SelectedTabIndex = regionTarget.Items.Count - 1;
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var view in e.OldItems)
                {
                    LayoutPanel viewPanel = null;
                    foreach (LayoutPanel panel in regionTarget.Items)
                    {
                        if (panel.Content == view)
                        {
                            viewPanel = panel;
                            break;
                        }
                    }
                    if (viewPanel == null) continue;
                    viewPanel.Content = null;
                    _lockItemsChanged = true;
                    regionTarget.Items.Remove(viewPanel);
                    _lockItemsChanged = false;
                    regionTarget.SelectedTabIndex = regionTarget.Items.Count - 1;
                }
            }
        }
    }
}
