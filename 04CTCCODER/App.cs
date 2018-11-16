#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Media.Imaging;
#endregion

namespace _04CTCCODER
{
    class App : IExternalApplication
    {
        //*****************************Startup()*****************************
        public Result OnStartup(UIControlledApplication a)
        {
            //create Tab
            string tab = "CTC_BIMTool"; // Tab name
            try
            {
                a.CreateRibbonTab(tab);
            }
            catch { }

            // Create Panel
            RibbonPanel panelCommon = NewribbonPanel("CTC_BIMTool", "Common", a);
            // Add button2 to panel Common
            RibbonButton buttonC1 = NewButton(panelCommon, "ButtonC1", "Company's Information", "_04CTCCODER.COMPANYINFO", "There are some CTC's informations", "a1.png", a);
            // Add button2 to panel Common
            RibbonButton buttonC2 = NewButton(panelCommon, "ButtonC2", "Coder's Information", "_04CTCCODER.CODERINFO", "There are some Coder's informations", "a2.png", a);
           
            // Create PanelModel
            RibbonPanel panelModel = NewribbonPanel("CTC_BIMTool", "Model", a);
            // Create buttonModel1
            RibbonButton buttonM1 = NewButton(panelModel, "ButtonM1", "Support For Modelling", "_04CTCCODER.Modelling", "This is tool for modeling", "aM1.png", a);
            // Create buttonDisallowjoinWall
            RibbonButton bt_DisallowJoinWall = NewButton(panelModel, "Bt_DisallowJoinWall", "Disallow For All Wall", "_04CTCCODER.cm_DisallowJoinWall", "Disallow For All Wall", "Img_DisallowJoinWall.png", a);
            // Create buttonRebarBeam
            RibbonButton bt_RebarBeam = NewButton(panelModel, "Bt_RebarBeam", "Model Rebar for Beam", "_04CTCCODER.Cm_RebarBeam", "Model Rebar for Beam", "img_RebarBeam.png", a);
            // Create Data
            RibbonPanel panelData = NewribbonPanel("CTC_BIMTool", "Data", a);
            // Create buttonSumFloorAreaVolume
            RibbonButton bt_SumFloorAreaVolume = NewButton(panelData, "Bt_SumFloorAreaVolume", "Sum Floors Area Volune", "_04CTCCODER.Cm_SumFloorAreaVolumne", "Calculate Sum Area and Volume of Selected Floor","img_SumFloorAreaVolume.png", a);

            // Create PanelDrawing
            RibbonPanel panelDrawing = NewribbonPanel("CTC_BIMTool", "Drawing", a);
            // Create ButtonDrawing1
            RibbonButton buttonD1 = NewButton(panelDrawing,"ButtonD1", "Support For Drawing", "_04CTCCODER.Drawing", "This is tool for drawing", "aD1.png", a);


            a.ApplicationClosing += a_ApplicationClosing;

            //Set Application to Idling
            a.Idling += a_Idling;

            return Result.Succeeded;
        }



        //*****************************a_Idling()*****************************
        void a_Idling(object sender, Autodesk.Revit.UI.Events.IdlingEventArgs e)
        {

        }

        //*****************************a_ApplicationClosing()*****************************
        void a_ApplicationClosing(object sender, Autodesk.Revit.UI.Events.ApplicationClosingEventArgs e)
        {
            throw new NotImplementedException();
        }

        //*****************************ribbonPanel()*****************************
   
        public RibbonPanel NewribbonPanel(string tab, string panelName, UIControlledApplication a)
        {

            RibbonPanel NewribbonPanel = null;
            // Try to create ribbon tab. 
            try
            {
                a.CreateRibbonTab(tab);
            }
            catch { }

            // Try to create ribbon panel Common.
            try
            {
                RibbonPanel panel = a.CreateRibbonPanel(tab, panelName);
            }
            catch { }
            // Search existing tab for your panel Common.
            List<RibbonPanel> panels = a.GetRibbonPanels(tab);
            foreach (RibbonPanel p in panels)
            {
                if (p.Name == panelName)
                {
                    NewribbonPanel = p;
                }
            }

            //return panel 
            return NewribbonPanel;
        }

        //*****************************ribbonButton()*****************************

        public RibbonButton NewButton(RibbonPanel panel, string Bt_Name, string bt_Title, string bt_CommandPath, string bt_ToolTip, string bt_icons, UIControlledApplication a)
        {


            // Reflection to look for this assembly path 
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            // Add button to panel
            PushButton NewButton = panel.AddItem(new PushButtonData(Bt_Name, bt_Title, thisAssemblyPath, bt_CommandPath)) as PushButton;
            // Add tool tip 
            NewButton.ToolTip = bt_ToolTip;
            // Reflection of path to image 
            var globePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), bt_icons);
            Uri uriImage = new Uri(globePath);
            // Apply image to bitmap
            BitmapImage largeImage = new BitmapImage(uriImage);
            // Apply image to button 
            NewButton.LargeImage = largeImage;

            return NewButton;
        }

        //*****************************ShutDown()*****************************
        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }

     }

    
}
