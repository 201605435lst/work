# SnAbp 模块化框架

心像科技模块化框架，后端基于 Abp 模块化机制，前端基于 Vue 组件模块化机制。开发前请务必熟读此开发文档。

项目地址：[禅道地址](http://47.99.247.82/zentao/product-browse-12.html)，请详细查看开发需求。

### 1. 需求文档

---

请查看禅道文档：<a href="http://47.99.247.82/zentao/doc-browse-32.html" target="_blank">http://47.99.247.82/zentao/doc-browse-32.html</a>。

### 2. 前端开发文档

---

#### 2.1 安装必备环境

- NodeJs：按照 BBS 指南：“公司公共软件包私服（Nexus）使用方法” [http://bbs.dev.seensun.cn/d/161-nexus-npm-nuget-maven](http://bbs.dev.seensun.cn/d/161-nexus-npm-nuget-maven) 配置 npm 仓库地址；
- Yarn：同上；
- Git：按照 BBS 指南：“设置 Git 设置全局账号” [http://bbs.dev.seensun.cn/d/175-git](http://bbs.dev.seensun.cn/d/175-git)配置 Git 账号信息；
- Visual Studio Code：微软官网 [https://code.visualstudio.com/](https://code.visualstudio.com/) 安装 vscode。

#### 2.2 前端模块 npm\vue-packs

Vue 模块的主项目在 npm\vue-packs 目录里，是一个标准的 Nodejs 项目。

##### 2.2.1 运行步骤：

- cd npm\vue-packs（进入 Vue 项目主目录）
- yarn （初始化 nodejs 依赖包）
- yarn start （运行所有模块）
- yarn dev （运行单个模块）

##### 2.2.2 模块创建说明：

---

### 3. 后端开发文档

---

#### 3.1 安装必备环境

- Visual Studio 2019 16.3.0+
- Git：按照 BBS 指南：[《设置 Git 设置全局账号》](http://bbs.dev.seensun.cn/d/175-git)配置 Git 账号信息；
-Nuget: 在vs的nuget包管理器添加 可用程序包源。公司服务器地址：http://repositories.dev.seensun.cn/repository/nuget-hosted/ 

#### 3.2 Framework

Abp 核心库，不做任何改动。

#### 3.3 Templates

模块模板及项目模板通过通过工具创建。

#### 3.4 Modules

如有新增模块，模块模板通过版本创建工具 创建模块解决方案；
模块的测试可通过测试项目(引入需要测试的模块的dll)进行测试，测试项目仓库路径：SnAbp\tools\SnAbp.Example 
模块的仓库路径 SnAbp\modules ，每个模块一个单独的解决方案；模块开发好后，通过打包工具上传至公司Nuget服务器，可供其他项目使用；

### 4 异常处理编码

---

异常处理按照统一规则，分模块编码，包括模块、编码、异常消息；
格式为

```
{
    Module: "[ 模块名 ]",
    Code: [ 三位数字 ],
    Event: "[ 触发异常事件 ]",
    Message: "[ 触发异常信息及原因 ]"
}
```

如：“System 模块”

```
{
    Module: "System",
    Code: 001,
    Event: "创建用户失败",
    Message: "用户已经存在"
}

{
    Module: "System",
    Code: 002,
    Event: "编辑用户失败"，
    Message: "用户名不能为空"
}
```

### 4 接口规范

#### 4.1 路由

如 System 模块下的 User 相关接口
Services 为 SystemUserAppService 即 Swagger 分组为 SystemUser；
以 GET、POST、PUT、DELETE 为一次排序，以方法名称英文排序为二次排序，如：update、updateRoles、updatePermissions。

```
SystemUser // 此处只是作为案例，真实项目中的 System 模块并没有遵循此规范（因为此模块混合了 Abp 默认的一些接口，无法完全遵循），除 System 模块外均要遵循

// GET
GET:/api/app/user/get                       // 查询详情
GET:/api/app/user/getList                   // 查询列表（默认带分页）

// POST
POST:/api/app/user/create                   // 添加
POST:/api/app/user/addForOrganzation        // 添加

// PUT
POST:/api/app/user/update                   // 更新
POST:/api/app/user/updateRoles              // 更新角色
POST:/api/app/user/updatePermissions        // 更新权限

// DELETE
GET:/api/app/user/remove                   // 删除
GET:/api/app/user/removeFromOrganization   // 从组织机构移出
```

#### 4.1.1 方法名称及参数
```
XXXDetailDto Get ( Guid Id ) ; //获取单个实体详情
XXXDto GetList ( XXXSearchInputDto input ) ;//获取数据列表
XXXDto GetTreeList ( XXXSearchInputDto input) ;//获取树结构 返回值可用 XXXSimpleDto
XXXDetailDto Create ( XXXCreateDto input ) ; // 新增数据的接口
XXXDetailDto Update ( XXXUpdateDto input ) ;// 修改数据的接口
bool Remove ( Guid Id ) ; //删除的接口
//其他接口按业务而定，接口名清晰表述接口的作用，禁止随意简写单词
```
<!-- ```
Class UserAppService {
    // 获取用户详情
    //
    // 以 Id 获取单条数据，Id 直接写到方法参数里;
    // 不管方法是否为异步，方法名称不能包含 Async；
    //
    async async Task<UserDto> get(Guid Id)


    // 获取用户列表，带分页，带查询条件
    //
    // 参数：UserGetListInputDto为 [ 实体名称 ] + [ 方法名称 ] + InputDto，带分页的必须继承自 PagedAndSortedResultRequestDto
    // 返回值：带分页的必须为 PagedResultDto，不带分页的直接为数组 [ UserDto ]
    //
    async async Task<List<UserDto>> getList( UserGetListInputDto input )
} 
```-->

#### 4.1.2 数据传输对象 Dto

Dto命名规范：
```
XXXCreateDto 添加的输入参数（无Id）
XXXUpdateDto 修改的输入参数 （有Id）
XXXSearchInputDto 搜索条件输入参数
XXXDto 数据列表输出结果Dto
XXXSimpleDto 用于下拉框等简化输出结果Dto （如 仅包含Id和Name）
XXXDetailDto 用于显示详情的包含详细信息的Dto 
```
<!-- 一个实体的相关 Dto 应该命名统一，均已实体名称开始，如：UserDto、UserCreateDto、UserUpdateDto、UserGetListInputDto；
当前实体的主键统一以 Id 形式出现，外键以 EntityId 形式出现；
Dto 应该如果有关联数据，应该以所属关系树状形式传输，如：

```
// 职位 Position 是 User 的职位，所以必须挂载到 User 下
UserDto: {
    Id: "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    Name: "管理员",
    PositionId: "4fa85f64-5717-4562-b3fc-2c963f66afa6",
    Position: {
        Id: "5fa85f64-5717-4562-b3fc-2c963f66afa6",
        Name: "管理员"
    }
}
``` -->
