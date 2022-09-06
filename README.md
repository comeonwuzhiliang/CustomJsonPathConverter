### 介绍

为了实现json字符串的映射，该库提供了两种json path 转换方式，一种是替换key，一种是生成新的数据结构的对象。

### DLL

#### 接口层

##### JsonPathConverter.Abstractions

重点接口：

1. IJsonColumnMapper：Json列转换器
2. IJsonDataProvider：Json数据来源提供者

#####  JsonPathConverter.JsonSource.HttpApi.Abstractions

HttpApi 抽象类



#### 实现层

##### JsonPathConverter.ColumnMapper.NewObject

json列转换器，主要是实现生成新的json数据的功能。

##### JsonPathConverter.ColumnMapper.ReplaceKey

json列转换器，主要是实现生成替换key值的功能。

##### JsonPathConverter.JsonSoure.HttpApi

json数据来源提供者，该提供者是通过访问Api的方式来获取json数据。

### 依赖注入

```C#
serviceCollection.AddHttpApiJsonDataProvider();

serviceCollection.AddColumnMapperReplaceKey();
// 或者
serviceCollection.AddColumnMapperNewObject();
```

### 业务层调用

```
IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

 using (var scope = serviceProvider.CreateScope())
 {
     IJsonDataProvider jsonDataProvider = scope.ServiceProvider.GetService<IJsonDataProvider>()!;
     JsonPathRoot jsonPathRoot = new JsonPathRoot("$.pages", new List<DestinationJsonColumn>()
     {
         new DestinationJsonColumn{ Code ="PageName",Name ="页面名称" },
         new DestinationJsonColumn{ Code ="PageUrl",Name ="页面地址" },
         new DestinationJsonColumn{ Code ="PageNamespace",Name ="页面空间" },
         new DestinationJsonColumn{ Code ="id",Name ="Id" },
     });

    jsonPathRoot.AddJsonPathMapper(new JsonPathMapperRelation { DestinationJsonColumnCode = "PageName", SourceJsonPath = "$.name", RootPath = jsonPathRoot.RootPath });
    jsonPathRoot.AddJsonPathMapper(new JsonPathMapperRelation { DestinationJsonColumnCode = "PageUrl", SourceJsonPath = "$.url", RootPath = jsonPathRoot.RootPath });
    jsonPathRoot.AddJsonPathMapper(new JsonPathMapperRelation { DestinationJsonColumnCode = "PageNamespace", SourceJsonPath = "$.namespace", RootPath = jsonPathRoot.RootPath });


    IJsonRequestSource requestSource = new JsonHttpApiRequestSource(new HttpRequestMessage { Method = HttpMethod.Get, RequestUri = new Uri("https://s.alicdn.com/@xconfig/flasher_classic/manifest") });

    var apiJsonStr = await jsonDataProvider.GetJsonDataAsync(requestSource, default);

    IJsonColumnMapper jsonColumnMapper = scope.ServiceProvider.GetService<IJsonColumnMapper>()!;

    var resultJson = jsonColumnMapper.MapToCollection(apiJsonStr, jsonPathRoot);
}
```



### 待做列表

- [ ] 支持列类型的转换
- [ ] 使用新的json字符串的提供者，内部使用JsonElement的ObjectEnumerator内置对象的Current的Name，来实现忽略大小写的功能
- [ ] HttpApi层里面的token的来源需要与自定义拦截器解耦，需要提供标准的OAuth2.0协议的获取token方式