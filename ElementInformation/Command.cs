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
                Reference pickedRef = null;     //Создаётся ссылка на элемент с пустым значением
                Selection select = uiapp.ActiveUIDocument.Selection;        //Выбранные ползователем элементы
                ElementPickFilter selectFilter = new ElementPickFilter();       //Создаётся фильтр выбора
                pickedRef = select.PickObject(ObjectType.Element, selectFilter, "Пожалуйста выберите элемент");     //выбор элемента пользователем + фильтр
                Element elem = doc.GetElement(pickedRef); //получение выбранного элемента из файла
                Category category = elem.Category;
                string categoryName = category.Name;        //Категория
                ElementId typeId = elem.GetTypeId();        //ID типа
                Element type = doc.GetElement(typeId);      //получение ID типа из файла
                ElementType elemType = (ElementType)type;
                string typeName = elemType.Name;        //Название типа
                string familyName = elemType.FamilyName.ToString();     //Название семейства
                string elemName = elem.Name;       //Название элемента
                ElementId elemId = elem.Id;     //ID

                //TaskDialog.Show("Выбранный элемент", categoryName + "\n" + familyName + "\n" + elemName + "\n" + elemId.ToString());

                ElementInformation ui = new ElementInformation();
                ui.TB_Family.Text = familyName;
                ui.TB_Category.Text = categoryName;
                ui.TB_Type.Text = typeName;
                ui.TB_Name.Text = elemName;
                ui.TB_Id.Text = elemId.ToString();
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
