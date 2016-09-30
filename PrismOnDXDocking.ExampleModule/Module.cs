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
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using PrismOnDXDocking.ExampleModule.Views;
using PrismOnDXDocking.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace PrismOnDXDocking.ExampleModule {
	[ModuleExport(typeof(ExampleModule))]
	public class ExampleModule : IModule {
		private readonly IRegionManager regionManager;
		private readonly IMenuService menuService;
		private readonly DelegateCommand showOutput;
		private readonly DelegateCommand showProperties;
		private readonly DelegateCommand showToolbox;
		private readonly DelegateCommand newDocument;
		[ImportingConstructor]
        public ExampleModule(IRegionManager regionManager, IMenuService menuService) {
			this.regionManager = regionManager;
            this.menuService = menuService;
            this.showOutput = new DelegateCommand(ShowOutput);
            this.showProperties = new DelegateCommand(ShowProperties);
            this.showToolbox = new DelegateCommand(ShowToolbox);
            this.newDocument = new DelegateCommand(AddNewDocument);
		}
        public void Initialize() {
            regionManager.RegisterViewWithRegion(RegionNames.DefaultViewRegion, typeof(DefaultView));

            regionManager.AddToRegion(RegionNames.LeftRegion, ServiceLocator.Current.GetInstance<ToolBoxView>());
            regionManager.AddToRegion(RegionNames.RightRegion, ServiceLocator.Current.GetInstance<PropertiesView>());
            regionManager.AddToRegion(RegionNames.MainRegion, ServiceLocator.Current.GetInstance<DocumentView>());

            menuService.Add(new MenuItem() { Command = showOutput, Parent = "View", Title = "Output"});
            menuService.Add(new MenuItem() { Command = showProperties, Parent = "View", Title = "Properties Window"});
            menuService.Add(new MenuItem() { Command = showToolbox, Parent = "View", Title = "Toolbox"});
            menuService.Add(new MenuItem() { Command = newDocument, Parent = "File", Title = "New"});
        }

		void ShowOutput() {
			regionManager.AddToRegion(RegionNames.TabRegion, ServiceLocator.Current.GetInstance<OutputView>());
		}
		void ShowToolbox() {
			regionManager.AddToRegion(RegionNames.LeftRegion, ServiceLocator.Current.GetInstance<ToolBoxView>());
		}
		void ShowProperties() {
			regionManager.AddToRegion(RegionNames.RightRegion, ServiceLocator.Current.GetInstance<PropertiesView>());
		}
		void AddNewDocument() {
			regionManager.AddToRegion(RegionNames.MainRegion, ServiceLocator.Current.GetInstance<DocumentView>());
		}
	}
}