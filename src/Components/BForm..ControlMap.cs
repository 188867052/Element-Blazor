﻿using Blazui.Component.ControlRender;
using Blazui.Component.ControlRenders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Blazui.Component
{
    public class FormFieldControlMap
    {
        private Dictionary<Func<PropertyInfo, bool>, Type> fieldsControlMap = new Dictionary<Func<PropertyInfo, bool>, Type>();
        private Dictionary<string, List<BFormItemObject>> formControls = new Dictionary<string, List<BFormItemObject>>();
        public FormFieldControlMap(IServiceProvider provider)
        {
            fieldsControlMap.Add(property => property.PropertyType == typeof(string), typeof(BInput<string>));
            fieldsControlMap.Add(property => property.PropertyType == typeof(int), typeof(BInput<int>));
            fieldsControlMap.Add(property => property.PropertyType == typeof(int?), typeof(BInput<int?>));
            fieldsControlMap.Add(property => property.PropertyType == typeof(DateTime), typeof(BDatePicker));
            fieldsControlMap.Add(property => property.PropertyType == typeof(DateTime?), typeof(BDatePicker));
            fieldsControlMap.Add(property => property.PropertyType == typeof(decimal), typeof(BInput<decimal>));
            fieldsControlMap.Add(property => property.PropertyType == typeof(decimal?), typeof(BInput<decimal?>));
            fieldsControlMap.Add(property => property.PropertyType == typeof(float), typeof(BInput<float>));
            fieldsControlMap.Add(property => property.PropertyType == typeof(float?), typeof(BInput<float?>));
            fieldsControlMap.Add(property => property.PropertyType == typeof(double?), typeof(BInput<double?>));
            fieldsControlMap.Add(property => property.PropertyType == typeof(double), typeof(BInput<double>));
            fieldsControlMap.Add(property => property.PropertyType == typeof(bool), typeof(BSwitch<bool>));
            fieldsControlMap.Add(property => property.PropertyType == typeof(bool?), typeof(BSwitch<bool?>));
            fieldsControlMap.Add(property => property.PropertyType == typeof(List<string>), typeof(BSelect<string>));
            fieldsControlMap.Add(property =>
            {
                if (property.PropertyType.IsEnum)
                {
                    return true;
                }
                if (!property.PropertyType.IsGenericParameter)
                {
                    return false;
                }
                if (property.PropertyType.GetGenericTypeDefinition() != typeof(Nullable<>))
                {
                    return false;
                }
                if (Nullable.GetUnderlyingType(property.PropertyType).IsEnum)
                {
                    return true;
                }
                return false;
            }, typeof(BSelect<>));
            fieldsControlMap.Add(property => property.PropertyType == typeof(IFileModel[]), typeof(BUpload));
            this.provider = provider.CreateScope().ServiceProvider;
        }

        private IServiceProvider provider { get; }

        internal Dictionary<string, FormItemConfig> GetFormItems(Type entityType, string formName)
        {
            var properties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty);
            var formItems = new Dictionary<string, FormItemConfig>();
            var index = 0;
            foreach (var property in properties)
            {
                Type controlType = null;
                foreach (var map in fieldsControlMap)
                {
                    if (!map.Key(property))
                    {
                        continue;
                    }
                    controlType = map.Value;
                }
                if (controlType == null)
                {
                    throw new BlazuiException($"类型 {property.PropertyType.FullName} 没有配置对应的组件，表单无法生成");
                }
                var formControl = property.GetCustomAttribute<FormControlAttribute>();
                if (formControl == null)
                {
                    formControl = new FormControlAttribute()
                    {
                        ControlType = controlType,
                        IsRequired = true,
                        Label = property.Name
                    };
                }
                if (string.IsNullOrWhiteSpace(formControl.RequiredMessage))
                {
                    formControl.RequiredMessage = "请填写此字段";
                }
                var formItemType = typeof(BFormItem<>).MakeGenericType(property.PropertyType);
                if (controlType.IsGenericType && controlType.GetGenericTypeDefinition() == typeof(BSelect<>))
                {
                    controlType = controlType.MakeGenericType(property.PropertyType);
                }
                formItems.Add(property.Name, new FormItemConfig()
                {
                    FormItem = formItemType,
                    IsRequired = formControl.IsRequired,
                    RequiredMessage = formControl.RequiredMessage,
                    Index = index,
                    InputControl = controlType,
                    InputControlRender = GetInputControlRender(controlType),
                    Label = formControl.Label,
                    Image = formControl.Image,
                    LabelWidth = formControl.LabelWidth,
                    Placeholder = formControl.Placeholder,
                    Name = property.Name,
                    PropertyType = property.PropertyType
                }); ; ;
                index += 11;
            }
            return formItems;
        }

        private IControlRender GetInputControlRender(Type controlType)
        {
            if (controlType == typeof(BDatePicker))
            {
                return this.provider.GetRequiredService<IDatePickerRender>();
            }
            var genericDefine = controlType.GetGenericTypeDefinition();
            if (genericDefine == typeof(BInput<>))
            {
                return this.provider.GetRequiredService<IInputRender>();
            }

            if (genericDefine == typeof(BSwitch<>))
            {
                return this.provider.GetRequiredService<ISwitchRender>();
            }
            if (genericDefine.GetGenericTypeDefinition() == typeof(BSelect<>))
            {
                return this.provider.GetRequiredService<ISelectRender>();
            }
            throw new BlazuiException($"组件 {controlType.FullName} 尚未实现对应的渲染器");
        }
    }
}
