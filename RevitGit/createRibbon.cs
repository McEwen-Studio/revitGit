using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace RevitGit
{
    public class CreateRibbon : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            // Create a custom ribbon tab
            String tabName = "Git";
            application.CreateRibbonTab(tabName);

            // Create a ribbon panel
            RibbonPanel panel = application.CreateRibbonPanel(tabName, "Get Repository");
            populatePanel(panel);

            return Result.Succeeded;
        }
        // Both OnStartup and OnShutdown must be implemented as public method
        public Result populatePanel(RibbonPanel ribbonPanel)
        {
            addTextBox(ribbonPanel);
            addPushButton(ribbonPanel);
            return Result.Succeeded;
        }
        private void addPushButton(RibbonPanel ribbonPanel)
        {
            // Create a push button to trigger a command add it to the ribbon panel.
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
            PushButtonData buttonData = new PushButtonData("cmdHelloWorld", "Hello World", thisAssemblyPath, "RevitGit.HelloWorld");

            PushButton pushButton = ribbonPanel.AddItem(buttonData) as PushButton;

            // Optionally, other properties may be assigned to the button
            // a) tool-tip
            pushButton.ToolTip = "Say hello to the entire world.";

            // b) large bitmap
            Uri uriImage = new Uri(@"C:\Users\Andreas\Pictures\arrows2.png");
            BitmapImage largeImage = new BitmapImage(uriImage);
            pushButton.LargeImage = largeImage;
        }
        private void addTextBox(RibbonPanel ribbonPanel)
        {
            TextBoxData textData = new TextBoxData("Text Box");
            TextBox textBox = ribbonPanel.AddItem(textData) as TextBox;
            textBox.PromptText = "Enter the URL of a Git repository";
            textBox.ToolTip = "Enter some text";
            // Register event handler ProcessText
            textBox.EnterPressed += new EventHandler<TextBoxEnterPressedEventArgs>(ProcessText);
        }
        void ProcessText(object sender, TextBoxEnterPressedEventArgs args)
        {
            // cast sender as TextBox to retrieve text value
            TextBox textBox = sender as TextBox;
            string strText = textBox.Value as string;
            TaskDialog.Show("Revit", strText);
        }
        public Result OnShutdown(UIControlledApplication application)
        {
            // nothing to clean up in this simple case
            return Result.Succeeded;
        }
    }
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    public class HelloWorld : IExternalCommand
    {
        // The main Execute method (inherited from IExternalCommand) must be public
        public Result Execute(ExternalCommandData revit, ref string message, ElementSet elements)
        {
            TaskDialog.Show("Revit", "Hello World");
            return Result.Succeeded;
        }
    }
    public class setupRepository : IExternalCommand
    {
        public Result Execute(ExternalCommandData revit, ref string message, ElementSet elements)
        {
            return Result.Succeeded;
        }
    }
}
