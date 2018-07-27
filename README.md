#unityAutopackage
========
最近在做直播行业混，项目组需要提供直播SDK给游戏厂商，一帮Native程序猿比较彪，直接弄个Native的Demo扔给了游戏开发商接入，可想而知，游戏开发商负责接入的程序猿肯定一万个草泥马在头顶飘啊！想想五六年年前就做过类似的工作，于是安耐不住，准备撸一个U3D自动打包的适配层和自动打包的脚本。在网上搜了一下，其实很多开发者都有这样的需求，但是完整的项目不多，其实这东西是一劳永逸的东西，做一遍以后接入渠道SDK只需要配置就行了。

> 关于我，欢迎关注  
  微信：[macwink]()  
  
#简介
-------------
无论是SDK厂商还是游戏开发商，都会涉及到U3D和Native代码的桥接，桥接后一般都会采用脚本来打包生成APK或者IPA包，尤其对于游戏厂商，在发布游戏的时候需要接入几百个渠道手动打包不太现实，下面主要介绍如何完成U3D的脚本话打包。

#环境
-------------
- 1.Android
- 1.iOS

#项目结构
-------------

``` xml
unityAutopackage
|---Assets                          // unity code folder
|  |---Plugins                      // plugin folder
|  |---unityAutopackage             // main code folder
|---Export                          // export folder
|  |---Android                      // android export folder
|  |  |---Ant                       // ant build
|  |  |---Gradle                    // gradle build
|  |---iOS
|  |  |---Dev                       // dev build
|  |---BuildScript                  // shell script
|---ProjectSettings  
|---README.md               
```
