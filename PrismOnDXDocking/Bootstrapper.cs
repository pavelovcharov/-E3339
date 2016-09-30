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
using System.Windows;
using DevExpress.Xpf.Docking;
using Microsoft.Practices.Prism.MefExtensions;
using Microsoft.Practices.Prism.Regions;
using PrismOnDXDocking.Infrastructure;
using PrismOnDXDocking.Infrastructure.Adapters;
using PrismOnDXDocking.Infrastructure.Behaviors;

namespace PrismOnDXDocking {
	public class Bootstrapper : MefBootstrapper {
		protected override void ConfigureAggregateCatalog() {
			AggregateCatalog.Catalogs.Add(new System.ComponentModel.Composition.Hosting.AssemblyCatalog(typeof(Bootstrapper).Assembly));
			AggregateCatalog.Catalogs.Add(new System.ComponentModel.Composition.Hosting.AssemblyCatalog(typeof(RegionNames).Assembly));
			AggregateCatalog.Catalogs.Add(new System.ComponentModel.Composition.Hosting.AssemblyCatalog(typeof(ExampleModule.ExampleModule).Assembly));
		}
		protected override DependencyObject CreateShell() {
			return Container.GetExportedValue<Shell>();
		}
		protected override void InitializeShell() {
			base.InitializeShell();
			Application.Current.MainWindow = (Shell)Shell;
			Application.Current.MainWindow.Show();
		}
		protected override RegionAdapterMappings ConfigureRegionAdapterMappings() {
			RegionAdapterMappings mappings = base.ConfigureRegionAdapterMappings();
            mappings.RegisterMapping(typeof(LayoutPanel), Container.GetExportedValue<LayoutPanelAdapter>());
            mappings.RegisterMapping(typeof(LayoutGroup), Container.GetExportedValue<LayoutGroupAdapter>());
            mappings.RegisterMapping(typeof(DocumentGroup), Container.GetExportedValue<DocumentGroupAdapter>());
            //mappings.RegisterMapping(typeof(TabbedGroup), Container.GetExportedValue<TabbedGroupAdapter>());
            return mappings;
		}
	}
}