/**
 * @file    DataTemplateManager.cs
 * @author  Drew Hoffer
 */

using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;

namespace FAST_Converter.Navigation.Startup
{
    /**
     * @brief DataTemplateManager class links the views to the viewmodels on a name by name basis
     */
    public class DataTemplateManager
    {


        /**
         *  Takes all data types associated with a window and cuts viewmodel out of the data type, if there is a corresponding
         *  datatype with the same base name it joins the two together
         *  
         *  @param  void
         *  @return void
         */
        public void LoadDataTemplateByConvention() 
        {
            var assembly = Assembly.GetCallingAssembly();
            var assemblyTypes = assembly.GetTypes();

            var viewModels = assemblyTypes.Where(x => x.Name.Contains("ViewModel"));


           foreach (var vm in viewModels) 
            {
               var baseName = vm.Name.Replace("ViewModel", string.Empty);

               var viewType = assemblyTypes.FirstOrDefault(x => x.Name == baseName + "View");

               if (viewType != null) 
                {
                   RegisterDataTemplate(vm, viewType);
               }
           }
        }



        /**
         *  Displays window that current viewmodel is associated with
         *  
         *  @param  void
         *  @return void
         */
        public void RegisterDataTemplate<VM, V>() 
        {
            var template = CreateTemplate(typeof(VM), typeof(V));
            Application.Current.Resources.Add(template.DataTemplateKey, template);
        }





        /**
         *  Takes a view and a viewmodel that will be linked
         *  
         *  @param  type viewModel
         *  @param  type view
         *  @return void
         */
        public void RegisterDataTemplate(Type viewModel, Type view) 
        {
            var template = CreateTemplate(viewModel, view);
            Application.Current.Resources.Add(template.DataTemplateKey, template);
        }


        //Maps data templates (http://www.ikriv.com/dev/wpf/DataTemplateCreation/)
        /**
         *  Generates XAML code for the window that takes both the name of the view and viewmodel and links them 
         *  
         *  @param  Type - viewModelType
         *  @param  Type - viewType
         *  @return A new datatemplate
         */
        private DataTemplate CreateTemplate(Type viewModelType, Type viewType)
        {
            const string xamlTemplate = "<DataTemplate DataType=\"{{x:Type vm:{0}}}\"><v:{1} /></DataTemplate>";
            var xaml = String.Format(xamlTemplate, viewModelType.Name, viewType.Name, viewModelType.Namespace, viewType.Namespace);
            var context = new ParserContext
            {
                XamlTypeMapper = new XamlTypeMapper(new string[0])
            };
            context.XamlTypeMapper.AddMappingProcessingInstruction("vm", viewModelType.Namespace, viewModelType.Assembly.FullName);
            context.XamlTypeMapper.AddMappingProcessingInstruction("v", viewType.Namespace, viewType.Assembly.FullName);

            context.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            context.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");
            context.XmlnsDictionary.Add("vm", "vm");
            context.XmlnsDictionary.Add("v", "v");

            var template = (DataTemplate)XamlReader.Parse(xaml, context);
            return template;
        }

    }
}
