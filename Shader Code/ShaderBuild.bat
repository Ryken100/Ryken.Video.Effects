echo Shader build script

echo Compiling %1 Shader

set fxc="%WindowsSdkDir%bin\%WindowsSDKVersion%x64\fxc.exe"
echo Shader compiler path: %fxc%
set INCLUDEPATH="%WindowsSdkDir%Include\%WindowsSDKVersion%um"
echo Shader include path: %INCLUDEPATH%

set COMPILE1=%fxc% %1.hlsl /nologo /T lib_4_0_level_9_3_ps_only /D D2D_FUNCTION /D D2D_ENTRY=main /Fl %1.fxlib /I %INCLUDEPATH%
set COMPILE2=%fxc% %1.hlsl /nologo /T ps_4_0_level_9_3 /D D2D_FULL_SHADER /D D2D_ENTRY=main /E main /setprivate %1.fxlib /Fo:..\ShaderBin\%1.bin /I %INCLUDEPATH%

echo %COMPILE1%
%COMPILE1%
echo %COMPILE2%
%COMPILE2%