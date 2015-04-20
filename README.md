# XPLMDataAccess
X-Plane offers the possibility to create own plugins, unfortunately, only in ANSI c, a computer language I’m not really good in (pointers makes me crazy). As an alternative I would like to use c#. However, I have to find out this is not possible so simply. My plan was to connect other Devices to X-Plane over UDP Network. But it was not possible for my purposes. There are two reasons:
1) The datarefs of the x737 from eadt team are not published over the interface (may be my error, I don't know)

2) You can only connect one device to the UDP Interface of X-Plane

The reason, why I need this I don't want to say yet. I will make a post on right time.

Well. In search of more information about X-Plane and programming in c# I found the following side: 
https://github.com/delacruz/XplaneRestApi/wiki
 Jason de la Cruz wrote a plugin to access the XPLMDataAccess API from X-Plane over a REST Webserver. The plugin is written in ANSI c and the Rest Webserver is written in c#. First the solution seems resolve all my problems. After closer consideration I still found some little things I have to change. So I’ve done a complete rewrite of the code and split the API from webserver.

At this point I would like to thank Jason de la Cruz for the excellent work and publication. With pleasure I would like to step with him in contact to discuss my changes, however, I couldn't find out any contact dates. Because the code was published on github hopefully my release is ok for him.

 

Now, I’ve created a .Net Library (DLL) in combination with an .xpl plugin for X-Plane. The library offers the possibility to access all methods of the XPLMDataAccess API from a .Net application. The communication between both takes place about shared memory.

##Installation


1) Extract XPLMDataAccess_{version}.zip

2) Copy the folder "DotNetDataRefConnector" into the x-plane folder {X-PLANE}/Resources/plugins/ Please copy the folder not only the content.

3) Put the DotNetDataRefConnector.dll into your reference of your visual studio project.

4) Do not forget the using or imports statement:

c#:

using DotNetDataRefConnector;

VB.net:

Imports DotNetDataRefConnector

 

##Using the library

(c#)

// define connection
 DotNetDataRefConnector.XPLMDataAccess da = new DotNetDataRefConnector.XPLMDataAccess();

// open connection
 da.Open();

// get reference of DataRef
 UInt32 res = da.XPLMFindDataRef(tbDataRef.Text);

// show result
 this.textBox1.Text = res.ToString();

// close connection
 da.Close();

Use for your own own risk!

 For more information check the demo app.

## Licensing
 
MIT
