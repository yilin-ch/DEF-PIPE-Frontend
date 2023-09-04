using DataCloud.PipelineDesigner.CanvasModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataCloud.PipelineDesigner.Services
{
    public static class Constants
    {
        public static class BuiltInTemplateIDs
        {
            public static String Start => "WORK_START";

            public static String Start_Name => "START";

            public static String End => "WORK_END";

            public static String End_Name => "END";

            public static Guid If => new Guid("e8a39e61-ad91-47b5-8a59-2464ce80f1d7");

            public static String Type_Name => "CONDITIONAL";

            public static Guid Loop => new Guid("3d422380-6adc-4cd8-8679-f5a6ac1cc9bb");
        }

        public static class SimpleDSLTemplateIDs
        {
            public static Guid Sort => new Guid("9fec6942-6411-418e-ac7a-b91286ec5f2c");
            public static Guid Filter => new Guid("383aea0d-0979-45fd-a432-87883431bf95");
            public static Guid CSV => new Guid("cb9f3e3e-d4e9-4b57-a57d-751868fd5f26");
        }

        public static List<CanvasShapeTemplate> BuiltInTemplates = new List<CanvasShapeTemplate>()
        {
            new CanvasShapeTemplate { 
                Id = BuiltInTemplateIDs.Start.ToString(),
                Name = "Start",
                Category = "Workflow",
                Shape = BuiltyInCanvasShape.Rectangle,
                Width = 50,
                Height = 50,
                Properties = new List<CanvasElementProperty>() {
                    new CanvasElementProperty{ Name = "Input path",  Type = CanvasElementPropertyType.SingleLineText, AllowEditing = true },
                    new CanvasElementProperty{ Name = "Communication Medium",  Type = CanvasElementPropertyType.Select, Options = new List<string>{ "Message Queue", "REST API" }, AllowEditing = true },
                },
                ConnectionPoints = new List<CanvasShapeConnectionPoint>() {                    
                    new CanvasShapeConnectionPoint { Id = "1", Position = new CanvasPosition { X = 25, Y = 50 }, Type= CanvasConnectionPointType.Output } 
                },
            },
             new CanvasShapeTemplate {
                Id = BuiltInTemplateIDs.End.ToString(),
                Name = "End",
                Category = "Workflow",
                Shape = BuiltyInCanvasShape.Rectangle,
                Width = 50,
                Height = 50,
                Properties = new List<CanvasElementProperty>(),
                ConnectionPoints = new List<CanvasShapeConnectionPoint>() {
                    new CanvasShapeConnectionPoint { Id = "1", Position = new CanvasPosition { X = 25, Y = 50 }, Type= CanvasConnectionPointType.Input }
                },
            }, 
            new CanvasShapeTemplate {
                Id = BuiltInTemplateIDs.If.ToString(),
                Name = "If",
                Category = "Workflow",
                Shape = BuiltyInCanvasShape.Diamond,
                Width = 100,
                Height = 100,
                Properties = new List<CanvasElementProperty>() {                  
                    new CanvasElementProperty{ Name = Constants.BuiltInCanvasShapeProperties.Condition,  Type = CanvasElementPropertyType.MultiLineText, AllowEditing = true }
                },
                ConnectionPoints = new List<CanvasShapeConnectionPoint>() {
                    new CanvasShapeConnectionPoint { Id = "1", Position = new CanvasPosition { X = 0, Y = 50 }, Type= CanvasConnectionPointType.Input },
                    new CanvasShapeConnectionPoint { Id = "2", Position = new CanvasPosition { X = 100, Y = 50 }, Type = CanvasConnectionPointType.Output },
                    new CanvasShapeConnectionPoint { Id = "3", Position = new CanvasPosition { X = 50, Y = 100 }, Type = CanvasConnectionPointType.Output },
                },
            },
            new CanvasShapeTemplate {
                Id = BuiltInTemplateIDs.Loop.ToString(),
                Name = "Loop",
                Category = "Workflow",
                Shape = BuiltyInCanvasShape.Container,
                Width = 100,
                Height = 100,
                Properties = new List<CanvasElementProperty>() {
                    new CanvasElementProperty{ Name = Constants.BuiltInCanvasShapeProperties.Iteration,  Type = CanvasElementPropertyType.SingleLineText, AllowEditing = true }
                },
                ConnectionPoints = new List<CanvasShapeConnectionPoint>() {
                    new CanvasShapeConnectionPoint { Id = "1", Position = new CanvasPosition { X = 0, Y = 50 }, Type= CanvasConnectionPointType.Input },
                    new CanvasShapeConnectionPoint { Id = "2", Position = new CanvasPosition { X = 100, Y = 50 }, Type = CanvasConnectionPointType.Output }
                },
                IsContainer = true
            }
        };

        public static List<CanvasShapeTemplate> SimpleDSLTemlates = new List<CanvasShapeTemplate>()
        {
           new CanvasShapeTemplate() { 
               Id = SimpleDSLTemplateIDs.Sort.ToString(),
               Name = "Sort",
               Category = "Data Transform",
               Shape = BuiltyInCanvasShape.Rectangle,
               Width = 300,
               Height = 100,
               Properties = new List<CanvasElementProperty> {
                   new CanvasElementProperty{ Name = "ActionMethod", Type = CanvasElementPropertyType.SingleLineText, Value = "Sort", AllowEditing = false },
                   new CanvasElementProperty{ Name = "Sort Property", Type = CanvasElementPropertyType.SingleLineText, AllowEditing = true },
                   new CanvasElementProperty{ Name = "Sort Order", Type = CanvasElementPropertyType.Select, Options = new List<string> { "Asceding", "Descending" }, AllowEditing = true },
                 
               },
               ConnectionPoints = new List<CanvasShapeConnectionPoint> {
                   new CanvasShapeConnectionPoint { Id = "1", Position = new CanvasPosition { X = 100, Y = 100 }, Type = CanvasConnectionPointType.Input },
                   new CanvasShapeConnectionPoint { Id = "2", Position = new CanvasPosition { X = 200, Y = 100 }, Type = CanvasConnectionPointType.Output },               
               }
           },
           new CanvasShapeTemplate() {
               Id = SimpleDSLTemplateIDs.Filter.ToString(),
               Name = "Filter",
               Category = "Data Transform",
               Shape = BuiltyInCanvasShape.Rectangle,
               Width = 300,
               Height = 100,
               Properties = new List<CanvasElementProperty> {
                   new CanvasElementProperty{ Name = "ActionMethod", Type = CanvasElementPropertyType.SingleLineText, Value = "Filter", AllowEditing = false },
                   new CanvasElementProperty{ Name = "Filter Expression", Type = CanvasElementPropertyType.SingleLineText, AllowEditing = true }

               },
               ConnectionPoints = new List<CanvasShapeConnectionPoint> {
                   new CanvasShapeConnectionPoint { Id = "1", Position = new CanvasPosition { X = 100, Y = 100 }, Type = CanvasConnectionPointType.Input },
                   new CanvasShapeConnectionPoint { Id = "2", Position = new CanvasPosition { X = 200, Y = 100 }, Type = CanvasConnectionPointType.Output },
               }
           },
           new CanvasShapeTemplate() {
               Id = SimpleDSLTemplateIDs.CSV.ToString(),
               Name = "CSV",
               Category = "Data",
               Shape = BuiltyInCanvasShape.Database,
               Width = 150,
               Height = 100,
               Properties = new List<CanvasElementProperty> {
                   new CanvasElementProperty{ Name = "File Path", Type = CanvasElementPropertyType.SingleLineText, AllowEditing = true }

               },
                 ConnectionPoints = new List<CanvasShapeConnectionPoint> {
                   new CanvasShapeConnectionPoint { Id = "1", Position = new CanvasPosition { X = 75, Y = 0 }, Type = CanvasConnectionPointType.Input },
                   new CanvasShapeConnectionPoint { Id = "2", Position = new CanvasPosition { X = 150, Y = 50 }, Type = CanvasConnectionPointType.Output },
               }
           },
        };

        public class BuiltInCanvasShapeProperties
        {
            public const string Condition = "Condition";
            public const string Iteration = "Iteration";
        }

        public class BuiltyInCanvasShape
        {
            public const string Rectangle = "Rectangle";
            public const string Diamond = "Diamond";
            public const string Container = "Container";
            public const string Database = "Database";
        }
    }
}
