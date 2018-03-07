# NAVsdk-Examples
NAVsdk example code to help users create applications. 

**NOTE** You will need a license key to use the realtime functionality or to make a NAVdisplay plugin, please contact project@oxts.com for more information. The file based functionality does not require a license.

# Folder Structure

- NAVlib - This is where the main NAVlib dlls live
- RealtimeStreams - Examples showing how to access streams via ethernet or serial
- FileBasedStreams - Examples demonstrating how to read and use OxTS format navigation data (e.g. NCOM/ROCM etc.)
- NAVdisplayPlugins - Examples of plugins that can be used with NAVdisplay


# NAVdisplay plugins

When making a plugins, you will need to place your resulting dll file in the plugins folder of NAVdisplay which is by default located here:

C:\Program Files (x86)\OxTS\NAVdisplay\plugins

Make sure to include any dependencies aswell, although the dlls in the root folder NAVdisplay will all be loaded.







