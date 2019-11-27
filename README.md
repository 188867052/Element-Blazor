# blazui

#### 介绍
Element的blazor版本

API 模仿 Element，CSS 直接使用 Element的样式，HTML 结构直接使用 Element 的 HTML 结构

Element 地址：https://element.eleme.cn/#/zh-CN/component/layout

Blazui 演示地址：http://blazui.com:9000

如果该地址不能访问了请发 Issue 告诉我， **目前版本不稳定** 

目前演示服务器配置较低，点击过快可能会有问题

#### 使用前提
参考Blazor使用的前提条件

1. 安装 .Net Core 3.0
2. 安装 VS2019

#### 贡献说明

拉取代码，用 VS2019 打开，直接启动 Blazui.ServerRender 项目

#### 使用说明

基本组件已开发完成

1. 新建 Blazor 服务器端渲染应用
2. 安装 Nuget 包 Blazui.Component
3. 修改 Pages 文件夹下的 _Host.cshtml 文件内容，引入下面三个 css 文件

```
    <link href="/_content/Blazui.Component/element/index.css" rel="stylesheet" />
    <link href="/_content/Blazui.Component/element/fix.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://unpkg.com/element-ui/lib/theme-chalk/index.css" />
```
然后在官方引入的唯一的 js 文件上方引入下面的 js 文件

```
    <script src="/_content/Blazui.Component/js/dom.js"></script>
```
4. 在 _Imports.razor 文件中添加以下代码

```
@using Blazui.Component.Container
@using Blazui.Component.Button
@using Blazui.Component.Dom
@using Blazui.Component.Dynamic
@using Blazui.Component.NavMenu
@using Blazui.Component.Input
@using Blazui.Component.Radio
@using Blazui.Component.Select
@using Blazui.Component.CheckBox
@using Blazui.Component.Switch
@using Blazui.Component.Table
@using Blazui.Component.Popup
@using Blazui.Component.Pagination
@using Blazui.Component.Form
@using Blazui.Component.Select
```

5. 在任意一个页面输入以下代码，运行可看效果

```
<BButton Type="@ButtonType.Primary">主要按钮</BButton>
```
6. 根据演示页面的示例代码写出您想要的效果

7. 有可能会遇到一个由NavigtaionManager抛出的异常，忽略即可

#### 计划

###### 类似于LayAdmin的后台管理模板 https://gitee.com/wzxinchen/BlazAdmin :fa-spinner: 
###### nuget 包与开发文档 :fa-spinner: 

#### 可用组件列表

###### 表格组件 :fa-check: 
###### 消息提示组件 :fa-check: 
###### Loading组件 :fa-check: 
###### 下拉菜单组件 :fa-check: 
###### 对话框组件 :fa-check: 
###### 表单组件 :fa-check: 
###### 按钮组件 :fa-check: 
###### 输入框组件 :fa-check: 
###### Radio组件 :fa-check: 
###### 复选框组件 :fa-check: 
###### Switch组件 :fa-check: 
###### 导航菜单组件 :fa-check: 
###### 标签页组件 :fa-check: 
###### 分页组件 :fa-check: 
###### 消息框组件 :fa-check: 
###### 日期选择器组件 :fa-check: 

#### 关注与讨论

加入QQ群：74522853