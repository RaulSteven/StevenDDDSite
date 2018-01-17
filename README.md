#Asp.Net DDD架构浅谈——整体框架说明
说在前面的话。
不知不觉，已经写了8年的代码了，从最初的WebForm，到后来的MVC3，一路升级到现在的MVC5；ORM也从之前的ADO.Net，到EntityFramework Model First，到现在转到Dapper；项目分层也从最简单的三层架构，到现在用DDD。
技术一直在迭代更新，也会关注.Net Core，而除了.Net开发，还学会的Android、iOS开发，虽然都不是很精通，但是开发一般的应用都是没问题。但是一路走来，发现很多知识点在慢慢的淡化，所以，2018的目标就是学会写作，把这么多年学到的技术、知识点都通过Blog的形式记录下来，希望能形成一整套的知识点，以此鞭笞自己。

## 解决方案目录

![解决方案目录](http://upload-images.jianshu.io/upload_images/9686942-e61fa3d107665ee7.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)

整个方案分为7个目录
1. Core，主要是全局通用的Utility、Cache、Extensions等类，适用于所有的项目。
2. Domain，领域层，包含仓储Repository，以及复杂逻辑的Service。
3. Framework，为Web服务，Controller的基类，Filter，以及Html和Url的扩展类。
4. Presentation，表现层，比如说Web，网站项目；或者WinTools，一个工具类项目。
5. InternalService，后台服务，开发一些项目会经常需要后台任务，比如说发送邮件，生产统计数据等。
6. Tests，测试项目，目前这块是弱项，还没有实际的测试代码
7. Global，这个是用于存储一些静态的文件，比如SQL。该项目不用编译。
