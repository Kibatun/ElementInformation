using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Architecture;


namespace ElementInformation
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Получить объекты приложений и документов
            UIApplication uiapp = commandData.Application;
            Document doc = uiapp.ActiveUIDocument.Document;

            try
            {
                Reference pickedRef = null;     //Создаётся элемент с пустым значением
                Selection select = uiapp.ActiveUIDocument.Selection;        //Выбранные ползователем элементы
                ElementPickFilter selectFilter = new ElementPickFilter();       //Создаётся фильтр выбора
                pickedRef = select.PickObject(ObjectType.Element, selectFilter, "Пожалуйста выберите элемент");     //Созданный ранее элемент = выбранный элемент
                Element elem = doc.GetElement(pickedRef);
                ElementId elemId = elem.Id;
                TaskDialog.Show("Выбранный элемент", elemId.ToString());
               

                ///Должна браться инфопмация выбранного элемента


                ElementInformation ui = new ElementInformation();
                ui.ShowDialog();

            }
            catch(Exception ex)
            {
                message = ex.Message;
            }
            return Result.Succeeded;
        }
    }

    public class ElementPickFilter : ISelectionFilter       //Фильтр выбора элемента
    {
        public bool AllowElement(Element elem)
        {
           ///int a = elem.Category.Id.IntegerValue.Equals((int)BuiltInCategory.OST_Rooms);
            return true;        //Нет условия, т.к. нас устроит любой элемент
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return false;
        }
    }
}
