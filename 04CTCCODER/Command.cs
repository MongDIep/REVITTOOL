#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace _04CTCCODER
{
    [Transaction(TransactionMode.Manual)]
    public class CODERINFO : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            TaskDialog.Show("Revit", "She is a BIM coordinator");

            return Result.Succeeded;
        }
    }
    [Transaction(TransactionMode.Manual)]
    public class COMPANYINFO : IExternalCommand
    {
        // The main Execute method (inherited from IExternalCommand) must be public
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData revit,
            ref string message, ElementSet elements)
        {
            UIApplication uiapp = revit.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;
            TaskDialog.Show("Revit", "The Biggest Construction Company in Viet Nam");
            return Autodesk.Revit.UI.Result.Succeeded;
        }
    }
    [Transaction(TransactionMode.Manual)]
    public class Modelling : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            TaskDialog.Show("Revit", "This is a tool for modeling");

            return Result.Succeeded;
        }
    }
    [Transaction(TransactionMode.Manual)]
    public class Drawing : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            TaskDialog.Show("Revit", "This is a tool for drawing");

            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class cm_DisallowJoinWall : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            Application app = uiapp.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            // Use the rectangle picking tool to identify model elements to select.
            IList<Element> pickedWalls = uidoc.Selection.PickElementsByRectangle(new WallSelectionFilter());

            if (pickedWalls.Count == 0)
            {

                TaskDialog.Show("Revit", "Please Select by Rectangle, included Wall");
            }
            if (pickedWalls.Count > 0)
            {

                TaskDialog.Show("Revit", string.Format("{0} Walls added to Selection.", pickedWalls.Count));
            }



            return Result.Succeeded;
        }


       
    }

    [Transaction(TransactionMode.Manual)]
    public class Cm_RebarBeam : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            Application app = uiapp.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            Selection sel = uidoc.Selection;
            Reference rf = sel.PickObject(ObjectType.Element, new BeamSelectionFilter());
            Element elem = doc.GetElement(rf);
            ElementId elemTypeId = elem.GetTypeId();
            Element elemType = doc.GetElement(elemTypeId);
            Parameter bParam = elemType.LookupParameter("b");
            double width = bParam.AsDouble();
            Parameter hParam = elemType.LookupParameter("h");
            double height = hParam.AsDouble();
            Parameter lParam = elem.LookupParameter("Length");
            double length = lParam.AsDouble();
            Location loc = elem.Location;
            LocationCurve locCur = loc as LocationCurve;
            Curve curve = locCur.Curve;
            Line line = curve as Line;
            XYZ vectorX = line.Direction;
            XYZ vectorZ = XYZ.BasisZ;
            XYZ vectorY = vectorZ.CrossProduct(vectorX);
            XYZ pnt = line.GetEndPoint(0);
            Parameter zJus = elem.LookupParameter("z Justification");
            int zPos = zJus.AsInteger();
            double x = 0;
            switch (zPos)
            {
                case 0: // Top   
                    x = height;
                    break;
                case 1: // Center  
                case 2: // Origin  
                    x = 0.5;
                    break;
                case 3: // Bottom  
                    x = 0;
                    break;
            }
            XYZ origin = pnt - vectorY * width / 2 - vectorZ * height * x;
            List<XYZ> sectionPnts = new List<XYZ>{

                origin,
                origin + vectorY * width,
                origin + vectorY * width + vectorZ * height,
                origin + vectorZ * height
            };
            TaskDialog.Show("Revit", $"Tọa độ cấu kiện:\nP1: { sectionPnts[0]},\nP2: { sectionPnts[1]},\nP3: { sectionPnts[2]}\nP4: { sectionPnts[3]}");

            return Result.Succeeded;
        }  
    }

    [Transaction(TransactionMode.Manual)]
    public class Cm_SumFloorAreaVolumne : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            Application app = uiapp.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            // Use the rectangle picking tool to identify model elements to select.
            IList<Element> pickedFloors = uidoc.Selection.PickElementsByRectangle(new FloorSelectionFilter());
           


            if (pickedFloors.Count == 0)
            {

                TaskDialog.Show("Revit", "Please Select by Rectangle, included Floors");
            }

            if (pickedFloors.Count > 0)
            {
                Double SumAreaFloors = 0;
                float sumVolumeFloors = 0;

                foreach (Element item in pickedFloors)
                {

                    Double aREA = SumAreaFloors + (100 / 1076.39104167097) * (item.LookupParameter("Area").AsDouble());

                    Double vOLUME = sumVolumeFloors + (100 / 3531.46667214889) * (item.LookupParameter("Volume").AsDouble());
                    SumAreaFloors = (float)aREA;
                    sumVolumeFloors = (float)vOLUME;


                    //foreach (Parameter PRA in item.Parameters)
                    //{
                    //    Double PA =  PRA.AsDouble();

                    //    Double COB = UnitUtils.Convert(PA, Autodesk.Revit.DB.DisplayUnitType.DUT_SQUARE_METERS,Autodesk.Revit.DB.DisplayUnitType.DUT_SQUARE_INCHES);
                    //    SumAreaFloors = SumAreaFloors + COB;
                    //    IList<DisplayUnitType> duts;
                    //    Array a = Enum.GetValues(typeof(UnitType));
                    //    foreach (var item in a)
                    //    {
                    //        duts = UnitUtils.GetValidDisplayUnits(item);
                    //    }

                    //if (PRA.Definition.Name.Equals("Area"))
                    //
                    //    string aREA = (GetParameterValueTH(PRA));
                    //    float a = Convert.ToInt16(value: aREA);
                    //    SumAreaFloors = SumAreaFloors + a;

                    //}
                    //if (PRA.Definition.Name == "Volume")
                    //{

                    //    string Volu = (GetParameterValueTH(PRA));
                    //    sumVolumeFloors = sumVolumeFloors + Convert.ToUInt16(20.7);
                    //}
                }

            
                TaskDialog.Show("Revit", string.Format("{0} Floors selected.\n Total Area of Selected Floors is: {1} m2."+
                    "\n Total Concrete of Selected Floors is: {2} m3", pickedFloors.Count, SumAreaFloors, sumVolumeFloors));

            }


            return Result.Succeeded;
        }
        //public String GetParameterValueTH(Parameter parameter)
        //{
        //    switch (parameter.StorageType)
        //    {
        //        case StorageType.Double:
        //            return parameter.AsValueString();
        //        case StorageType.ElementId:
        //            return parameter.AsElementId().IntegerValue.ToString();
        //        case StorageType.Integer:
        //            return parameter.AsValueString();
        //        case StorageType.None:
        //            return parameter.AsValueString();
        //        case StorageType.String:
        //            return parameter.AsValueString();
        //        default:
        //            return "";
        //    }
        //}
    }


    //*************************Các hàm filter theo category*****************
    public class FloorSelectionFilter : ISelectionFilter
        {
            public bool AllowElement(Element elem)
            {
                if (elem.Category.Id.IntegerValue == (int)BuiltInCategory.OST_Floors)
                    return true;
                return false;
            }
            public bool AllowReference(Reference reference, XYZ position)
            {
                return false;
            }
        }

    public class BeamSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            if (elem.Category.Id.IntegerValue == (int)BuiltInCategory.OST_StructuralFraming)
                return true;
            return false;
        }
        public bool AllowReference(Reference reference, XYZ position)
        {
            return false;
        }
    }

    public class WallSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            if (elem.Category.Id.IntegerValue == (int)BuiltInCategory.OST_Walls)
                return true;
            return false;
        }
        public bool AllowReference(Reference reference, XYZ position)
        {
            return false;
        }
    }
    
}





