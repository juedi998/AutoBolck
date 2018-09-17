# AdBolck
本项目基于C#实现的一个简易式广告过滤器，仅供学习研究之用，主要组件为FiddlerCode，常用的函数命令如下：
FiddlerCore组件函数：

Fiddler.FiddlerApplication.Startup(8887,FiddlerCoreStartupFlags.Default);

FiddlerApplication.BeforeRequest += delegate(Fiddler.Session oSession);

FiddlerApplication.BeforeResponse += delegate(Fiddler.Session oSession);

oSession.GetRequestBodyAsString();

oSession.GetResponseBodyAsString();

oSession.RequestHeaders[“”] = “”;

oSession.RequestHeaders.Remove(“Cookie”);

oSession.utilSetResponseBody();

FiddlerApplication.Shutdown();

Fiddler.CertMaker.rootCertExists();

Fiddler.CertMaker.createRootCert();

Fiddler.CertMaker.rootCertExists();

1、Fiddler.FiddlerApplication.Startup 函数用于开启Fiddler组件，提供了3个重载的方式，其中比较常用的是 int,bool,bool，或者int FiddlerCoreStartupFlags.Flags (Default表示修改全局代理)

同时为了方便他人使用，我建议大家使用 FiddlerCoreStartupFlags.AllowRemoteClients （表示支持远程客户端接入），值得注意的是使用完代理后，请必须记得关闭哦，否则将导致您的浏览器无法正常使用！关闭的函数为：FiddlerApplication.Shutdown(); 该函数一般在处理完后调用。

2、FiddlerApplication.BeforeRequest += delegate(Fiddler.Session oSession) 函数，该接口函数负责处理请求前的数据，即：通过该接口函数，可以定义其将发送到远程服务器之前的数据篡改后发送，适用于爬虫调试、接口拼接、URL请求劫持、广告屏蔽时使用，更多的方式大家请自行挖掘。这里要说明下：FiddlerApplication.BeforeRequest 接口函数通常出现在 “+=” 的左边，也就意味着，大家需要以上面的形式编写，后边可以是定义一个函数或与上面的那样以delegate()开头，注意，无论以哪种形式写，请务必使用Fiddler.Session oSession作为参数，该接口不支持oSession类型之外的参数，也就意味着只能在参数里写入Session，单独的写法应为：public void interface(Fiddler.oSession oSession){},值得注意的时，在调用时不需要填写参数，直接+= interface即可。

3、FiddlerApplication.BeforeResponse += delegate(Fiddler.Session oSession);接口函数用作数据请求后操作，即：通过该接口函数，可以定义将远程服务器返回的数据进行一系列的修改后，将其转发给客户端，该接口适用于过滤不良信息、劫持返回结果等场景使用，更多功能请自行挖掘。这里要说明下：FiddlerApplication.BeforeResponse 接口函数与上述的FiddlerApplication.BeforeRequest 接口类似，前者在请求前触发，后者在请求后触发，无论选择哪个，都可以实现我们的目的，返回的参数我们可以通过一个内置的接口函数：oSession.GetResponseBodyAsString;来获得，得到返回的数据后，可以通过替换、正则匹配等形式将其替换，最后用：oSession.utilSetResponseBody();接口将其设置回去，request方式一致。

4、oSession.RequestHeaders[“”] = “”; 接口函数用于修改请求头部的信息，这个与普通的请求函数修改类似，没啥好说的，值得注意的是，Cookie也在里面，如果你了解Http传输协议的话，我相信你会明白的，所以修改Cookie的话，也是通过该接口进行修改，但请注意，Cookie不支持单个修改，但支持Remove，意味着只能删除或添加，删除的方式为：oSession.RequestHeaders.Remove(“Cookie”); 也就是说添加也是如此，如需修改Cookie的值，可以借助正则、或者Replace方法进行替换即可，实在不懂欢迎交流。

5、Fiddler.CertMaker.rootCertExists() 该接口返回一个bool类型的参数，用于判断证书是否存在，如需监测ssl的流量，则需使用该函数进行判断，而FiddlerCore也提供了一个生成证书的接口，直接调用：Fiddler.CertMaker.createRootCert();方法即会弹出一个询问是否需要安装证书的窗口，点击后将创建证书，注意，该证书与Fiddler的证书不兼容，即如果您电脑已经安装了Fiddler，则该证书不适合，所以，在监测ssl流量之前，请务必使用该命令完成证书的创建。当然最后也别忘记了，清除证书哦，使用该方法可以清除证书：Fiddler.CertMaker.rootCertExists(); 当然如果您想留着也没大碍！
